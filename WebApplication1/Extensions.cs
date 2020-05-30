using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class Extensions
    {
        public static byte[] GetSHA256(this string str) => new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(str));
    }

    public static class RandomExtension
    {
        public static readonly char[] EnglishLower = new[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        public static readonly char[] EnglishUpper = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'H', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        public static readonly char[] Numbers = new[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        public static readonly char[] EnglishLowerUpper;
        public static readonly char[] EnglishLowerUpperNumbers;

        static RandomExtension()
        {
            EnglishLowerUpper = EnglishLower.Union(EnglishUpper).ToArray();
            EnglishLowerUpperNumbers = EnglishLowerUpper.Union(Numbers).ToArray();
        }

        public static string NextString(this Random random, int length, char[] alphabet = null)
        {
            if (length < 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder(length);
            if (alphabet == null)
                alphabet = EnglishLower;
            for (int i = 0; i < length; i++)
                builder.Append(alphabet[random.Next(alphabet.Length)]);
            return builder.ToString();
        }

        public static uint Next(this Random random, uint max)
        {
            return (uint)random.Next(int.MinValue, int.MaxValue) % max;
        }
    }
}
