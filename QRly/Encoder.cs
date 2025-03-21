using QRly.Helpers;

namespace QRly.Encoder
{
    public static class Encoder
    {
        public static byte[] EncodeQRCodeData(string input, QRMode mode)
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

            List<string> fullBitStringParts = new List<string>
            {
                modeIndicator,
                charCountIndicator,
                encodedData
            };

            string bitString = string.Concat(PadBitString(fullBitStringParts, mode));

            // Convert the binary string into a byte array
            int byteCount = bitString.Length / 8;
            byte[] byteArray = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                string byteChunk = bitString.Substring(i * 8, 8); // Take 8 bits
                byteArray[i] = Convert.ToByte(byteChunk, 2); // Convert "11001010" to byte
            }

            return byteArray;
        }

        private static List<string> PadBitString(List<string> bitString, QRMode mode)
        {
            const int totalCapacity = 512; // Version 4, Level M = 64 codewords * 8 bits
            const string terminator = "0000";
            const string padByte1 = "11101100"; // 0xEC
            const string padByte2 = "00010001"; // 0x11

            List<string> paddedBitStrings = new List<string>();

            foreach (var part in bitString)
            {
                paddedBitStrings.Add(part);
            }

            int totalLength = paddedBitStrings.Sum(s => s.Length);
            if (totalLength + terminator.Length > totalCapacity)
            {
                paddedBitStrings.Add(terminator.Substring(0, totalCapacity - totalLength));
            }
            else
            {
                paddedBitStrings.Add(terminator);
            }

            int remainingBits = paddedBitStrings.Sum(s => s.Length) % 8;
            if (remainingBits > 0)
            {
                paddedBitStrings.Add(new string('0', 8 - remainingBits));
            }

            while (paddedBitStrings.Sum(s => s.Length) + 8 <= totalCapacity)
            {
                paddedBitStrings.Add(padByte1);

                if (paddedBitStrings.Sum(s => s.Length) + 8 <= totalCapacity)
                {
                    paddedBitStrings.Add(padByte2);
                }
            }

            return paddedBitStrings;
        }


        public static string EncodeNumeric(string input)
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

        public static string EncodeAlphanumeric(string input)
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

        public static string EncodeByte(string input)
        {
            return string.Concat(input.Select(c => Convert.ToString((int)c, 2).PadLeft(8, '0')));
        }

        public static string EncodeKanji(string input)
        {
            throw new NotImplementedException("Kanji encoding is not implemented yet.");
        }
    }
}