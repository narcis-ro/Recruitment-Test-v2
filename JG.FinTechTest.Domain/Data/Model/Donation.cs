using System;

namespace JG.FinTechTest.Domain.Data.Model
{
    public class Donation
    {
        public Guid Id { get; set; }
        public decimal DonationAmount { get; set; }
        public decimal GiftAidAmount { get; set; }
        public DonorDetails DonorDetails { get; set; }
        public string GiftAidReference { get; set; }
    }
}