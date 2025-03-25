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

        PlaceDataBits(encodedData);

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

        PlacePattern(pattern, 26, 26);
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
        matrix[26, 7] = 1;
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
        for (int i = 8; i < Size - 7; i++)
        {
            matrix[6, i - 1] = i % 2;
            matrix[i - 1, 6] = i % 2;
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
        string filename = "qr_output.png";
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

        bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
    }

    private void PlaceDataBits(byte[] data)
    {
        int bitIndex = 0; // Tracks the current bit in the data stream
        int dataLength = data.Length * 8; // Total number of bits
        int x = Size - 1; // Start from bottom-right corner
        int y = Size - 1;
        bool goingUp = true; // Direction flag

        while (x > 0)
        {
            if (x == 6) x--; // Skip the vertical timing pattern column

            for (int i = 0; i < Size; i++)
            {
                int currentY = goingUp ? (Size - 1 - i) : i; // Zigzag pattern

                // Place bit in two adjacent columns (right to left)
                for (int j = 0; j < 2; j++)
                {
                    int currentX = x - j;

                    // Skip reserved areas (finder patterns, format areas, etc.)
                    if (IsReserved(currentX, currentY))
                        continue;

                    if (bitIndex < dataLength)
                    {
                        int bit = (data[bitIndex / 8] >> (7 - (bitIndex % 8))) & 1;
                        matrix[currentY, currentX] = bit;
                        bitIndex++;
                    }
                }
            }

            x -= 2; // Move to the next column pair
            goingUp = !goingUp; // Reverse direction
        }
    }
    private bool IsReserved(int x, int y)
    {
        // Finder patterns and their separators (top-left, top-right, bottom-left)
        if ((x < 9 && y < 9) || (x >= Size - 8 && y < 9) || (x < 9 && y >= Size - 8))
            return true;

        // Timing patterns (horizontal and vertical)
        if (x == 6 || y == 6)
            return true;

        // Alignment pattern (centered at 26,26 for Version 4)
        if ((x >= 24 && x <= 28) && (y >= 24 && y <= 28))
            return true;

        // Format information areas
        if ((y == 8 && x < 9) || (x == 8 && y < 9) || (x == 8 && y >= Size - 8) || (y == 8 && x >= Size - 8))
            return true;

        return false; // Not reserved, can place data
    }

}
