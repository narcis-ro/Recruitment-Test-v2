using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace JG.FinTechTest.Domain.TaxProcessors
{
    public interface ITaxProcessorFactory
    {
        ITaxProcessor Create(TaxProcessorType type, IConfigurationSection options = default);
    }
}
