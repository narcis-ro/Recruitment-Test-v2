using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JG.FinTechTest.Domain.Exceptions;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.UnitTests.Handlers.Donation.Builders;
using NUnit.Framework;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation
{
    [TestFixture]
    public class DonationControllerTests
    {
        [Test]
        public async Task RegisterDonation_Creates_Donation()
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            await arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            arrangements.Donation.Should().BeEquivalentTo(arrangements.Donation);
        }

        [Test]
        public async Task RegisterDonation_Files_GiftAid_Declaration()
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            await arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            var fileGiftAidDeclarationRequest = arrangements.FileGiftAidDeclarationRequest;

            fileGiftAidDeclarationRequest.Should().NotBeNull();
            fileGiftAidDeclarationRequest.Should().BeEquivalentTo(new FileGiftAidDeclarationRequest
            {
                Donation = arrangements.Donation
            });
        }

        [Test]
        public void RegisterDonation_Request_IsNotNull()
        {
            // Arrange
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            Func<Task> handle = () => arrangements.Sut.Handle(null, CancellationToken.None);

            // Assert
            handle.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task RegisterDonation_Requests_GiftAid_Calculation()
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            await arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            var calculateGiftAidRequest = arrangements.CalculateGiftAidRequest;

            calculateGiftAidRequest.Should().NotBeNull();
            calculateGiftAidRequest.Should().BeEquivalentTo(new CalculateGiftAidRequest
            {
                DonationAmount = request.DonationAmount
            });
        }

        [Test]
        public async Task RegisterDonation_Returns_DonationDetails()
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            var response = await arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(new RegisterDonationResponse { Donation = arrangements.Donation });
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void RegisterDonation_Validates_FirstName(string firstName)
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().WithFirstName(firstName).Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            Func<Task> handle = () => arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            handle.Should().Throw<DonationException>().Which.ErrorCodeId.Should().Be((int)DomainErrorCodes.InvalidDonorName);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void RegisterDonation_Validates_LastName(string lastName)
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().WithLastName(lastName).Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            Func<Task> handle = () => arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            handle.Should().Throw<DonationException>().Which.ErrorCodeId.Should().Be((int)DomainErrorCodes.InvalidDonorName);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void RegisterDonation_Validates_PostCode(string postCode)
        {
            // Arrange
            var request = new RegisterDonationRequestBuilder().WithPostCode(postCode).Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            Func<Task> handle = () => arrangements.Sut.Handle(request, CancellationToken.None);

            // Assert
            handle.Should().Throw<DonationException>().Which.ErrorCodeId.Should().Be((int)DomainErrorCodes.InvalidPostcode);
        }
    }
}
