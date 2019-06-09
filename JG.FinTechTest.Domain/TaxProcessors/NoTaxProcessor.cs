using JG.FinTechTest.Domain.TaxProcessors;

namespace JG.FinTechTest.Domain.Handlers
{
    public class NoTaxProcessor : ITaxProcessor
    {
        public static NoTaxProcessor Default { get; set; } = new NoTaxProcessor();

        public TaxProcessorResult CalculateTax(decimal amount)
        {
           return new TaxProcessorResult{ Amount = amount};
        }
    }
}
