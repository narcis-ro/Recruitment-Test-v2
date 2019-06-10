using System;
using JG.FinTechTest.Domain.Requests;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation
{
    public class Arrangements
    {
        public Domain.Handlers.DonationHandler Sut { get; set; }

        public CalculateGiftAidResponse CalculateGiftAidResponse { get; set; }

        public Func<CalculateGiftAidRequest> CalculateGiftAidRequestAccessor { get; set; }
        public CalculateGiftAidRequest CalculateGiftAidRequest => CalculateGiftAidRequestAccessor?.Invoke();

        public Func<FileGiftAidDeclarationRequest> FileGiftAidDeclarationRequestAccessor { get; set; }
        public FileGiftAidDeclarationRequest FileGiftAidDeclarationRequest => FileGiftAidDeclarationRequestAccessor?.Invoke();

        public Func<Data.Model.Donation> DonationAccessor { get; set; }
        public Data.Model.Donation Donation => DonationAccessor?.Invoke();
        public FileGiftAidDeclarationResponse FileGiftAidDeclarationResponse { get; set; }
    }
}
