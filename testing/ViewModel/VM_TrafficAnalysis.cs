﻿using System;
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

namespace testing.ViewModels
{
    public class VM_TrafficAnalysis : testing.BaseViewModel
    {
        public String DummyTextBlock { get; set; } = "bheege naina";
        public ObservableCollection<SidebarModel> SidebarItems { get; set; }
        public ObservableCollection<StringItemModel> TextElements { get; set; }
        // Width of the frame
        public float DesignWidth { get; set; } = 50.0f;
        public String ConsoleMessage { get; set; } = "Lorem ipsum dolor sit amet";
        public float SidebarWidth { get; set; } = 50.0f;
        public ImageSource ImageSource { get; set; } = new BitmapImage(new Uri($"pack://application:,,,/images/logo/document.png"));

        public ICommand AddToList { get; set; }
        public ICommand OpenFile { get; set; }

        private String ExcelFilePath {get; set;} = null;

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
        #region Method Calls
        private void AddingToList()
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                ConsoleMessage = "Error: At least Excel 2016 is required to run this feature."
                return;
            }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add("misValue");
            Excel.Worksheet _Sheet = (Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);
            // _Sheet.Cells[1, 1] = "ID";
            // _Sheet.Cells[1, 2] = "Name";
            // try {
            //     xlWorkbook.SaveAs("example02.xlsx");
            // }
            catch {
                ConsoleMessage = "File could not be accessed. Make sure you are not editing the file.";
            }
            // MessageBox.Show("Victoria");
            // this.TextElements.Add(new StringItemModel("Wheezie"));
            // this.SidebarItems.Add(new SidebarModel("Stupidity"));
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
        private void OpenExcelFile() {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Connectivity Sheet(*.xlsx)|*.xlsx|Connectivity Sheet(old format)(*.xlsx)|*.xlsx";
            diag.DefaultExt = "*.xlxs";

            String outdir = System.AppDomain.CurrentDomain.BaseDirectory + "output\\";
            diag.InitialDirectory = outdir;
            
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
            // if (diag.ShowDialog()) {
                
            // }
        }
        #endregion
    }
}