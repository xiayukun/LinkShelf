using System.Diagnostics;
using ShowWhatProcessLocksFile.LockFinding;

namespace ShowWhatProcessLocksFile.Utils;

internal static class ProcessKiller
{
    public static void Kill(IEnumerable<ProcessInfo> processes)
    {
        var processList = processes.ToList();
        foreach (var p in processList)
        {
            try
            {
                var process = Process.GetProcessById(p.Pid);
                process.Kill();
            }
            catch (ArgumentException)
            {
            }
        }

        foreach (var p in processList)
        {
            try
            {
                var process = Process.GetProcessById(p.Pid);
                var res = process.WaitForExit(3000);
                if (!res)
                {
                    throw new Exception($"Timeout waiting for a process to exit: Pid={p.Pid} Name='{p.ProcessName}'");
                }
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
