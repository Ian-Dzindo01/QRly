using QRly.Encoder;
using QRly.Helpers;

string input = "https://wordcounter.net/character-count";
QRMode mode = QRHelper.DetermineMode(input);
byte[] encodedData = Encoder.EncodeQRCodeData(input, mode);
byte[] finalCodewords = ReedSolomon.EncodeVersion4M(encodedData);
Console.WriteLine("Final QR Code Data: " + BitConverter.ToString(finalCodewords));