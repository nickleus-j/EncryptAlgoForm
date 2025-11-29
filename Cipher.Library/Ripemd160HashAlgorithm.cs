using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Cipher.Library
{
    public class Ripemd160HashAlgorithm : HashAlgorithm
    {
        private readonly RipeMD160Digest _digest;
        private MemoryStream _buffer;

        public Ripemd160HashAlgorithm()
        {
            _digest = new RipeMD160Digest();
            _buffer = new MemoryStream();
            HashSizeValue = 160; // RIPEMD-160 produces 160-bit (20-byte) hashes
        }

        public override void Initialize()
        {
            _buffer.SetLength(0);
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _buffer.Write(array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            var data = _buffer.ToArray();
            _digest.BlockUpdate(data, 0, data.Length);

            byte[] result = new byte[_digest.GetDigestSize()];
            _digest.DoFinal(result, 0);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buffer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
