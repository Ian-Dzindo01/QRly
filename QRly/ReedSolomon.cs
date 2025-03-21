namespace QRly.Encoder;

public class ReedSolomon
{
    private static readonly byte[] GF256_EXP = new byte[512]; // GF(256) exponent table
    private static readonly byte[] GF256_LOG = new byte[256]; // GF(256) log table
    private static readonly byte[] GENERATOR_POLY = { // Generator polynomial for 18 EC codewords
    1, 206, 134, 67, 149, 61, 32, 68, 46, 94, 190, 91, 102, 163, 94, 102, 37, 26, 1
};

    static ReedSolomon()
    {
        // Generate lookup table
        // Build GF(256) tables using polynomial 0x11D (285 in decimal)
        int x = 1;
        for (int i = 0; i < 255; i++)
        {
            GF256_EXP[i] = (byte)x;
            GF256_LOG[x] = (byte)i;
            x <<= 1;
            if (x >= 256) x ^= 0x11D; // Reduce modulo 285
        }
        for (int i = 255; i < 512; i++) GF256_EXP[i] = GF256_EXP[i - 255];
    }

    public static byte[] ComputeECC(byte[] data, int ecCodewords)
    {
        int dataLength = data.Length;
        byte[] ecc = new byte[ecCodewords];

        for (int i = 0; i < dataLength; i++)
        {
            byte feedback = (byte)(data[i] ^ ecc[0]);
            for (int j = 0; j < ecCodewords - 1; j++)
            {
                ecc[j] = (byte)(ecc[j + 1] ^ GaloisMultiply(feedback, GENERATOR_POLY[j + 1]));
            }
            ecc[ecCodewords - 1] = GaloisMultiply(feedback, GENERATOR_POLY[ecCodewords]);
        }

        return ecc;
    }

    private static byte GaloisMultiply(byte a, byte b)
    {
        return (a == 0 || b == 0) ? (byte)0 : GF256_EXP[GF256_LOG[a] + GF256_LOG[b]];
    }

    public static byte[] EncodeVersion4M(byte[] data)
    {
        int blockSize = 32;
        int ecCodewords = 18;

        byte[] block1 = new byte[blockSize];
        byte[] block2 = new byte[blockSize];

        Array.Copy(data, 0, block1, 0, blockSize);
        Array.Copy(data, blockSize, block2, 0, blockSize);

        byte[] ecc1 = ReedSolomon.ComputeECC(block1, ecCodewords);
        byte[] ecc2 = ReedSolomon.ComputeECC(block2, ecCodewords);

        byte[] finalCodewords = new byte[data.Length + 2 * ecCodewords];
        int index = 0;

        for (int i = 0; i < blockSize; i++)
        {
            finalCodewords[index++] = block1[i];
            finalCodewords[index++] = block2[i];
        }

        for (int i = 0; i < ecCodewords; i++)
        {
            finalCodewords[index++] = ecc1[i];
            finalCodewords[index++] = ecc2[i];
        }

        return finalCodewords;
    }
}
