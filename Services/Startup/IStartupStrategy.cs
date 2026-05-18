namespace NetPresence.Services.Startup;

public interface IStartupStrategy
{
    bool IsEnabled();
    void Enable(string executablePath);
    void Disable();
}
