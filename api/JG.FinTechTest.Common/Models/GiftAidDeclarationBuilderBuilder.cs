using System;
using JG.FinTechTest.Domain.Data.Model;

namespace JG.FinTechTest.Common.Models
{
    internal class GiftAidDeclarationBuilderBuilder
    {
        private GiftAidDeclaration _model;

        public GiftAidDeclarationBuilderBuilder()
        {
            var donation = new DonationModelBuilder().Build();
            _model = new GiftAidDeclaration
            {
                GiftAidAmount = donation.GiftAidAmount,
                DonationAmount = donation.DonationAmount,
                Created = DateTimeOffset.Now,
                DonationId = donation.Id,
                DonorDetails = donation.DonorDetails,
                Id = Guid.NewGuid(),
                GiftAidReference = donation.GiftAidReference
            };
        }

        public GiftAidDeclarationBuilderBuilder With(GiftAidDeclaration model)
        {
            _model = model;
            return this;
        }

        public GiftAidDeclarationBuilderBuilder With(Action<GiftAidDeclaration> props)
        {
            props?.Invoke(_model);

            return this;
        }

        public GiftAidDeclaration Build()
        {
            return _model;
        }
    }
}
