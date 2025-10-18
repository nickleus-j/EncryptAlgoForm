using Blake3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cipher.Library
{
    public class Blake3HashAlgo : HashAlgorithm
    {
        private Hasher _blake3Hasher;

        public Blake3HashAlgo()
        {
            // Set the desired hash size (e.g., 32 bytes for a 256-bit hash)
            HashSizeValue = 256;
            _blake3Hasher = Hasher.New(); // Initialize a new BLAKE3 hasher
        }

        public override void Initialize()
        {
            _blake3Hasher = Hasher.New(); // Reset the hasher
        }

        protected override void HashCore(byte[] array, int offset, int count)
        {
            _blake3Hasher.Update(array.AsSpan(offset, count)); // Feed data to BLAKE3
        }

        protected override byte[] HashFinal()
        {
            return _blake3Hasher.Finalize().AsSpan().ToArray(); // Get the final hash
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _blake3Hasher.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
