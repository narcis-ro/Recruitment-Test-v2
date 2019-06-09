namespace JG.FinTechTest.Domain.TaxProcessors
{
    public interface ITaxProcessor
    {
        TaxProcessorResult CalculateTax(decimal amount);
    }
}
