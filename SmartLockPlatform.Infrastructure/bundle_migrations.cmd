dotnet build
dotnet ef migrations bundle --self-contained -r win-x64 --startup-project ../SmartLockPlatform.Host/ -v -o bin/migration_bundle.exe
pause