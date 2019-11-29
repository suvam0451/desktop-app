using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
// using System.Windows.Forms;
using System.Runtime.InteropServices;
using testing.Libraries;
using daedalus_clr;
using GongSolutions.Wpf.DragDrop;
using System.IO;
using testing.UserControls;
using System.Windows.Threading;
using testing.Library;

namespace testing.ViewModels
{
    public class VM_YOLOSample : testing.BaseViewModel, GongSolutions.Wpf.DragDrop.IDropTarget
    {
        public String Title { get; set; } = "YOLO Sample";
        public String VideoPath { get; set; }

        public ObservableCollection<String> ImageQueue { get; }
        public ObservableCollection<String> VideoQueue { get; }
        public ObservableCollection<FrameworkElement> ConsoleStackPanel { get; }

        // Commands
        public ICommand ClearCommand { get; }
        public ICommand RunYOLO { get; }
        public ICommand RunDarknetVideo { get; }

        public VM_YOLOSample()
        {
            ImageQueue = new ObservableCollection<String>();
            ClearCommand = new RelayCommand(o => { ClearCommand_Impl(); }, o => true);
            RunYOLO = new RelayCommand(o => { RunYOLO_Impl(); }, o => true);
            RunDarknetVideo = new RelayCommand(o => { RunDarknetVideo_Impl(); }, o => true);

            ConsoleStackPanel = new ObservableCollection<FrameworkElement>();
        }

        // Boolean switches
        public bool ShowResultImage { get; set; } = true;
        public bool ProcessInBatch { get; set; } = false;

        #region DragDrop_Impl

        void RunDarknetVideo_Impl() {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projects", 
                                                    "Sample");

            CmdProcess Proc = new CmdProcess(DarknetDir);

            if (ShowResultImage)
            {
                Proc.AddToQueue(@"darknet.exe detector demo data/coco.data cfg/yolov3.cfg weights/yolov3.weights " + VideoPath);
                // darknet.exe detector demo data/ coco.data yolov3.cfg yolov3.weights http://192.168.0.80:8080/video?dummy=param.mjpg -i 0

                // Proc.QueueCopy(ImageCopyStartPath, ImageCopyEndPath);
            }
            else
            {
                Proc.AddToQueue(@"darknet.exe detector demo data/coco.data cfg/yolov3.cfg -dont_show weights/yolov3.weights -dont_show " + VideoPath);
            }
            Proc.ExecuteAndDestroy();
            // Proc.QueueCopy(ResultFileStartPath, ResultFileEndPath);
        }

        void ClearCommand_Impl() { ImageQueue.Clear(); }

        /* Runs YOLO Image classification for the queued list of images. */
        private async void RunYOLO_Impl()
        {
            if (ShowResultImage)
            {
                ConsoleStackPanel.Add(new MD_Definition
                {
                    Term = DateTime.Now.ToLongTimeString(),
                    Description = "Tagging " + ImageQueue.Count + " images.",
                    ExtraInfo = "(Images will be displayed everytime.)"
                });
            }
            else {
                ConsoleStackPanel.Add(new MD_Definition
                {
                    Term = DateTime.Now.ToLongTimeString(),
                    Description = "Tagging " + ImageQueue.Count + " images.",
                    ExtraInfo = "(Data will be collected in background.)"
                });
            }

            await (Task.Run(Async_ImageTagging_Start));

            ConsoleStackPanel.Add(new MD_Definition
            {
                Term = DateTime.Now.ToLongTimeString(),
                Description = "Tagged " + ImageQueue.Count + " images. Results can be found in /ImageData.",
                ExtraInfo = "(Showing tagged images as well.)"
            });
        }

        /* Processes individual image entries one by one.
         * NOTE: There is an overhead for reading the weight file every iteration.
         * Route to the other async function to batch process files
         */
        private async void Async_ImageTagging_Start() {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projects", "Sample");


            CmdProcess Proc = new CmdProcess(DarknetDir);
            foreach (string str in ImageQueue)
            {
                // Section to copy over image
                String ImageCopyStartPath = Path.Combine(DarknetDir, "predictions.jpg");
                String ImageCopyEndPath = Path.Combine(WorkspaceDir, "ImageTag", Path.GetFileName(str));

                // Check if file already exists (Bounding rectangle output file)
                String ResultFileStartPath = Path.Combine(DarknetDir, "output.txt");
                String ResultFileEndPath = Path.Combine(WorkspaceDir, "ImageData", Path.GetFileName(str) + ".txt");


                if (ShowResultImage)
                {
                    Proc.AddToQueue(@"darknet.exe detect cfg/yolov3.cfg weights/yolov3.weights " + str);
                    Proc.QueueCopy(ImageCopyStartPath, ImageCopyEndPath);
                }
                else
                {
                    Proc.AddToQueue(@"darknet.exe detect cfg/yolov3.cfg -dont_show weights/yolov3.weights " + str);
                }
                Proc.QueueCopy(ResultFileStartPath, ResultFileEndPath);
            }
            Proc.Execute();
            Proc.Destroy();
        }


        void IDropTarget.DragOver(IDropInfo dropInfo) {
            dropInfo.Effects = DragDropEffects.Copy;
            
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            int Counter = 0;
            foreach (String str in dragFileList)
            {
                if (System.IO.Path.GetExtension(str).ToUpperInvariant() == ".JPG") {
                    ImageQueue.Add(str);
                    Counter++;
                }
                else if (System.IO.Path.GetExtension(str).ToUpperInvariant() == ".PNG")
                {
                    ImageQueue.Add(str);
                    Counter++;
                }
            }
            dropInfo.Effects = DragDropEffects.Copy;

            ConsoleLog(Counter + " files out of " + dragFileList.Count + " files are valid.");
        }

        void ConsoleLog(String _Description, String _ExtraInfo = "") {
            ConsoleStackPanel.Add(new MD_Definition
            {
                Term = DateTime.Now.ToLongTimeString(),
                Description = _Description,
                ExtraInfo = _ExtraInfo
            });
        }

        #endregion
    }
}