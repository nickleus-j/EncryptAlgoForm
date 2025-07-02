using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Cipher.Library
{
    public class HashFactory
    {
        public SHA256 CreateSha256()
        {
            return SHA256.Create();
        }
        public SHA512 CreateSha512() => SHA512.Create();
        public MD5 CreateMD5() => MD5.Create();
        public SHA384 CreateSHA384() => SHA384.Create();
        public Whirlpool CreateWhirlpool() => new Whirlpool();
        byte[] key = { 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0x0b, 0xf8, 0x86};
        public KeyedHashAlgorithm CreateKeyHash() => new HMACMD5(key);
        /// <summary>
        /// Returns a collection of hash algorithms that can be used
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, HashAlgorithm> GetHashAlgorithms()
        {
            Dictionary<string, HashAlgorithm> hashAlgorithms = new Dictionary<string, HashAlgorithm>();
            hashAlgorithms.Add("Sha-256", CreateSha256());
            hashAlgorithms.Add("Sha-512", CreateSha512());
            hashAlgorithms.Add("MD5", CreateMD5());
            hashAlgorithms.Add("Whirlpool", CreateWhirlpool());
            hashAlgorithms.Add("SHA384", CreateSHA384());
            hashAlgorithms.Add("Key hash", CreateKeyHash());
            return hashAlgorithms;
        }
    }
}
