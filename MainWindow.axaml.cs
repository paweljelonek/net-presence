using Avalonia.Controls;

namespace NetPresence;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Hide();
    }

    protected override void OnClosing(Avalonia.Controls.WindowClosingEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }
}