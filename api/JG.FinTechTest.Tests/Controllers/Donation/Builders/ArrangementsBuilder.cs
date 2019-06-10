using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Api.Controllers;
using JG.FinTechTest.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace JG.FinTechTest.UnitTests.Controllers.Donation
{
    public class ArrangementsBuilder
    {
        private readonly Mock<ILogger<DonationController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;

        private readonly RegisterDonationResponse _registerDonationResponse = new RegisterDonationResponseBuilder().Build();
        private RegisterDonationRequest _registerDonationRequest;

        public ArrangementsBuilder()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<DonationController>>();

            _mediatorMock.Setup(s => s.Send(It.IsAny<RegisterDonationRequest>(), It.IsAny<CancellationToken>()))
                .Returns((RegisterDonationRequest r, CancellationToken c) =>
                {
                    _registerDonationRequest = r;
                    return Task.FromResult(_registerDonationResponse);
                });
        }


        public Arrangements Build()
        {
            return new Arrangements
            {
                Sut = CreateSut(),
                RegisterDonationResponse = _registerDonationResponse,
                RegisterDonationRequestAccessor = () => _registerDonationRequest
            };
        }

        private DonationController CreateSut()
        {
            var sut = new DonationController(_mediatorMock.Object, _loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return sut;
        }
    }
}
