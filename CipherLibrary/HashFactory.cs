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
        public SHA512 CreateSha512() => new SHA512Managed();
        public MD5 CreateMD5() => MD5.Create();

        public Dictionary<string, HashAlgorithm> GetHashAlgorithms()
        {
            Dictionary<string, HashAlgorithm> hashAlgorithms = new Dictionary<string, HashAlgorithm>();
            hashAlgorithms.Add("Sha-256", CreateSha256());
            hashAlgorithms.Add("Sha-512", CreateSha512());
            hashAlgorithms.Add("MD5", CreateMD5());
            return hashAlgorithms;
        }
    }
}
