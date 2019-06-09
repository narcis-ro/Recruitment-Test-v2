using System;

namespace JG.FinTechTest.Models
{
    /// <summary>
    ///     Donation registration response
    /// </summary>
    public class DonationResponse
    {
        /// <summary>
        ///     Id of the newly registered donation
        /// </summary>
        public Guid DonationId { get; set; }

        /// <summary>
        ///     Gift-Aid declaration reference
        /// </summary>
        public string GiftAidReference { get; set; }

        /// <summary>
        ///     Gift-Aid reclaim value
        /// </summary>
        public decimal GiftAidAmount { get; set; }

        /// <summary>
        ///     Donated amount
        /// </summary>
        public decimal DonationAmount { get; set; }
    }
}
