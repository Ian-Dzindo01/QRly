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

    private void AddSeparators()
    {
        // Pattern positions 
        int[] positions = { 0, 26 };

        foreach (var pos in positions)
        {
            for (int i = 0; i < 8; i++)
            {
                if (pos + i < Size)
                {
                    matrix[pos + 7, i] = 0; // Bottom of finder
                    matrix[i, pos + 7] = 0; // Right of finder
                }
            }
        }

        // Bottom-left separator
        for (int i = 0; i < 8; i++)
        {
            matrix[26 + 7, i] = 0;
            matrix[i, 26 + 7] = 0;
        }
    }

    public void RenderQRCode(string filename)
    {
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
