using QRly.Decoder;
using QRly.Encoder;
using QRly.Tests.Generators;

public class EncodingTests
{
    private static readonly Random random = new Random();

    [Theory]
    [InlineData("123456")]
    [InlineData("987654321")]
    [InlineData("1")]
    [InlineData("2223333")]
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
        for (int i = 0; i < 30; i++)
        {
            string input = RandomGenerator.GenerateRandomNumericString(random.Next(1, 20));
            string encoded = Encoder.EncodeNumeric(input);
            string decoded = Decoder.DecodeNumeric(encoded);

            Assert.Equal(input, decoded);
        }
    }

    [Theory]
    [InlineData("HELLO123")]
    [InlineData("QRLY$TEST")]
    [InlineData("C++RULES")]
    [InlineData("42*X9Y")]
    [InlineData("ABCD")]
    public void EncodeDecode_Alphanumeric_FixedCases(string input)
    {
        string encoded = Encoder.EncodeAlphanumeric(input);
        string decoded = Decoder.DecodeAlphanumeric(encoded);

        Assert.Equal(input, decoded);
    }

    [Fact]
    public void EncodeDecode_Alphanumeric_RandomCases()
    {
        for (int i = 0; i < 30; i++)
        {
            string input = RandomGenerator.GenerateRandomAlphanumericString(random.Next(1, 20));
            string encoded = Encoder.EncodeAlphanumeric(input);
            string decoded = Decoder.DecodeAlphanumeric(encoded);

            Assert.Equal(input, decoded);
        }
    }

    [Theory]
    [InlineData("Hello, World!")]
    [InlineData("QRly")]
    [InlineData("1234567890")]
    [InlineData("Test123")]
    [InlineData("byte\x00\x01\x02\x03")]
    public void EncodeDecode_Byte_FixedCases(string input)
    {
        string encoded = Encoder.EncodeByte(input);
        string decoded = Decoder.DecodeByte(encoded);

        Assert.Equal(input, decoded);
    }

    [Fact]
    public void EncodeDecode_Byte_RandomCases()
    {
        for (int i = 0; i < 30; i++)
        {
            string input = RandomGenerator.GenerateRandomByteString(random.Next(1, 20));
            string encoded = Encoder.EncodeByte(input);
            string decoded = Decoder.DecodeByte(encoded);

            Assert.Equal(input, decoded);
        }
    }
}