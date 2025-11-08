
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cipher.Library
{
    /// <summary>
    /// Centralized the creation of various hash and signature algorithm instances
    /// </summary>
    public class HashFactory
    {
        public SHA256 CreateSha256()
        {
            return SHA256.Create();
        }
        public SHA512 CreateSha512() => SHA512.Create();
        public MD5 CreateMD5() => MD5.Create();
        public SHA384 CreateSHA384() => SHA384.Create();
        public BCryptHashAlgorithm CreateBCrypt() => new BCryptHashAlgorithm();
        public Argon2Hash CreateArgon2() => new Argon2Hash();
        public Ed25519SignatureAlgorithm CreateEd25519()=>new Ed25519SignatureAlgorithm();
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
            hashAlgorithms.Add("Blake3", new Blake3HashAlgo());
            hashAlgorithms.Add("Key hash", CreateKeyHash());
            hashAlgorithms.Add("B crypt", CreateBCrypt());
            hashAlgorithms.Add("Argon 2", CreateArgon2());
            hashAlgorithms.Add("Ed25519 Signature", CreateEd25519());
            return hashAlgorithms;
        }
        public HashAlgorithm CreateByName(string name)
        {
            return GetHashAlgorithms().TryGetValue(name, out var algo) ? algo : throw new ArgumentException($"Unknown algorithm: {name}");
        }
    }

}
