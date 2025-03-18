using QRly.Encoder;
using QRly.Helpers;

string input = "SAMIRMANGA #2234";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

//Console.WriteLine(encodedData[2]);
//Console.WriteLine(Decoder.DecodeByte(encodedData[2]));


//foreach (string bitString in encodedData)
//{
//    Console.WriteLine(bitString);
//}