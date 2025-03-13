using QRly.Helpers;

Console.WriteLine(QRHelper.GetCharacterCountIndicator("123456", QRMode.Numeric));
Console.WriteLine(QRHelper.GetCharacterCountIndicator("HELLO1", QRMode.Alphanumeric));
Console.WriteLine(QRHelper.GetCharacterCountIndicator("Hello!", QRMode.Byte));
