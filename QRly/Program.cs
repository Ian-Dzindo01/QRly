using QRly.Decoder;
using QRly.Encoder;
using QRly.Helpers;

string input = "https://wordcounter.net/character-count";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

//Console.WriteLine(input);
//Console.WriteLine(encodedData[2]);

int totalLength = encodedData.Sum(s => s.Length);
Console.WriteLine($"Total bit length: {totalLength}");

foreach (string bitString in encodedData)
{
    Console.WriteLine(bitString);
}

Console.WriteLine(Decoder.DecodeByte(encodedData[2]));