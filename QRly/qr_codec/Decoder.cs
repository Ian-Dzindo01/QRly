using System.Text;

namespace QRly.Decoder
{
    public static class Decoder
    {
        public static string DecodeNumeric(string input)
        {
            string result = "";
            int i = 0;

            while (i < input.Length)
            {
                int bitLength;
                int digitCount;

                if (input.Length - i >= 10)
                {
                    bitLength = 10;
                    digitCount = 3; // 3 digits in 10 bits
                }
                else if (input.Length - i >= 7)
                {
                    bitLength = 7;
                    digitCount = 2; // 2 digits in 7 bits
                }
                else
                {
                    bitLength = 4;
                    digitCount = 1; // 1 digit in 4 bits
                }

                string binChunk = input.Substring(i, bitLength);
                int number = Convert.ToInt32(binChunk, 2);

                // Preserve leading zeroes
                result += number.ToString().PadLeft(digitCount, '0'); // Preserve leading zeros

                i += bitLength;
            }

            return result;
        }


        public static string DecodeAlphanumeric(string bitString)
        {
            const string ALPHANUMERIC_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            StringBuilder result = new();

            for (int i = 0; i < bitString.Length;)
            {
                if (bitString.Length - i >= 11)
                {
                    int value = Convert.ToInt32(bitString.Substring(i, 11), 2);
                    result.Append(ALPHANUMERIC_CHARS[value / 45]);
                    result.Append(ALPHANUMERIC_CHARS[value % 45]);
                    i += 11;
                }
                else if (bitString.Length - i >= 6)
                {
                    int value = Convert.ToInt32(bitString.Substring(i, 6), 2);
                    result.Append(ALPHANUMERIC_CHARS[value]);
                    i += 6;
                }
                else
                {
                    break;
                }
            }

            return result.ToString();
        }


        public static string DecodeByte(string bitString)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < bitString.Length; i += 8)
            {
                string byteSegment = bitString.Substring(i, 8);
                int charCode = Convert.ToInt32(byteSegment, 2);
                result.Append((char)charCode);
            }

            return result.ToString();
        }
    }
}