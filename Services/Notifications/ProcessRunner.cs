using System.Diagnostics;

namespace NetPresence.Services.Notifications;

public class ProcessRunner : IProcessRunner
{
    public void Run(string fileName, params string[] arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var arg in arguments)
            psi.ArgumentList.Add(arg);
        Process.Start(psi);
    }
}
