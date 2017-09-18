using System;
using System.Collections.Generic;
using System.ComponentModel;
using CipherLibrary;
using System.Security.Cryptography;

namespace HashAlgoForm.ViewModels
{
    public class FormModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region properties
        public string ForHashing { get; set; }
        public string ForSalt { get; set; }
        private string result { get; set; }
        public string Result { get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("result");
            }
        }
        private bool useSalt { get; set; }
        public bool UseSalt { get { return useSalt; } set { useSalt = value;OnPropertyChanged("useSalt"); } }
        private Dictionary<string, HashAlgorithm> hashDictionary { get; set; }
        public Dictionary<string, HashAlgorithm> HashOptions {
            get {
                if (hashDictionary == null)
                {
                    HashFactory factory = new HashFactory();
                    hashDictionary = factory.GetHashAlgorithms();
                }
                return hashDictionary;
            } }
        public HashAlgorithm SelectedHashAlgorithm { get; set; }
        private List<HashedTerm> termLog { get; set; }
        public List<HashedTerm> HashedHistory
        {
            get
            {
                if (termLog == null)
                {
                    termLog = new List<HashedTerm>();
                }
                return termLog;
            }
        }
        #endregion
        #region Notification
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
