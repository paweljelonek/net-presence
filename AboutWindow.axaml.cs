using System.Reflection;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace NetPresence;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        SetInfo();
    }

    private void SetInfo()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        VersionText.Text = version != null
            ? $"Version {version.Major}.{version.Minor}.{version.Build}"
            : "unknown build";

        string platform;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            platform = "Windows";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            platform = "macOS";
        else
            platform = "Linux/Unix";

        PlatformText.Text = $"Platform: {platform}";
    }

    private void Close_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}
