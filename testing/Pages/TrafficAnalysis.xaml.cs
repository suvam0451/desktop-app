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
using testing.ViewModels;

namespace testing.Pages
{
    public partial class TrafficAnalysis : Page
    {
        public TrafficAnalysis()
        {
            InitializeComponent();
            DataContext = new VM_TrafficAnalysis();
        }
    }
}
