dotnet clean -c Release
dotnet build -c Release
dotnet publish  -c Release -f net8.0-windows10.0.19041.0 /p:RuntimeIdentifierOverride=win10-x64
pause