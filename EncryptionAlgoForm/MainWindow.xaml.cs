using HashAlgoForm.Commands;
using HashAlgoForm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace EncryptionAlgoForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FormModel Model { get; set; }
        public HasherUiHandler clickhandler { get; set; }
        public MainWindow()
        {
            Model = new FormModel();
            clickhandler=new HasherUiHandler(Model);
            Model.ForHashing = "Give text";
            Model.Result = "Result Here";
            InitializeComponent();
            this.DataContext = Model;
            EncryptBt.Click += clickhandler.EncryptBt_Click;
        }
        
    }
}
