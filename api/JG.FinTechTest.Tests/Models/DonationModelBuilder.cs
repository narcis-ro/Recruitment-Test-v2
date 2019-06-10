using System;
using JG.FinTechTest.Domain.Data.Model;

namespace JG.FinTechTest.UnitTests.Models
{
    internal class DonationModelBuilder
    {
        private Donation _donation;

        public DonationModelBuilder()
        {
            _donation = new Donation
            {
                Id = Guid.NewGuid(),
                DonationAmount = 1,
                GiftAidAmount = 0.5m,
                GiftAidReference = "12344",
                DonorDetails = new DonorDetails
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    PostCode = "12345"
                }
            };
        }

        public DonationModelBuilder With(Donation request)
        {
            _donation = request;
            return this;
        }

        public DonationModelBuilder With(Action<Donation> props)
        {
            props?.Invoke(_donation);

            return this;
        }

        public Donation Build()
        {
            return _donation;
        }
    }
}
