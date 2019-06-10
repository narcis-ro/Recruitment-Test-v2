using System.Threading.Tasks;
using FluentAssertions;
using JG.FinTechTest.Api.Models;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.UnitTests.Controllers.Donation.Builders;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace JG.FinTechTest.UnitTests.Controllers.Donation
{
    [TestFixture]
    public class DonationControllerTests
    {
        [Test]
        public async Task POST_Returns_Donation_Details()
        {
            // Arrange
            var donationRequest = new DonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();
            var donation = arrangements.RegisterDonationResponse.Donation;

            // Act
            var donationResponse = (DonationResponse) ((CreatedResult) (await arrangements.Sut.Post(donationRequest)).Result).Value;

            // Assert
            donationResponse.Should().BeEquivalentTo(new DonationResponse
            {
                DonationId = donation.Id,
                DonationAmount = donation.DonationAmount,
                GiftAidAmount = donation.GiftAidAmount,
                GiftAidReference = donation.GiftAidReference,
                DonationReference = donation.Reference
            });
        }

        [Test]
        public async Task POST_Returns_Status_Created()
        {
            // Arrange
            var donationRequest = new DonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            var response = await arrangements.Sut.Post(donationRequest);

            // Assert
            response.Result.Should().BeOfType<CreatedResult>();
        }

        [Test]
        public async Task POST_Sends_RegisterDonationRequest_WithData_FromBody()
        {
            // Arrange
            var donationRequest = new DonationRequestBuilder().Build();
            var arrangements = new ArrangementsBuilder().Build();

            // Act
            await arrangements.Sut.Post(donationRequest);

            // Assert
            arrangements.RegisterDonationRequest.Should().BeEquivalentTo(new RegisterDonationRequest
            {
                DonationAmount = donationRequest.Amount,
                FirstName = donationRequest.FirstName,
                LastName = donationRequest.LastName,
                PostCode = donationRequest.PostCode
            });
        }
    }
}
