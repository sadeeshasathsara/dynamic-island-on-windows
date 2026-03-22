# Dynamic Island for Windows

A sleek, iOS-inspired Dynamic Island notification overlay for Windows 11. Replaces the default Windows toast notifications with a beautiful, animated pill that drops down from the top of your screen.

![Windows 11](https://img.shields.io/badge/Windows-11-0078D6?logo=windows)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/License-MIT-green)

## ✨ Features

- 🎯 **Dynamic Island Pill** — Animated notification overlay at the top-center of your screen
- 🎬 **iOS-style Animations** — Drop-down bounce, horizontal expand, slide-up exit
- 🖱️ **Interactive Hover Actions** — Hover to reveal Mute and Clear buttons
- 🔇 **Native Toast Suppression** — Automatically hides Windows default notifications
- 👆 **Click to Open** — Click the notification to launch the source app
- 🎨 **Deterministic Accent Colors** — Each app gets a unique, consistent avatar color
- 🔄 **Always on Top** — Works over fullscreen apps and games
- 📌 **System Tray** — Runs silently in the background with tray icon controls

## 📋 Prerequisites

- **Windows 11** (build 22621 or later)
- [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Notification Access** — The app will request permission on first launch

## 🚀 Getting Started

```bash
# Clone the repository
git clone https://github.com/your-username/dynamic-island-windows.git
cd dynamic-island-windows

# Build
dotnet build

# Run
dotnet run
```

On first launch, Windows will ask you to grant notification listener access. Go to:
**Settings → Privacy & Security → Notifications** → enable **Dynamic Island Notifier**.

> **Tip:** Enable **Do Not Disturb** in Windows to fully suppress native toast banners while the Dynamic Island handles everything.

## 🏗️ Architecture

```
├── Core/
│   ├── NotificationData.cs      # Immutable data model
│   └── NativeInterop.cs         # Win32 P/Invoke utilities
├── Services/
│   ├── NotificationListener.cs  # WinRT notification polling + events
│   └── ToastSuppressor.cs       # Registry-based DND management
├── UI/
│   └── IslandAnimator.cs        # Animation primitives & orchestration
├── IslandWindow.xaml / .cs      # The pill overlay window
├── App.xaml / .cs               # Entry point & system tray
└── Assets/
    └── island.ico               # App icon
```

| Layer | Responsibility |
|-------|---------------|
| **Core** | Data models, Win32 interop — zero UI dependencies |
| **Services** | Notification listening, toast suppression — zero UI dependencies |
| **UI** | Animation engine, overlay window — depends on Core only |

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## 📄 License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.
