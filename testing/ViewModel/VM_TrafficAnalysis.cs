using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using testing.Libraries;
using daedalus_clr;

namespace testing.ViewModels
{
    public class VM_TrafficAnalysis : testing.BaseViewModel, ISidebarTarget
    {
        public String DummyTextBlock { get; set; } = "bheege naina";
        public ObservableCollection<SidebarModel> SidebarItems { get; set; }
        public ObservableCollection<StringItemModel> TextElements { get; set; }
        // Width of the frame
        public float DesignWidth { get; set; } = 50.0f;
        public String ConsoleMessage { get; set; } = "Lorem ipsum dolor sit amet";
        public String ProjectPrefix { get; set; } = "Default_";
        public float SidebarWidth { get; set; } = 50.0f;
        public ImageSource ImageSource { get; set; } = new BitmapImage(new Uri($"pack://application:,,,/images/logo/document.png"));

        public ICommand AddToList { get; set; }
        public ICommand OpenFile { get; set; }

        public String ExcelInputFile_01 { get; set; } = null;
        public String ExcelInputFile_02 { get; set; } = null;
        public String ExcelOutputFile_01 { get; set; } = null;

        public VM_TrafficAnalysis()
        {
            this.SidebarItems = new ObservableCollection<SidebarModel>();
            this.TextElements = new ObservableCollection<StringItemModel>();

            this.SidebarItems.Add(new SidebarModel("Stupidity"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));

            AddToList = new RelayCommand(
                o => { AddingToList(); },
                o => true );
            OpenFile = new RelayCommand(
                o => { OpenExcelFile(); },
                o => true );
        }

        #region Sidebar Interaface

        public int currentPage { get; set; } = 1;
        public int MaximumPages { get; set; } = 5;
        public String ConsoleText { get; set; } = "Lorem ipsum dolor sit amet";
        
        public void ChangePage(int In) => _ = (In < MaximumPages) ? 1 : (currentPage = In);
        public void NextPage() => currentPage = (currentPage == MaximumPages) ? 1 : (currentPage + 1);

        #endregion

        #region Method Calls
        private void AddingToList()
        {
            Dictionary<int, List<int>> mine = new Dictionary<int, List<int>>();
            ExcelParsers.FetchConnectivityMatrix<int, int>(mine, ExcelInputFile_01, true);
            DotHelpers.BuildConnectivityGraph(mine);
            
            ExcelHelper.UniformFactorMethod(ExcelInputFile_01, true);
        }

        private void OpenExcelFile() {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Connectivity Sheet(*.xlsx)|*.xlsx|Connectivity Sheet(old format)(*.xlsx)|*.xlsx";
            diag.DefaultExt = "*.xlxs";
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            diag.ShowDialog();

            if (diag.CheckFileExists == true) {
                try {
                    ExcelInputFile_01 = diag.FileName;
                    ConsoleMessage = "File: " + ExcelInputFile_01 + " loaded.";
                }
                catch {
                    ConsoleMessage = "Could not open file. Check permissions";
                }
            }
        }
        #endregion
    }
}