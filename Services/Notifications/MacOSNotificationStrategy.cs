using System.Runtime.Versioning;

namespace NetPresence.Services.Notifications;

[SupportedOSPlatform("macos")]
public class MacOSNotificationStrategy : INotificationStrategy
{
    private readonly IProcessRunner _processRunner;

    public MacOSNotificationStrategy(IProcessRunner? processRunner = null)
    {
        _processRunner = processRunner ?? new ProcessRunner();
    }

    public void ShowNotification(string title, string message)
    {
        var escapedTitle = title.Replace("\\", "\\\\").Replace("\"", "\\\"");
        var escapedMessage = message.Replace("\\", "\\\\").Replace("\"", "\\\"");
        var script = $"display notification \"{escapedMessage}\" with title \"{escapedTitle}\"";
        _processRunner.Run("osascript", "-e", script);
    }
}
