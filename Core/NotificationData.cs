namespace DynamicIslandNotifier.Core;

using System.Windows.Media.Imaging;

/// <summary>
/// Immutable data object representing a single notification.
/// Decouples the notification source from the UI layer.
/// </summary>
public sealed class NotificationData
{
    public string AppName { get; init; } = "App";
    public string Title { get; init; } = "";
    public string Body { get; init; } = "";
    public BitmapSource? Icon { get; init; }
    public string? AppUserModelId { get; init; }
}
