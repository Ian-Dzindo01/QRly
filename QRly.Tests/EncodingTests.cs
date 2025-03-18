using QRly.Decoder;
using QRly.Encoder;

public class EncodingTests
{
    private static readonly Random random = new Random();

    private static string GenerateRandomNumericString(int length)
    {
        char[] digits = new char[length];
        for (int i = 0; i < length; i++)
        {
            digits[i] = (char)('0' + random.Next(10));
        }
        return new string(digits);
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("987654321")]
    [InlineData("000000")]
    [InlineData("1")]
    [InlineData("99")]
    public void EncodeDecode_Numeric_FixedCases(string input)
    {
        string encoded = Encoder.EncodeNumeric(input);
        string decoded = Decoder.DecodeNumeric(encoded);

        Assert.Equal(input, decoded);
    }

    [Fact]
    public void EncodeDecode_Numeric_RandomCases()
    {
        for (int i = 0; i < 10; i++)
        {
            string input = GenerateRandomNumericString(random.Next(1, 20));
            string encoded = Encoder.EncodeNumeric(input);
            string decoded = Decoder.DecodeNumeric(encoded);

            Assert.Equal(input, decoded);
        }
    }
}