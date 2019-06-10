using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace JG.Infrastructure.AspNetCore.StartupTasks
{
    public class TaskExecutingServer : IServer
    {
        private readonly ILogger<TaskExecutingServer> _logger;
        private readonly IServer _server;
        private readonly IEnumerable<IStartupTask> _startupTasks;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaskExecutingServer" /> class.
        /// </summary>
        /// <param name="server">The decorated server instance </param>
        /// <param name="startupTasks">The tasks to execute on startup</param>
        public TaskExecutingServer(IServer server, IEnumerable<IStartupTask> startupTasks,
            ILogger<TaskExecutingServer> logger)
        {
            _server = server;
            _startupTasks = startupTasks;
            _logger = logger;
        }

        /// <inheritdoc />
        public IFeatureCollection Features => _server.Features;

        /// <inheritdoc />
        public async Task StartAsync<TContext>(IHttpApplication<TContext> application,
            CancellationToken cancellationToken)
        {
            foreach (var startupTask in _startupTasks)
                try
                {
                    await startupTask.StartAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "StartupTask failed to execute.");

                    throw;
                }

            await _server.StartAsync(application, cancellationToken);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _server.Dispose();
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.StopAsync(cancellationToken);

            foreach (var startupTask in _startupTasks) await startupTask.ShutdownAsync(cancellationToken);
        }
    }
}
