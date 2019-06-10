using System;
using JG.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace JG.Infrastructure.Exceptions
{
    public static class ExUtils
    {
        public static void Safe(Action action, string log = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                GlobalLog.Logger?.LogError("Exceptions.Safe not safe: {ExceptionMessage}. Other info: {log}",
                    ex.Message, log);
            }
        }

        public static T Safe<T>(Func<T> action, string log = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                GlobalLog.Logger?.LogError("Exceptions.Safe not safe: {ExceptionMessage}. Other info: {log}",
                    ex.Message, log);
            }

            return default;
        }
    }
}
