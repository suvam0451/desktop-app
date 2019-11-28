using System.Windows.Controls;
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
