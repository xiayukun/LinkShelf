# 仓库资料

English: [repository-profile.md](repository-profile.md)

建议 GitHub 仓库名：

```text
link-shelf
```

建议描述：

```text
Portable Windows app-state and dotfiles manager: move scattered config into one syncable cache root, then restore original paths with symlinks.
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
sync
dotfiles
dotfiles-manager
syncthing
wpf
dotnet
ai-tools
developer-tools
config-management
```

建议社交预览文字：

```text
Link Shelf turns scattered Windows app state, dotfiles, developer settings, and AI coding tool config into one portable cache root while apps keep using their original paths.
```

简短发布介绍：

```text
我做了 Link Shelf，这是一个 Windows 上的便携应用状态和 dotfiles 收纳工具。它可以把分散的应用状态、开发工具设置和 AI 编程工具配置收进一个可同步缓存根目录，同时通过符号链接让应用继续使用原路径。它支持推荐本机常见配置路径、在新机器上恢复链接、用命令行检查链接健康，也可以在不想继续管理时把项目搬回原位。Syncthing 和它很搭，但任何备份或同步工作流都可以使用同样思路。
```

## 推广判断

- 暂时保留 `README.md` 作为默认 GitHub 入口。GitHub、外部索引、搜索摘要和很多外链都默认识别这个文件名，把所有文档改成 `*-en.md` 会带来大量链接和维护成本，推广收益并不确定。
- 更稳的做法是增强默认 README：保留英文入口，顶部放中文说明链接，并在首屏加入中英文都能命中的搜索词。
- AI 方向值得做，但不要写成“让 AI 推销产品”。更可信的角度是：Link Shelf 能帮助用户安全同步和恢复本地 AI 编程工具配置。
- 适合放进 README、Release、topics 和发布帖的关键词：Windows symlink manager、symbolic link、hard link、dotfiles manager、config sync、app state backup、Syncthing、AI coding tools、Codex、Claude、Gemini、Cursor、Windows 符号链接、软链接、硬链接、配置同步。
