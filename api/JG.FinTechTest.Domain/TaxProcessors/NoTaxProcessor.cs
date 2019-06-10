namespace JG.FinTechTest.Domain.TaxProcessors
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
