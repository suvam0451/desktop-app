using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;

namespace testing.ViewModels
{
    public class VM_CVPlayVideo : testing.BaseViewModel, IDropTarget
    {
        public String ConsoleMessage { get; set; } = "Konichiwa Desu";
        public String CardTitle { get; set; } = "Metallic Roughness";
        public String CardDescription { get; set; } = "Processes a Metallic Roughness set of textures.";
        public Page _mWindow;

        public ICommand SaveVideo { get; set; }

        public VM_CVPlayVideo() {
            SaveVideo = new RelayCommand(AllIs);
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

        private void Yeet() {
            // ConsoleMessage = "Not again. Noob.";
            ConsoleMessage = "Not ready yet.";
            MessageBox.Show("yeeto");
        }

        private void AllIs()
        {
            ConsoleMessage = "Not ready yet.";
           // MessageBox.Show("Okay then. That works.");
        }
        // RelayCommand()
        // public ICommand SaveVideo
        // {
        //     get
        //     {
        //         if (mUpdater == null)
        //             mUpdater = new Updater();
        //         return mUpdater;
        //     }
        //     set
        //     {
        //         mUpdater = value;
        //     }
        // }

        // Gets the mouse position (using user32.dll)
        // private Point GetMousePosition()
        // {
        //     //Win32Point w32Mouse = new Win32Point();
        //     //GetCursorPos(ref w32Mouse);
        // 
        //     var w32Mouse = Mouse.GetPosition(mWindow);
        //     // Window position added
        //     return new Point(w32Mouse.X + mWindow.Left, w32Mouse.Y + mWindow.Top);
        // }
    }
}