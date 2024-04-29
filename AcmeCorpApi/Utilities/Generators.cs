using System;
using System.Security.Cryptography;

namespace AcmeCorpApi.Utils.Generators
{
    public class StringGenerator
    {
        public string GenerateString(int length, int seed = 0)
        {
            Random r;
            if (seed == 0)
                r = new Random();
            else
                r = new Random(seed);

            char[] pool = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
                            'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
                            '4', '5', '6', '7', '8', '9'};
            string name = null;
            for (int i = 0; i < length; i++) 
            { 
                name += pool[r.Next(0, pool.Length - 1)]; 
            }
            return name ?? ("Abstract" + r.Next(0, 8192).ToString());
        }
    }

    public class SecureRandom
    {
        private const int BufferSize = 1024;  // Must be a multiple of 4.
        private byte[] RandomBuffer;
        private int BufferOffset;
        private RandomNumberGenerator rng;

        public SecureRandom()
        {
            RandomBuffer = new byte[BufferSize];
            rng = RandomNumberGenerator.Create();
            BufferOffset = RandomBuffer.Length;
        }

        private void FillBuffer()
        {
            rng.GetBytes(RandomBuffer);
            BufferOffset = 0;
        }

        public int Next()
        {
            if (BufferOffset >= RandomBuffer.Length)
            {
                FillBuffer();
            }
            int val = BitConverter.ToInt32(RandomBuffer, BufferOffset) & 0x7fffffff;
            BufferOffset += sizeof(int);
            return val;
        }

        public int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("maxValue must be greater than or equal to minValue");
            }
            int range = maxValue - minValue;
            return minValue + Next(range);
        }

        public double NextDouble()
        {
            int val = Next();
            return (double)val / int.MaxValue;
        }

        public void GetBytes(byte[] buff)
        {
            rng.GetBytes(buff);
        }

        public byte[] GetBytes(int bufferLength)
        {
            byte[] data = new byte[bufferLength];
            rng.GetBytes(data);
            return data;
        }
    }
}