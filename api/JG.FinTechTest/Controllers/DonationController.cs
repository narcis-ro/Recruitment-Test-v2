﻿using System;
using System.Net;
using System.Threading.Tasks;
using JG.FinTechTest.Api.Models;
using JG.FinTechTest.Domain.Requests;
using JG.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JG.FinTechTest.Api.Controllers
{
    /// <summary>
    ///     Donations Api
    /// </summary>
    [Route("api/donations")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly ILogger<DonationController> _logger;
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public DonationController(IMediator mediator, ILogger<DonationController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        /// <summary>
        ///     Register Donation and file the Gift-Aid Declaration.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Created, Type = typeof(DonationResponse))]
        [ProducesResponseType((int) HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        public async Task<ActionResult<DonationResponse>> Post([FromBody] DonationRequest request)
        {
            var response = await _mediator.Send(new RegisterDonationRequest
            {
                DonationAmount = request.Amount,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PostCode = request.PostCode
            });

            var donation = response.Donation;

            _logger.LogDebug(
                "Registered donation {DonationId} for {DonationAmount} with Gift-Aid reference {GiftAidReference}.", donation.Id, donation.DonationAmount, donation.GiftAidReference);

            return Created(donation.Id.ToString(), new DonationResponse
            {
                DonationId = donation.Id,
                DonationReference = donation.Reference,
                GiftAidReference = donation.GiftAidReference,
                GiftAidAmount = donation.GiftAidAmount,
                DonationAmount = donation.DonationAmount
            });
        }
    }
}
