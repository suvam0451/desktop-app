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

namespace testing.Pages
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar_Home : Page
    {
        public Sidebar_Home()
        {
            InitializeComponent();
        }

        // Action on MOUSE CLICK
        private void rect_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            Image r = (Image)sender;

            //MessageBox.Show(r.Source.ToString());
            DataObject dataObj = new DataObject(r.Source);

            DragDrop.DoDragDrop(r, dataObj, DragDropEffects.Move);
        }
    }
}
