using System.Windows;
using System.Runtime.InteropServices;

namespace testing
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ViewModel => WindowViewModel
            DataContext = new WindowViewModel(this);
        }

        // Test function fired on button click...
        private void Fire_that(object sender, RoutedEventArgs e)
        {
            //int retval = Add(10, 20);
            int[] data = { 1, 2, 3, 4, 5};
            //if (ReturnIntArraySize(data, 5) == 15)
            //{
                //MessageBox.Show(ReturnIntArraySize(data, 5).ToString());
            //receiveIR(data, 10);
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

        // Handlers window change events from main menu...
        private void Start_TextureCombine(object sender, RoutedEventArgs e) {

            // Texture Combine window
            //Workspace.Content = new TextureCombine();
            //Workspace.Content = new AppWelcome();
        }
        private void SendHelp(object sender, RoutedEventArgs e) {
            MessageBox.Show("Fuck off, incel");
        }

        private void Start_OpenCVVideo(object sender, RoutedEventArgs e) {

            MessageBox.Show("Fuck off, incel");
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
