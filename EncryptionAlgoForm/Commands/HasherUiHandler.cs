using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using EncryptionLibrary;
using HashAlgoForm.ViewModels;
using System.Windows;
using System.Security.Cryptography;

namespace HashAlgoForm.Commands
{
    public class HasherUiHandler
    {
        public FormModel ViewModel {get;set;}
        public HasherUiHandler(FormModel givenViewModel)
        {
            ViewModel = givenViewModel;
        }
        public void Hash()
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(ViewModel.ForHashing);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            ViewModel.Result = hex;
            //ViewModel.result=Hasher.Hash_sha256(ViewModel.ForHashing);
        }
        public void EncryptBt_Click(object sender, RoutedEventArgs e)
        {
            Hash();
        }
    }
}
