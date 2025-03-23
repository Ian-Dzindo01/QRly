using QRly.utils;

public class QRCodeModeTests
{
    [Theory]
    [InlineData("123456", QRMode.Numeric)]
    [InlineData("HELLO123", QRMode.Alphanumeric)]
    [InlineData("HELLO-WORLD*", QRMode.Alphanumeric)]
    [InlineData("hello123", QRMode.Byte)]
    [InlineData("こんにちは", QRMode.Kanji)]
    [InlineData("你好", QRMode.Byte)]
    [InlineData("💖", QRMode.Byte)]

    public void TestDetermineMode(string input, QRMode expected)
    {
        Assert.Equal(expected, QRHelper.DetermineMode(input));
    }
}
