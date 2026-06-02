namespace NetPresence.Services.Notifications;

public class LinuxNotificationStrategy : INotificationStrategy
{
    private readonly IProcessRunner _processRunner;

    public LinuxNotificationStrategy(IProcessRunner? processRunner = null)
    {
        _processRunner = processRunner ?? new ProcessRunner();
    }

    public void ShowNotification(string title, string message)
    {
        var escapedTitle = title.Replace("\"", "\\\"");
        var escapedMessage = message.Replace("\"", "\\\"");
        _processRunner.Run("notify-send", $"\"{escapedTitle}\" \"{escapedMessage}\"");
    }
}
