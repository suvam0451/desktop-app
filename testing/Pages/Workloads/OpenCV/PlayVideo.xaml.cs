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
using daedalus_clr;

namespace testing.Modules.OpenCV
{
    // Interaction logic for PlayVideo.xaml
    public partial class PlayVideo : Page
    {
        // Most of back-end work is handled by this C++ wrapper class...
        public TextureCombine_Type1_Backend backend;
        public String[] Files { get; set; }

        public PlayVideo()
        {
            InitializeComponent();
            backend = new TextureCombine_Type1_Backend();

            // Bind references to dll
            backend._consoleref = Console;
            backend._displayref = MainDisplay;
            backend._dispatcherref = Dispatcher;
        }

        // Handle media file drop...
        private void MovieDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                //BackgroundWorker worker = sender as BackgroundWorker;

                Files = (string[])e.Data.GetData(DataFormats.FileDrop);
            }
            else
            {
                //Console.Text = "You did not drop files.";
            }

        }

        private void ClearPressed(object sender, RoutedEventArgs e) {
            // Handle OpenCV video  without locking fs
            backend.HandleMediaDrop(Files);
        }

        private void FilesDropped(object sender, DragEventArgs e) {
            // Do nothing
        }

        private void SaveImage(object sender, RoutedEventArgs e) {
            // DO nothing
        }
    }
}
