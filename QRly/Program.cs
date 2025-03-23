using QRly.Encoder;

string input = "https://wordcounter.net/character-count";

byte[] hexString = Encoder.GenerateQRCodeBytes(input);

Console.WriteLine("Final QR Code Data: " + BitConverter.ToString(hexString));