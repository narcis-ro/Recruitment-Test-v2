using System;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.UnitTests.Models;

namespace JG.FinTechTest.UnitTests.Controllers.Donation
{
    internal class RegisterDonationResponseBuilder
    {
        private RegisterDonationResponse _response;

        public RegisterDonationResponseBuilder()
        {
            _response = new RegisterDonationResponse
            {
                Donation = new DonationModelBuilder().Build()
            };
        }

        public RegisterDonationResponseBuilder With(RegisterDonationResponse response)
        {
            _response = response;

            return this;
        }

        public RegisterDonationResponseBuilder With(Action<RegisterDonationResponse> props)
        {
            props?.Invoke(_response);

            return this;
        }

        public RegisterDonationResponse Build()
        {
            return _response;
        }
    }
}
