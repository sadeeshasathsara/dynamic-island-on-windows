[Setup]
AppName=Dynamic Island Notifier
AppVersion=1.0.0
AppPublisher=Sadeesha Sathsara
DefaultDirName={autopf}\Dynamic Island Notifier
DefaultGroupName=Dynamic Island Notifier
UninstallDisplayIcon={app}\DynamicIslandNotifier.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.
OutputBaseFilename=DynamicIslandSetup_v1.0.0_arm64
PrivilegesRequired=lowest
ArchitecturesAllowed=arm64
ArchitecturesInstallIn64BitMode=arm64

[Files]
Source: "publish-arm64\DynamicIslandNotifier.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish-arm64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Dynamic Island Notifier"; Filename: "{app}\DynamicIslandNotifier.exe"
Name: "{autodesktop}\Dynamic Island Notifier"; Filename: "{app}\DynamicIslandNotifier.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "DynamicIslandNotifier"; ValueData: """{app}\DynamicIslandNotifier.exe"""; Flags: uninsdeletevalue

[Run]
Filename: "{app}\DynamicIslandNotifier.exe"; Description: "{cm:LaunchProgram,Dynamic Island Notifier}"; Flags: nowait postinstall skipifsilent
