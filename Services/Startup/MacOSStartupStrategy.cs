using System;
using System.IO;
using System.Runtime.Versioning;

namespace NetPresence.Services.Startup;

[SupportedOSPlatform("macos")]
public class MacOSStartupStrategy : IStartupStrategy
{
    private const string Label = "dev.jazzcat.net-presence";

    protected virtual string GetPlistPath() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "Library", "LaunchAgents", $"{Label}.plist");

    public bool IsEnabled() => File.Exists(GetPlistPath());

    public void Enable(string executablePath)
    {
        var path = GetPlistPath();
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var content = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
            <plist version="1.0">
            <dict>
                <key>Label</key>
                <string>{Label}</string>
                <key>ProgramArguments</key>
                <array>
                    <string>{executablePath}</string>
                </array>
                <key>RunAtLoad</key>
                <true/>
            </dict>
            </plist>
            """;

        File.WriteAllText(path, content);
    }

    public void Disable()
    {
        var path = GetPlistPath();
        if (File.Exists(path))
            File.Delete(path);
    }
}
