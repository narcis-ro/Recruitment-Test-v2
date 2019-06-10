using System;
using JG.FinTechTest.Domain.Requests;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation.Builders
{
    internal class CalculateGiftAidResponseBuilder
    {
        private CalculateGiftAidResponse _response;

        public CalculateGiftAidResponseBuilder()
        {
            _response = new CalculateGiftAidResponse
            {
                DonationAmount = 1,
                GiftAidAmount = 0.5m
            };
        }

        public CalculateGiftAidResponseBuilder With(CalculateGiftAidResponse response)
        {
            _response = response;

            return this;
        }

        public CalculateGiftAidResponseBuilder With(Action<CalculateGiftAidResponse> props)
        {
            props?.Invoke(_response);

            return this;
        }

        public CalculateGiftAidResponse Build()
        {
            return _response;
        }
    }
}
