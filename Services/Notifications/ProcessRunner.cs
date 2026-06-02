using System.Diagnostics;

namespace NetPresence.Services.Notifications;

public class ProcessRunner : IProcessRunner
{
    public void Run(string fileName, string arguments)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }
}
