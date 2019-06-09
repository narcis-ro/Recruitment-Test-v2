using JG.FinTechTest.Domain.TaxProcessors;

namespace JG.FinTechTest.Domain.TaxEngine
{
    public interface IApplicableTaxSelector
    {
        ITaxProcessor GetProcessor(TaxType taxType);
    }
}
