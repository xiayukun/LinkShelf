英文发布说明：[docs/release-notes-v1.0.1.md](https://github.com/xiayukun/LinkShelf/blob/v1.0.1/docs/release-notes-v1.0.1.md)

这个版本改进了添加项目流程。当 Windows 返回拒绝访问时，通常是某个正在运行的程序占用了选中的目录或目录里的文件，程序现在会给出更清楚的处理入口。

## 亮点

- Link Shelf 现在会先尝试正常移动，只有 Windows 阻止移动后才扫描占用进程。
- 图形界面会显示文件占用处理窗口，列出进程名、进程号、用户、程序路径和被占用文件。
- 用户可以右键结束选中进程，也可以选择“结束全部并继续”。
- 结束进程后，Link Shelf 会短暂等待并重新执行原来的移动操作。如果路径仍然被占用，会再次打开处理窗口。
- 占用检测实现改写自 `ShowWhatProcessLocksFile`，并在项目文档中补充了对应鸣谢。
- 命令行健康检查保持只读，行为不变。

![文件占用处理窗口](https://raw.githubusercontent.com/xiayukun/LinkShelf/v1.0.1/Assets/lock-resolution-window-cn.png)

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 推荐工作流

1. 把 `LinkShelf.exe` 放到要作为缓存根目录的文件夹中。
2. 双击程序并添加文件或目录。
3. 如果添加操作提示路径被阻止，请查看列出的占用进程。
4. 手动关闭它们、右键结束选中进程，或选择“结束全部并继续”。
5. 使用 Syncthing 或其他工具同步或备份整个缓存根目录。
6. 在另一台机器上，把 `LinkShelf.exe` 放到恢复后的缓存根目录，然后点击 `恢复链接`。

## 自动化

本地健康检查推荐使用：

```powershell
.\LinkShelf.exe check --json
```

只有 `problemCount` 大于 `0` 时才提醒用户。

## 安全说明

- Link Shelf 会移动文件并创建符号链接。
- 替换目标内容前，请仔细查看冲突提示。
- 首次运行前请备份重要数据。
