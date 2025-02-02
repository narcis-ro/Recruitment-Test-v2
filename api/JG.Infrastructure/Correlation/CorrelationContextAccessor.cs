﻿using System.Threading;

namespace JG.Infrastructure.Correlation
{
    /// <inheritdoc />
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private static readonly AsyncLocal<CorrelationContext> _correlationContext =
            new AsyncLocal<CorrelationContext>();

        /// <inheritdoc />
        public CorrelationContext CorrelationContext
        {
            get => _correlationContext.Value;
            set => _correlationContext.Value = value;
        }
    }
}
