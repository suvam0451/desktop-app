using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.Models;

namespace testing.ViewModels
{
    public class VM_Sidebar : testing.BaseViewModel
    {
        public String DummyTextBlock { get; set; } = "bheege naina";
        public ObservableCollection<SidebarModel> SidebarItems { get; set; }
        public ObservableCollection<StringItemModel> TextElements { get; set; }
        // Width of the frame
        public float DesignWidth { get; set; } = 50.0f;
        public String ConsoleMessage { get; set; } = "Konichiwa Desu";
        public float SidebarWidth { get; set; } = 50.0f;

        public ICommand AddToList { get; set; }


        public VM_Sidebar() {
            this.SidebarItems = new ObservableCollection<SidebarModel>();
            this.TextElements = new ObservableCollection<StringItemModel>();

            this.SidebarItems.Add(new SidebarModel("Stupidity"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));
            this.TextElements.Add(new StringItemModel("Wheezie"));

            AddToList = new RelayCommand(AddingToList);
        }

        private void AddingToList() {
            // MessageBox.Show("Victoria");
            // this.TextElements.Add(new StringItemModel("Wheezie"));
            this.SidebarItems.Add(new SidebarModel("Stupidity"));
        }
    }
}
