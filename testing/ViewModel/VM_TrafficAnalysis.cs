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

        private String ExcelFilePath { get; set; } = null;

        public VM_TrafficAnalysis()
        {
            this.SidebarItems = new ObservableCollection<SidebarModel>();
            this.TextElements = new ObservableCollection<StringItemModel>();

            this.SidebarItems.Add(new SidebarModel("Stupidity"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));

            AddToList = new RelayCommand(AddingToList);
            OpenFile = new RelayCommand(OpenExcelFile);
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
            // NextPage();
            // System.Windows.MessageBox.Show(currentPage.ToString());

            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null || ExcelFilePath == null)
            {
                ConsoleMessage = "Error: At least Excel 2016 is required to run this feature.";
                return;
            }
            else
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelFilePath);
                Excel._Worksheet xlWorkSheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorkSheet.UsedRange;

                int NodeCount = xlRange.Rows.Count;
                Dictionary<int, List<int>> mine = new Dictionary<int, List<int>>();
                for (int i = 1; i < NodeCount; i++) {
                    int tmp01 = (int)xlRange.Cells[i + 1, 1].Value2;
                    mine.Add(tmp01, new List<int>());

                    String tmp = (String)xlRange.Cells[i + 1, 2].Value2.ToString();
                    
                    mine[tmp01] = tmp.Split(',').Select(int.Parse).ToList();
                }
                clsGraph lazy = new clsGraph(mine);
                // lazy.BFS(4);
                lazy.BuildConnectivity();
                lazy.WriteFile("any");
                lazy.RunDOT();

                try {
                    xlWorkbook.SaveAs("example02.xlsx");
                }
                catch {
                    ConsoleMessage = "File could not be accessed. Make sure you are not editing the file.";
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();       
                Marshal.ReleaseComObject(xlRange);
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
        }

        private void OpenExcelFile() {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Connectivity Sheet(*.xlsx)|*.xlsx|Connectivity Sheet(old format)(*.xlsx)|*.xlsx";
            diag.DefaultExt = "*.xlxs";
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            diag.ShowDialog();

            if (diag.CheckFileExists == true) {
                try {
                    ExcelFilePath = diag.FileName;
                    ConsoleMessage = "File: " + ExcelFilePath + " loaded.";
                }
                catch {
                    ConsoleMessage = "Could not open file. Check permissions";
                }
            }
        }
        #endregion
    }
}
