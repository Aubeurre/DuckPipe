[Setup]
AppName=DuckPipe
AppVersion=1.2.1
AppPublisher=Alexandre Wojtkow
DefaultDirName={pf}\DuckPipe
AppPublisherURL=https://github.com/Aubeurre/DuckPipe
AppSupportURL=https://github.com/Aubeurre/DuckPipe/issues
AppUpdatesURL=https://github.com/Aubeurre/DuckPipe/releases
AppCopyright=© 2025 Alexandre Wojtkow
DefaultGroupName=DuckPipe
OutputBaseFilename=DuckPipeSetup
Compression=lzma
SolidCompression=yes

[Files]
; Inclure tous les fichiers nécessaires de Release
Source: "D:\A_WORK\DuckPipe\DuckPipe\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs

; Inclure la documentation HTML
Source: "D:\A_WORK\DuckPipe\DuckPipe\bin\Release\net8.0-windows\Docs\*"; DestDir: "{app}\Docs"; Flags: recursesubdirs createallsubdirs

[Icons]
Name: "{group}\DuckPipe"; Filename: "{app}\DuckPipe.exe"
Name: "{group}\Documentation"; Filename: "{app}\Docs\index.html"
Name: "{group}\Désinstaller DuckPipe"; Filename: "{uninstallexe}"
Name: "{userdesktop}\DuckPipe"; Filename: "{app}\DuckPipe.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Créer un raccourci sur le Bureau"; GroupDescription: "Raccourcis"

[Run]
Filename: "{app}\DuckPipe.exe"; Description: "Lancer DuckPipe"; Flags: nowait postinstall skipifsilent
