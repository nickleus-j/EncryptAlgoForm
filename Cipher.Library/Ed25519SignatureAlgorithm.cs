using System;
using System.IO;
using System.Security.Cryptography;
using NSec.Cryptography;
using HashAlgorithm = System.Security.Cryptography.HashAlgorithm;
namespace Cipher.Library
{
    public class Ed25519SignatureAlgorithm : HashAlgorithm
    {
        private MemoryStream _buffer;
        private Key _privateKey;
        private PublicKey _publicKey;

        public Ed25519SignatureAlgorithm(Key privateKey = null)
        {
            _buffer = new MemoryStream();
            _privateKey = privateKey ?? Key.Create(SignatureAlgorithm.Ed25519);
            _publicKey = _privateKey.PublicKey;
            HashSizeValue = 512; // Ed25519 signatures are 64 bytes (512 bits)
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
            return SignatureAlgorithm.Ed25519.Sign(_privateKey, data);
        }

        public bool Verify(byte[] data, byte[] signature)
        {
            return SignatureAlgorithm.Ed25519.Verify(_publicKey, data, signature);
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
