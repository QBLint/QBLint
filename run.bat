@ECHO OFF
set "params=%*"
cd /d "%~dp0" && ( if exist "%temp%\getadmin.vbs" del "%temp%\getadmin.vbs") && fsutil dirty query %systemdrive% 1>nul 2>nul || ( echo Set UAC = CreateObject^("Shell.Application"^) : UAC.ShellExecute "cmd.exe", "/k cd ""%~sdp0"" && %~s0 %params%", "", "runas", 1 >> "%temp%\getadmin.vbs" && "%temp%\getadmin.vbs" && Exit /b)
cd QBLintLinter
dotnet build --configuration Release
cd ..\QBLint
setx /M QBLINTER %CD%\QBLintLinter\bin\Release\net5.0-windows\
dotnet run
start "" localhost:5001