using System.Reflection;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace NetPresence;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        BuildTrayMenu();
        base.OnFrameworkInitializationCompleted();
    }

    private void BuildTrayMenu()
    {
        var trayIcons = TrayIcon.GetIcons(this);
        if (trayIcons == null || trayIcons.Count == 0) return;

        var trayIcon = trayIcons[0];

        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var versionText = version != null
            ? $"Version {version.Major}.{version.Minor}.{version.Build}"
            : "unknown build";

        string platform;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            platform = "Windows";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            platform = "macOS";
        else
            platform = "Linux/Unix";

        var showWindowItem = new NativeMenuItem("Show window");
        showWindowItem.Click += ShowWindow_Click;

        var exitItem = new NativeMenuItem("Exit");
        exitItem.Click += Exit_Click;

        var menu = new NativeMenu();
        // Sekcja informacyjna
        menu.Items.Add(new NativeMenuItem("NetPresence") { IsEnabled = false });
        menu.Items.Add(new NativeMenuItem(versionText) { IsEnabled = false });
        menu.Items.Add(new NativeMenuItem($"Platform: {platform}") { IsEnabled = false });
        // Separator
        menu.Items.Add(new NativeMenuItemSeparator());
        // Akcje
        menu.Items.Add(showWindowItem);
        // Separator
        menu.Items.Add(new NativeMenuItemSeparator());
        // Wyjście
        menu.Items.Add(exitItem);

        trayIcon.Menu = menu;
    }

    private void TrayIcon_Clicked(object? sender, System.EventArgs e)
    {
        ShowMainWindow();
    }

    private void ShowWindow_Click(object? sender, System.EventArgs e)
    {
        ShowMainWindow();
    }

    private void ShowMainWindow()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        var window = desktop.MainWindow;
        if (window == null) return;

        window.Show();
        window.WindowState = WindowState.Normal;
        window.Activate();
    }

    private void Exit_Click(object? sender, System.EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}