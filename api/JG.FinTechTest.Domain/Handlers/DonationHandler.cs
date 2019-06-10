using System;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Commands;
using JG.FinTechTest.Domain.Data.Model;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.Requests;
using JG.Infrastructure.Utils;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JG.FinTechTest.Domain.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public class DonationHandler : IRequestHandler<RegisterDonationRequest, RegisterDonationResponse>
    {
        private readonly ICreateDonationCommand _createDonationCommand;
        private readonly ILogger<DonationHandler> _logger;
        private readonly IMediator _mediator;

        public DonationHandler(IMediator mediator, ICreateDonationCommand createDonationCommand, ILogger<DonationHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _createDonationCommand = createDonationCommand ?? throw new ArgumentNullException(nameof(createDonationCommand));
            _logger = logger;
        }

        public async Task<RegisterDonationResponse> Handle(RegisterDonationRequest request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            _logger.LogDebug("Calculating GiftAid for {DonationAmount}", request.DonationAmount);

            var giftAidResponse = await _mediator.Send(new CalculateGiftAidRequest
            {
                DonationAmount = request.DonationAmount
            }, cancellationToken);

            var donation = new Donation
            {
                Id = Guid.NewGuid(),
                Reference = FriendlyUId.NewId(),
                GiftAidReference = FriendlyUId.NewId(),
                DonationAmount = request.DonationAmount,
                GiftAidAmount = giftAidResponse.GiftAidAmount,
                DonorDetails = new DonorDetails
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PostCode = request.PostCode
                }
            };

            _logger.LogDebug(
                "Registering donation {DonationId} for {DonationAmount} amount with gift-aid reference {GiftAidReference}", donation.Id, request.DonationAmount, donation.GiftAidReference);

            await _createDonationCommand.Execute(donation, cancellationToken);


            // Note: For now this can be a fire-and-forget action and there's not need to be inside a transaction.
            // Note: The idea is that the donation should succeed even if filing the declaration fails, it's recoverable information and can be retried later on queue.
#pragma warning disable 4014
            _mediator.Send(new FileGiftAidDeclarationRequest {Donation = donation}, cancellationToken);
#pragma warning restore 4014

            return new RegisterDonationResponse
            {
                Donation = donation
            };
        }
        
        private void ValidateRequest(RegisterDonationRequest request)
        {
            _logger.LogDebug("Validating request");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw DonationException.InvalidDonorName(nameof(request.FirstName), request.FirstName);

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw DonationException.InvalidDonorName(nameof(request.LastName), request.LastName);

            if (string.IsNullOrWhiteSpace(request.PostCode))
                throw DonationException.InvalidPostcode(request.PostCode);
        }
    }
}
