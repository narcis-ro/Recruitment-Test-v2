using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JG.Infrastructure.Constants;
using JG.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.IO;
using Serilog;
using Serilog.Events;

namespace JG.Infrastructure.AspNetCore.Logging
{
    /// <summary>
    /// Middleware that logs HTTP request and response details.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private const string MessageTemplate = "HTTP {Verb} to {Path} responded with {StatusCode} in {Elapsed:0.0000} ms";

        private readonly RequestDelegate _next;
        private readonly RequestLoggingOptions _options;
        private readonly ILogger _logger;
        private RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger logger, RequestLoggingOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger?.ForContext<RequestLoggingMiddleware>() ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        // ReSharper disable once UnusedMember.Global
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var stopwatch = new Stopwatch();

            var requestLogContext = new ConcurrentDictionary<string, object>();
            var requestLogContextForError = new ConcurrentDictionary<string, object>();


            httpContext.Items.Add(LogProps.REQUEST_LOG_CONTEXT_ITEM, requestLogContext);
            httpContext.Items.Add(LogProps.REQUEST_LOG_CONTEXT_FOR_ERROR_ITEM, requestLogContextForError);

            var originalBodyStream = httpContext.Response.Body;

            using (var responseBody = _recyclableMemoryStreamManager.GetStream())
            {
                httpContext.Response.Body = responseBody;

                try
                {
                    using (new BenchmarkToken(stopwatch))
                    {
                        await _next(httpContext);
                    }

                    var statusCode = httpContext.Response?.StatusCode;

                    // Note: Frontend should usually send the correct values, so a BadRequest it's usually a front-end error and it should be logger.
                    var forError = statusCode > 399;
                    var level = forError ? LogEventLevel.Error : LogEventLevel.Information;

                    var contextualLogger = await PopulateLogContext(_logger, httpContext, responseBody, requestLogContext, requestLogContextForError, forError);

                    contextualLogger.Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, stopwatch.Elapsed.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    (await PopulateLogContext(_logger, httpContext, responseBody, requestLogContext, requestLogContextForError, true))
                        .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, stopwatch.Elapsed.TotalMilliseconds);

                    // Note: The MVC Exception filter should have caught the error. If it got to this point, it means it
                    // Note: it's an unhandled (unknown exception) and we just log it. The higher middleware (UnhandledExceptionMiddleware) will catch it.
                    throw;
                }
                finally
                {
                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

        private async Task<ILogger> PopulateLogContext(ILogger logger, HttpContext httpContext, MemoryStream responseBody, ConcurrentDictionary<string, object> requestLogContext,
            ConcurrentDictionary<string, object> requestLogContextForError, bool forError)
        {
            try
            {
                var request = httpContext.Request;

                var props = new Dictionary<string, object>(requestLogContext)
                {
                    [LogProps.QUERY] = request.QueryString.ToString(),
                };

                var upstreamServiceName = request.Headers.FirstOrDefault(s => s.Key == HttpHeaders.X_ORIGIN_SERVICE_NAME);
                var upstreamServiceId = request.Headers.FirstOrDefault(s => s.Key == HttpHeaders.X_ORIGIN_SERVICE_ID);

                if (upstreamServiceId.Key != default)
                    props[LogProps.CLIENT_SERVICE_ID] = upstreamServiceId;

                if (upstreamServiceName.Key != default)
                    props[LogProps.CLIENT_SERVICE_NAME] = upstreamServiceName;


                if (forError)
                {
                    // Note: Merge overwrite
                    foreach (var pair in requestLogContextForError)
                        props[pair.Key] = pair.Value;

                    foreach (var pair in await PopulateLogContextForError(httpContext, responseBody))
                        props[pair.Key] = pair.Value;
                }

                var projection = _options.RequestProjection?.Invoke(request, forError);

                if (projection != null)
                    foreach (var pair in projection)
                        props[pair.Key] = pair.Value;

                return logger.ForContext(LogProps.REQUEST_PROP, props, true);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to create request logger.");

                return _logger;
            }
        }

        private async Task<Dictionary<string, object>> PopulateLogContextForError(HttpContext httpContext, MemoryStream responseBody)
        {
            var request = httpContext.Request;

            var props = new Props
            {
                [LogProps.HEADERS] = request.Headers.Where(h => h.Key != HttpHeaders.AUTHORIZATION).ToDictionary(h => h.Key, h => h.Value.ToString())
            };


            if (request.HasFormContentType)
                props.Add(LogProps.FORM, request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            if (request.Body != null)
            {
                try
                {
                    request.EnableBuffering();
                    request.EnableRewind();

                    using (var stream = _recyclableMemoryStreamManager.GetStream())
                    {
                        await request.Body.CopyToAsync(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        request.Body.Seek(0, SeekOrigin.Begin);

                        using (var streamReader = new StreamReader(stream))
                        {
                            var body = await streamReader.ReadToEndAsync();

                            props.Add(LogProps.BODY, body);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Failed to read http stream body.");
                }
            }


            var responseProps = new Props();

            try
            {
                // Note: This is not our stream, leave it open.
                using (var streamReader = new StreamReader(responseBody, Encoding.UTF8, true, 1024, true))
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var body = await streamReader.ReadToEndAsync();

                    responseProps.Add(LogProps.BODY, body);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to read http stream body.");
            }

            props.Add(LogProps.RESPONSE, responseProps);


            return props;
        }

        /// <summary>
        /// This class ensures that a stopwatch is stopped when an exception occurs without 
        /// needing multiple <see cref="Stopwatch.Stop"/> calls.
        /// </summary>
        class BenchmarkToken : IDisposable
        {
            private readonly Stopwatch _stopwatch;

            public BenchmarkToken(Stopwatch stopwatch = default)
            {
                _stopwatch = stopwatch ?? new Stopwatch();
                _stopwatch.Start();
            }

            public void Dispose() => _stopwatch.Stop();
        }
    }
}