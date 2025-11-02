using NSec.Cryptography;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using HashAlgorithm = System.Security.Cryptography.HashAlgorithm;
namespace Cipher.Library
{
    public class Argon2Hash : HashAlgorithm
    {
        private MemoryStream _buffer;
        private byte[] _salt;
        private int _outputLength;

        public Argon2Hash(
            byte[] salt = null,
            int outputLength = 32)
        {
            _buffer = new MemoryStream();
            _salt = salt ?? GenerateRandomSalt(16);
            _outputLength = outputLength;
            HashSizeValue = _outputLength * 8;
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
            var password = _buffer.ToArray();
            var argon2Parameters = new Argon2Parameters
            {
                DegreeOfParallelism = 1, // Number of threads to use
                MemorySize = 1024 * 64,  // Memory usage in kibibytes (e.g., 64MB)
                NumberOfPasses = 3       // Number of iterations
            };

            // 3. Get the Argon2id algorithm instance
            var argon2idAlgorithm = PasswordBasedKeyDerivationAlgorithm.Argon2id(argon2Parameters);

            var key = argon2idAlgorithm.DeriveBytes(password, _salt, 32);
            return key;
        }

        private static byte[] GenerateRandomSalt(int length)
        {
            var salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return salt;
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
