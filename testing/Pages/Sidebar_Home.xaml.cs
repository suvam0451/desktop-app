using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testing.ViewModels;

namespace testing.Pages
{
    public partial class Sidebar_Home : Page
    {
        public Sidebar_Home()
        {
            InitializeComponent();
            DataContext = new VM_Sidebar();
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
