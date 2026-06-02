using System.Runtime.Versioning;

namespace NetPresence.Services.Notifications;

[SupportedOSPlatform("windows")]
public class WindowsNotificationStrategy : INotificationStrategy
{
    private readonly IProcessRunner _processRunner;

    public WindowsNotificationStrategy(IProcessRunner? processRunner = null)
    {
        _processRunner = processRunner ?? new ProcessRunner();
    }

    public void ShowNotification(string title, string message)
    {
        var escapedTitle = title.Replace("'", "''");
        var escapedMessage = message.Replace("'", "''");
        
        var psCommand = $"[void][System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms'); " +
                        $"$notification = New-Object System.Windows.Forms.NotifyIcon; " +
                        $"$notification.Icon = [System.Drawing.SystemIcons]::Information; " +
                        $"$notification.BalloonTipTitle = '{escapedTitle}'; " +
                        $"$notification.BalloonTipText = '{escapedMessage}'; " +
                        $"$notification.Visible = $true; " +
                        $"$notification.ShowBalloonTip(5000); " +
                        $"Start-Sleep -Seconds 1; " +
                        $"$notification.Dispose();";

        _processRunner.Run("powershell", $"-NoProfile -ExecutionPolicy Bypass -Command \"{psCommand}\"");
    }
}
