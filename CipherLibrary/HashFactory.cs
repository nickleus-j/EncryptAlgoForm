using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CipherLibrary
{
    public class HashFactory
    {
        public SHA256 CreateSha256()
        {
            return new SHA256Managed();
        }
        public Dictionary<string, HashAlgorithm> GetHashAlgorithms()
        {
            Dictionary<string, HashAlgorithm> hashAlgorithms = new Dictionary<string, HashAlgorithm>();
            hashAlgorithms.Add("Sha-256", CreateSha256());
            hashAlgorithms.Add("Sha-512", new SHA512Managed());
            hashAlgorithms.Add("MD5", MD5.Create());
            return hashAlgorithms;
        }
    }
}
