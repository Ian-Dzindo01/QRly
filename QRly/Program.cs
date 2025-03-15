using QRly.Encoder;
using QRly.Helpers;

string input = "222222222";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

foreach (var bitString in encodedData)
{
    Console.WriteLine(bitString);
}
