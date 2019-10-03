using System;
using System.Windows;
using System.Windows.Input;

namespace testing.ViewModels
{
    public class VM_DefaultPage : testing.BaseViewModel
    {
        public String ConsoleMessage { get; set; } = "Konichiwa Desu";
        public String ExampleTitle { get; set; } = "Konichiwa Desu";

        public ICommand SaveVideo { get; set; }

        public VM_DefaultPage() {
            SaveVideo = new RelayCommand(AllIs);
        }

        private void AllIs() {
            MessageBox.Show("Okay then. That works.");
        }
    }
}
