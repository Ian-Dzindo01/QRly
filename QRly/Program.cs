using QRly.Encoder;
using QRly.Helpers;

string input = "HELLO CC WORLD";
QRMode mode = QRMode.Byte;
string encodedData = Encoder.EncodeQRCodeData(input, mode);
Console.WriteLine(encodedData.Length);
Console.WriteLine(encodedData);