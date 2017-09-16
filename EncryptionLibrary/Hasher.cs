using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionLibrary
{
    public class Hasher
    {
        /// <summary>
        /// Salts the text with the sha 256 algorithm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Hash_sha256(String value)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(value);

            SHA256 hashString = SHA256.Create();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
            //return Hash_sha256(value, "RandomString");
        }

        public static String Hash_sha256(string value, string addToBytes)
        {
            return Hash_sha256(value + CreateSalt(addToBytes));
        }

        private static string CreateSalt(string toBeSalted)
        {
            string username = toBeSalted;
            byte[] userBytes;
            string salt;
            userBytes = ASCIIEncoding.ASCII.GetBytes(username);
            long XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            Random rand = new Random(Convert.ToInt32(XORED));
            salt = rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            return salt;
        }

        public string GenerateSalt(int length)
        {
            var rng = RandomNumberGenerator.Create();
            var buffer = new byte[length];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}
