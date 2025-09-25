# NativeWinFormDialog
收集了 `WindowsForms` 提供的几种常用会话，编译为本机动态库供 Unity 编译的程序调用。

需要 Unity 2022.3 或者更新，Windows 7 x86-64 或者更新。

插件按 Unity 包分发，如果需要重新编译本机代码库的部分，找到 `com.asurafanclub.nativewinformdialog\NetProject~\DialogLib\DialogLib\AOTBuild.bat` 运行即可。编译需要 `.Net9` 和 Windows 桌面开发依赖，编译产物会移动到 `com.asurafanclub.nativewinformdialog\NativePlugins` 文件夹。

支持：

- ColorDialog
- FolderBrowserDialog
- MessageBox
- OpenFileDialog
- SaveFileDialog

