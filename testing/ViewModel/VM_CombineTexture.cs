using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using testing.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace testing.ViewModels
{
    public class VM_CombineTexture : BaseViewModel, IDropTarget
    {
        public String ConsoleOutput { get; set; } = "Idle";
        public ImageObject SelectedFileObject;

        public ObservableCollection<ImageObject> ImageCollection { get; set; }
        public ICommand UpdateImageList { get; set; }

        public VM_CombineTexture() {
            this.ImageCollection = new ObservableCollection<ImageObject>();

            UpdateImageList = new RelayCommand(Alls);
        }
        private void Alls() {
            this.ImageCollection.Add(new ImageObject
            {
                displayName = "Kombawan",
                location = "Nyaruhodo"
            });
        }
        private string path = @"C:\Users\pcName\Downloads\Gifs";

        private void setFiles() {
            if (this.path != string.Empty) {

                DirectoryInfo dInfo = new DirectoryInfo(this.path); 
                FileInfo[] fInfo = dInfo.GetFiles("*.go");
                fInfo.Cast<FileInfo>().ToList().ForEach(setFileObjectCollection());
            }
        }

        private Action<FileInfo> setFileObjectCollection() {
            this.ImageCollection = new ObservableCollection<ImageObject>();
            return f => this.ImageCollection.Add(new ImageObject{
                displayName = f.Name, location = f.DirectoryName });
        }

        #region Gong dragdrop

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            // var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<String>();
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            ConsoleOutput = "trying to add: " + dragFileList.Count + " files";
            dropInfo.Effects = DragDropEffects.Copy;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            ConsoleOutput = dragFileList[0];

            dropInfo.Effects = DragDropEffects.Copy;
        }

        #endregion
    }
}
