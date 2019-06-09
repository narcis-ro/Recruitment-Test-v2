using System;
using System.Net;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Models;
using JG.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JG.FinTechTest.Controllers
{
    /// <summary>
    ///     GiftAid Api
    /// </summary>
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly ILogger<GiftAidController> _logger;
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public GiftAidController(IMediator mediator, ILogger<GiftAidController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        /// <summary>
        ///     Get the amount of gift aid reclaimable for donation amount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(GiftAidResponse))]
        [ProducesResponseType((int) HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        public async Task<ActionResult<GiftAidResponse>> Get([FromQuery] GiftAidRequest request)
        {
            var response = await _mediator.Send(new CalculateGiftAidRequest
            {
                DonationAmount = request.Amount
            });

            _logger.LogDebug("Reclaim {GiftAidAmount} for a {DonationAmount} donation.", request.Amount, response.GiftAidAmount);

            return new GiftAidResponse
            {
                GiftAidAmount = response.GiftAidAmount,
                DonationAmount = request.Amount
            };
        }
    }
}
