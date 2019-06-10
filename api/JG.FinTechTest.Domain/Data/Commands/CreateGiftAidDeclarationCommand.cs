using System;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Data.Model;
using LiteDB;

namespace JG.FinTechTest.Domain.Data.Commands
{
    // ReSharper disable once UnusedMember.Global
    public class CreateGiftAidDeclarationCommand : ICreateGiftAidDeclarationCommand
    {
        private readonly LiteDatabase _db;

        public CreateGiftAidDeclarationCommand(LiteDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task Execute(GiftAidDeclaration giftAidDeclaration, CancellationToken cancellationToken = default)
        {
            if (giftAidDeclaration == null) throw new ArgumentNullException(nameof(giftAidDeclaration));

            var collection = _db.GetCollection<GiftAidDeclaration>("gift-aid-declarations");

            collection.Insert(giftAidDeclaration);

            return Task.CompletedTask;
        }
    }
}
