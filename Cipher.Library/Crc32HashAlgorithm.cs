using System;
using System.IO;
using System.Security.Cryptography;

namespace Cipher.Library
{
    public class Crc32HashAlgorithm : HashAlgorithm
    {
        private const uint DefaultPolynomial = 0xEDB88320u;
        private const uint DefaultSeed = 0xFFFFFFFFu;

        private uint _hash;
        private readonly uint _seed;
        private readonly uint[] _table;

        public Crc32HashAlgorithm()
            : this(DefaultPolynomial, DefaultSeed)
        {
        }

        public Crc32HashAlgorithm(uint polynomial, uint seed)
        {
            _table = InitializeTable(polynomial);
            _seed = seed;
            HashSizeValue = 32; // CRC32 is 32 bits
            Initialize();
        }

        public override void Initialize()
        {
            _hash = _seed;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < ibStart + cbSize; i++)
            {
                unchecked
                {
                    _hash = (_hash >> 8) ^ _table[array[i] ^ (_hash & 0xFF)];
                }
            }
        }

        protected override byte[] HashFinal()
        {
            unchecked
            {
                uint finalHash = ~_hash;
                byte[] result = BitConverter.GetBytes(finalHash);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(result);
                }
                return result;
            }
        }

        private static uint[] InitializeTable(uint polynomial)
        {
            var table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint entry = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ polynomial;
                    }
                    else
                    {
                        entry >>= 1;
                    }
                }
                table[i] = entry;
            }
            return table;
        }
    }
}
