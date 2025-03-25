using QRly.Encoder;

string input = "https://wordcounter.net/character-count";
byte[] hexString = Encoder.GenerateQRCodeBytes(input);

QRMatrix matrix = new QRMatrix();
matrix.GenerateQRCode(input);
//Console.WriteLine("Final QR Code Data: " + hexString);
