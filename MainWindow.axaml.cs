using Avalonia.Controls;
using NetPresence.Models;
using NetPresence.Services;
using NetPresence.Services.Startup;

namespace NetPresence;

public partial class MainWindow : Window
{
    private AppConfig _config;

    public MainWindow()
    {
        InitializeComponent();
        _config = ConfigManager.Load();
        ApplyConfigToUI();
    }

    private void ApplyConfigToUI()
    {
        ModeComboBox.SelectedIndex = _config.Mode;
        IntervalNumeric.Value = _config.IdleIntervalSeconds;
        PixelsNumeric.Value = _config.MousePixels;
        CircularCheckBox.IsChecked = _config.CircularMouseMovement;
        StartImmediatelyCheckBox.IsChecked = _config.StartImmediatelyOnLaunch;
        NotificationCheckBox.IsChecked = _config.ShowNotification;
        StartupCheckBox.IsChecked = _config.LaunchOnStartup;
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _config.Mode = ModeComboBox.SelectedIndex;
        _config.IdleIntervalSeconds = (int)(IntervalNumeric.Value ?? 60);
        _config.MousePixels = (int)(PixelsNumeric.Value ?? 1);
        _config.CircularMouseMovement = CircularCheckBox.IsChecked ?? false;
        _config.StartImmediatelyOnLaunch = StartImmediatelyCheckBox.IsChecked ?? true;
        _config.ShowNotification = NotificationCheckBox.IsChecked ?? false;
        _config.LaunchOnStartup = StartupCheckBox.IsChecked ?? false;

        ConfigManager.Save(_config);
        StartupManager.Apply(_config.LaunchOnStartup);
        this.Hide();
    }

    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyConfigToUI();
        this.Hide();
    }

    protected override void OnClosing(Avalonia.Controls.WindowClosingEventArgs e)
    {
        e.Cancel = true;
        ApplyConfigToUI();
        this.Hide();
    }
}