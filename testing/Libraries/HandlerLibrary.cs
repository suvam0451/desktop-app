using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.Library
{
    public class CmdProcess : System.Diagnostics.Process
    {

        Queue<String> CommandList { get; set; } = new Queue<String>();

        public CmdProcess()
        {
            // CommandList = new Queue<String>();
            StartInfo.FileName = @"cmd.exe";
            StartInfo.RedirectStandardInput = true;
            StartInfo.UseShellExecute = false;
        }
        public CmdProcess(String CWD)
        {
            // CommandList = new Queue<String>();
            StartInfo.WorkingDirectory = CWD;
            StartInfo.FileName = @"cmd.exe";
            StartInfo.RedirectStandardInput = true;
            StartInfo.UseShellExecute = false;
        }

        public void Execute()
        {
            this.Start();

            StreamWriter myStream = this.StandardInput;

            while (CommandList.Count > 0) {
                String line = CommandList.Dequeue();
                myStream.WriteLine(line);
            }
            myStream.Close();
        }

        public void Destroy() {
            this.WaitForExit();
            this.Dispose();
        }

        public void ExecuteAndDestroy()
        {
            Execute();
            Destroy();
        }

        public void AddToQueue(String _Command) {
            CommandList.Enqueue(_Command);
        }

        public void QueueCopy(String Origin, String Dest,  bool ForceCopy = true) {
            if (ForceCopy)
            {
                CommandList.Enqueue("copy /y " + Origin + " " + Dest);
            }
            else {
                CommandList.Enqueue("copy " + Origin + " " + Dest);
            }

        }
    }

    public class Benchmarking {
        public static String TimePassed(Stopwatch watch)
        {
            TimeSpan ts = watch.Elapsed;
            String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            return elapsedTime;
        }
    }

    public class FileHandler
    {
        public static void CopyFile(CmdProcess _Proc) { 
            
        } 
    }
}
