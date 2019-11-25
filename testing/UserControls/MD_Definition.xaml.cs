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
    /// Interaction logic for MD_Definition.xaml
    /// </summary>
    public partial class MD_Definition : UserControl
    {
        public String Term
        {
            get { return (String)GetValue(TermProperty); }
            set { SetValue(TermProperty, value); }
        }
        public static readonly DependencyProperty TermProperty =
            DependencyProperty.Register("Term", typeof(String), typeof(MD_Definition), new PropertyMetadata("Term is needed."));

        public String Description
        {
            get { return (String)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(String), typeof(MD_Definition), new PropertyMetadata("Description is needed."));

        public String ExtraInfo
        {
            get { return (String)GetValue(ExtraInfoProperty); }
            set { SetValue(ExtraInfoProperty, value); }
        }
        public static readonly DependencyProperty ExtraInfoProperty =
            DependencyProperty.Register("ExtraInfo", typeof(String), typeof(MD_Definition), new PropertyMetadata(""));


        public MD_Definition()
        {
            InitializeComponent();
        }
    }
}
