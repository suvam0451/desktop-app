using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using testing.Modules.CombineTextures;
using testing.Modules.OpenCV;
using testing.ViewModels;

namespace testing.Pages.Workloads
{
    public partial class Workload_Default : Page
    {
        public Workload_Default()
        {
            InitializeComponent();
            DataContext = new VM_DefaultPage();
        }

        // private CombineTextures_Type1 handle;
        // private PlayVideo handle2;

        // Test function fired on button click...
        private void Fire_that(object sender, RoutedEventArgs e)
        {
            Frame tmp = new Frame();
            tmp.Content = new CombineTextures_Type1();
            
            ContentHolder.Children.Insert(1, tmp);
        }

        // Test function fired on button click...
        private void Change_frame(object sender, RoutedEventArgs e)
        {
        }

        private void Start_OpenCV(object sender, RoutedEventArgs e)
        {
            Frame tmp = new Frame();
            tmp.Content = new PlayVideo();
            ContentHolder.Children.Insert(1, tmp);
        }

        private void Start_TextureCombine(object sender, RoutedEventArgs e)
        {
            Frame tmp = new Frame();
            tmp.Content = new CombineTextures_Type1();
            ContentHolder.Children.Insert(1, tmp);
        }

        private void Start_Graphviz(object sender, RoutedEventArgs e)
        {

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
