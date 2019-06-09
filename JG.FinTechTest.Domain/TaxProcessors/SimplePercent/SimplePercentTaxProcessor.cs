using System;

namespace JG.FinTechTest.Domain.TaxProcessors.SimplePercent
{
    public class SimplePercentTaxProcessor : ITaxProcessor
    {
        private readonly SimplePercentTaxProcessorOptions _options;

        public SimplePercentTaxProcessor(SimplePercentTaxProcessorOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public TaxProcessorResult CalculateTax(decimal amount)
        {
            return new TaxProcessorResult
            {
                Amount = _options.TaxRate <= 0
                    ? amount // Note: Depending on requirements we might need to throw here.
                    : amount * (_options.TaxRate / (100 - _options.TaxRate))
            };
        }
    }
}
