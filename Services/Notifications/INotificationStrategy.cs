namespace NetPresence.Services.Notifications;

public interface INotificationStrategy
{
    void ShowNotification(string title, string message);
}
