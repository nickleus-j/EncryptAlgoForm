using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HashAlgoForm.ViewModels
{
    public class FormModel : System.ComponentModel.INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
