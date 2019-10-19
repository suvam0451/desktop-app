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
            SaveVideo = new RelayCommand(o => { AllIs(); },
                                        o => true );
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

        private void AllIs()
        {
            ConsoleMessage = "Not ready yet.";
           // MessageBox.Show("Okay then. That works.");
        }
    }
}