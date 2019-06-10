namespace JG.Infrastructure.Exceptions
{
    public class DomainException : AppException
    {
        public DomainException(int errorCode) : this(errorCode, null)
        {
        }

        public DomainException(int errorCode, string developerMessage)
        {
            ErrorCodeId = errorCode;
            DeveloperMessage = developerMessage;
        }
    }
}
