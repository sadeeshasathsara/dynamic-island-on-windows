# Architecture

Dynamic Island for Windows is structured using a clean, layered architecture to make the codebase easy to understand, extend, and contribute to.

## Layers

```
dynamic-island-on-windows/
├── Core/
│   ├── NotificationData.cs       # Immutable notification data model
│   └── NativeInterop.cs          # Win32 P/Invoke helpers & window styling
│
├── Services/
│   ├── NotificationListener.cs   # WinRT notification listener & event dispatcher
│   └── ToastSuppressor.cs        # Registry-based DND & toast suppression
│
├── UI/
│   └── IslandAnimator.cs         # WPF animation primitives & choreography
│
├── IslandWindow.xaml             # Pill overlay XAML layout
├── IslandWindow.xaml.cs          # Pill overlay code-behind
├── App.xaml                      # Application definition
├── App.xaml.cs                   # Entry point, tray icon, wiring
└── Assets/
    └── island.ico                # Application icon
```

## Layer Responsibilities

| Layer | Files | Responsibility |
|---|---|---|
| **Core** | `NotificationData.cs`, `NativeInterop.cs` | Pure data models and Win32 interop utilities. No UI, no WinRT, no side effects. |
| **Services** | `NotificationListener.cs`, `ToastSuppressor.cs` | Business logic layer. Listens to WinRT notification events and manages registry-based DND. Zero UI dependencies. |
| **UI** | `IslandAnimator.cs`, `IslandWindow.xaml(.cs)` | All WPF rendering, animation, and user interaction. Depends on Core but not on Services directly. |
| **App** | `App.xaml.cs` | Entry point. Wires all layers together, bootstraps the system tray, manages the app lifetime. |

## Data Flow

```
Windows Notification API (WinRT)
        |
        v
NotificationListener (Services)
  - Polls for new notifications
  - Loads app icon (BitmapSource)
  - Fires OnNotificationReceived event
        |
        v
App.xaml.cs
  - Subscribes to the event
  - Calls IslandWindow.ShowNotification(NotificationData)
        |
        v
IslandWindow (UI)
  - Stores notification content
  - Uses IslandAnimator to play drop-down animation
  - Shows app name, message, avatar
        |
        v
User sees Dynamic Island pill
```

## Key Design Decisions

### Event-driven, not callback-driven
`NotificationListener` fires a C# `event Action<NotificationData>` instead of holding a reference to the UI window. This decouples the service from the presentation layer — the service does not know or care how the data is displayed.

### Immutable data model
`NotificationData` is a simple sealed class with readonly properties. It is created once in the service layer and passed to the UI unchanged. This prevents accidental mutation across layers.

### Centralized Win32 interop
All `DllImport` declarations and window-styling helpers live in `NativeInterop.cs` (Core). No other file calls Win32 APIs directly. This makes it easy to audit, test, and maintain native interop code.

### Registry-based toast suppression
`ToastSuppressor` enables Windows Do Not Disturb and suppresses per-app toast banners via registry keys. It tracks every key it modifies and restores them on exit, ensuring no permanent system changes are left behind.

## Dependency Rules

```
App  →  UI  →  Core
App  →  Services  →  Core
UI   ✗  Services (no direct dependency)
```

Each layer may only depend on layers below it. The `App` layer is the only place where all layers are wired together.
