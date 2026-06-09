# Link Shelf

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![License](https://img.shields.io/badge/license-MIT-green)
![Build](https://img.shields.io/badge/build-GitHub%20Actions-informational)

[English](README.en.md)

Windows 配置迁移与符号链接工具：把分散的应用设置、dotfiles 和小型状态目录收进一个缓存根目录，再用符号链接恢复原路径。

适合整理开发环境、AI 编程工具配置、终端/编辑器设置和小型应用状态。备份或同步交给你信任的工具，Link Shelf 只负责本机路径搬迁、链接恢复和健康检查。

关键词：Windows 符号链接、软链接、硬链接、dotfiles、配置迁移、配置备份、AI 编程工具配置、开发环境恢复。

**下载：** [LinkShelf.exe](https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe) | [最新发布页](https://github.com/xiayukun/LinkShelf/releases/latest)

![Link Shelf preview](Assets/app-preview-cn.png)

## 快速开始

1. 从发布页下载 [`LinkShelf.exe`](https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe)。
2. 把它放到你想作为缓存根目录的文件夹里。
3. 双击 `LinkShelf.exe`。
4. 点击 `添加项目`。
5. 选择一个文件或目录。
6. Link Shelf 会把它移动到缓存根目录，并在原位置创建符号链接。

如果你已经把缓存根目录备份、复制或同步到另一台电脑，把 `LinkShelf.exe` 放进去，然后点击 `恢复链接`。

## 目录

- [存在意义](#存在意义)
- [主要功能](#主要功能)
- [安装](#安装)
- [图形界面用法](#图形界面用法)
- [命令行用法](#命令行用法)
- [AI 和自动化](#ai-和自动化)
- [配置](#配置)
- [运行时目录](#运行时目录)
- [隐私](#隐私)
- [从源码构建](#从源码构建)
- [适合场景](#适合场景)
- [不适合场景](#不适合场景)
- [备份和同步提示](#备份和同步提示)
- [路线图](#路线图)
- [参与贡献](#参与贡献)
- [维护者文档](#维护者文档)
- [鸣谢](#鸣谢)
- [许可证](#许可证)

## 存在意义

很多工具会把重要状态存放在项目目录之外：

- 编辑器设置
- 命令行工具资料
- 模型代理配置
- 包管理器配置文件
- 小型应用数据目录

Link Shelf 可以把这些路径放到一个“架子”上，让它们作为一个目录被备份、迁移或检查。

它不是同步软件，也不会替你判断哪些缓存可以跨机器共享。你可以搭配云盘、备份软件、外置硬盘、网络共享或其他同步工具，但应先确认内容适合被复制到另一台机器。

## 主要功能

- 把一个或多个文件或目录加入缓存根目录。
- 在原位置创建符号链接。
- 在另一台 Windows 电脑上按配置恢复所有链接。
- 检查损坏链接、缺失缓存项、错误链接目标和目标路径冲突。
- 遇到冲突时逐项确认，不静默覆盖。
- 添加本机检测到的推荐路径，例如 Cursor、VS Code、Git、npm、Codex、Claude、JetBrains 和 Clash 状态。
- 当本机存在对应路径时，收纳 Codex、Claude、Gemini、Cursor、Cline、Roo Code、Continue、aider、Windsurf、GitHub Copilot 等 AI 编程工具状态。
- 当添加、恢复链接或搬回原位/撤销时遇到 Windows 返回拒绝访问，检测正在使用目标路径的进程，并可结束进程后重试。
- 把选中的项目搬回原始位置，并移除对应缓存项和配置记录。
- 通过硬链接把程序投射到另一个目录，让那个目录成为独立缓存根目录，而不需要复制完整可执行文件。
- 同一个程序既可以双击打开界面，也可以作为命令行检查工具。
- 使用 `~` 保存用户目录下的路径，方便跨电脑使用。
- 配置、日志、程序名和运行目录使用英文。
- 界面支持中英文切换，并可按当前 Windows 界面语言自动选择。

## 安装

从发布页下载最新的 [`LinkShelf.exe`](https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe)，然后把它放到你想作为缓存根目录的文件夹里。

示例：

```powershell
C:\Users\you\AppData\Local\LinkShelf\LinkShelf.exe
```

程序所在目录就是缓存根目录。这是刻意设计：工具和被管理的缓存可以一起移动。

## 图形界面用法

双击 `LinkShelf.exe`。

图形界面会请求管理员权限，因为 Windows 创建符号链接通常需要提权，除非开发者模式已经允许当前用户创建链接。

常见流程：

1. 点击 `添加项目`。
2. 选择 `添加推荐项目`、`添加目录` 或 `添加文件`。
3. 选择一个或多个原始路径。
4. Link Shelf 把它移动到缓存根目录。
5. Link Shelf 在原始路径创建符号链接。
6. 用你喜欢的工具备份、同步或移动缓存根目录。

推荐项目只会显示本机存在、并且尚未写入 `link-shelf.config.json` 的路径。内置清单来自作者本人日常 Windows 环境，以及 AI 联网调研后认为很多人会使用的开发工具、AI 编程工具、编辑器、终端和包管理器配置路径。选择推荐项目后，会执行和手动选择该路径一样的添加流程。

部分推荐路径可能包含账号名、令牌、本地历史或其他私有状态。用云盘、同步工具或备份工具处理前，请先检查对应文件夹内容。

![推荐项目窗口](Assets/recommended-items-window-cn.png)

在第二台电脑上：

1. 把 `LinkShelf.exe` 放到已备份、复制或同步过来的缓存根目录。
2. 双击它。
3. 点击 `恢复链接`。
4. 处理任何目标路径冲突。

如果配置中有记录，但缓存项已经不存在，`恢复链接` 仍然可以在原始位置存在真实内容时，把原始位置内容导入缓存。此时和缓存内容相关的选项会被置灰，因为没有缓存内容可用。

如果缓存根目录里有文件或目录，但没有写入 `link-shelf.config.json`，Link Shelf 会把它显示为多出的缓存项。选中它并点击 `恢复链接` 时，可以选择原始路径；Link Shelf 会先保存配置记录，再询问该路径存在冲突时应该如何处理。

### 文件占用处理

添加文件或目录时，Link Shelf 会先尝试正常移动。只有 Windows 返回拒绝访问时，才会打开文件占用处理窗口并扫描选中路径，找出正在占用它的进程。

在这个窗口里，你可以查看占用进程，右键结束选中的进程，也可以点击“结束全部并继续”。程序会在结束进程后短暂等待，然后重新执行刚才的移动操作。如果路径仍然被占用，会再次打开同一个处理窗口。

在另一台电脑上点击 `恢复链接`，或使用 `搬回原位/撤销` 时，如果目标路径同样因为拒绝访问而失败，也会打开同一个文件占用处理窗口，处理后再重试原操作。

![文件占用处理窗口](Assets/lock-resolution-window-cn.png)

### 投射程序

点击语言选择器旁边的 `投射程序`，可以在另一个目录中创建 `LinkShelf.exe` 的硬链接。从这个投射出来的程序启动时，会把它所在目录作为缓存根目录，因此可以管理另一个独立缓存根目录，而不需要再存放一份完整的单文件程序。

投射程序使用 Windows 硬链接，所以目标目录必须和当前 `LinkShelf.exe` 在同一个盘符下。如果所选目录里已经有 `LinkShelf.exe`，Link Shelf 不会覆盖它。

### 搬回原位/撤销

选中一行或多行后点击 `搬回原位/撤销`，Link Shelf 会移除原路径链接，把缓存内容搬回原始位置，并删除配置记录。只有当原始位置不存在，或仍然是指向该缓存项的正确链接时才会执行。如果原始位置已经有真实内容，程序会跳过撤销，避免覆盖用户数据。如果 Windows 在移除链接或搬回缓存项时返回拒绝访问，会先打开文件占用处理窗口，处理后再重试。

如果缓存项已经不存在，Link Shelf 会询问是否只删除配置记录。确认后不会移动或删除任何外部文件或目录。

如果选中行是没有配置记录的多出缓存项，`搬回原位/撤销` 会提示 Link Shelf 不知道它的目标位置，只能从缓存根目录中删除这个缓存项。

## 命令行用法

同一个可执行文件支持适合自动化的命令：

```powershell
.\LinkShelf.exe check
.\LinkShelf.exe check --json
.\LinkShelf.exe check --verbose
.\LinkShelf.exe status
.\LinkShelf.exe cache-root
.\LinkShelf.exe version
.\LinkShelf.exe help
.\LinkShelf.exe -help
```

退出码：

- `0`：所有检查项正常
- `1`：发现一个或多个问题
- `2`：命令错误或程序错误

推荐自动化命令：

```powershell
.\LinkShelf.exe check --json
```

只有 `problemCount` 大于 `0` 时才需要提醒用户。

## AI 和自动化

很多 AI 编程工具会把重要状态放在项目目录之外。Link Shelf 可以把 Codex、Claude、Gemini、Cursor、Cline、Roo Code、Continue、aider、Windsurf、GitHub Copilot 和编辑器设置收进同一个缓存根目录，然后在另一台 Windows 电脑上通过符号链接恢复原路径。

让用户自己的 AI 助手检查 Link Shelf 时，最安全的入口是命令行：

```powershell
.\LinkShelf.exe cache-root
.\LinkShelf.exe check --json
.\LinkShelf.exe help
```

可以直接给 AI 助手的提示词：

```text
我在 Windows 上使用 Link Shelf，把分散的应用状态、dotfiles、AI 编程工具设置和小型配置文件夹移动到一个缓存根目录，再通过符号链接恢复原路径。

请先运行 `LinkShelf.exe cache-root` 找到缓存根目录。
再运行 `LinkShelf.exe check --json`，只解释不健康的项目。
除非我明确要求，不要移动、删除、覆盖、恢复或重新创建链接。
如果我要你推荐可收纳路径，优先考虑开发工具、AI 编程工具、编辑器、终端、包管理器和小型应用状态目录。同步前提醒我检查密钥、令牌、本地历史记录和账号相关文件。
```

本仓库还包含 [AGENTS.md](AGENTS.md)，这是给维护本项目的 AI 编程助手看的项目说明。如果你使用 Codex 或其他 AI 编程助手维护这个项目，建议先让它读取 `AGENTS.md`，这样它能理解项目目标、安全边界、配置格式和发布流程。

Codex 自动化也适合用来定时检查链接健康状态。可以让自动化任务定时在缓存根目录运行：

```powershell
.\LinkShelf.exe check --json
```

推荐行为：

- 定时从缓存根目录运行命令。
- 读取 JSON 输出。
- `problemCount` 为 `0` 时保持安静。
- `problemCount` 大于 `0` 时提醒用户，因为这通常表示链接缺失、指向错误，或缓存项不存在。

## 配置

Link Shelf 写入：

```text
link-shelf.config.json
```

当前配置架构使用英文键：

```json
{
  "version": 2,
  "cacheId": "generated-id",
  "updatedAt": "2026-06-04 19:00:00",
  "items": [
    {
      "cacheName": ".codex",
      "originalPath": "~\\.codex",
      "type": "directory",
      "linkMode": "symbolic-link",
      "status": "enabled",
      "sourceMachine": "DESKTOP",
      "createdAt": "2026-06-04 19:00:00",
      "updatedAt": "2026-06-04 19:00:00",
      "lastOperation": "add-sync-item",
      "note": ""
    }
  ]
}
```

旧配置文件不会自动迁移。使用这个版本前，请把配置文件改名或重新生成成 `link-shelf.config.json`。

## 运行时目录

Link Shelf 会在缓存根目录里创建这些文件夹：

```text
.link-shelf-logs
.link-shelf-backups
```

在冲突处理移动或替换已有内容前，程序会先创建备份。

## 隐私

Link Shelf 是本地工具。它不会把文件、路径、日志、配置或机器名上传到远程服务。如果缓存根目录由同步工具、云盘或其他工具处理，网络传输由那些外部工具负责。

更多细节见 [隐私说明](PRIVACY.md)。

## 从源码构建

要求：

- Windows
- .NET SDK

构建：

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

发布 Windows 单文件程序：

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

## 适合场景

Link Shelf 适合：

- 备份、迁移或恢复开发工具设置
- 让应用状态更容易备份
- 把分散配置路径移动到一个受管理的文件夹
- 检查基于符号链接的环境是否仍然健康
- 重装 Windows 或设置新电脑后恢复链接

## 不适合场景

Link Shelf 不适合：

- 盲目共享非常大的应用缓存、数据库或高频变化目录
- 替代版本控制系统
- 替代云同步、文件同步或备份软件
- 只链接目录内部的一部分文件
- 管理 Windows 快捷方式文件（`.lnk`），因为它们被移动并链接回原位置后行为不可靠
- 静默覆盖已有目标内容

## 备份和同步提示

Link Shelf 只负责本机路径搬迁和符号链接恢复。缓存根目录如何备份、复制或同步，由你选择的外部工具负责。

建议优先处理小型配置文件、dotfiles、编辑器设置和低频变化的应用状态。不要盲目跨机器共享大型缓存、数据库、浏览器资料、正在运行程序占用的目录，或包含令牌和本地历史记录的路径。

如果确实要使用同步工具，先关闭相关应用，备份原始内容，并确认冲突处理规则。Link Shelf 不会替外部同步工具解决文件冲突。

## 路线图

- 签名发布
- 更安全的首次运行引导
- 恢复链接的可选试运行模式
- 可导出的诊断报告

## 参与贡献

欢迎贡献。请从 [CONTRIBUTING.md](CONTRIBUTING.md) 开始，并保持文件移动行为保守：Link Shelf 不应该静默删除、覆盖或合并用户内容。

发布规划和 GitHub 上线说明见 [docs/github-launch-checklist.md](docs/github-launch-checklist.md)。

## 维护者文档

仅维护者需要的上线、发布、截图、签名和交接说明放在 [docs](docs)。普通用户可以忽略它们。签名说明见 [Windows 程序签名](docs/windows-code-signing.md)。

## 鸣谢

文件占用检测代码改写自 PolarGoose 的 [ShowWhatProcessLocksFile](https://github.com/PolarGoose/ShowWhatProcessLocksFile)。

文件占用处理流程也参考了微软开源项目 PowerToys 的 [File Locksmith](https://learn.microsoft.com/en-us/windows/powertoys/file-locksmith)。详细来源见 [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md)。


## 许可证

MIT。
