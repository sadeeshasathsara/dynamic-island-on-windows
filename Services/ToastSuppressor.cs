namespace DynamicIslandNotifier.Services;

using Microsoft.Win32;
using System.Collections.Generic;

/// <summary>
/// Manages Windows notification banner suppression via Registry.
/// Enables DND on start, restores settings on exit.
/// </summary>
public sealed class ToastSuppressor
{
    private const string NotifSettingsKey =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings";

    private readonly HashSet<string> _suppressedApps = new();

    // ── Global DND ────────────────────────────────────────────────────
    public void EnableDND()
    {
        try
        {
            Registry.SetValue(NotifSettingsKey,
                "NOC_GLOBAL_SETTING_TOASTS_ENABLED", 0, RegistryValueKind.DWord);
        }
        catch { }
    }

    public void DisableDND()
    {
        try
        {
            Registry.SetValue(NotifSettingsKey,
                "NOC_GLOBAL_SETTING_TOASTS_ENABLED", 1, RegistryValueKind.DWord);
        }
        catch { }
    }

    // ── Per-App Suppression ───────────────────────────────────────────
    public void SuppressApp(string appUserModelId)
    {
        if (string.IsNullOrEmpty(appUserModelId)) return;
        if (_suppressedApps.Contains(appUserModelId)) return;

        try
        {
            string keyPath = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\{appUserModelId}";
            using var key = Registry.CurrentUser.CreateSubKey(keyPath);
            key.SetValue("ShowBanner", 0, RegistryValueKind.DWord);
            key.SetValue("ShowInActionCenter", 1, RegistryValueKind.DWord);
            _suppressedApps.Add(appUserModelId);
        }
        catch { }
    }

    public void RestoreAll()
    {
        DisableDND();
        // Per-app keys are left as-is to avoid breaking user settings.
        // They only suppress banners, not the notifications themselves.
    }
}
