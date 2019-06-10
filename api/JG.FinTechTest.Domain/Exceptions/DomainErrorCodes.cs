namespace JG.FinTechTest.Domain.Exceptions
{
    public enum DomainErrorCodes
    {
        Invalid = 0,
        DonationLessThanMinimum = 1,
        DonationExceedsMaximum = 2,
        NoTaxesDefined = 3,
        InvalidDonorName = 4,
        InvalidPostcode = 5,
    }
}
