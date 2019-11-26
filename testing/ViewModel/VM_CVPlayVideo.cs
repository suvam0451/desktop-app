using daedalus_clr;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace testing.ViewModels
{
    public class VM_CVPlayVideo : testing.BaseViewModel, IDropTarget
    {
        // Descriptors
        public String ConsoleMessage { get; set; } = "OpenCV Demo submodule. Idle...";
        public String CardTitle { get; set; } = "OpenCV Demo";
        public String CardDescription { get; set; } = "Processes a Metallic Roughness set of textures.";
        public String VideoPath { get; set; } = "";

        // Backend application
        private TextureCombine_Type1_Backend backend;

        public Page _mWindow;

        // Boolean State Switches
        public bool Rescale_Locked { get; set; } = false;
        public bool Rescale_Enabled { get; set; } = true;
        public bool Collapse_Poster { get; set; } = false;

        // State variables
        public int SelectedAspectRatio { get; set; } = 0;

        // Commands
        public ICommand SaveVideo { get; set; }
        public ICommand InvertRescaleControls { get; set; }
        public ICommand CollapseImage { get; set; }
        public ICommand RunOpenCV { get; set; }

        // Control Bindings
        public ObservableCollection<String> AspectRatios { get; set; }
        public IList<String> AspectRatio = new List<String>();

        public VM_CVPlayVideo() {
            SaveVideo = new RelayCommand(o => { Validate_Impl(); }, o => true );
            InvertRescaleControls = new RelayCommand(o => { InverseRescale(); }, o => true);
            CollapseImage = new RelayCommand(o => { FlipBoolean(); }, o => true);
            RunOpenCV = new RelayCommand(o => { RunOpenCV_Impl(); }, o => true);


            AspectRatios = new ObservableCollection<string>();
            this.AspectRatios.Add("1280 x 720");
            this.AspectRatios.Add("1920 x 1080");

            // Fill application level default keys
            VideoPath = Properties.Settings.Default.OpenCV_DefaultVideo;

            // Initialize the backend program..
            backend = new TextureCombine_Type1_Backend();
        }

        

        void IDropTarget.DragOver(IDropInfo dropInfo) {
            // var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<String>();
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            ConsoleMessage = "trying to add: " + dragFileList.Count + " files";
            dropInfo.Effects = DragDropEffects.Copy;
        }

        void IDropTarget.Drop(IDropInfo dropInfo) {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            ConsoleMessage = dragFileList[0];

            dropInfo.Effects = DragDropEffects.Copy;
        }

        private void FlipBoolean() {
            // State = !State;
        }

        private void Validate_Impl()
        {
            MessageBox.Show("Path stored is: " + VideoPath);
        }

        private void RunOpenCV_Impl() {
            MessageBox.Show("Lets do this...");
            backend.HandleMediaDrop(VideoPath);
        }

        private void InverseRescale() {
            Rescale_Locked = !Rescale_Locked;
            Rescale_Enabled = !Rescale_Enabled;
        }
    }
}