@ECHO OFF
set "params=%*"
cd /d "%~dp0" && ( if exist "%temp%\getadmin.vbs" del "%temp%\getadmin.vbs") && fsutil dirty query %systemdrive% 1>nul 2>nul || ( echo Set UAC = CreateObject^("Shell.Application"^) : UAC.ShellExecute "cmd.exe", "/k cd ""%~sdp0"" && %~s0 %params%", "", "runas", 1 >> "%temp%\getadmin.vbs" && "%temp%\getadmin.vbs" && Exit /b)
set url="http://localhost:18390"
set /p url="Enter a url to host QBLint at (Default: http://localhost:18390): "
cd QBLintLinter
dotnet restore
dotnet build --configuration Release --no-restore
cd ..\
setx /M QBLINTER %CD%\QBLintLinter\bin\Release\net5.0-windows\
cd QBLint
dotnet restore
dotnet build --no-restore
dotnet run --no-build --urls=%url%