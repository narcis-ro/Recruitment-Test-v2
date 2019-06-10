using System;
using JG.FinTechTest.Api.Controllers;
using JG.FinTechTest.Domain.Requests;

namespace JG.FinTechTest.UnitTests.Controllers.Donation
{
    public class Arrangements
    {
        public DonationController Sut { get; set; }
        public RegisterDonationResponse RegisterDonationResponse { get; set; }
        public Func<RegisterDonationRequest> RegisterDonationRequestAccessor { get; set; }
        public RegisterDonationRequest RegisterDonationRequest => RegisterDonationRequestAccessor?.Invoke();
    }
}
