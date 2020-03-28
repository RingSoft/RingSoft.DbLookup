@echo off
cd\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\Extensions\TestPlatform\
vstest.console.exe C:\Users\petem\source\repos\RingSoft.DbLookup\RingSoft.DbLookup.Tests\bin\Debug\net472\RingSoft.DbLookup.Tests.dll
rundll32 user32.dll,MessageBeep
echo .
echo .NET Framework 4.7.2  Finished!
pause
vstest.console.exe C:\Users\petem\source\repos\RingSoft.DbLookup\RingSoft.DbLookup.Tests\bin\Debug\netcoreapp3.1\RingSoft.DbLookup.Tests.dll
rundll32 user32.dll,MessageBeep
echo .
echo .NET Core 3.1  Finished!
pause