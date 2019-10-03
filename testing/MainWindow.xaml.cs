using System.Windows;
using System.Runtime.InteropServices;

namespace testing
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
        }

        // Handlers window change events from main menu...
        private void Start_TextureCombine(object sender, RoutedEventArgs e) {
        }
        private void SendHelp(object sender, RoutedEventArgs e) {
            // MessageBox.Show("Fuck off, incel");
        }

        private void Start_OpenCVVideo(object sender, RoutedEventArgs e)
        {
        }

        #region DLL imports
        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int receiveIR(uint[] data, int length);

        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Add(int first, int second);

        [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int ReturnIntArraySize(int[] initial, int size);
        #endregion
    }

}
