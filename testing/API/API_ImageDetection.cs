using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing.Library;

namespace testing.API
{
    class API_ImageDetection
    {
        private API_FileSystem API_FS;

        public API_ImageDetection(API_FileSystem _API_FS) {
            API_FS = _API_FS;
        }

        public static void TagVideo(String VideoPath, bool PreviewOutput = false)
        {
            String VideoOutputPath = Path.Combine(Properties.Settings.Default.ProjectDir, "TaggedVideos", Path.GetFileName(VideoPath));

            CmdProcess Proc = new CmdProcess(Properties.Settings.Default.Darknet_Path);

            Proc.AddToQueue(@"darknet.exe detector demo data/coco.data cfg/yolov3.cfg weights/yolov3.weights " + VideoPath + " -out_filename " + VideoOutputPath);
            Proc.ExecuteAndDispose();
        }
    }
}
