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
    /// Interaction logic for Verbose_FilePicker.xaml
    /// </summary>
    public partial class Verbose_FilePicker : UserControl
    {
        /*
        *   Inherited properties: 
        *       Secret : The text element (in case we need custom initial display).
        *       Filter : Sets the filter for file selection.
        *       DirectoryMode : Sets if a directory should be selected.
        *   New properties:
        *       Caption : The caption of the card.
        */
        #region DependencyProps

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("Title goes here."));


        public bool SetDirectoryMode
        {
            get { return (bool)GetValue(SetDirectoryModeProperty); }
            set { SetValue(SetDirectoryModeProperty, value); }
        }
        public static readonly DependencyProperty SetDirectoryModeProperty =
            DependencyProperty.Register("SetDirectoryMode", typeof(bool), typeof(Verbose_FilePicker), new PropertyMetadata(true));

        public String CurrentVal
        {
            get { return (String)GetValue(CurrentValProperty); }
            set { SetValue(CurrentValProperty, value); }
        }
        public static readonly DependencyProperty CurrentValProperty =
            DependencyProperty.Register("CurrentVal", typeof(String), typeof(Verbose_FilePicker), new PropertyMetadata("String expected."));

        #endregion


        #region ctor
        public Verbose_FilePicker()
        {
            InitializeComponent();
        }
        #endregion
    }
}