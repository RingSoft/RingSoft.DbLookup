; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "RingSoft WPF Database Lookup Demo"
#define MyAppVersion "2.00.10"
#define MyAppPublisher "RingSoft"
#define MyAppURL "http://www.ringsoft.site/"
#define MyAppExeName "RingSoft.DbLookup.App.WPFCore.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{22c2910a-4ef1-415d-b707-8b07bfe6da58}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableDirPage=no
DisableProgramGroupPage=no
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Inno Install Output
OutputBaseFilename=RingSoft.WPFDbLookupDemoApp
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\petem\source\repos\RingSoft\RingSoft.DbLookup\RingSoft.DbLookup.App.WPFCore\bin\Release\net8.0-windows7.0\*"; Excludes: "*.xml, *.sqlite"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\petem\source\repos\RingSoft\RingSoft.DbLookup\RingSoft.DbLookup.App.WPFCore\bin\Release\net8.0-windows7.0\Northwind\Northwind.sqlite"; DestDir: "{commonappdata}\RingSoft\DbLookupDemoApp\WPF\Northwind\"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

