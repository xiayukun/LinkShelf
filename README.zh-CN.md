# Link Shelf

[English](README.md)

Link Shelf 是一个 Windows 桌面和命令行工具。它可以把分散在本机各处的文件或目录移动到一个可携带的缓存根目录中，然后在原位置创建符号链接，让原来的应用继续按原路径访问这些内容。

它适合用来管理开发工具配置、应用状态、小型工作目录、命令行工具资料等。Syncthing 是推荐搭配方式，但不是唯一用途；你也可以把缓存根目录交给云盘、备份工具、移动硬盘或网络共享来管理。

![Link Shelf preview](Assets/app-preview-cn.png)

## 快速开始

1. 从发布页下载 `LinkShelf.exe`。
2. 把它放到你想作为缓存根目录的文件夹里。
3. 双击 `LinkShelf.exe`。
4. 点击 `添加项目`。
5. 选择一个文件或目录。
6. Link Shelf 会把它移动到缓存根目录，并在原位置创建符号链接。

在另一台电脑上，只需要把 `LinkShelf.exe` 放到已经同步或恢复的缓存根目录里，然后点击 `恢复链接`。

## 主要功能

- 把文件或目录加入缓存根目录。
- 在原位置创建符号链接。
- 在另一台 Windows 电脑上按配置恢复链接。
- 检查缓存项缺失、链接缺失、链接指向错误、目标位置冲突等问题。
- 遇到冲突时逐项确认，不静默覆盖。
- 同一个程序既可以双击打开界面，也可以作为命令行检查工具。
- 使用 `~` 保存用户目录下的路径，方便跨电脑使用。
- 配置、日志、备份目录、程序名都使用英文。
- 界面支持中英文切换，并可按系统语言自动选择。

## 命令行

```powershell
.\LinkShelf.exe check
.\LinkShelf.exe check --json
.\LinkShelf.exe check --verbose
.\LinkShelf.exe status
.\LinkShelf.exe cache-root
.\LinkShelf.exe version
.\LinkShelf.exe help
```

退出码：

- `0`：所有项目正常
- `1`：发现异常项目
- `2`：命令错误或程序错误

推荐自动化命令：

```powershell
.\LinkShelf.exe check --json
```

只有 `problemCount` 大于 `0` 时才需要通知用户。

## AI 和自动化

本仓库包含 [AGENTS.md](AGENTS.md)，这是给 AI 编程助手看的项目说明。如果你使用 Codex 或其他 AI 编程助手维护这个项目，建议先让它读取 `AGENTS.md`，这样它能理解项目目标、安全边界、配置格式和发布流程。

Codex 自动化也很适合用来定时检查链接健康状态。可以让自动化任务定时在缓存根目录运行：

```powershell
.\LinkShelf.exe check --json
```

推荐行为：

- 定时运行命令。
- 读取 JSON 输出。
- `problemCount` 为 `0` 时保持安静。
- `problemCount` 大于 `0` 时提醒用户，因为这通常表示链接脱落、链接指向错误，或者缓存项已经不存在。

## 配置文件

Link Shelf 使用：

```text
link-shelf.config.json
```

运行目录：

```text
.link-shelf-logs
.link-shelf-backups
```

旧中文配置不会自动兼容。使用新版时，请直接把配置文件改名或重新生成成 `link-shelf.config.json`。

## 隐私

Link Shelf 是本地工具。它不会把文件、路径、日志、配置或机器名上传到远程服务。如果缓存根目录由 Syncthing、云盘或其他工具同步，网络传输由那些外部工具负责。

更多细节见 [PRIVACY.md](PRIVACY.md)。

## 适合场景

- 多电脑同步开发工具配置。
- 把分散的应用状态集中到一个可备份目录。
- 重装系统后快速恢复链接。
- 用命令行定时检查链接是否失效。
- 配合 Syncthing、云盘、备份工具或移动硬盘管理同一个缓存根目录。

## 不适合场景

- 盲目同步超大的纯缓存目录。
- 替代版本控制系统。
- 替代 Syncthing、云盘或备份工具。
- 只链接目录内部的一部分文件。
- 静默覆盖已有目标内容。

## 配合 Syncthing

推荐搭配方式：

- Syncthing 负责同步缓存根目录。
- Link Shelf 负责在本机创建和恢复符号链接。
- Syncthing 的忽略规则负责决定目录内部哪些文件不同步。

Link Shelf 以整个文件或整个目录为最小管理单位。

## 维护文档

`docs` 目录建议一起提交到 GitHub。它不是程序运行必需文件，而是给维护者使用的发布清单、仓库介绍资料、截图检查清单和上线准备说明。

- [首次推送清单](docs/first-push.md)
- [GitHub 上线清单](docs/github-launch-checklist.md)
- [发布清单](docs/release-checklist.md)
- [发布说明模板](docs/release-notes-template.md)
- [Link Shelf 1.0.0 发布说明](docs/release-notes-v1.0.0.md)
- [仓库介绍资料](docs/repository-profile.md)
- [截图检查清单](docs/screenshots.md)

## 许可证

MIT。
