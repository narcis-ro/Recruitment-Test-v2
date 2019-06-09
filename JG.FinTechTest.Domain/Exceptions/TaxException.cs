using JG.FinTechTest.Domain.TaxEngine;
using JG.FinTechTest.Domain.TaxProcessors;
using JG.Infrastructure.Exceptions;

namespace JG.FinTechTest.Domain.Exceptions
{
    public class TaxException: DomainException
    {
        public const string TAX_TYPE_DATA_KEY = "TaxType";

        private const string NoTaxesException = @"No taxes have been defined in appSettings.json. If you don't want to apply any tax, please use the " + nameof(NoTaxProcessor)+ " processor.\r\n" +
                                                "Example:" +
                                                "{ \"Name\": \"Default\",\r\n" +
                                                "  \"TaxType\": \"GiftAid\",\r\n" +
                                                "  \"ProcessorType\": \"NoTax\",\r\n" +
                                                "  \"FromDate\": \"0000-01-01 00:00\",\r\n" +
                                                "}";

        public TaxException(int errorCode) : base(errorCode)
        {
        }

        public TaxException(int errorCode, string developerMessage) : base(errorCode, developerMessage)
        {
        }

        public static TaxException NoTaxesDefined()
        {
            throw new DonationException((int) DomainErrorCodes.NoTaxesDefined, NoTaxesException);
        }

        public static TaxException NoTaxesDefinedForType(TaxType taxType)
        {
            throw new DonationException((int)DomainErrorCodes.NoTaxesDefined, $@"No taxes of type '{taxType}' have been defined in appSettings.json.")
            {
                Data =
                {
                    [TAX_TYPE_DATA_KEY] = taxType
                }
            };
        }
    }
}
