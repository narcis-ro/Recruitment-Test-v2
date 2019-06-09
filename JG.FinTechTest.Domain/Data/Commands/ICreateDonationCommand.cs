using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Model;
using JG.Infrastructure.Cqs;

namespace JG.FinTechTest.Domain.Data.Commands
{
    public interface ICreateDonationCommand : ICommand
    {
        Task Execute(Donation donation, CancellationToken cancellationToken = default);
    }
}
