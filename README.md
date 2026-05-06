# Net Presence

![Status: Unstable](https://img.shields.io/badge/status-unstable-red)
![Work in progress](https://img.shields.io/badge/work%20in%20progress-%F0%9F%9A%A7-orange)

> [!WARNING]
> **Disclaimer:** This project is highly unstable and represents a very early version of the application. Use at your own risk.

> [!NOTE]
> This application is being developed in our free time purely for fun. We don't have a planned release date, and we can't promise that a stable version will ever see the light of day. Above all, this is a collaborative, hobbyist project meant for experimenting, learning, and helping us grow as developers.

A multi-platform .NET desktop application with a system tray icon, designed as a template/starting point for a keep-presence or similar tool. 

## Inspiration
This project draws heavy inspiration from the following open-source projects:
- https://github.com/carrot69/keep-presence
- https://github.com/paweljelonek/keep-presence-gui

## Features
- Built with [Avalonia UI](https://avaloniaui.net/) for cross-platform support (Windows, macOS, Linux).
- System tray icon with basic context menu.
- Extensible, minimalistic base setup.

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Build and Run
To run the application locally:
```bash
dotnet run
```

To build a release version:
```bash
dotnet build -c Release
```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
