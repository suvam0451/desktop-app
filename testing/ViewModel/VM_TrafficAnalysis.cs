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
using testing.Library;
using CsvHelper;
using System.IO;
using System.Threading;

namespace testing.ViewModels
{
    public class Foo {
        public int Id { get; set; }
        public string Name { get; set; }
    }

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
        public ImageSource TestImageSource { get; set; } = new BitmapImage(new Uri($"pack://application:,,,/images/logo/document.png"));



        public ICommand RunDFS { get; set; }
        public ICommand CSVTest { get; set; }
        // public ICommand OpenFile { get; set; }

        public String ExcelInputFile_01 { get; set; } = null;
        public String ExcelInputFile_02 { get; set; } = null;
        public String ExcelOutputFile_01 { get; set; } = null;
        public String CsvInputFile { get; set; }

        public VM_TrafficAnalysis()
        {
            CsvInputFile = "F:\\WinterWildfire\\YOLO_Sample\\Archive.csv";
            this.SidebarItems = new ObservableCollection<SidebarModel>();
            this.TextElements = new ObservableCollection<StringItemModel>();

            this.SidebarItems.Add(new SidebarModel("Stupidity"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));

            RunDFS = new RelayCommand( o => { AddingToList(); }, o => true );
            CSVTest = new RelayCommand(o => { CSVTest_Impl(); }, o => true);
            // OpenFile = new RelayCommand( o => { OpenExcelFile(); }, o => true );
        }

        #region Sidebar Interaface

        public int currentPage { get; set; } = 1;
        public int MaximumPages { get; set; } = 5;
        public String ConsoleText { get; set; } = "Lorem ipsum dolor sit amet";
        
        public void ChangePage(int In) => _ = (In < MaximumPages) ? 1 : (currentPage = In);
        public void NextPage() => currentPage = (currentPage == MaximumPages) ? 1 : (currentPage + 1);

        #endregion

        #region Method Calls
        private async void CSVTest_Impl()
        {
            Dictionary<int, List<int>> mine = new Dictionary<int, List<int>>();
            var reader = new StreamReader(CsvInputFile);
            var config = new CsvHelper.Configuration.Configuration();
            config.HasHeaderRecord = false;

            using (var csv = new CsvReader(reader, config)) {
               
                var records = csv.GetRecords<Foo>();
                // System.Windows.MessageBox.Show("Number of entries found: " + records.Count().ToString());
                foreach(var record in records)
                {
                    // System.Windows.MessageBox.Show(record.Id.ToString() + " has value of " + record.Name);
                    mine[record.Id] = record.Name.Split(',').Select(int.Parse).ToList();
                }
                DotHelpers.BuildConnectivityGraph(mine);
                // ExcelHelper.UniformFactorMethod(ExcelInputFile_01, true);
            }
            reader.Dispose();


            
            var path = Path.Combine(Environment.CurrentDirectory, "Projects", "Sample", "Connectivity.png");

            /* This is part of an asynchronous function */
            // For preventing file-locking...
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            TestImageSource = image;
        }

        private void AddingToList()
        {
            Dictionary<int, List<int>> mine = new Dictionary<int, List<int>>();
            ExcelParsers.FetchConnectivityMatrix<int, int>(mine, ExcelInputFile_01, true);
            DotHelpers.BuildConnectivityGraph(mine);
        }

        #endregion
    }
}