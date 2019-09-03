using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using testing.CombineTextures;
using System.ComponentModel;
using testing;
using daedalus_clr;
using Microsoft.Win32;
using System.IO;

//using System.Windows.Forms;

namespace testing.Modules.CombineTextures {
    /// Module : CombineTextures, Submodule : MetallicRoughness
    public partial class CombineTextures_Type1 : Page
    {
        // Most of back-end work is handled by this C++ wrapper class...
        public TextureCombine_Type1_Backend backend;

        public String[] Files { get; set; }
        public ImageSource _ImageSource {get; set;}

        //public delegate void MyDel();
        //public delegate void ManagedCallbackHandler(int _In);

        public CombineTextures_Type1()
        {
            InitializeComponent();
            DataContext = new VM_CombineTexture_Type1(this);
            backend = new TextureCombine_Type1_Backend();

            //backend._consoleText = Console.Text;
            // Bind the console...
            backend._consoleref = Console;
            backend._displayref = MainDisplay;
            backend._dispatcherref = Dispatcher; 
        }
        
        // Passes files from file drop event...
        public void FilesDropped(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                
                //BackgroundWorker worker = sender as BackgroundWorker;

                Files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //Thread thr = new Thread(FileDropAsync);
                //thr.Start();
                //backend.HandleFileDrop(SetMainImage, Files);

                ThreadStart mine = FileDropAsync;
                mine += SetMainImage;
                Thread thread = new Thread(mine) { IsBackground = true };
                thread.Start();

                //new Thread(() =>
                //{
                //    Thread.CurrentThread.IsBackground = true;
                //    /* run your code here */
                //    backend.HandleFileDrop(SaveMainImage, Files);
                //    //Console.WriteLine("Hello, world");
                //}).Start();

                //CallbackFunc callback = (value) => {
                //    MessageBox.Show(value.ToString());
                //};

                //ManagedCallbackHandler mine = new ManagedCallbackHandler(SetConsoleText);
                //backend.TestCallback(mine);

                //backend.TestCallback(new ManagedCallbackHandler(SetConsoleText));
                //backend.TestCallback(SetConsoleText);
                //backend.TestCallback(Marshal.GetFunctionPointerForDelegate(callback));
                //BackgroundWorker horse;
                //ThreadStart starter = simpletask;
                //this.Dispatcher.BeginInvoke(new MyDel(simpletask));
                //Thread thread = new Thread(starter) {
                //    IsBackground = true
                //};
                //thread.Start();
                //this.Dispatcher.Invoke()
                //this.Dispatcher.BeginInvoke(DispatcherPriority.Background, simpletask);
                //backend.TestCallback(SetConsoleText);
            }
            else {
                //Console.Text = "You did not drop files.";
            }
        }
        private void FileDropAsync() {
            backend.HandleFileDrop(SaveMainImage, Files);
        }
        private void SaveMainImage(ImageSource _In) {
            _ImageSource = _In;
        }

        // Responsible for updating the texture preview...
        private void SetMainImage() {
            //MessageBox.Show("Okay reached here");
            Dispatcher.BeginInvoke(
                new ThreadStart(() => MainDisplay.Source = _ImageSource));
            //MainDisplay.Source = _In;
        }

        private void SetConsoleText(int In) {
            Console.Text = In.ToString();
        }

        private void ClearPressed(object sender, RoutedEventArgs e) {
            // backend.PlayMovie(Files);
        }

        private void ChangeTheWorld(object sender, RoutedEventArgs e) {
            
        }
        private void SaveImage(object sender, RoutedEventArgs e) {

            // Used for save file dialog...
            String outdir = System.AppDomain.CurrentDomain.BaseDirectory + "output\\";

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "3 channel PNG(*.png)|*.png|3 channel JPG(*.jpg)|*.jpg";
            dlg.DefaultExt = "*.cs";
            
            if (System.IO.Directory.Exists(outdir) == false) {
                System.IO.Directory.CreateDirectory(outdir);
            }
           
            dlg.InitialDirectory = outdir;

            //MessageBox.Show(cwd + "output");

            if (dlg.ShowDialog() == true) {

                // Write out files
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)MainDisplay.Source));
                using (FileStream stream = new FileStream(dlg.FileName, FileMode.Create))
                encoder.Save(stream);
            }
        }


        //public void CalculateImage() {
        //   backend.HandleFileDrop(Files);
        //}
        #region DLL imports

        [DllImport("daedalus-core.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern string ProcessStrings(string strarr, int arrsize);

        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Add(int first, int second);

        [DllImport("daedalus-core.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Adder(int first, int second);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void CallbackFunc(int value);

        #endregion
    }
}
