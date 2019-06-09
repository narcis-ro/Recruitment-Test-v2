namespace JG.FinTechTest.Models
{
    /// <summary>
    /// Response containing Gift-Aid amount for a donation.
    /// </summary>
    public class GiftAidResponse
    {
        /// <summary>
        /// Gift-Aid amount for <see cref="DonationAmount"/>
        /// </summary>
        public decimal GiftAidAmount { get; set; }

        /// <summary>
        /// Donation Amount
        /// </summary>
        public decimal DonationAmount { get; set; }
    }
}
