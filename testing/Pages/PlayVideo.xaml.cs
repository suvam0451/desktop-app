﻿using System;
using System.Windows;
using System.Windows.Controls;
using daedalus_clr;
using testing.ViewModels;

namespace testing.Pages
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
            // DataContext = new VM_DefaultPage();
            DataContext = new VM_CVPlayVideo();
            // this.DataContext = new WindowViewModel(this);
            backend = new TextureCombine_Type1_Backend();
        }

        // Handle media file drop...
        private void MovieDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                MessageBox.Show("lmao yeet");
                //BackgroundWorker worker = sender as BackgroundWorker;

                Files = (string[])e.Data.GetData(DataFormats.FileDrop);
                StartVideoProcessing(Files[0]);
                
                // backend.HandleMediaDrop(Files);
                
            }
            else
            {
                //Console.Text = "You did not drop files.";
            }

        }
        private void StartVideoProcessing(String In) {
            backend.HandleMediaDrop(In);
        }

        private void ClearPressed(object sender, RoutedEventArgs e) {
            // Handle OpenCV video  without locking fs
            // backend.HandleMediaDrop(Files);
        }

        private void FilesDropped(object sender, DragEventArgs e) {
            // Do nothing
        }
    }
}
