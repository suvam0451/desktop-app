using System.Windows;

namespace testing
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
        }

        // #region DLL imports
        // [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        // static extern int receiveIR(uint[] data, int length);
        // 
        // [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        // static extern int Add(int first, int second);
        // 
        // [DllImport("devops_test.dll", CallingConvention = CallingConvention.Cdecl)]
        // static extern int ReturnIntArraySize(int[] initial, int size);
        // #endregion
    }
}
