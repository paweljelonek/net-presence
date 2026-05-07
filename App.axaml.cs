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

        var showWindowItem = new NativeMenuItem("Show window");
        showWindowItem.Click += ShowWindow_Click;

        var aboutItem = new NativeMenuItem("About");
        aboutItem.Click += About_Click;

        var exitItem = new NativeMenuItem("Exit");
        exitItem.Click += Exit_Click;

        var menu = new NativeMenu();
        
        menu.Items.Add(showWindowItem);
        menu.Items.Add(aboutItem);

        menu.Items.Add(new NativeMenuItemSeparator());

        menu.Items.Add(exitItem);

        trayIcon.Menu = menu;
    }

    private void About_Click(object? sender, System.EventArgs e)
    {
        new AboutWindow().Show();
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