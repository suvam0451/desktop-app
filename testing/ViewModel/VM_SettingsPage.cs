using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace testing.ViewModel
{
    public class VM_SettingsPage : testing.BaseViewModel
    {
        public String GraphViz_Path { get; set; } = Properties.Settings.Default.GraphViz_Path;
        public String Darknet_Path { get; set; } = Properties.Settings.Default.Darknet_Path;
        public bool USE_CUDA { get; set; } = Properties.Settings.Default.USE_CUDA;
        public bool USE_HIGH_RAM { get; set; } = Properties.Settings.Default.USE_HIGH_RAM;

        public ICommand ApplyCommand { get; set; }
        public ICommand OKCommand { get; set; }

        public VM_SettingsPage() {
            ApplyCommand = new RelayCommand( o => { Apply(); }, o => true);
            OKCommand = new RelayCommand( o => { OK(); }, o => true);
        }

        private void Apply() { 

        }
        private void OK() {
            if (GraphViz_Path != "")
            {
                Properties.Settings.Default.GraphViz_Path = GraphViz_Path;
            }
            if (Darknet_Path != "")
            {
                Properties.Settings.Default.Darknet_Path = Darknet_Path;
            }
            Properties.Settings.Default.USE_CUDA = USE_CUDA;
            Properties.Settings.Default.USE_HIGH_RAM = USE_HIGH_RAM;

            Properties.Settings.Default.Save();
        }
    }
}