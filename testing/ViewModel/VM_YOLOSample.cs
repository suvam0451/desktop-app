﻿using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.Library;
using testing.UserControls;

namespace testing.ViewModels
{
    public class VM_YOLOSample : testing.BaseViewModel, GongSolutions.Wpf.DragDrop.IDropTarget
    {
        #region Variables

        public String Title { get; set; } = "YOLO Sample";
        public String VideoPath { get; set; }
        public int FramesToDrop { get; set; }
        public ObservableCollection<String> ImageQueue { get; }
        public ObservableCollection<String> VideoQueue { get; }
        public ObservableCollection<FrameworkElement> ConsoleStackPanel { get; }
        public List<int> FramesToDropList { get; }
        // public int FramesToDrop { get; }

        // Commands
        public ICommand ClearCommand { get; }
        public ICommand RunYOLO { get; }
        public ICommand RunDarknetVideo { get; }
        public ICommand GenerateImageBatch { get; }

        // Boolean switches
        public bool ShowResultImage { get; set; } = true;
        public bool ProcessInBatch { get; set; } = false;
        public bool IsVideoValid { get; set; } = false;
        public bool AllowVideoUI { get; set; } = false;

        #endregion

        #region Constructor

        public VM_YOLOSample()
        {
            ImageQueue = new ObservableCollection<String>();
            ClearCommand = new RelayCommand(o => { ClearCommand_Impl(); }, o => true);
            RunYOLO = new RelayCommand(o => { RunYOLO_Impl(); }, o => true);
            RunDarknetVideo = new RelayCommand(o => { RunDarknetVideo_Impl(); }, o => true);
            GenerateImageBatch = new RelayCommand(o => { GenerateImageBatch_Impl(); }, o => true);

            ConsoleStackPanel = new ObservableCollection<FrameworkElement>();

            FramesToDropList = new List<int>();
            FramesToDropList.Add(5);
            FramesToDropList.Add(10);
            FramesToDropList.Add(15);
            FramesToDropList.Add(20);

            // If a valid path to video was configured, open up the controls. MADE IT TRUE TEMPORARILY
            // IsVideoValid = (String.IsNullOrEmpty(VideoPath)) ? false : true;
            IsVideoValid = true;
        }

        #endregion

        #region ICommand Implementations

        void ClearCommand_Impl() { ImageQueue.Clear(); }

        /* Runs YOLO Image classification for the queued list of images. */
        private async void RunYOLO_Impl()
        {
            if (ShowResultImage)
            {
                ConsoleLog("Tagging " + ImageQueue.Count + " images.", "(Close image popups to advance.)");
            }
            else
            {
                ConsoleLog("Tagging " + ImageQueue.Count + " images.", "(Silent mode.)");
            }
            // MessageBox.Show(Environment.CurrentDirectory);

            Stopwatch watch = new Stopwatch();
            watch.Start();


            List<String> OneQueue = new List<String>();
            foreach (var str in ImageQueue)
            {
                OneQueue.Add(str);
            }

            if (ShowResultImage)
            {
                await (Task.Run(() => { TagImages_AlwaysShow_Task(OneQueue); }).ConfigureAwait(true));
            }
            else
            {
                await (Task.Run(() => { TagImages_NeverShow_Task(OneQueue); }).ConfigureAwait(true));
            }

            // TagImages_AlwaysShow_Task

            watch.Stop();
            ConsoleLog("Tagged " + ImageQueue.Count + " images in " + Benchmarking.TimePassed(watch), "(Showing tagged images as well.)");
        }


        private async void RunDarknetVideo_Impl()
        {
            ConsoleLog("Viewing " + Path.GetFileName(VideoPath) + ". You can continue using the app.");
            Stopwatch watch = new Stopwatch();
            watch.Start();

            await (Task.Run(Async_VideoTagging_Start).ConfigureAwait(true));

            watch.Stop();
            ConsoleLog("Video completed in " + Benchmarking.TimePassed(watch), "Find ouput in videos folder.");
        }

