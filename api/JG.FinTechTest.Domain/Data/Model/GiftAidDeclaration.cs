using System;

namespace JG.FinTechTest.Domain.Data.Model
{
    public class GiftAidDeclaration
    {
        public Guid Id { get; set; }
        public decimal GiftAidAmount { get; set; }
        public decimal DonationAmount { get; set; }
        public string GiftAidReference { get; set; }
        public DonorDetails DonorDetails { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid DonationId { get; set; }
    }
}
