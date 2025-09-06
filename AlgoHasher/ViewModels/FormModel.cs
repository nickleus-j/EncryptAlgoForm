using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cipher.Library;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace AlgoHasher.ViewModels
{
    public class FormModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region properties
        private string forHashing { get; set; }
        private string forSalt { get; set; }
        
        private string result { get; set; }
        private int _randomTextLength { get; set; }
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
        public int RandomTextLength { get { return _randomTextLength; } set { _randomTextLength = value; OnPropertyChanged("_randomTextLength"); } }
        private Dictionary<string, HashAlgorithm> algoDictionary { get; set; }
        public Dictionary<string, HashAlgorithm> HashOptions {
            get {
                if (algoDictionary == null)
                {
                    HashFactory factory = new HashFactory();
                    algoDictionary = factory.GetHashAlgorithms();
                }
                return algoDictionary;
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
        public string ConcatenatedMessages
        {
            get
            {
                List<string> messages = new List<string>();
                foreach(HashedTerm terms in HashedHistory)
                {
                    messages.Add(String.Concat(terms.Term, " "));
                }
                return String.Concat(messages);
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
        }
        #endregion
    }
}
