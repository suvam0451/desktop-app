using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using testing.Libraries;

namespace testing.API
{
    public enum SpecialFolder
    {
        ImageData,
        ImageTagged,
        VideoData,
        VideoTag,
        ImageBatch
    } 

    class API_FileSystem
    {
        private String ProjectDirectory;
        private String ProjectName;
        
        public String DarknetDir { get; }
        public String WorkspaceDir { get; }

        public API_FileSystem() {
            ProjectDirectory = Properties.Settings.Default.ProjectDir;
            ProjectName = Properties.Settings.Default.ProjectName;

            DarknetDir = Properties.Settings.Default.Darknet_Path;
            WorkspaceDir = Properties.Settings.Default.ProjectDir;
        }


        /// <summary>
        /// Returns the folder the user chooses/creates relative to the RootDir
        /// </summary>
        /// <param name="RootDir"></param>
        /// <returns></returns>
        public String RequestFolderRelativeToRootDir(String RootDir)
        {
            return FileSystem.RequestFolderRelativeToRootDir(RootDir);
        }

        public String GetSpecialFolderPath(SpecialFolder FolderPath) {
            String retval;
            switch (FolderPath)
            {
                case SpecialFolder.ImageData: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageData"); break; }
                case SpecialFolder.ImageTagged: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageTagged"); break; }
                case SpecialFolder.VideoData: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "VideoData"); break; }
                case SpecialFolder.VideoTag: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "VideoTag"); break; }
                case SpecialFolder.ImageBatch: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "ImageBatch"); break; }
                default: { retval = Path.Combine(Properties.Settings.Default.ProjectDir, "Trashbin"); break; }
            }
            return retval;
        }

        public String RequestUniqueFolder(SpecialFolder FolderPath) {
            return "";
        }

        public String RequestFileRelativeToRootDir(String RootDir)
        {
            return "";
        }
    }
}
