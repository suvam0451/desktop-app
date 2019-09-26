using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using testing.Modules.CombineTextures;
using testing.Modules.OpenCV;

namespace testing.Pages.Workloads
{
    /// <summary>
    /// Interaction logic for Workload_Default.xaml
    /// </summary>
    public partial class Workload_Default : Page
    {
        public Workload_Default()
        {
            InitializeComponent();
        }

        private CombineTextures_Type1 handle;
        private PlayVideo handle2;
        //private TrollPage handle2;

        // Test function fired on button click...
        private void Fire_that(object sender, RoutedEventArgs e)
        {
            // Add our fancy UI interaction...
            Frame tmp = new Frame();
            handle = new CombineTextures_Type1();
            tmp.Content = handle;
            //ContentHolder.Children.Add(tmp);
            ContentHolder.Children.Insert(1, tmp);
        }

        // Test function fired on button click...
        private void Change_frame(object sender, RoutedEventArgs e)
        {
            //Workspace.Content = new AppWelcome();
            //int retval = Add(10, 20);
            int[] data = { 1, 2, 3, 4, 5 };
            //if (ReturnIntArraySize(data, 5) == 15)
            //{
            //MessageBox.Show(ReturnIntArraySize(data, 5).ToString());


            //receiveIR(data, 10);
        }

        private void Start_OpenCV(object sender, RoutedEventArgs e) {
            //handle.Height = 0;
            // MessageBox.Show("Fuck you, Go kill yourself.");

            // Add our fancy UI interaction...
            Frame tmp = new Frame();
            handle2 = new PlayVideo();
            tmp.Content = handle2;
            //ContentHolder.Children.Add(tmp);
            ContentHolder.Children.Insert(1, tmp);

            // Add fancy UI
            //Frame tmp = new Frame();
            //handle.MainDisplay.Source = "/images/trolling/sad.jpg";
            //handle2 = new TrollPage();
            //tmp.Content = handle2;
            //ContentHolder.Children.Insert(1, tmp);
        }

        // Handlers window change events from main menu...
        private void Start_TextureCombine(object sender, RoutedEventArgs e)
        {
            Frame tmp = new Frame();
            // handle = new CombineTextures_Type1();
            // tmp.Content = handle;
            tmp.Content = new CombineTextures_Type1();
            ContentHolder.Children.Insert(1, tmp);
        }
        private void Start_Graphviz(object sender, RoutedEventArgs e) {

        }


        //[DllImport("downtown_dll.dll", CallingConvention = CallingConvention.ThisCall)]
        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int receiveIR(uint[] data, int length);

        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Add(int first, int second);

        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern bool ReturnIntArraySize(int initial, int size);
        static extern int ReturnIntArraySize(int[] initial, int size);
    }
}
