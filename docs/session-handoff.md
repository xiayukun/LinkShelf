# 会话交接文档

中文：[session-handoff.zh-CN.md](session-handoff.zh-CN.md)

这个文档用于把本次会话里沉淀出的高价值上下文交给后续会话。新的 Codex 会话接手本项目时，应该先读取 `AGENTS.md`，再读取本文件。

## 当前项目

- 项目名：`Link Shelf`
- 远程仓库：`git@github.com:xiayukun/LinkShelf.git`
- GitHub 页面：`https://github.com/xiayukun/LinkShelf`
- 最新 GitHub Release：`v1.1.4`
- 当前源码和本地运行程序版本：`1.1.4`
- 发布页面：`https://github.com/xiayukun/LinkShelf/releases/tag/v1.1.4`
- 下载地址：`https://github.com/xiayukun/LinkShelf/releases/download/v1.1.4/LinkShelf.exe`
- 主分支：`main`
- 当前 Git HEAD：`Prepare Link Shelf 1.1.4`（具体 hash 以 `git log -1 --oneline` 为准）
- 当前工作区状态：`main` 已包含 `v1.1.4` 发布提交；发布前应确认 tag 已推送、GitHub Release 已生成。

## 本地结构

- 缓存根目录：`C:\Users\11467\AppData\Local\同步缓存`
- 源码仓库：`C:\git\其他\LinkShelf`
- 运行程序：`C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe`
- 配置文件：`C:\Users\11467\AppData\Local\同步缓存\link-shelf.config.json`
- 运行日志目录：`C:\Users\11467\AppData\Local\同步缓存\.link-shelf-logs`
- 冲突备份目录：`C:\Users\11467\AppData\Local\同步缓存\.link-shelf-backups`

如果迁移源码仓库目录，优先复制整个 `LinkShelf` 仓库目录，包括 `.git` 和任何未提交改动。不要只从 GitHub 重新克隆仍未发布的本地修正。

如果迁移运行缓存根目录，要记住 `LinkShelf.exe` 所在目录就是缓存根目录。把 `LinkShelf.exe` 移到新目录后，新目录会成为新的运行根目录，配置、日志、备份和缓存项都应一起迁移。

## 项目目的

`Link Shelf` 的目标是把分散在本机各处的文件或目录移动到程序所在的缓存根目录中，然后在原始位置创建符号链接，让原来的应用继续按原路径访问这些内容。

典型流程：

1. 把 `LinkShelf.exe` 放在缓存根目录中。
2. 选择需要同步或备份的文件、目录。
3. 程序把它们移动到缓存根目录。
4. 程序在原始位置创建链接。
5. `Syncthing` 或其他同步工具同步整个缓存根目录。
6. 另一台电脑拿到同一个缓存根目录后，运行 `Link Shelf` 恢复本机链接。

`Syncthing` 是推荐搭配工具，但不是唯一用途。

## 关键设计决策

- 程序所在目录就是缓存根目录。
- 同一个 `LinkShelf.exe` 同时支持图形界面和命令行。
- 配置键、运行目录名、程序名、操作码都使用英文。
- 界面支持中文和英文。
- 语言下拉框里永远显示 `English` 和 `中文`，不要随当前语言翻译这两个选项。
- 当前配置版本是 `2`。
- 不再兼容旧中文键配置。
- 用户目录下的路径保存时使用 `~`，方便跨电脑兼容。
- 最小管理单位是整个文件或整个目录；目录内部的排除规则交给 `Syncthing` 或其他同步工具处理。
- 添加文件和添加目录支持多选；批量添加中遇到取消或失败时应停止后续项目。
- `投射程序` 使用同盘硬链接把 `LinkShelf.exe` 投射到另一个目录。从投射链接启动时，该目录会成为独立缓存根目录。
- Windows 快捷方式文件（`.lnk`）不支持加入 Link Shelf。文件选择器需要保留 `.lnk` 本体路径，以便识别后弹出本地化警告并跳过。
- 如果配置中有记录但缓存项不存在，`恢复链接` 会在原始路径有真实内容时只允许“使用原位置内容”导入缓存；`搬回原位/撤销` 会询问是否只删除 JSON 记录。
- 如果缓存根目录中有文件或目录但 JSON 中没有记录，GUI 会把它显示为“多出缓存项”。对它执行 `恢复链接` 时，先让用户选择原始路径并立即写入 JSON，再进入正常冲突处理；对它执行 `搬回原位/撤销` 时，会提示没有已知目标位置，确认后删除这个缓存项。

## 已发布内容

GitHub 上已经有自动构建和自动发布能力：

- 构建工作流：`.github/workflows/build.yml`
- 发布工作流：`.github/workflows/release.yml`
- 首版发布说明：`docs/release-notes-v1.0.0.md`
- 最新发布说明：`docs/release-notes-v1.1.4.md`

`release` 工作流可以创建或更新 GitHub Release，并上传 `LinkShelf.exe`。

首个公开版本已经手动运行过发布工作流：

- 分支：`main`
- 标签：`v1.0.0`
- 发布结果：成功

## v1.1.4 发布内容

本次发布重点：

- 拒绝添加 Windows 快捷方式文件（`.lnk`），弹出警告并跳过。
- 当 JSON 记录存在但缓存项缺失时：
  - `恢复链接` 如果原始路径有真实内容，只启用“使用原位置内容”选项。
  - `搬回原位/撤销` 可确认只删除 JSON 记录，不移动文件。
