namespace NetPresence.Services.Notifications;

public interface IProcessRunner
{
    void Run(string fileName, params string[] arguments);
}
