using System.Text;

namespace QRly.Helpers
{
    public enum QRMode
    {
        Numeric,
        Alphanumeric,
        Byte,
        Kanji
    }

    public static class QRHelper
    {
        public static QRMode DetermineMode(string input)
        {
            if (input.All(c => c >= '0' && c <= '9'))
                return QRMode.Numeric;

            const string AlphanumericSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            if (input.All(c => AlphanumericSet.Contains(c)))
                return QRMode.Alphanumeric;

            if (IsKanji(input))
                return QRMode.Kanji;

            return QRMode.Byte;
        }

        public static string GetCharacterCountIndicator(string input, QRMode mode)
        {
            int charCount = input.Length;
            int bitLength = mode switch
            {
                QRMode.Numeric => 10,
                QRMode.Alphanumeric => 9,
                QRMode.Byte => 8,
                QRMode.Kanji => 8,
                _ => throw new ArgumentException("Unsupported mode")
            };

            string binary = Convert.ToString(charCount, 2);
            return binary.PadLeft(bitLength, '0');
        }

        private static bool IsKanji(string input)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] bytes = Encoding.GetEncoding("shift-jis").GetBytes(input);
            if (bytes.Length % 2 != 0) return false; // double byte

            for (int i = 0; i < bytes.Length; i += 2)
            {
                int combined = bytes[i] << 8 | bytes[i + 1];
                // In valid range for Shift JIS Kanji
                if (combined < 0x8140 || combined > 0xEBBF)
                    return false;
            }
            return true;
        }
    }
}
