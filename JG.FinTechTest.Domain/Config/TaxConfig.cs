using System;
using JG.FinTechTest.Domain.TaxEngine;
using JG.FinTechTest.Domain.TaxProcessors;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace JG.FinTechTest.Domain.Config
{
    public class TaxConfig
    {
        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TaxProcessorType ProcessorType { get; set; }

        public IConfigurationSection ProcessorOptions { get; set; }

        public TaxType TaxType { get; set; }
    }
}
