using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testing.UserControls
{
    /// <summary>
    /// Interaction logic for OutputLog.xaml
    /// </summary>
    public partial class OutputLog : UserControl
    {
        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("Output Console."));

        public OutputLog()
        {
            InitializeComponent();
        }
    }
}
