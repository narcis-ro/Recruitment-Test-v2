using System.Collections.Generic;

namespace JG.FinTechTest.Domain.Config
{
    public class DonationConfig
    {
        public decimal MinDonationAmount { get; set; }

        public decimal? MaxDonationAmount { get; set; }

        public List<TaxConfig> Taxes { get; set; }
    }
}
