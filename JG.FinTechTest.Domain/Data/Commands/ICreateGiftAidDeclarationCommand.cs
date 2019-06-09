using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Model;
using JG.Infrastructure.Cqs;

namespace JG.FinTechTest.Domain.Data.Commands
{
    public interface ICreateGiftAidDeclarationCommand : ICommand
    {
        Task Execute(GiftAidDeclaration giftAidDeclaration, CancellationToken cancellationToken = default);
    }
}
