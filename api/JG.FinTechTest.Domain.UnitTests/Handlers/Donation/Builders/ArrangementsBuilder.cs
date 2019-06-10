using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Commands;
using JG.FinTechTest.Domain.Handlers;
using JG.FinTechTest.Domain.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation.Builders
{
    public class ArrangementsBuilder
    {
        private readonly CalculateGiftAidResponse _calculateGiftAidResponse = new CalculateGiftAidResponseBuilder().Build();
        private readonly Mock<ICreateDonationCommand> _createDonationCommandMock;
        private readonly FileGiftAidDeclarationResponse _fileGiftAidDeclarationResponse = new FileGiftAidDeclarationResponseBuilder().Build();
        private readonly Mock<ILogger<DonationHandler>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private CalculateGiftAidRequest _calculateGiftAidRequest;
        private Data.Model.Donation _createdDonation;
        private FileGiftAidDeclarationRequest _fileGiftAidDeclarationRequest;

        public ArrangementsBuilder()
        {
            _mediatorMock = new Mock<IMediator>();
            _createDonationCommandMock = new Mock<ICreateDonationCommand>();
            _loggerMock = new Mock<ILogger<DonationHandler>>();

            _mediatorMock.Setup(s => s.Send(It.IsAny<CalculateGiftAidRequest>(), It.IsAny<CancellationToken>()))
                .Returns((CalculateGiftAidRequest r, CancellationToken c) =>
                {
                    _calculateGiftAidRequest = r;
                    return Task.FromResult(_calculateGiftAidResponse);
                });

            _mediatorMock.Setup(s => s.Send(It.IsAny<FileGiftAidDeclarationRequest>(), It.IsAny<CancellationToken>()))
                .Returns((FileGiftAidDeclarationRequest r, CancellationToken c) =>
                {
                    _fileGiftAidDeclarationRequest = r;
                    return Task.FromResult(_fileGiftAidDeclarationResponse);
                });

            _createDonationCommandMock.Setup(s => s.Execute(It.IsAny<Data.Model.Donation>(), It.IsAny<CancellationToken>()))
                .Returns((Data.Model.Donation d, CancellationToken c) =>
                {
                    _createdDonation = d;
                    return Task.FromResult(true);
                });
        }


        public Arrangements Build()
        {
            return new Arrangements
            {
                Sut = CreateSut(),
                CalculateGiftAidResponse = _calculateGiftAidResponse,
                FileGiftAidDeclarationResponse = _fileGiftAidDeclarationResponse,
                CalculateGiftAidRequestAccessor = () => _calculateGiftAidRequest,
                FileGiftAidDeclarationRequestAccessor = () => _fileGiftAidDeclarationRequest,
                DonationAccessor = () => _createdDonation
            };
        }

        private DonationHandler CreateSut()
        {
            var sut = new DonationHandler(_mediatorMock.Object, _createDonationCommandMock.Object, _loggerMock.Object);

            return sut;
        }
    }
}
