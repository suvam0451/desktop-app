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
    /// Interaction logic for Verbose_FilePicker.xaml
    /// </summary>
    public partial class Verbose_FilePicker : System.Windows.Controls.UserControl
    {
        /*
        *   Inherited properties: 
        *       Secret : The text element (in case we need custom initial display).
        *       Filter : Sets the filter for file selection.
        *       DirectoryMode : Sets if a directory should be selected.
        *   New properties:
        *       Caption : The caption of the card.
        */
        #region DependencyProps

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("Title goes here."));


        public String SelectedPath
        {
            get { return (String)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }
        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register("SelectedPath", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("No file assigned."));


        public String FileFilter
        {
            get { return (String)GetValue(FileFilterProperty); }
            set { SetValue(FileFilterProperty, value); }
        }
        public static readonly DependencyProperty FileFilterProperty =
            DependencyProperty.Register("FileFilter", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("Connectivity Sheet(*.xlsx)|*.xlsx|Connectivity Sheet(old format)(*.xlsx)|*.xlsx"));


        public bool DirectoryMode
        {
            get { return (bool)GetValue(DirectoryModeProperty); }
            set { SetValue(DirectoryModeProperty, value); }
        }
        public static readonly DependencyProperty DirectoryModeProperty =
            DependencyProperty.Register("DirectoryMode", typeof(bool), typeof(Verbose_FilePicker), new PropertyMetadata(true));

        #endregion


        #region ctor
        public Verbose_FilePicker()
        {
            InitializeComponent();
        }
        #endregion

        private void TestFunc(object sender, RoutedEventArgs e)
        {
            // Store value to revert back later...
            String previous = SelectedPath;

            if (DirectoryMode)
            {
                FolderBrowserDialog diag = new FolderBrowserDialog();
                diag.RootFolder = Environment.SpecialFolder.MyComputer;
                diag.ShowDialog();

                SelectedPath = diag.SelectedPath;
            }
            else
            {
                OpenFileDialog diag = new OpenFileDialog();
                diag.Filter = FileFilter;
                diag.DefaultExt = "*.xlxs";
                diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                diag.ShowDialog();

                SelectedPath = (diag.CheckFileExists == true && diag.FileName != "") ? diag.FileName : previous;
            }
        }
    }
}