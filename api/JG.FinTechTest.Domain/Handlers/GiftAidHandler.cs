using System;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.Data.Commands;
using JG.FinTechTest.Domain.Data.Model;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.TaxEngine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JG.FinTechTest.Domain.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public class GiftAidHandler :
        IRequestHandler<CalculateGiftAidRequest, CalculateGiftAidResponse>,
        IRequestHandler<FileGiftAidDeclarationRequest, FileGiftAidDeclarationResponse>
    {
        private readonly IApplicableTaxSelector _applicableTaxSelector;
        private readonly ICreateGiftAidDeclarationCommand _createGiftAidDeclarationCommand;
        private readonly DonationConfig _donationConfig;
        private readonly ILogger<GiftAidHandler> _logger;

        public GiftAidHandler(IApplicableTaxSelector applicableTaxSelector, ICreateGiftAidDeclarationCommand createGiftAidDeclarationCommand, DonationConfig donationConfig, ILogger<GiftAidHandler> logger)
        {
            _applicableTaxSelector = applicableTaxSelector ?? throw new ArgumentNullException(nameof(applicableTaxSelector));
            _createGiftAidDeclarationCommand = createGiftAidDeclarationCommand ?? throw new ArgumentNullException(nameof(createGiftAidDeclarationCommand));
            _donationConfig = donationConfig ?? throw new ArgumentNullException(nameof(donationConfig));
            _logger = logger;
        }

        public Task<CalculateGiftAidResponse> Handle(CalculateGiftAidRequest request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var processor = _applicableTaxSelector.GetProcessor(TaxType.GiftAid);

            _logger.LogDebug("Calculating Gift-Aid taxes with {TaxProcessor} for {DonationAmount}", processor.GetType().Name, request.DonationAmount);

            var processorResult = processor.CalculateTax(request.DonationAmount);

            return Task.FromResult(new CalculateGiftAidResponse
            {
                DonationAmount = request.DonationAmount,
                GiftAidAmount = processorResult.Amount
            });
        }

        public async Task<FileGiftAidDeclarationResponse> Handle(FileGiftAidDeclarationRequest request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var donation = request.Donation;

            var giftAidDeclaration = new GiftAidDeclaration
            {
                Id = Guid.NewGuid(),
                DonationId = donation.Id,
                DonationAmount = donation.DonationAmount,
                GiftAidAmount = donation.GiftAidAmount,
                GiftAidReference = donation.GiftAidReference,
                DonorDetails = donation.DonorDetails,
                Created = DateTimeOffset.Now
            };

            _logger.LogDebug("Filing Gift-Aid declaration with reference {GiftAidReference} for donation with id {DonationId}.", donation.GiftAidReference, request.Donation.Id);

            await _createGiftAidDeclarationCommand.Execute(giftAidDeclaration, cancellationToken);

            return new FileGiftAidDeclarationResponse
            {
                Declaration = giftAidDeclaration,
            };
        }

        private void ValidateRequest(CalculateGiftAidRequest request)
        {
            _logger.LogDebug("Validating request");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.DonationAmount < _donationConfig.MinDonationAmount)
                throw DonationException.LessThanMinimum(request.DonationAmount, _donationConfig.MinDonationAmount);

            if (request.DonationAmount >= _donationConfig.MaxDonationAmount)
                throw DonationException.ExceedsMaximum(request.DonationAmount, _donationConfig.MaxDonationAmount);
        }

        private void ValidateRequest(FileGiftAidDeclarationRequest request)
        {
            _logger.LogDebug("Validating request");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Donation == null)
                throw new ArgumentNullException($"{nameof(request)}.{nameof(request.Donation)}");
        }
    }
}
