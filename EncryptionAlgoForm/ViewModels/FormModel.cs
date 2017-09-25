using System;
using System.Collections.Generic;
using System.ComponentModel;
using CipherLibrary;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace HashAlgoForm.ViewModels
{
    public class FormModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region properties
        private string forHashing { get; set; }
        private string forSalt { get; set; }
        
        private string result { get; set; }
        public string Result { get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("result");
            }
        }
        private bool useSalt { get; set; }
        public string ForHashing { get { return forHashing; } set { forHashing = value; OnPropertyChanged("forHashing"); } }
        public string ForSalt { get { return forSalt; } set { forSalt = value; OnPropertyChanged("forSalt"); } }
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
        private ObservableCollection<HashedTerm> termLog { get; set; }
        public ObservableCollection<HashedTerm> HashedHistory
        {
            get
            {
                if (termLog == null)
                {
                    termLog = new ObservableCollection<HashedTerm>();
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
        public void AddHashedTerm(HashedTerm createdTerm)
        {
            HashedHistory.Add(createdTerm);
            OnPropertyChanged("termLog");
            //OnPropertyChanged("HashedHistory");
        }
        #endregion
    }
}
