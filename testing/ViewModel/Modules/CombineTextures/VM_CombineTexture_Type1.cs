// Plug-in to handle drag drop operations

using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace testing.Modules.CombineTextures
{
    // Using fodyweaver to weave "PropertyChangedEventHnadler"
    [AddINotifyPropertyChangedInterface]
    public class VM_CombineTexture_Type1 : BaseViewModel
    {
        #region private
        /// The window this view model controls
        private Page _mPage;
        #endregion


        #region public
        // Collapsable content height
        private float CollpsedContent_Height { get; set; } = 200;

        // Collapsable content height
        public Visibility DetailSegment_Height { get; set; } = Visibility.Visible;

        public Visibility ShowDetails { get; set; } = Visibility.Visible;
        // Height of the page
        public float ModuleHeight { get; set; } = 450;

        public bool IsMinimized { get; set; } = false;
        #endregion
        public string ConsoleOutput { get; set; } = "idle";

        public List<string> HeldData { get; set; } = new List<string>();

        //private RelayCommand<object> _OneParamaRelay;
        #region constructor
        /// Default constructor
        public VM_CombineTexture_Type1(Page pageIn)
        {
            _mPage = pageIn;

            // Routing commands
            CollapseDetails = new RelayCommand(o => {
                IsMinimized = (IsMinimized == false) ? true : false;
                DetailSegment_Height = (DetailSegment_Height == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                ShowDetails = (ShowDetails == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                ModuleHeight = (ModuleHeight == 450) ? 300 : 450;
            }, o => true );
        }
        #endregion

        #region Commands
        /// Usage : Hide extra details when minimize is pressed.
        public ICommand CollapseDetails { get; set; }
        // Fires on : Dropping anything onto the drop field
        //public ICommand HandleFileDrop { get; set; }

        #endregion

        #region Method Declarations
        public void HandleFileDrop() {
            MessageBox.Show("boop");
        }

        public void FilesDropped(object sender, DragEventArgs e)
        {
            //FilesDropped?.Invoke(sender, e);
            // Check if files were dropped...
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                MessageBox.Show(files.Length.ToString());
            }
            else
            {
                ConsoleOutput = "You did not drop files.";
            }
        }

        #endregion

        #region GongSolutions DragDrop

        //public void DragOver(IDropInfo dropInfo)
        //{
        //    var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
        //    //MessageBox.Show("boop");
        //    dropInfo.Effects = DragDropEffects.Copy;
        //    
        //    return;
        //}
        //
        //public void Drop(IDropInfo dropInfo)
        //{
        //    //
        //    var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
        //    HeldData.Clear();
        //    //HeldData = dragFileList;
        //
        //    //MessageBox.Show(dragFileList.Count().ToString());
        //    //MessageBox.Show(HeldData.Count().ToString());
        //    //HeldData = new IEnumerable<string>();
        //    //dropInfo.Effects = dragFileList.Any(item =>
        //    //{
        //    //    var extension = Path.GetExtension(item);
        //    //    return extension != null && extension.Equals(".html");
        //    //}) ? DragDropEffects.Copy : DragDropEffects.None;
        //
        //    IEnumerator<string> iter = dragFileList.GetEnumerator();
        //    //foreach (string item in dragFileList) {
        //    //    HeldData.Append(item);
        //    //}
        //        while (iter.MoveNext()) {
        //            //if (Path.GetExtension(iter.Current).Equals(".html")) {
        //                HeldData.Add(iter.Current);
        //            
        //            //}
        //        }
        //    //HeldData = dragFileList;
        //
        //    MessageBox.Show(HeldData.Count().ToString());
        //    MessageBox.Show(HeldData[0]);
        //    ProcessStrings(HeldData.ToArray(), HeldData.Count);
        //    MessageBox.Show(HeldData[0]);
        //    dropInfo.Effects = DragDropEffects.Copy;
        //}

        #endregion

        #region DLL imports
        [DllImport("daedalus-core.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void ProcessStrings(String[] strarr, int arrsize);

        #endregion
    }
}
