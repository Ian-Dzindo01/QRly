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

            string fullBitString =
                modeIndicator + " " +
                charCountIndicator + " " +
                encodedData + " ";

            return PadBitString(fullBitString, mode);
        }

        private static string PadBitString(string bitString, QRMode mode)
        {

            // Version 4, level M = 512 bits
            int totalCapacity = 512; // 64 codewords * 8 bits
            string terminator = "0000 ";

            bitString += terminator;

            // Ensure its a multiple of 8
            int currentBitLength = bitString.Length;
            int remainingBits = currentBitLength % 8;
            if (remainingBits > 0)
            {
                int paddingBits = 8 - remainingBits;
                bitString += " ";
                bitString = bitString.PadRight(currentBitLength + paddingBits, '0');
            }

            int finalLength = bitString.Length;

            // Alternating padding
            if (finalLength < totalCapacity)
            {
                while (finalLength < totalCapacity)
                {
                    bitString += " 11101100";
                    finalLength += 8;
                    if (finalLength < totalCapacity)
                    {
                        bitString += " 00010001";
                        finalLength += 8;
                    }
                }
            }

            return bitString.Substring(0, totalCapacity);
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

        private static string EncodeByte(string input)
        {
            return string.Concat(input.Select(c => Convert.ToString((int)c, 2).PadLeft(8, '0')));
        }

        private static string EncodeKanji(string input)
        {
            throw new NotImplementedException("Kanji encoding is not implemented yet.");
        }

    }
}
