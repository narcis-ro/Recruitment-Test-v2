using JG.Infrastructure.Exceptions;

namespace JG.FinTechTest.Domain.Exceptions
{
    public class DonationException : DomainException
    {
        public const string DONATION_AMOUNT_DATA_KEY = "DonatioAmount";
        public const string DONATION_MIN_DATA_KEY = "DonatioMinAmount";
        public const string DONATION_MAX_DATA_KEY = "DonatioMaxAmount";

        public DonationException(int errorCode) : base(errorCode)
        {
        }

        public DonationException(int errorCode, string developerMessage) : base(errorCode, developerMessage)
        {
        }

        public static DonationException LessThanMinimum(decimal donationAmount, decimal minAmount)
        {
            return new DonationException((int) DomainErrorCodes.DonationLessThanMinimum, $"Donation amount '{donationAmount}' is less than min amount '{minAmount}'")
            {
                Data =
                {
                    [DONATION_AMOUNT_DATA_KEY] = donationAmount,
                    [DONATION_MIN_DATA_KEY] = minAmount,
                }
            };
        }

        public static DonationException ExceedsMaximum(decimal donationAmount, decimal? maxAmount)
        {
            return new DonationException((int)DomainErrorCodes.DonationLessThanMinimum, $"Donation amount '{donationAmount}' is greater than max amount '{maxAmount}'")
            {
                Data =
                {
                    [DONATION_AMOUNT_DATA_KEY] = donationAmount,
                    [DONATION_MAX_DATA_KEY] = maxAmount,
                }
            };
        }
    }
}
