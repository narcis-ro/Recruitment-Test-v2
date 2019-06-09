using JG.FinTechTest.Domain.Data.Model;
using MediatR;

namespace JG.FinTechTest.Domain.Requests
{
    public class FileGiftAidDeclarationRequest : IRequest<FileGiftAidDeclarationResponse>
    {
        public Donation Donation { get; set; }
    }
}