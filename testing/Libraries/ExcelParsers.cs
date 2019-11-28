using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace testing.Libraries
{
    public class ExcelParsers
    {
        /// <summary>
        /// Reads a connectivity matrix from Excel file. Inputs: array: Dictionary<S,T>
        /// "Connectivity is represented by comma separated indices. For invalid indices, the value is rejected
        /// </summary>
        public static bool FetchConnectivityMatrix<S, T>(Dictionary<int, List<int>> array, String path, bool SkipFirstRow = false)
        {
            // MessageBox.Show("Running connectivity matrix query");
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null || path == null) { return false; }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            Excel._Worksheet xlWorkSheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorkSheet.UsedRange;

            int UpperBound = xlRange.Rows.Count;
            int NumberOfNodes = SkipFirstRow ? UpperBound : UpperBound - 1;
            int i = SkipFirstRow ? 2 : 1;

            // array = new Dictionary<int, List<int>>(NumberOfNodes);

            try {
                for (; i <= UpperBound; i++)
                {
                    int tmp01 = (int)xlRange.Cells[i, 1].Value2;
                    array.Add(tmp01, new List<int>());

                    String tmp = (String)xlRange.Cells[i, 2].Value2.ToString();
                    array[tmp01] = tmp.Split(',').Select(int.Parse).ToList();
                    array[tmp01].RemoveAll((int x) => { return x > UpperBound; });
                }
                // MessageBox.Show(array[1][0].ToString());
            }
            catch (RuntimeBinderException e) {
                MessageBox.Show("Failed as: " + e);
                return false;
            }
            

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(xlRange);
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            return true;
        }

        /// <summary>
        /// Reads matrix from Excel file. Inputs: array: Dictionary<S,T>
        /// </summary>
        public static bool RecieveMatrix<S, T>(Dictionary<S, List<T>> array, String path, bool SkipFirstRow = false)
        {

            return true;
        }
            public static bool WriteMatrixToExcelFile<S,T>(Dictionary<S, List<T>> array, String path) {
            try
            {
                // xlWorkbook.SaveAs("example02.xlsx");
                return true;
            }
            catch
            {
                MessageBox.Show("File could not be accessed. Make sure you are not editing the file.");
                return false;
            }
        }
    }
}
