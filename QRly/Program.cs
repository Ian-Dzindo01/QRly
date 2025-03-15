using QRly.Encoder;
using QRly.Helpers;

string input = "222333";
QRMode mode = QRHelper.DetermineMode(input);
List<string> encodedData = Encoder.EncodeQRCodeData(input, mode);

Console.WriteLine(encodedData[2]);
Console.WriteLine(DecodeNumeric(encodedData[2]));


static string DecodeNumeric(string input)
{
    string result = "";
    int i = 0;

    while (i < input.Length)
    {
        // Determine the length of the binary chunk
        int bitLength;
        if (input.Length - i >= 10)
            bitLength = 10;  // 3 digits encoded in 10 bits
        else if (input.Length - i >= 7)
            bitLength = 7;   // 2 digits encoded in 7 bits
        else
            bitLength = 4;   // 1 digit encoded in 4 bits

        // Get the binary chunk and convert it to a number
        string binChunk = input.Substring(i, bitLength);
        int number = Convert.ToInt32(binChunk, 2);

        // Add the decoded number to the result
        result += number.ToString();

        // Move to the next chunk
        i += bitLength;
    }

    return result;
}
