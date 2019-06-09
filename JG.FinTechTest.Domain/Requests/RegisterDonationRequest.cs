using MediatR;

namespace JG.FinTechTest.Domain.Requests
{
    public class RegisterDonationRequest : IRequest<RegisterDonationResponse>
    {
        public decimal DonationAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostCode { get; set; }
    }
}