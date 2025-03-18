using QRly.Decoder;
using QRly.Encoder;
using QRly.Helpers;

string input = "48101686512";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

Console.WriteLine(encodedData[2]);
Console.WriteLine(Decoder.DecodeNumeric(encodedData[2]));


//foreach (string bitString in encodedData)
//{
//    Console.WriteLine(bitString);
//}