using System;

namespace JG.Infrastructure.Exceptions
{
    public class AppException : Exception
    {
        public int ErrorCodeId { get; set; }

        public string DeveloperMessage { get; set; }
    }
}