        private async void GenerateImageBatch_Impl()
        {
            ConsoleLog("Batching images from " + Path.GetFileName(VideoPath));
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Populates images in "{ProjectDir}/ImageBatch"...
            await (Task.Run(GenerateImageBatch_Task).ConfigureAwait(true));
            ConsoleLog("(1/3) Batching complete. Preparing list...");

            // Makes a list of images from "{ProjectDir}/ImageBatch"...
            await (Task.Run(MakeImageQueueFileFromFolder_Task).ConfigureAwait(true));
            ConsoleLog("(2/3) List prepared. Proceeding with detection...");

            // Processing all the images... 
            // await (Task.Run(ImageBatchProcessQueue_Task).ConfigureAwait(true));
            String QueueFileDir = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageBatch", "ImageQueue.txt");

            // await (Task.Run(() => { 
            //     ImageTagging_Task(QueueFileDir); 
            // }).ConfigureAwait(true));
            // 
            // ConsoleLog("(3/3) Completed tagging images.");

            watch.Stop();
            ConsoleLog("Video taggin completed in " + Benchmarking.TimePassed(watch));
        }

        #endregion

        #region Asynchronous Functions

        // STEP 1
        private void GenerateImageBatch_Task()
        {
            String BatchOutputDir = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageBatch");

            CmdProcess Proc = new CmdProcess(Environment.CurrentDirectory);
            Proc.AddToQueue(@"VideoToImage.exe " + BatchOutputDir + " cap_video " + VideoPath + " " + "10");
            Proc.ExecuteAndDestroy();
        }

        // STEP 2
        private void MakeImageQueueFileFromFolder_Task()
        {
            String CWD = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageBatch");

            CmdProcess Proc = new CmdProcess(CWD);
            Proc.AddToQueue("dir /s/b *.jpg > ImageQueue.txt");
            Proc.Execute();
            Proc.Dispose();
        }

        // STEP 3
        private void ImageBatchProcessQueue_Task()
        {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String QueueFileDir = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageBatch", "ImageQueue.txt");
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;
            String VideoName = Path.GetFileName(VideoPath);

            CmdProcess Proc = new CmdProcess(DarknetDir);
            if (ShowResultImage)
            {
                Proc.AddToQueue(@"darknet.exe detect cfg/yolov3.cfg weights/yolov3.weights " + " > " + QueueFileDir);
                // Proc.QueueCopy(ImageCopyStartPath, ImageCopyEndPath);
            }
            else
            {
                Proc.AddToQueue(@"darknet.exe detect cfg/yolov3.cfg -dont_show weights/yolov3.weights " + " > " + QueueFileDir);
            }
            // Proc.QueueCopy(ResultFileStartPath, ResultFileEndPath);

            int counter = 0;
            string line;


            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }



        private void Async_VideoTagging_Start()
        {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;
            String VideoOutputPath = Path.Combine(WorkspaceDir, "Videos") + Path.GetFileName(VideoPath);

            CmdProcess Proc = new CmdProcess(DarknetDir);
            Proc.AddToQueue(@"darknet.exe detector demo data/coco.data cfg/yolov3.cfg weights/yolov3.weights " + VideoPath + " -out_filename " + VideoOutputPath);
            // Proc.QueueCopy(Path.Combine(DarknetDir, VideoName), Path.Combine(WorkspaceDir, "Videos"));
            // Proc.AddToQueue("del " + VideoName);
            Proc.ExecuteAndDestroy();
            Proc.Dispose();
        }

        private void FileToImageList_Task(String ImageListPath, List<String> _In)
        {
            String line;

            System.IO.StreamReader file = new System.IO.StreamReader(ImageListPath);
            while ((line = file.ReadLine()) != null)
            {
                _In.Add(line);
            }
            file.Close();
        }


