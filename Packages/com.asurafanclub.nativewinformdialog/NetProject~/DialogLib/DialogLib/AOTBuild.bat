dotnet publish -c Release -r win-x64 /p:PublishAot=true

:: 尝试复制编译产物到目标目录
copy bin\Release\net9.0-windows7.0\win-x64\publish ..\..\..\NativePlugins\

pause