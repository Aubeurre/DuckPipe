[Setup]
AppName=DuckPipe
AppVersion=2.0.3
AppVerName=DuckPipe 2.0.3
VersionInfoVersion=2.0.3.0
AppPublisher=Alexandre Wojtkow
DefaultDirName={pf}\DuckPipe
DefaultGroupName=DuckPipe
OutputBaseFilename=DuckPipeSetup
Compression=lzma
SolidCompression=yes

AppPublisherURL=https://github.com/Aubeurre/DuckPipe
AppSupportURL=https://github.com/Aubeurre/DuckPipe/issues
AppUpdatesURL=https://github.com/Aubeurre/DuckPipe/releases

AppCopyright=© 2026 Alexandre Wojtkow

CloseApplications=yes
RestartApplications=no
PrivilegesRequired=admin
DisableProgramGroupPage=yes

[Tasks]
Name: "desktopicon"; Description: "Créer un raccourci sur le Bureau"; GroupDescription: "Raccourcis"

[Files]
Source: "A:\02 WORK IN\DuckPipe\DuckPipe\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs ignoreversion
Source: "A:\02 WORK IN\DuckPipe\DuckPipe\bin\Release\net8.0-windows\Docs\*"; DestDir: "{app}\Docs"; Flags: recursesubdirs createallsubdirs ignoreversion

[Icons]
Name: "{group}\DuckPipe"; Filename: "{app}\DuckPipe.exe"
Name: "{group}\Documentation"; Filename: "{app}\Docs\index.html"
Name: "{userdesktop}\DuckPipe"; Filename: "{app}\DuckPipe.exe"; Tasks: desktopicon
Name: "{group}\Désinstaller DuckPipe"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\DuckPipe.exe"; Description: "Lancer DuckPipe"; Flags: nowait postinstall skipifsilent
