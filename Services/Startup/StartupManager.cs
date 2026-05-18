using System;

namespace NetPresence.Services.Startup;

public static class StartupManager
{
    private static readonly IStartupStrategy _strategy = CreateStrategy();

    private static IStartupStrategy CreateStrategy()
    {
        if (OperatingSystem.IsMacOS())   return new MacOSStartupStrategy();
        if (OperatingSystem.IsWindows()) return new WindowsStartupStrategy();
        return new LinuxStartupStrategy();
    }

    public static bool IsEnabled() => _strategy.IsEnabled();

    public static void Apply(bool enable)
    {
        var exePath = Environment.ProcessPath
            ?? throw new InvalidOperationException("Cannot determine executable path.");

        if (enable)
            _strategy.Enable(exePath);
        else
            _strategy.Disable();
    }
}
