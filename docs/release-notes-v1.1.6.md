English release notes: [docs/release-notes-v1.1.6.en.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.1.6.en.md)

这个版本把 2.0 探索中最稳妥的自动化能力回迁到当前 Windows 版：新增只读推荐项命令，方便脚本和 AI 助手查看本机当前可添加的路径，同时继续保持主页简短直接。

## 亮点

- 新增 `LinkShelf.exe recommended`，以文本形式列出本机当前可添加的推荐路径。
- 新增 `LinkShelf.exe recommended --json`，输出机器可读 JSON。
- 推荐项命令只读，不会移动、删除、恢复或创建链接。
- `README.md` 继续保持短主页定位，完整说明放在 `docs/user-guide.md`。
- 首页不再把某个同步工具作为主卖点，改为强调通用备份/同步风险提示。
- 2.0 跨平台核心拆分和 macOS 推进已保留在独立分支，当前版本不引入这部分结构调整。

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 创建符号链接通常需要管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 说明

- Link Shelf 仍然是本地工具。它不会上传文件、路径、日志、配置或机器名。
- `recommended --json` 只会列出本机存在、且尚未被启用配置覆盖的推荐项；真正移动内容仍需用户在图形界面中确认。
