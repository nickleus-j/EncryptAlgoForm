﻿using HashAlgoForm.Commands;
using HashAlgoForm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SourceChord.FluentWPF;
namespace EncryptionAlgoForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AcrylicWindow
    {
        public FormModel Model { get; set; }
        public HasherUiHandler clickhandler { get; set; }
        public MainWindow()
        {
            Model = new FormModel();
            clickhandler=new HasherUiHandler(Model);
            Model.ForHashing = "Give text";
            Model.Result = "Result Here";
            Model.SelectedHashAlgorithm = Model.HashOptions.Values.First();
            InitializeComponent();
            this.DataContext = Model;
            SetupUiEvents();
        }

        private void SetupUiEvents()
        {
            EncryptBt.Click += clickhandler.EncryptBt_Click;
            HashChoice.SelectionChanged += clickhandler.HashForm_SelectionChanged;
            LoggedHashes.SelectionChanged += clickhandler.TermHistory_Select;
            clearLogBtn.Click += clickhandler.Trigger_ClearLoggedTerms;
            hashAll.Click += clickhandler.Encrypt_ConcatenatedtermHistory;
        }
        
    }
}
