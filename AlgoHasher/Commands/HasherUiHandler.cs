using System;
using System.Collections.Generic;
using System.Text;
using AlgoHasher.ViewModels;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace AlgoHasher.Commands
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
            e.Handled = true;
            string forHashing = ViewModel.UseSalt ? String.Concat(ViewModel.ForHashing, ViewModel.ForSalt) : ViewModel.ForHashing;
            if (ViewModel.SelectedHashAlgorithm == null)
            {
                Hash(SHA256.Create(), forHashing);
            }
            else Hash(ViewModel.SelectedHashAlgorithm, forHashing);
            ViewModel.AddHashedTerm(new HashedTerm(ViewModel.ForHashing, ViewModel.UseSalt, ViewModel.ForSalt));
        }
        public void Encrypt_ConcatenatedtermHistory(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            string forHashing = ViewModel.ConcatenatedMessages;
            if (ViewModel.SelectedHashAlgorithm == null)
            {
                Hash(SHA256.Create(), forHashing);
            }
            else Hash(ViewModel.SelectedHashAlgorithm, forHashing);
        }
        public void HashForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (sender is ComboBox)
            ViewModel.SelectedHashAlgorithm = (HashAlgorithm)(((ComboBox)sender).SelectedValue);
        }
        public void TermHistory_Select(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (sender is ListBox && ((ListBox)sender).SelectedIndex>=0)
            {
                ListBox lb = (ListBox)sender;
                HashedTerm selectedTerm = ViewModel.HashedHistory[lb.SelectedIndex];
                ViewModel.ForHashing = selectedTerm.ForHashing;
                ViewModel.ForSalt = selectedTerm.ForSalt;
                ViewModel.UseSalt = selectedTerm.isSalted;
                Hash(ViewModel.SelectedHashAlgorithm
                    , ViewModel.UseSalt ? String.Concat( ViewModel.ForHashing, ViewModel.ForSalt) : ViewModel.ForHashing);
            }

        }
        public void Trigger_ClearLoggedTerms(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ViewModel.HashedHistory.Clear();
        }
        public void RandomBt_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ViewModel.ForHashing = GenerateRandomString(ViewModel.RandomTextLength);
        }
        private string GenerateRandomString(int length)
        {
            Random randomGenerator = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, length).Select(s => s[randomGenerator.Next(s.Length)]).ToArray());
        }
    }
}
