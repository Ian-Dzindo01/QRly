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
                if (input.Length - i >= 10)
                    bitLength = 10;
                else if (input.Length - i >= 7)
                    bitLength = 7;
                else
                    bitLength = 4;

                string binChunk = input.Substring(i, bitLength);
                int number = Convert.ToInt32(binChunk, 2);

                result += number.ToString();

                i += bitLength;
            }

            return result;
        }

        public static string DecodeAlphanumeric(string bitString)
        {
            const string ALPHANUMERIC_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            string result = "";

            for (int i = 0; i < bitString.Length;)
            {
                if (bitString.Length - i >= 11)
                {
                    int value = Convert.ToInt32(bitString.Substring(i, 11), 2);
                    int first = value / 45;
                    int second = value % 45;
                    result += ALPHANUMERIC_CHARS[first].ToString() + ALPHANUMERIC_CHARS[second].ToString();
                    i += 11;
                }
                else if (bitString.Length - i >= 6)
                {
                    int value = Convert.ToInt32(bitString.Substring(i, 6), 2);
                    result += ALPHANUMERIC_CHARS[value].ToString();
                    i += 6;
                }
            }

            return result;
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