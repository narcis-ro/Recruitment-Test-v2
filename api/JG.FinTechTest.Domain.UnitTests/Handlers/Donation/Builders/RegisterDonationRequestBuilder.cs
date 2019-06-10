using System;
using JG.FinTechTest.Domain.Requests;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation.Builders
{
    internal class RegisterDonationRequestBuilder
    {
        private RegisterDonationRequest _request;

        public RegisterDonationRequestBuilder()
        {
            _request = new RegisterDonationRequest
            {
                DonationAmount = 1,
                PostCode = "1234",
                FirstName = "FirstName",
                LastName = "LastName"
            };
        }

        public RegisterDonationRequestBuilder With(RegisterDonationRequest request)
        {
            _request = request;
            return this;
        }

        public RegisterDonationRequestBuilder With(Action<RegisterDonationRequest> props)
        {
            props?.Invoke(_request);

            return this;
        }

        public RegisterDonationRequestBuilder WithFirstName(string firstName)
        {
            _request.FirstName = firstName;
            return this;
        }

        public RegisterDonationRequestBuilder WithLastName(string lastName)
        {
            _request.LastName = lastName;
            return this;
        }

        public RegisterDonationRequestBuilder WithPostCode(string postCode)
        {
            _request.PostCode = postCode;
            return this;
        }

        public RegisterDonationRequest Build()
        {
            return _request;
        }
    }
}
