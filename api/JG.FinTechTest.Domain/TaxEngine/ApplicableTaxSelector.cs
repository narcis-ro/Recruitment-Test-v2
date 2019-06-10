using System;
using System.Collections.Generic;
using System.Linq;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.TaxProcessors;
using Microsoft.Extensions.Logging;

namespace JG.FinTechTest.Domain.TaxEngine
{
    public class ApplicableTaxSelector : IApplicableTaxSelector
    {
        private readonly Dictionary<TaxType, TaxConfig[]> _taxes;
        private readonly ITaxProcessorFactory _taxProcessorFactory;
        private readonly ILogger<ApplicableTaxSelector> _logger;

        public ApplicableTaxSelector(ITaxProcessorFactory taxProcessorFactory, DonationConfig donationConfig, ILogger<ApplicableTaxSelector> logger)
        {
            _taxProcessorFactory = taxProcessorFactory ?? throw new ArgumentNullException(nameof(taxProcessorFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (donationConfig == default)
                throw new ArgumentNullException(nameof(donationConfig));

            if (!(donationConfig.Taxes?.Any() ?? false))
                throw TaxException.NoTaxesDefined();

            logger.LogDebug("Found {NoOfTaxConfigs} tax configurations", donationConfig.Taxes.Count);

            _taxes = donationConfig.Taxes.GroupBy(s => s.TaxType).ToDictionary(s => s.Key, s => s.ToArray());
        }

        public ITaxProcessor GetProcessor(TaxType taxType)
        {
            if (!_taxes.ContainsKey(taxType))
                throw TaxException.NoTaxesDefinedForType(taxType);

            var now = DateTimeOffset.UtcNow;

            var taxConfig = _taxes[taxType].FirstOrDefault(s => s.TaxType == taxType && s.FromDate >= now && (s.ToDate == default || s.ToDate < now));

            if (taxConfig == default)
            {
                _logger.LogDebug("No {TaxType} matched for current date. No taxes will be applied", taxType);
                return NoTaxProcessor.Default;
            }
            
            return _taxProcessorFactory.Create(taxConfig.ProcessorType, taxConfig.ProcessorOptions);
        }
    }
}
