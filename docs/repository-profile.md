# 仓库资料

English: [repository-profile.en.md](repository-profile.en.md)

建议 GitHub 仓库名：

```text
link-shelf
```

建议描述：

```text
Windows 配置迁移和符号链接工具 / Windows config mover: collect app settings, dotfiles, and small state folders, then restore paths with symlinks.
```

建议网站：

```text
https://github.com/xiayukun/LinkShelf/releases/latest
```

建议主题：

```text
windows
symlink
symbolic-link
hardlink
backup
app-state
config-backup
config-migration
dotfiles
dotfiles-manager
wpf
dotnet
ai-tools
developer-tools
config-management
```

建议社交预览文字：

```text
Link Shelf collects scattered Windows app settings, dotfiles, developer config, and AI coding tool state into one cache root, then restores original paths with symlinks.
```

简短发布介绍：

```text
我做了 Link Shelf，这是一个 Windows 配置迁移和符号链接工具。它可以把分散的应用设置、dotfiles、开发工具配置和 AI 编程工具状态收进一个缓存根目录，再用符号链接恢复原路径。它支持推荐本机常见配置路径、在新机器上恢复链接、用命令行检查链接健康，也可以在不想继续管理时把项目搬回原位。备份、复制或同步缓存根目录可以交给用户自己信任的工具，但不建议盲目跨机器共享大型缓存、数据库或高频变化目录。
```

## 推广判断

- README 应保持短主页；完整说明放在 `docs/user-guide.md` / `docs/user-guide.en.md`。
- 保留 `README.md` 作为默认 GitHub 入口，并让它中文优先，因为当前启动推广的核心受众是中文用户。完整英文版放在 `README.en.md`，用于 GitHub 搜索、全球用户和外部引用。
- 仓库里面向读者的 Markdown 改为中文默认 `.md`，英文配套使用 `.en.md`。不再保留旧中文命名兼容文件；发布说明里的互链应指向当前 `main` 分支，避免旧 tag 中没有新文件名时出现断链。
- `LICENSE` 继续保留标准英文 MIT 文本，方便 GitHub 识别许可证并维持法律文本原貌；其他说明性页面使用中文默认文档。
- AI 方向值得做，但不要写成“让 AI 推销产品”。更可信的角度是：Link Shelf 能帮助用户整理、备份和恢复本地 AI 编程工具配置。
- 不再把任何特定同步工具作为主页主卖点。外部工具只作为可选搭配出现，并提醒用户不要盲目共享缓存、数据库和高频变化目录。
- 适合放进 README、Release、topics 和发布帖的关键词：Windows symlink manager、symbolic link、hard link、dotfiles manager、config migration、config backup、app state backup、AI coding tools、Codex、Claude、Gemini、Cursor、Windows 符号链接、软链接、硬链接、配置迁移。
