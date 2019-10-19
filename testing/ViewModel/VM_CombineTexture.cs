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
using daedalus_clr;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace testing.ViewModels
{
    public class VM_CombineTexture : BaseViewModel, IDropTarget
    {
        private TextureCombine_Type1_Backend backend;
        private Dictionary<String, String> FileData;

        public String ConsoleOutput { get; set; } = "Idle";
        public ImageObject SelectedFileObject;
        public ImageSource ImageSource { get; set; } = new BitmapImage(new Uri($"pack://application:,,,/images/logo/heart.png"));
        public Image MainDisplay { get; set; } = new Image();
        public ObservableCollection<ImageObject> ImageCollection { get; set; }
        public ICommand UpdateImageList { get; set; }

        public VM_CombineTexture() {
            FileData = new Dictionary<String, String>();

            ImageCollection = new ObservableCollection<ImageObject>();

            UpdateImageList = new RelayCommand(o => { ListImagePaths(); },
                                                o => true );

            backend = new TextureCombine_Type1_Backend();
        }
        private void ListImagePaths() {
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
            dropInfo.Effects = DragDropEffects.Copy;

            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            String[] fileList = (string[])((DataObject)dropInfo.Data).GetData(DataFormats.FileDrop);

            ImageCollection.Clear();
            for (int i = 0; i < dragFileList.Count; i++) {
                ImageCollection.Add(new ImageObject
                {
                    displayName = dragFileList[i],
                    location = dragFileList[i]
                });
            }

            if (fileList.Count() > 0) {
                // backend.HandleFileDrop(SaveMainImage, fileList, UpdateConsole);
                backend.HandleFileDrop(SaveMainImage, fileList);
            }
        }
        private void SaveMainImage(ImageSource _In)
        {
            ImageSource = _In;
        }
        private void UpdateConsole(String _In)
        {
            ConsoleOutput = _In;
        }

        private void ParseImages(String[] _In, Dictionary<String,String> _Dict){
            foreach (String it in _In) {
                Regex rx;
                MatchCollection matches;

                rx = new Regex("(albedo|color)+(.?)+(.png|.jpg)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("albedo", it); continue; }

                rx = new Regex("(normal)+(.?)+(.png|.jpg)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("normal", it); continue; }

                rx = new Regex("(height|displacement|bump)+(.?)+(.png|.jpg)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("height", it); continue; }

                rx = new Regex("(ao|ambient|occlusion)+(.?)+(.png|.jpg)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("ao", it); continue; }

                rx = new Regex("(rough)+(.?)+(.png|.jpg)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("roughness", it); continue; }

                rx = new Regex("(metal)+(.?)+(.png|.jpg)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                matches = rx.Matches(it);
                if (matches.Count > 0) { FileData.Add("metallic", it); continue; }
            }
        }
        #endregion
    }
}
