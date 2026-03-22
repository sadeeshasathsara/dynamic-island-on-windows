namespace DynamicIslandNotifier.Core;

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

/// <summary>
/// Centralised Win32 P/Invoke helpers for window styling, transparency,
/// and always-on-top behaviour.
/// </summary>
public static class NativeInterop
{
    // ── Constants ──────────────────────────────────────────────────────
    private const int GWL_EXSTYLE       = -20;
    private const int WS_EX_NOACTIVATE  = 0x08000000;
    private const int WS_EX_TOOLWINDOW  = 0x00000080;
    private static readonly IntPtr HWND_TOPMOST = new(-1);
    private const uint SWP_NOMOVE = 0x0002, SWP_NOSIZE = 0x0001, SWP_NOACTIVATE = 0x0010;

    // ── Imports ────────────────────────────────────────────────────────
    [DllImport("user32.dll")] private static extern int  GetWindowLong(IntPtr h, int idx);
    [DllImport("user32.dll")] private static extern int  SetWindowLong(IntPtr h, int idx, int val);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr insert,
        int x, int y, int cx, int cy, uint flags);

    // ── Public API ────────────────────────────────────────────────────
    /// <summary>
    /// Makes the window non-activatable and hidden from Alt+Tab.
    /// </summary>
    public static void MakeOverlay(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        int style = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
    }

    /// <summary>
    /// Forces the window to stay above all other windows including fullscreen apps.
    /// </summary>
    public static void ForceTopMost(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
    }
}
