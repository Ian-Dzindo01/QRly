using System.Text;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


foreach (var encoding in Encoding.GetEncodings())
{
    Console.WriteLine(encoding.Name);
}