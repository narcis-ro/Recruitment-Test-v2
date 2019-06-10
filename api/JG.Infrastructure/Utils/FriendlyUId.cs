using System;

namespace JG.Infrastructure.Utils
{
    public static class FriendlyUId
    {
        private const string Base62Chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string NewId(int length = 6)
        {
            return string.Create(6, new Random(), (chars, random) =>
            {
                for (var i = 0; i < chars.Length; i++)
                    chars[i] = Base62Chars[random.Next(chars.Length)];
            });
        }
    }
}
