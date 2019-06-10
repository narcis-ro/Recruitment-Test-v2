using System;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Model;
using LiteDB;

namespace JG.FinTechTest.Domain.Data.Commands
{
    // ReSharper disable once UnusedMember.Global
    public class CreateDonationCommand : ICreateDonationCommand
    {
        private readonly LiteDatabase _db;

        public CreateDonationCommand(LiteDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public Task Execute(Donation donation, CancellationToken cancellationToken = default)
        {
            if (donation == null) throw new ArgumentNullException(nameof(donation));

            var collection = _db.GetCollection<Donation>("donations");

            collection.Insert(donation);

            return Task.CompletedTask;
        }
    }
}
