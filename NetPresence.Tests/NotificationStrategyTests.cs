#pragma warning disable CA1416

using NetPresence.Services.Notifications;
using Xunit;

namespace NetPresence.Tests;

public class FakeProcessRunner : IProcessRunner
{
    public string? LastFileName { get; private set; }
    public string? LastArguments { get; private set; }
    public int RunCount { get; private set; }

    public void Run(string fileName, string arguments)
    {
        LastFileName = fileName;
        LastArguments = arguments;
        RunCount++;
    }
}

public class NotificationStrategyTests
{
    [Fact]
    public void LinuxNotificationStrategy_CallsProcessRunnerWithCorrectArguments()
    {
        var fakeRunner = new FakeProcessRunner();
        var strategy = new LinuxNotificationStrategy(fakeRunner);

        strategy.ShowNotification("Hello World", "12:34:56");

        Assert.Equal("notify-send", fakeRunner.LastFileName);
        Assert.Equal("\"Hello World\" \"12:34:56\"", fakeRunner.LastArguments);
        Assert.Equal(1, fakeRunner.RunCount);
    }

    [Fact]
    public void MacOSNotificationStrategy_CallsProcessRunnerWithCorrectArguments()
    {
        var fakeRunner = new FakeProcessRunner();
        var strategy = new MacOSNotificationStrategy(fakeRunner);

        strategy.ShowNotification("Hello World", "12:34:56");

        Assert.Equal("osascript", fakeRunner.LastFileName);
        Assert.Contains("Hello World", fakeRunner.LastArguments);
        Assert.Contains("12:34:56", fakeRunner.LastArguments);
        Assert.Equal(1, fakeRunner.RunCount);
    }

    [Fact]
    public void WindowsNotificationStrategy_CallsProcessRunnerWithCorrectArguments()
    {
        var fakeRunner = new FakeProcessRunner();
        var strategy = new WindowsNotificationStrategy(fakeRunner);

        strategy.ShowNotification("Hello World", "12:34:56");

        Assert.Equal("powershell", fakeRunner.LastFileName);
        Assert.Contains("Hello World", fakeRunner.LastArguments);
        Assert.Contains("12:34:56", fakeRunner.LastArguments);
        Assert.Equal(1, fakeRunner.RunCount);
    }
}
