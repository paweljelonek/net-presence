using System;
using System.IO;

namespace NetPresence.Services.Startup;

public class LinuxStartupStrategy : IStartupStrategy
{
    protected virtual string GetDesktopFilePath() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "autostart", "net-presence.desktop");

    public bool IsEnabled() => File.Exists(GetDesktopFilePath());

    public void Enable(string executablePath)
    {
        var path = GetDesktopFilePath();
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var content = $"""
            [Desktop Entry]
            Type=Application
            Name=Net Presence
            Exec={executablePath}
            Hidden=false
            NoDisplay=false
            X-GNOME-Autostart-enabled=true
            """;

        File.WriteAllText(path, content);
    }

    public void Disable()
    {
        var path = GetDesktopFilePath();
        if (File.Exists(path))
            File.Delete(path);
    }
}
