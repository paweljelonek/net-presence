using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NetPresence;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (OperatingSystem.IsMacOS())
            HideDockIcon();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = new MainWindow();
        }

        BuildTrayMenu();
        base.OnFrameworkInitializationCompleted();
    }

    [SupportedOSPlatform("macos")]
    private static void HideDockIcon()
    {
        var nsAppClass = objc_getClass("NSApplication");
        var sharedApp = objc_msgSend_retval(nsAppClass, sel_registerName("sharedApplication"));
        objc_msgSend(sharedApp, sel_registerName("setActivationPolicy:"), 2); // NSApplicationActivationPolicyAccessory = 2
    }

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    private static extern IntPtr objc_getClass(string name);

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    private static extern IntPtr sel_registerName(string name);

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit", EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retval(IntPtr receiver, IntPtr selector);

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    private static extern void objc_msgSend(IntPtr receiver, IntPtr selector, int policy);

    private void BuildTrayMenu()
    {
        var trayIcons = TrayIcon.GetIcons(this);
        if (trayIcons == null || trayIcons.Count == 0) return;

        var trayIcon = trayIcons[0];

        var showWindowItem = new NativeMenuItem("Show window");
        showWindowItem.Click += ShowWindow_Click;

        var checkNotificationItem = new NativeMenuItem("Check notification");
        checkNotificationItem.Click += CheckNotification_Click;

        var aboutItem = new NativeMenuItem("About");
        aboutItem.Click += About_Click;

        var exitItem = new NativeMenuItem("Exit");
        exitItem.Click += Exit_Click;

        var menu = new NativeMenu();
        
        menu.Items.Add(showWindowItem);
        menu.Items.Add(checkNotificationItem);
        menu.Items.Add(aboutItem);

        menu.Items.Add(new NativeMenuItemSeparator());

        menu.Items.Add(exitItem);

        trayIcon.Menu = menu;
    }

    private void CheckNotification_Click(object? sender, System.EventArgs e)
    {
        var currentTime = DateTime.Now.ToString("HH:mm:ss");
        Services.Notifications.NotificationManager.ShowNotification("Hello World", currentTime);
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