- 当缓存根目录存在未被 JSON 记录管理的文件或目录时：
  - 表格显示为“多出缓存项”。
  - `恢复链接` 会先选择原始路径并保存配置，再继续冲突处理。
  - `搬回原位/撤销` 会确认删除该缓存项。
- 已更新 `AGENTS.md`、`README.md`、`CHANGELOG.md`、发布说明和相关 GUI/模型代码。
- `v1.1.4` 发布说明不再包含顶层版本标题，避免 GitHub Release 页面出现重复标题。

当前已修改文件大致包括：

- `AGENTS.md`
- `AGENTS.zh-CN.md`
- `ConflictChoiceWindow.xaml.cs`
- `MainWindow.xaml.cs`
- `Models/SyncModels.cs`
- `README.md`
- `README.zh-CN.md`
- `Services/FileOperations.cs`
- `Services/LocalizationService.cs`
- `docs/release-notes-template.md`
- `docs/release-notes-template.zh-CN.md`
- `docs/session-handoff.md`
- `docs/session-handoff.zh-CN.md`

## 当前自动化巡检

当前会话中创建过一个 Codex 心跳自动化：

- 自动化编号：`link-shelf`
- 名称：`Link Shelf 链接健康巡检`
- 频率：每 30 分钟
- 命令：`& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" check --json`
- 当前设置为普通权限运行，不申请管理员权限。
- 如果 `problemCount` 是 `0`，保持安静。
- 如果 `problemCount` 大于 `0`，提醒用户并列出异常项。

它改为普通权限运行，是为了避免每次巡检都走管理员提权审批。之前有一次提权审批失败，原因是审批模型临时容量不足，不是会话上下文容量不足。

## 已知缓存项

当前缓存里包含过这些工具或应用状态：

- `.claude`
- `.claude.json`
- `.codex`
- `.cursor`
- `.gemini`
- `.vscode`
- `.mcp.json`
- `Cursor` 相关子目录，例如 `Backups`、`Workspaces`、`Dictionaries`、`User`
- `Clash Verge` 配置目录：`io.github.clash-verge-rev.clash-verge-rev`

不要把这个列表当成最终事实。实际状态以 `LinkShelf.exe check --json` 和配置文件为准。

## 重要经验

`.claude.json` 可能会被相关工具反复创建。如果原始位置变成真实文件而不是链接，巡检会报告 `target-has-content`。这时应通过恢复链接的冲突处理流程决定使用缓存内文件，还是使用原始位置文件。

`Clash Verge` 配置目录即使在管理员权限下也可能移动失败。原因通常不是权限本身，而是程序、服务或内核进程正在占用目录内文件。需要先停止：

- `clash-verge`
- `clash-verge-service`
- `verge-mihomo`

恢复链接时也可能遇到拒绝访问，尤其是另一台电脑上已有程序正在使用目标路径时。图形界面应打开文件占用处理窗口，用户结束占用进程后继续重试同一个恢复项目。

搬回原位/撤销时也可能遇到拒绝访问。原始链接仍存在时优先扫描原始路径；如果原始链接已移除但缓存项搬回失败，应扫描缓存项路径，然后重试同一个撤销项目。

添加项目如果已经把内容移入缓存，但随后创建链接或保存配置失败，应尽量把已移动内容回滚到原始路径。用户在占用处理窗口点击取消后，不应继续处理批量选择中的后续项目。

投射功能已经验证过硬链接行为：从硬链接位置启动 `LinkShelf.exe cache-root` 时，程序返回硬链接所在目录，而不是原始可执行文件所在目录。因此投射链接可以作为另一个缓存根目录的入口；但硬链接不能跨盘符。

检查结果异常时，配置里的 `status` 仍然保持 `enabled`，但表格状态列应显示异常，避免用户误以为该项目健康。

用户目录情况比较特殊：账号名是 `xiayukun`，但很多实际路径仍然在 `C:\Users\11467` 下。不要只根据账号名推断用户目录，应以程序和系统接口返回的路径为准。

迁移目录时，优先用程序实际输出确认路径，不要凭记忆改配置。常用检查：

```powershell
& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" cache-root
& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" version
& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" check --json
```

如果源码仓库迁移到新目录，迁移后先运行：

```powershell
git status --short --branch
git log --oneline --decorate -5
dotnet build .\LinkShelf.csproj -c Release
```

如果运行程序迁移到新缓存根目录，迁移后先确认：

```powershell
.\LinkShelf.exe cache-root
.\LinkShelf.exe version
.\LinkShelf.exe check --json
```

## 常用命令

巡检链接状态：

```powershell
& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" check --json
```

构建：

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

发布本地单文件程序：

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

查看 Git 状态：

```powershell
git status --short --branch
git log --oneline --decorate -5
```

## 推荐下个会话开场白

```text
请先读取 AGENTS.md 和 docs/session-handoff.md，然后检查当前项目状态。这个项目是 Link Shelf，用于把分散的本地文件或目录移动到同步缓存根目录，并通过符号链接恢复原路径。项目已经发布到 GitHub，最新公开版本是 v1.1.4。请先不要重构，先确认当前仓库路径、运行缓存根目录、Git 状态、Release 状态、Markdown 中英文配对、自动化巡检和配置健康状态，再继续后续开发或迁移。
```

## 维护规则

- 如果发布状态、自动化行为、缓存根路径或重大设计决策发生变化，应更新本文件。
- 不要把敏感配置、密钥、令牌或私人文件内容写进本文件。
- 本文件用于交接上下文，不替代 `README.md`、`AGENTS.md` 或正式发布文档。
