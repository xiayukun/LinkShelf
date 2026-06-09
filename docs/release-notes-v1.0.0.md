English release notes: [docs/release-notes-v1.0.0.en.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.0.0.en.md)

首个公开发布版本。

## 亮点

- Windows 图形界面，用于把选中的文件和目录移动到可携带缓存根目录。
- 可在另一台 Windows 机器上恢复符号链接。
- 命令行健康检查支持纯文本和 JSON 输出。
- 支持英文和中文界面。
- 用户目录下的路径使用 `~` 保存，方便跨用户配置文件使用。
- 适合备份/同步工具工作流，但不要求使用特定工具。

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 快速开始

1. 把 `LinkShelf.exe` 放到要作为缓存根目录的文件夹中。
2. 双击程序并添加文件或目录。
3. 使用你信任的工具备份、复制或同步整个缓存根目录。
4. 在另一台机器上，把 `LinkShelf.exe` 放到恢复后的缓存根目录，然后点击 `恢复链接`。

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
