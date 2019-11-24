using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testing.UserControls
{
    /// <summary>
    /// Interaction logic for FilePickField.xaml
    /// </summary>
    public partial class FilePickField : System.Windows.Controls.UserControl
    {
        public String Secret {
            get { return (String)GetValue(SecretProperty); }
            set { SetValue(SecretProperty, value); }
        }
        public static readonly DependencyProperty SecretProperty =
            DependencyProperty.Register("Secret", typeof(String), typeof(FilePickField), new PropertyMetadata("No file assigned."));


        public String Filter
        {
            get { return (String)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(String), typeof(FilePickField), new PropertyMetadata("Connectivity Sheet(*.xlsx)|*.xlsx|Connectivity Sheet(old format)(*.xlsx)|*.xlsx"));


        public bool DirectoryMode
        {
            get { return (bool)GetValue(DirectoryModeProperty); }
            set { SetValue(DirectoryModeProperty, value); }
        }
        public static readonly DependencyProperty DirectoryModeProperty =
            DependencyProperty.Register("DirectoryMode", typeof(bool), typeof(FilePickField), new PropertyMetadata(true));


        public FilePickField()
        {
            InitializeComponent();
        }

        private void TestFunc(object sender, RoutedEventArgs e)
        {
            // Store value to revert back later...
            String previous = Secret;

            if (DirectoryMode)
            {
                FolderBrowserDialog diag = new FolderBrowserDialog();
                diag.RootFolder = Environment.SpecialFolder.MyComputer;
                diag.ShowDialog();

                Secret = diag.SelectedPath;
            }
            else
            {
                OpenFileDialog diag = new OpenFileDialog();
                diag.Filter = Filter;
                diag.DefaultExt = "*.xlxs";
                diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                diag.ShowDialog();

                Secret = (diag.CheckFileExists == true && diag.FileName!="") ? diag.FileName : previous;
            }
        }
    }
}
