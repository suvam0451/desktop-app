using System;
using System.Collections.Generic;
using System.IO;
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

        private void UpdateUI(Object sender, RoutedEventArgs e) {
            FrameworkElement feSource = e.Source as FrameworkElement;

            var path = Path.Combine(Environment.CurrentDirectory, "Projects", "Sample", "Connectivity.png");
            var uri = new Uri(path);
            try
            {
                Image ToAdd = new Image();
                BitmapImage ConnectivityGraph = new BitmapImage(uri);
                ToAdd.Source = ConnectivityGraph;
                ToAdd.Stretch = Stretch.None;
                // ToAdd.MaxWidth = ToAdd.ActualWidth;

                MainStackPanel.Children.Add(ToAdd);
                ConsoleOutput.Message = "Successfully updated the Interface.";
            }
            catch (DirectoryNotFoundException) {
                ConsoleOutput.Message = "Connectivity.png missing.";
            }
        }
    }
}
