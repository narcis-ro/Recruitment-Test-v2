using Microsoft.Extensions.Configuration;

namespace JG.FinTechTest.Domain.TaxProcessors
{
    public interface ITaxProcessorFactory
    {
        ITaxProcessor Create(TaxProcessorType type, IConfigurationSection options = default);
    }
}
