using System;
using System.Collections.Generic;
using System.Text;
using HashAlgoForm.ViewModels;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace HashAlgoForm.Commands
{
    public class HasherUiHandler
    {
        public FormModel ViewModel {get;set;}
        public HasherUiHandler(FormModel givenViewModel)
        {
            ViewModel = givenViewModel;
        }
        public void Hash(HashAlgorithm hasher,string ForHashing="Example")
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(ForHashing);
            string hex = "";

            hashValue = hasher.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            ViewModel.Result = hex;
        }
        public void EncryptBt_Click(object sender, RoutedEventArgs e)
        {
            string forHashing = ViewModel.UseSalt ? ViewModel.ForHashing + ViewModel.ForSalt : ViewModel.ForHashing;
            if (ViewModel.SelectedHashAlgorithm == null)
            {
                Hash(new SHA256Managed(), forHashing);
            }
            else Hash(ViewModel.SelectedHashAlgorithm, forHashing);
            ViewModel.HashedHistory.Add(new HashedTerm(ViewModel.ForHashing, ViewModel.UseSalt, ViewModel.ForSalt));
        }
        public void HashForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox)
            ViewModel.SelectedHashAlgorithm = (HashAlgorithm)(((ComboBox)sender).SelectedValue);
        }
    }
}
