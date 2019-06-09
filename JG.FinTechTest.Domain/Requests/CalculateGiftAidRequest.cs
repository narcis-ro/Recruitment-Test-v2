using MediatR;

namespace JG.FinTechTest.Domain.Requests
{
    public class CalculateGiftAidRequest : IRequest<CalculateGiftAidResponse>
    {
        public decimal DonationAmount { get; set; }
    }
}
