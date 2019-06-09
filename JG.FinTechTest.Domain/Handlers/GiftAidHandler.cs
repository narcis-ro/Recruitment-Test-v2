using System;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.TaxEngine;
using MediatR;

namespace JG.FinTechTest.Domain.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public class GiftAidHandler : IRequestHandler<CalculateGiftAidRequest, CalculateGiftAidResponse>
    {
        private readonly IApplicableTaxSelector _applicableTaxSelector;
        private readonly DonationConfig _donationConfig;

        public GiftAidHandler(IApplicableTaxSelector applicableTaxSelector, DonationConfig donationConfig)
        {
            _applicableTaxSelector = applicableTaxSelector ?? throw new ArgumentNullException(nameof(applicableTaxSelector));
            _donationConfig = donationConfig ?? throw new ArgumentNullException(nameof(donationConfig));
        }

        public Task<CalculateGiftAidResponse> Handle(CalculateGiftAidRequest request, CancellationToken cancellationToken)
        {
            // TODO: Logging ?!

            ValidateRequest(request);

            var processor = _applicableTaxSelector.GetProcessor(TaxType.GiftAid);

            var processorResult = processor.CalculateTax(request.DonationAmount);

            return Task.FromResult(new CalculateGiftAidResponse
            {
                DonationAmount = request.DonationAmount,
                GiftAidAmount = processorResult.Amount
            });
        }

        private void ValidateRequest(CalculateGiftAidRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.DonationAmount < _donationConfig.MinDonationAmount)
                throw DonationException.LessThanMinimum(request.DonationAmount, _donationConfig.MinDonationAmount);

            if (request.DonationAmount >= _donationConfig.MaxDonationAmount)
                throw DonationException.ExceedsMaximum(request.DonationAmount, _donationConfig.MaxDonationAmount);
        }
    }
}
