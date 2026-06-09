English release notes: [docs/release-notes-v1.1.5.md](https://github.com/xiayukun/LinkShelf/blob/v1.1.5/docs/release-notes-v1.1.5.md)

这个版本主要改进 GitHub 公开展示：默认主页改为中文优先，补齐完整英文配套文档，并让命令行帮助更适合 AI 助手和自动化读取。

## 亮点

- `README.md` 现在是中文优先的 GitHub 默认主页。
- `README.en.md` 是完整英文说明。
- `README.zh-CN.md` 保留为旧链接兼容入口。
- README 首屏强化了 Windows 符号链接、软链接备份、硬链接投射、dotfiles、应用状态备份、Syncthing 和 AI 编程工具配置同步等关键词。
- AI 和自动化章节现在包含可以直接复制给用户 AI 助手的提示词。
- `LinkShelf.exe -help` 现在可作为命令行帮助别名使用。
- 命令行帮助现在说明缓存根目录模型、`check --json` 和只读自动化行为。
- 发布工作流通过 `workflow_dispatch` 手动重跑时，现在会 checkout 用户指定的 tag。

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 创建符号链接通常需要管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 说明

- Link Shelf 仍然是本地工具。它不会上传文件、路径、日志、配置或机器名。
- 同步 AI 工具设置和其他推荐路径前，请先检查内容，因为其中可能包含令牌、账号名、本地历史记录或其他私有状态。
