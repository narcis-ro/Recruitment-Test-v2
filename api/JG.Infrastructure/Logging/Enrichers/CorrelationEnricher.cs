using JG.Infrastructure.Correlation;
using Serilog.Core;
using Serilog.Events;

namespace JG.Infrastructure.Logging.Enrichers
{
    public class CorrelationEnricher : ILogEventEnricher
    {
        /// <summary>
        ///     The property name added to enriched log events.
        /// </summary>
        public const string CORRELATION_ID_PROPERTY_NAME = "CorrelationId";

        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationEnricher(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        /// <summary>
        ///     Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(CORRELATION_ID_PROPERTY_NAME,
                _correlationContextAccessor.CorrelationContext?.CorrelationId));
        }
    }
}
