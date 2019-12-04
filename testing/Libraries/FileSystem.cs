using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testing.Libraries
{
    class FileSystem
    {
        public static String RequestFolderRelativeToRootDir(String RootDir)
        {
            if (Directory.Exists(RootDir))
            {
                FolderBrowserDialog diag = new FolderBrowserDialog();
                diag.RootFolder = Environment.SpecialFolder.MyComputer;
                diag.ShowDialog();
                String retval = diag.SelectedPath;
                diag.Dispose();
                return diag.SelectedPath;
            }
            return "";
        }
    }
}
