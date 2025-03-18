namespace QRly.Tests.Generators
{
    internal static class RandomGenerator
    {
        private static readonly Random random = new Random();

        public static string GenerateRandomNumericString(int length)
        {
            char[] digits = new char[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = (char)('0' + random.Next(10));
            }
            return new string(digits);
        }

        public static string GenerateRandomAlphanumericString(int length)
        {
            const string ALPHANUMERIC_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = ALPHANUMERIC_CHARS[random.Next(ALPHANUMERIC_CHARS.Length)];
            }
            return new string(chars);
        }

        public static string GenerateRandomByteString(int length)
        {
            byte[] bytes = new byte[length];
            random.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}

