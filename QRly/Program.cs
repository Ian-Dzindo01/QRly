using QRly.Decoder;
using QRly.Encoder;
using QRly.Helpers;

string input = "C#RULES";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

Console.WriteLine(input);
Console.WriteLine(encodedData[2]);
Console.WriteLine(Decoder.DecodeAlphanumeric(encodedData[2]));


//foreach (string bitString in encodedData)
//{
//    Console.WriteLine(bitString);
//}