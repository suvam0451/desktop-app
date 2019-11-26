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
    public class VM_YOLOSample : testing.BaseViewModel
    {
        public String Title { get; set; } = "N/A";

        public ObservableCollection<String> ImageQueue { get; }

        public VM_YOLOSample()
        {
            ImageQueue = new ObservableCollection<String>();
            ImageQueue.Add("yamete kudasai");
        }
    }
}