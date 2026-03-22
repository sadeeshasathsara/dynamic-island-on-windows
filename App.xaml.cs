using System.Windows;
using System.Windows.Controls;
using DynamicIslandNotifier.Services;

namespace DynamicIslandNotifier;

public partial class App : Application
{
    private NotificationListener? _listener;
    private IslandWindow? _island;
    private bool _enabled = true;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create the overlay pill window (invisible until a notification arrives)
        _island = new IslandWindow();
        _island.Show();

        // Create services
        var suppressor = new ToastSuppressor();
        _listener = new NotificationListener(suppressor);

        // Wire notification events to the UI
        _listener.OnNotificationReceived += data =>
        {
            _island.Dispatcher.Invoke(() => _island.ShowNotification(data));
        };

        await _listener.StartAsync();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _listener?.Stop();
        var tray = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon?)Current.Resources["TrayIcon"];
        tray?.Dispose();
        base.OnExit(e);
    }

    private void Toggle_Click(object sender, RoutedEventArgs e)
    {
        _enabled = !_enabled;
        if (_listener != null) _listener.Enabled = _enabled;
        var item = sender as MenuItem;
        if (item != null)
            item.Header = _enabled ? "Disable" : "Enable";
    }

    private void Quit_Click(object sender, RoutedEventArgs e)
    {
        Current.Shutdown();
    }
}
