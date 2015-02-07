using System;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;

namespace KillAtLogoff
{
    public sealed class Program
    {

        static void Main()
        {
            SystemEvents.SessionEnding += new SessionEndingEventHandler(KillProcess);
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        static void KillProcess(object sender, EventArgs e)
        {
            string processnames = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\KILLAT\\KillAtLogoff", "ProcessNames", "");
            string[] arrProcesses = processnames.Split(',');
            foreach (string processname in arrProcesses)
            {
                Process CurrentProcess = Process.GetCurrentProcess();
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.ProcessName.Contains(processname.ToString()) && (proc.SessionId == CurrentProcess.SessionId))
                    {
                        proc.Kill();
                        Console.WriteLine("Kill " + proc.ProcessName + " " + proc.Id.ToString());
                        Thread.Sleep(3000);
                        break;
                    }
                }
            }
            System.Environment.Exit(0);
        }

    }
}
