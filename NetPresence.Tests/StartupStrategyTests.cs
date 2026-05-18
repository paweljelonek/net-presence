#pragma warning disable CA1416

using System;
using System.IO;
using NetPresence.Services.Startup;
using Xunit;

namespace NetPresence.Tests;

public class MacOSStartupStrategyTests : IDisposable
{
    private readonly string _tempLaunchAgentsDir;
    private readonly MacOSStartupStrategyTestable _strategy;

    public MacOSStartupStrategyTests()
    {
        _tempLaunchAgentsDir = Path.Combine(Path.GetTempPath(), $"LaunchAgents_{Guid.NewGuid()}");
        _strategy = new MacOSStartupStrategyTestable(_tempLaunchAgentsDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempLaunchAgentsDir))
            Directory.Delete(_tempLaunchAgentsDir, recursive: true);
    }

    [Fact]
    public void IsEnabled_WhenPlistDoesNotExist_ReturnsFalse()
        => Assert.False(_strategy.IsEnabled());

    [Fact]
    public void Enable_CreatesPlistFile()
    {
        _strategy.Enable("/usr/local/bin/net-presence");

        Assert.True(_strategy.IsEnabled());
    }

    [Fact]
    public void Enable_PlistContainsExecutablePath()
    {
        const string exePath = "/usr/local/bin/net-presence";
        _strategy.Enable(exePath);

        var content = File.ReadAllText(_strategy.PlistPath);
        Assert.Contains(exePath, content);
    }

    [Fact]
    public void Enable_PlistContainsRunAtLoad()
    {
        _strategy.Enable("/usr/local/bin/net-presence");

        var content = File.ReadAllText(_strategy.PlistPath);
        Assert.Contains("RunAtLoad", content);
        Assert.Contains("<true/>", content);
    }

    [Fact]
    public void Enable_CreatesDirectoryIfNotExists()
    {
        Assert.False(Directory.Exists(_tempLaunchAgentsDir));

        _strategy.Enable("/usr/local/bin/net-presence");

        Assert.True(Directory.Exists(_tempLaunchAgentsDir));
    }

    [Fact]
    public void Disable_WhenPlistExists_DeletesFile()
    {
        _strategy.Enable("/usr/local/bin/net-presence");
        _strategy.Disable();

        Assert.False(_strategy.IsEnabled());
    }

    [Fact]
    public void Disable_WhenPlistDoesNotExist_DoesNotThrow()
    {
        var ex = Record.Exception(() => _strategy.Disable());

        Assert.Null(ex);
    }

    [Fact]
    public void Enable_CalledTwice_UpdatesPathInPlist()
    {
        _strategy.Enable("/old/path");
        _strategy.Enable("/new/path");

        var content = File.ReadAllText(_strategy.PlistPath);
        Assert.Contains("/new/path", content);
        Assert.DoesNotContain("/old/path", content);
    }
}

public class LinuxStartupStrategyTests : IDisposable
{
    private readonly string _tempAutostartDir;
    private readonly LinuxStartupStrategyTestable _strategy;

    public LinuxStartupStrategyTests()
    {
        _tempAutostartDir = Path.Combine(Path.GetTempPath(), $"autostart_{Guid.NewGuid()}");
        _strategy = new LinuxStartupStrategyTestable(_tempAutostartDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempAutostartDir))
            Directory.Delete(_tempAutostartDir, recursive: true);
    }

    [Fact]
    public void IsEnabled_WhenDesktopFileDoesNotExist_ReturnsFalse()
        => Assert.False(_strategy.IsEnabled());

    [Fact]
    public void Enable_CreatesDesktopFile()
    {
        _strategy.Enable("/usr/local/bin/net-presence");

        Assert.True(_strategy.IsEnabled());
    }

    [Fact]
    public void Enable_DesktopFileContainsExecutablePath()
    {
        const string exePath = "/usr/local/bin/net-presence";
        _strategy.Enable(exePath);

        var content = File.ReadAllText(_strategy.DesktopFilePath);
        Assert.Contains($"Exec={exePath}", content);
    }

    [Fact]
    public void Enable_DesktopFileContainsRequiredFields()
    {
        _strategy.Enable("/usr/local/bin/net-presence");

        var content = File.ReadAllText(_strategy.DesktopFilePath);
        Assert.Contains("[Desktop Entry]", content);
        Assert.Contains("Type=Application", content);
        Assert.Contains("Name=Net Presence", content);
        Assert.Contains("X-GNOME-Autostart-enabled=true", content);
    }

    [Fact]
    public void Enable_CreatesDirectoryIfNotExists()
    {
        Assert.False(Directory.Exists(_tempAutostartDir));

        _strategy.Enable("/usr/local/bin/net-presence");

        Assert.True(Directory.Exists(_tempAutostartDir));
    }

    [Fact]
    public void Disable_WhenDesktopFileExists_DeletesFile()
    {
        _strategy.Enable("/usr/local/bin/net-presence");
        _strategy.Disable();

        Assert.False(_strategy.IsEnabled());
    }

    [Fact]
    public void Disable_WhenDesktopFileDoesNotExist_DoesNotThrow()
    {
        var ex = Record.Exception(() => _strategy.Disable());

        Assert.Null(ex);
    }

    [Fact]
    public void Enable_CalledTwice_UpdatesPathInDesktopFile()
    {
        _strategy.Enable("/old/path");
        _strategy.Enable("/new/path");

        var content = File.ReadAllText(_strategy.DesktopFilePath);
        Assert.Contains("Exec=/new/path", content);
        Assert.DoesNotContain("Exec=/old/path", content);
    }
}

public class MacOSStartupStrategyTestable(string launchAgentsDir) : MacOSStartupStrategy
{
    public string PlistPath => Path.Combine(launchAgentsDir, "dev.jazzcat.net-presence.plist");

    protected override string GetPlistPath() => PlistPath;
}

public class LinuxStartupStrategyTestable(string autostartDir) : LinuxStartupStrategy
{
    public string DesktopFilePath => Path.Combine(autostartDir, "net-presence.desktop");

    protected override string GetDesktopFilePath() => DesktopFilePath;
}
