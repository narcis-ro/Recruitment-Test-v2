using System;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.TaxProcessors.SimplePercent;
using Microsoft.Extensions.Configuration;

namespace JG.FinTechTest.Domain.TaxProcessors
{
    public class TaxProcessorFactory : ITaxProcessorFactory
    {
        public ITaxProcessor Create(TaxProcessorType type, IConfigurationSection options = default)
        {
            switch (type)
            {
                case TaxProcessorType.NoTax:
                    return NoTaxProcessor.Default;

                case TaxProcessorType.SimplePercent:
                    if (options == default)
                        throw new ArgumentException(
                            $"Options for {nameof(SimplePercentTaxProcessor)} are required. Make sure key '{nameof(TaxConfig.ProcessorOptions)}' is defined in appSettings.json.");

                    var opts = new SimplePercentTaxProcessorOptions();
                    options.Bind(opts);

                    return new SimplePercentTaxProcessor(opts);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
