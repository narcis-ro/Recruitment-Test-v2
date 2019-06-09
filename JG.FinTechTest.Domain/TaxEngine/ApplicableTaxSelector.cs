using System;
using System.Collections.Generic;
using System.Linq;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.Handlers;
using JG.FinTechTest.Domain.TaxProcessors;

namespace JG.FinTechTest.Domain.TaxEngine
{
    public class ApplicableTaxSelector : IApplicableTaxSelector
    {
        private readonly Dictionary<TaxType, TaxConfig[]> _taxes;
        private readonly ITaxProcessorFactory _taxProcessorFactory;

        public ApplicableTaxSelector(ITaxProcessorFactory taxProcessorFactory, DonationConfig donationConfig)
        {
            _taxProcessorFactory = taxProcessorFactory ?? throw new ArgumentNullException(nameof(taxProcessorFactory));

            if (donationConfig == default)
                throw new ArgumentNullException(nameof(donationConfig));

            if (!(donationConfig.Taxes?.Any() ?? false))
                throw TaxException.NoTaxesDefined();

            _taxes = donationConfig.Taxes.GroupBy(s => s.TaxType).ToDictionary(s => s.Key, s => s.ToArray());
        }

        public ITaxProcessor GetProcessor(TaxType taxType)
        {
            if (!_taxes.ContainsKey(taxType))
                throw TaxException.NoTaxesDefinedForType(taxType);

            var now = DateTime.UtcNow;

            var taxConfig = _taxes[taxType].FirstOrDefault(s => s.TaxType == taxType && s.FromDate >= now && (s.ToDate == default || s.ToDate < now));

            // TODO: Log

            if (taxConfig == default)
                return NoTaxProcessor.Default;

            return _taxProcessorFactory.Create(taxConfig.ProcessorType, taxConfig.ProcessorOptions);
        }
    }
}
