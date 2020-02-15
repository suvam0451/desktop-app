using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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

        public void ExecuteAndDispose()
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

    public class Powershell_Custom {
        Runspace rs;
        PowerShell ps;

        public Powershell_Custom(String CWD, String ScriptPath) {
            rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            rs.SessionStateProxy.Path.SetLocation(CWD);
            ps = PowerShell.Create();
            ps.AddScript(ScriptPath);
            ps.Runspace = rs;
        }

        public void AddArgument(String Arg) {
            ps.AddArgument(Arg);
        }

        public void AddParameter(String name, Object value) {
            ps.AddParameter(name, value);
        }

        public void Execute()
        {
            ps.Invoke();
            rs.Close();
        }
    }

    public class PowerShell_Process : System.Diagnostics.Process
    {
        public PowerShell_Process(String CWD)
        {
            StartInfo.WorkingDirectory = CWD;
            StartInfo.FileName = @"powershell.exe";
            StartInfo.UseShellExecute = false;
            StartInfo.CreateNoWindow = true;
            StartInfo.RedirectStandardInput = true;
        }

        public PowerShell_Process(String CWD, String ScriptPath, String Parameters)
        {
            StartInfo.WorkingDirectory = CWD;
            StartInfo.FileName = @"powershell.exe";
            StartInfo.UseShellExecute = false;
            StartInfo.CreateNoWindow = false;
            StartInfo.RedirectStandardInput = true;
            // StartInfo.Arguments = "-ExecutionPolicy Bypass -NoLogo -NoExit -file " + ScriptPath + " " + Parameters;
            StartInfo.Arguments = " -ExecutionPolicy Bypass -NoLogo -NoExit -file " + ScriptPath + " " + Parameters;
            StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        }

        // Creates a copy of the class, executes functions and cleans up...
        public static void Invoke(String CWD, String ScriptPath, String Parameters) {
            PowerShell_Process tmp = new PowerShell_Process(CWD, ScriptPath, Parameters);
            tmp.Start();
            // tmp.WaitForExit();
            // tmp.Dispose();
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
