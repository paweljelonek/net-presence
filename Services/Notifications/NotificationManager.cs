using System;

namespace NetPresence.Services.Notifications;

public static class NotificationManager
{
    private static readonly INotificationStrategy _strategy = CreateStrategy();

    private static INotificationStrategy CreateStrategy()
    {
        if (OperatingSystem.IsMacOS())   return new MacOSNotificationStrategy();
        if (OperatingSystem.IsWindows()) return new WindowsNotificationStrategy();
        return new LinuxNotificationStrategy();
    }

    public static void ShowNotification(string title, string message)
    {
        _strategy.ShowNotification(title, message);
    }
}
