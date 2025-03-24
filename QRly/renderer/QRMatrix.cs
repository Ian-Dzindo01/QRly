using QRly.Encoder;
using System.Drawing;

public class QRMatrix
{
    private const int Size = 33;
    private int[,] matrix = new int[Size, Size];

    public QRMatrix()
    {
        InitializeMatrix();
    }

    private void InitializeMatrix()
    {
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                matrix[y, x] = 0;
            }
        }
    }

    public void GenerateQRCode(string input)
    {
        AddFinderPatterns();
        AddSeparators();
        AddAlignmentPatterns();
        AddTimingPatterns();
        AddDarkModule();
        ReserveFormatInformation();

        byte[] encodedData = Encoder.EncodePayload(input);

        //PlaceDataBits(encodedData);

        //ApplyMasking();

        RenderQRCode();

        Console.WriteLine($"QR Code generated.");
    }

    private void AddSeparators()
    {
        int[] positions = { 0, 26 };

        foreach (var pos in positions)
        {
            for (int i = 0; i < 8; i++)
            {
                if (pos + 7 < Size)
                {
                    matrix[pos + 7, i] = 0;
                    matrix[i, pos + 7] = 0;
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (26 + 7 < Size)
            {
                matrix[26 + 7, i] = 0;
                matrix[i, 26 + 7] = 0;
            }
        }
    }


    private void AddAlignmentPatterns()
    {
        int[,] pattern =
        {
        {1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 1, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1},
    };

        PlacePattern(pattern, 6, 22);
        PlacePattern(pattern, 22, 6);
    }

    private void AddFinderPatterns()
    {
        int[,] pattern =
        {
        {1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 1, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1},
        {1, 0, 1, 1, 1, 0, 1},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1},
    };

        PlacePattern(pattern, 0, 0); // top-left
        PlacePattern(pattern, 0, 26); // top-right
        PlacePattern(pattern, 26, 0); // bottom-left
    }

    private void AddDarkModule()
    {
        matrix[8, 27] = 1;
    }

    private void ReserveFormatInformation()
    {
        for (int i = 0; i < 9; i++)
        {
            if (i != 6) matrix[8, i] = -1; // top-left horizontal
            if (i != 6) matrix[i, 8] = -1; // top-left vertical
        }

        for (int i = 0; i < 8; i++)
        {
            matrix[8, 33 - 1 - i] = -1;
            matrix[33 - 1 - i, 8] = -1;
        }
    }

    private void AddTimingPatterns()
    {
        for (int i = 8; i < Size - 8; i++)
        {
            matrix[6, i] = i % 2;
            matrix[i, 6] = i % 2;
        }
    }

    private void PlacePattern(int[,] pattern, int startX, int startY)
    {
        for (int y = 0; y < pattern.GetLength(0); y++)
        {
            for (int x = 0; x < pattern.GetLength(1); x++)
            {
                matrix[startY + y, startX + x] = pattern[y, x];
            }
        }
    }

    public void RenderQRCode()
    {
        string filename = "qr_output.bmp";
        int scale = 10;
        using Bitmap bmp = new Bitmap(Size * scale, Size * scale);
        using Graphics g = Graphics.FromImage(bmp);

        g.Clear(Color.White);

        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                if (matrix[y, x] == 1)
                {
                    g.FillRectangle(Brushes.Black, x * scale, y * scale, scale, scale);
                }
            }
        }

        bmp.Save(filename);
    }
}
