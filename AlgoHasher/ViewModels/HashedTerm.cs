using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoHasher.ViewModels
{
    public class HashedTerm
    {
        public string ForHashing { get; set; }
        public string ForSalt { get; set; }
        public bool isSalted { get; set; }

        public HashedTerm(string TextToHash,bool useSalt,string TextForSalt)
        {
            ForHashing = TextToHash;
            isSalted = useSalt;
            ForSalt = TextForSalt;
        }

        public string Term => isSalted ? ForHashing + " | " + ForSalt : ForHashing;
    }
}
