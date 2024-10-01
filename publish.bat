dotnet clean -c Release
dotnet build -c Release
dotnet publish  -c Release -f ./Installation/PublishApp /p:RuntimeIdentifierOverride=win10-x64
pause