        private void ImageTagging_Task(List<String> ImageList, bool ShowImages = false, String ImageOutputPath = "ImageTag", String DataOutputPath = "ImageData")
        {
            // MessageBox.Show();
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;

            CmdProcess Proc = new CmdProcess(DarknetDir);
            foreach (String str in ImageList)
            {
                // Section to copy over image
                String ImageCopyStartPath = Path.Combine(DarknetDir, "predictions.jpg");
                String ImageCopyEndPath = Path.Combine(WorkspaceDir, ImageOutputPath, Path.GetFileName(str));

                // Check if file already exists (Bounding rectangle output file)
                String ResultFileStartPath = Path.Combine(DarknetDir, "result.json");
                String ResultFileEndPath = Path.Combine(WorkspaceDir, DataOutputPath, Path.GetFileName(str) + ".txt");

                if (ShowImages)
                {
                    Proc.AddToQueue(@"darknet.exe detector test cfg/coco.data cfg/yolov3.cfg weights/yolov3.weights -ext_output -out result.json " + str);
                    Proc.QueueCopy(ImageCopyStartPath, ImageCopyEndPath);
                }
                else
                {
                    Proc.AddToQueue(@"darknet.exe detect cfg/yolov3.cfg weights/yolov3.weights -ext_output -dont_show -out result.json " + str);
                }
                Proc.QueueCopy(ResultFileStartPath, ResultFileEndPath);
            }
            Proc.ExecuteAndDestroy();
        }

        private void TagImages_AlwaysShow_Task(List<String> ImageList, String ImageOutputPath = "ImageTag", String DataOutputPath = "ImageData")
        {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;

            CmdProcess Proc = new CmdProcess(DarknetDir);
            foreach (String str in ImageList)
            {
                // Section to copy over image
                String ImageCopyStartPath = Path.Combine(DarknetDir, "predictions.jpg");
                String ImageCopyEndPath = Path.Combine(WorkspaceDir, ImageOutputPath, Path.GetFileName(str));
                String ResultFileEndPath = Path.Combine(WorkspaceDir, DataOutputPath, Path.GetFileName(str) + ".json");
                
                Proc.AddToQueue(@"darknet.exe detector test cfg/coco.data cfg/yolov3.cfg weights/yolov3.weights -ext_output -out " + ResultFileEndPath + " " + str);
                Proc.QueueCopy(ImageCopyStartPath, ImageCopyEndPath);
            }
            Proc.ExecuteAndDestroy();
        }

        private void TagImages_NeverShow_Task(String InputFile) { 
            
        }
        private void TagImages_NeverShow_Task(List<String> ImageList, String DataOutputPath = "ImageData")
        {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;
            String ResultFileEndPath = Path.Combine(WorkspaceDir, DataOutputPath, "_Result.json");
            String ImageListDir = Path.Combine(Environment.CurrentDirectory, "Resources/ImageList.txt");

            CmdProcess Proc = new CmdProcess(DarknetDir);
            Proc.AddToQueue("type nul > " + ImageListDir);
            Proc.AddToQueue("type nul > " + ResultFileEndPath);
            Proc.ExecuteAndDestroy();
            Proc.Dispose();
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "Resources", "ImageList.txt")))
            {
                foreach (string line in ImageList)
                    outputFile.WriteLine(line);
            }


            CmdProcess Proc2 = new CmdProcess(DarknetDir);
            Proc2.AddToQueue(@"darknet.exe detector test cfg/coco.data cfg/yolov3.cfg weights/yolov3.weights -ext_output -dont_show -out " + ResultFileEndPath + " < " + ImageListDir);
            Proc2.ExecuteAndDestroy();
            Proc2.Dispose();
        }

        /* Processes individual image entries one by one.
         * NOTE: There is an overhead for reading the weight file every iteration.
         * Route to the other async function to batch process files
         */
        private void Async_ImageTagging_Start()
        {
            String DarknetDir = Properties.Settings.Default.Darknet_Path;
            String WorkspaceDir = Properties.Settings.Default.ProjectDir;

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
            Proc.ExecuteAndDestroy();
        }

        #endregion

        #region DragDrop_Impl

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList();
            int Counter = 0;
            foreach (String str in dragFileList)
            {
                if ((System.IO.Path.GetExtension(str).ToUpperInvariant() == ".JPG") ||
                    (System.IO.Path.GetExtension(str).ToUpperInvariant() == ".PNG"))
                {
                    ImageQueue.Add(str);
                    Counter++;
                }
            }

            dropInfo.Effects = DragDropEffects.Copy;
            ConsoleLog(Counter + "/" + dragFileList.Count + " files are valid entries.");
        }

        #endregion

        #region Utility(Console)

        /* Utility function to update the local console */
        void ConsoleLog(String _Description, String _ExtraInfo = "")
        {
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