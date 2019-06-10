using System;
using JG.FinTechTest.Api.Models;

namespace JG.FinTechTest.UnitTests.Controllers.Donation.Builders
{
    internal class DonationRequestBuilder
    {
        private DonationRequest _request;

        public DonationRequestBuilder()
        {
            _request = new DonationRequest
            {
                Amount = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                PostCode = "12345"
            };
        }

        public DonationRequestBuilder With(DonationRequest request)
        {
            _request = request;
            return this;
        }

        public DonationRequestBuilder With(Action<DonationRequest> props)
        {
            props?.Invoke(_request);

            return this;
        }

        public DonationRequest Build()
        {
            return _request;
        }
    }
}
