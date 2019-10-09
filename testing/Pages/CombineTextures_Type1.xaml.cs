using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using daedalus_clr;
using Microsoft.Win32;
using System.IO;
using NLua;
using testing.Libraries;
using testing.ViewModels;

namespace testing.Pages
{
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
            // this.DataContext = new 

            // DataContext = new VM_CombineTexture_Type1(this);
            DataContext = new VM_CombineTexture();
            backend = new TextureCombine_Type1_Backend();

            //backend._consoleText = Console.Text;
            // Bind the console...
            backend._consoleref = Console;
            
            // backend._dispatcherref = Dispatcher; 
        }
        
        // Passes files from file drop event...
        public void FilesDropped(object sender, DragEventArgs e)
        {
            // MessageBox.Show("yeet");

            Lua state = new Lua();

            // int res = (int)state.DoString("return 10 + 3*(5 + 2)")[0];
            var res = state.DoString("return 10 + 3*(5 + 2)")[0];

            MessageBox.Show(res.ToString());

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //Thread thr = new Thread(FileDropAsync);
                //thr.Start();
                //backend.HandleFileDrop(SetMainImage, Files);

                // ThreadStart mine = FileDropAsync;
                // mine += SetMainImage;
                // Thread thread = new Thread(mine) { IsBackground = true };
                // thread.Start();

                //{
                //    Thread.CurrentThread.IsBackground = true;
                //    /* run your code here */
                //    backend.HandleFileDrop(SaveMainImage, Fil
                // backend.HandleFileDrop(SaveMainImage, Files, ConsoleDummy);
                backend.HandleFileDrop(SaveMainImage, Files);
                //new Thread(() =>es);
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
        private void SaveMainImage(ImageSource _In) {
            _ImageSource = _In;
        }
        private void ConsoleDummy(String _In)
        {
            // _ImageSource = _In;
        }

        // Responsible for updating the texture preview...
        private void SetMainImage() {
            //MessageBox.Show("Okay reached here");
            // Dispatcher.BeginInvoke(
                // new ThreadStart(() => MainDisplay.Source = _ImageSource));
            //MainDisplay.Source = _In;
        }

        private void SetConsoleText(int In) {
            Console.Text = In.ToString();
        }

        private void ClearPressed(object sender, RoutedEventArgs e) {
            // clsGraph mine = new clsGraph(4);
            // mine.AddEdge(0, 1);
            // mine.AddEdge(0, 2);
            // mine.AddEdge(1, 2);
            // mine.AddEdge(2, 0);
            // mine.AddEdge(2, 3);
            // mine.AddEdge(3, 3);
            // 
            // mine.PrintAdjacencyMatrix();
            // mine.DFS(2);

            MST mine2 = new MST(5);

            int[,] graph = new int[,] { { 0, 2, 0, 6, 0 },
                                      { 2, 0, 3, 8, 5 },
                                      { 0, 3, 0, 0, 7 },
                                      { 6, 8, 0, 0, 9 },
                                      { 0, 5, 7, 9, 0 } };
            mine2.primMST(graph);
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
                // encoder.Frames.Add(BitmapFrame.Create((BitmapSource)MainDisplay.Source));
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
