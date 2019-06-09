using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace JG.Infrastructure.AspNetCore.Logging
{
    /// <summary>
    ///     Options for configuring the request logging middleware.
    /// </summary>
    public class RequestLoggingOptions
    {
        /// <summary>
        ///     The function used to transform and select information to log from an <see cref="HttpRequest" />.
        /// </summary>
        public Func<HttpRequest, bool, IEnumerable<KeyValuePair<string, object>>> RequestProjection { get; set; }
    }
}
