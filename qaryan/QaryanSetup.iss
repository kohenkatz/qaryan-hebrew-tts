[_ISToolPreCompile]
Name: C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe; Parameters: I:\Projects\Qaryan\Qaryan.sln /t:Build /p:Configuration=Release
[Files]
Source: I:\Projects\Qaryan\Dist\CSharpOptParse.dll; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\CSharpOptParse.xml; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\libao-sharp.dll; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\libao-sharp.xml; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\Qaryan.dll; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\Qaryan.dll.config; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\Qaryan.pdb; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\StressHeuristics.xml; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\ZedGraph.dll; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\ZedGraph.xml; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\QaryanCLI.exe; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\QaryanCLI.pdb; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\QaryanGUI.exe; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\QaryanGUI.exe.config; DestDir: {app}
Source: I:\Projects\Qaryan\Dist\QaryanGUI.pdb; DestDir: {app}
Source: ..\..\Downloads\Dev\innounp.exe; DestDir: {app}; Flags: deleteafterinstall; Tasks: MBROLA
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-ar1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-ar2.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-cr1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-cz1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-ee1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-es1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-es2.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-es4.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-hb1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-hb2.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-ir1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-la1.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-lt2.xml; DestDir: {app}\Voices
Source: I:\Projects\Qaryan\Dist\Voices\mbrola-us2.xml; DestDir: {app}\Voices
Source: ..\..\Downloads\System\unzip.exe; DestDir: {app}; Tasks: MBRVOICE_ar2 MBRVOICE_hb1 MBRVOICE_hb2; Flags: deleteafterinstall
Source: CLI\QaryanCLIEnv.bat; DestDir: {app}
Source: Dist\he-IL\QaryanGUI.resources.dll; DestDir: {app}\he-IL
Source: Dist\en\QaryanGUI.resources.dll; DestDir: {app}\en
Source: Start.txt; DestDir: {app}
[_ISToolDownload]
Source: http://tcts.fpms.ac.be/synthesis/mbrola/bin/pcwin/MbrolaTools35.exe; DestDir: {tmp}; DestName: MbrolaTools.exe; Tasks: MBROLA
Source: http://tcts.fpms.ac.be/synthesis/mbrola/dba/hb2/hb2.zip; DestDir: {tmp}; DestName: hb2.zip; Tasks: MBRVOICE_hb2
Source: http://tcts.fpms.ac.be/synthesis/mbrola/dba/hb1/hb1-000308.zip; DestDir: {tmp}; DestName: hb1.zip; Tasks: MBRVOICE_hb1
Source: http://tcts.fpms.ac.be/synthesis/mbrola/dba/ar2/ar2-001015.zip; DestDir: {tmp}; DestName: ar2.zip; Tasks: MBRVOICE_ar2
[Run]
Filename: {app}\innounp.exe; Parameters: "-q -b -e -d""{sys}"" ""MbrolaTools.exe"" mbr*.dll tctsaudio.dll"; WorkingDir: {tmp}; Tasks: MBROLA; StatusMsg: Unpacking MBROLA
Filename: {app}\unzip.exe; Parameters: "-o -d ""{app}\Voices"" -j hb2.zip hb2/hb2 hb2/license.txt"; WorkingDir: {tmp}; StatusMsg: Unpacking hb2; Flags: runhidden; Tasks: MBRVOICE_hb2
Filename: {sys}\cmd.exe; Parameters: /c rename license.txt hb2-license.txt; WorkingDir: {app}\Voices; Flags: runhidden; Tasks: MBRVOICE_hb2
Filename: {app}\unzip.exe; Parameters: "-o -d ""{app}\Voices"" -j hb1.zip hb1/hb1 hb1/license.txt"; WorkingDir: {tmp}; StatusMsg: Unpacking hb1; Flags: runhidden; Tasks: MBRVOICE_hb1
Filename: {sys}\cmd.exe; Parameters: /c rename license.txt hb1-license.txt; WorkingDir: {app}\Voices; Flags: runhidden; Tasks: MBRVOICE_ar2
Filename: {app}\unzip.exe; Parameters: "-o -d ""{app}\Voices"" -j ar2.zip ar2 license.txt"; WorkingDir: {tmp}; StatusMsg: Unpacking ar2; Flags: runhidden; Tasks: MBRVOICE_ar2
Filename: {sys}\cmd.exe; Parameters: /c rename license.txt ar2-license.txt; WorkingDir: {app}\Voices; Flags: runhidden; Tasks: MBRVOICE_ar2
Filename: {app}\innounp.exe; Parameters: "-q -b -e -d""{app}\MBROLA\"" ""MbrolaTools.exe"" License.txt"; WorkingDir: {tmp}; Tasks: MBROLA; StatusMsg: Unpacking MBROLA
Filename: write; Description: View MBROLA license; Flags: shellexec postinstall; Tasks: MBROLA; Parameters: """{app}\MBROLA\License.txt"""
Filename: write; Description: View license for voice: hb2; Flags: shellexec postinstall; Tasks: MBRVOICE_hb2; Parameters: """{app}\Voices\hb2-license.txt"""
Filename: write; Description: View license for voice: hb1; Flags: shellexec postinstall; Tasks: MBRVOICE_hb1; Parameters: """{app}\Voices\hb1-license.txt"""
Filename: write; Description: View license for voice: ar2; Flags: shellexec postinstall; Tasks: MBRVOICE_ar2; Parameters: """{app}\Voices\ar2-license.txt"""
[Tasks]
Name: MBROLA; Description: Download and install MBROLA
Name: MBRVOICE_hb2; Description: Hebrew - Esther (hb2); GroupDescription: Download and install MBROLA voices:
Name: MBRVOICE_hb1; Description: Hebrew - Yoram (hb1); Flags: unchecked; GroupDescription: Download and install MBROLA voices:
Name: MBRVOICE_ar2; Description: Arabic accent - ar2; Flags: unchecked; GroupDescription: Download and install MBROLA voices:
[Dirs]
Name: {app}\Voices
Name: {app}\MBROLA; Tasks: MBROLA
[Setup]
AppName=Qaryan Hebrew TTS
#define AppVer GetFileVersion('I:\Projects\Qaryan\Dist\Qaryan.dll')
#define AppVer Copy(AppVer,1,RPos('.',AppVer)-1)
AppVerName=Qaryan v{#AppVer}
LicenseFile=I:\Projects\Qaryan\gplv3.rtf
DefaultDirName={pf}\HebTTS\Qaryan
OutputBaseFilename=Qaryan-{#AppVer}-setup
OutputDir=.
DefaultGroupName=Qaryan
[UninstallDelete]
Name: {app}\Voices; Type: filesandordirs
Type: files; Name: {app}\Qaryan Project Website.url
[InstallDelete]
Name: {app}\mbrinst; Type: filesandordirs
[Code]
function InitializeSetup(): Boolean;
var
    ErrorCode: Integer;
    NetFrameWorkInstalled : Boolean;
    Result1 : Boolean;
begin
  NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\Policy\v2.0');
  if NetFrameWorkInstalled =true then
  begin
    Result := true;
  end;

  if NetFrameWorkInstalled =false then
  begin
    Result1 := MsgBox('This setup requires the .NET Framework version 2.0 or above. Please download and install the .NET Framework and run this setup again. Do you want to download the framework now?',
      mbConfirmation, MB_YESNO) = idYes;
    if Result1 =false then
    begin
      Result:=false;
    end
    else
    begin
     Result:=false;
      ShellExec('open', 'http://www.microsoft.com/downloads/details.aspx?familyid=79BC3B77-E02C-4AD3-AACF-A7633F706BA5&displaylang=en','','',SW_SHOWNORMAL,ewNoWait,ErrorCode);
    end;
  end;
end;
// Function generated by ISTool.
function NextButtonClick(CurPage: Integer): Boolean;
begin
	Result := istool_download(CurPage);
end;




[Icons]
Name: {group}\Qaryan GUI; Filename: {app}\QaryanGUI.exe; WorkingDir: {app}; IconFilename: {app}\QaryanGUI.exe; Comment: Hebrew text-to-speech application; IconIndex: 0; Tasks: 
Name: {group}\Qaryan Command Prompt; Filename: {%COMSPEC}; Parameters: "/k ""{app}\QaryanCLIEnv.bat"""; WorkingDir: {app}; IconFilename: {cmd}
Name: {group}\{cm:UninstallProgram, Qaryan Hebrew TTS}; Filename: {uninstallexe}
[INI]
Filename: {app}\Qaryan Project Website.url; Section: InternetShortcut; Key: URL; String: http://hebtts.sf.net/
