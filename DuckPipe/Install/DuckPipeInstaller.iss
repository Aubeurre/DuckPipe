[Setup]
AppName=DuckPipe
AppVersion=1.0
DefaultDirName={pf}\DuckPipe
DefaultGroupName=DuckPipe
OutputBaseFilename=DuckPipeSetup
Compression=lzma
SolidCompression=yes

[Files]
; Inclure tous les fichiers n�cessaires de Release
Source: "D:\A_WORK\DuckPipe\DuckPipe\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs

; Inclure la documentation HTML
Source: "D:\A_WORK\DuckPipe\DuckPipe\bin\Release\net8.0-windows\Docs\*"; DestDir: "{app}\Docs"; Flags: recursesubdirs createallsubdirs

[Icons]
Name: "{group}\DuckPipe"; Filename: "{app}\DuckPipe.exe"
Name: "{group}\Documentation"; Filename: "{app}\Docs\index.html"
Name: "{group}\D�sinstaller DuckPipe"; Filename: "{uninstallexe}"
Name: "{userdesktop}\DuckPipe"; Filename: "{app}\DuckPipe.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Cr�er un raccourci sur le Bureau"; GroupDescription: "Raccourcis"

[Run]
Filename: "{app}\DuckPipe.exe"; Description: "Lancer DuckPipe"; Flags: nowait postinstall skipifsilent
