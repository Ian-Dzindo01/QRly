using QRly.Helpers;

namespace QRly.Encoder
{
    public static class Encoder
    {
        public static string EncodeQRCodeData(string input, QRMode mode)
        {
            string modeIndicator = mode switch
            {
                QRMode.Numeric => "0001",
                QRMode.Alphanumeric => "0010",
                QRMode.Byte => "0100",
                QRMode.Kanji => "1000",
                _ => throw new ArgumentException("Unsupported mode")
            };

            string charCountIndicator = QRHelper.GetCharacterCountIndicator(input, mode);

            string encodedData = mode switch
            {
                QRMode.Numeric => EncodeNumeric(input),
                QRMode.Alphanumeric => EncodeAlphanumeric(input),
                QRMode.Byte => EncodeByte(input),
                QRMode.Kanji => EncodeKanji(input),
                _ => throw new ArgumentException("Unsupported mode")
            };

            return modeIndicator + charCountIndicator + encodedData;
        }

        private static string EncodeNumeric(string input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i += 3)
            {
                int length = Math.Min(3, input.Length - i);
                int number = int.Parse(input.Substring(i, length));
                // 3 digits - 10 bits, 2 - 7, 1 - 4
                int bitLength = (length == 3) ? 10 : (length == 2) ? 7 : 4;
                result += Convert.ToString(number, 2).PadLeft(bitLength, '0');
            }
            return result;
        }

        private static string EncodeAlphanumeric(string input)
        {
            const string ALPHANUMERIC_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            string result = "";

            for (int i = 0; i < input.Length; i += 2)
            {
                int first = ALPHANUMERIC_CHARS.IndexOf(input[i]);
                if (i + 1 < input.Length)
                {
                    int second = ALPHANUMERIC_CHARS.IndexOf(input[i + 1]);
                    int value = first * 45 + second;
                    result += Convert.ToString(value, 2).PadLeft(11, '0');
                }
                else
                {
                    result += Convert.ToString(first, 2).PadLeft(6, '0');
                }
            }
            return result;
        }
    }
}
