# Link Shelf 代理说明

本文件供在此仓库中工作的 AI 编码代理使用。面向用户的产品文档应放在 `README.md` 中。

修改项目前，也要读取 `docs/session-handoff.md`。它记录当前本地设置、GitHub 发布状态、自动化行为，以及设置会话中的重要经验。

## 产品目标

Link Shelf 是一个 Windows 工具，可以把用户选择的文件或目录移动到可执行程序所在目录，然后在原始位置创建符号链接。

它的主要用途是把路径迁移到一个便携缓存根目录：

- 只备份一个受管理目录，而不是分散路径。
- 在多台电脑之间同步选定的应用或开发工具状态。
- 把本地配置移动到便携目录中。
- 设置新机器后恢复链接。
- 通过命令行健康检查支持自动化。

Syncthing 是推荐搭配工具，但不是唯一支持场景。Syncthing 可以在机器之间同步缓存根目录，Link Shelf 负责处理本机符号链接。

## 核心约定

可执行程序所在目录就是缓存根目录。

主要运行文件和目录：

- `LinkShelf.exe`
- `link-shelf.config.json`
- `.link-shelf-logs`
- `.link-shelf-backups`

不要创建中文文件名、目录名、配置键或可执行文件名。中文界面文字只能放在 `Services/LocalizationService.cs` 和中文文档文件中。

## 配置

当前配置结构使用英文键：

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

`ConfigStore` 只读取当前英文结构。旧的中文键配置文件不再支持。

当前用户配置目录下的路径必须用 `~` 保存，这样配置才能跨不同 Windows 用户目录使用。

## 主要功能

添加项目：

1. 用户选择一个文件或目录。
2. 程序拒绝缓存根目录本身、缓存根目录内的路径，以及包含缓存根目录的父目录。
3. 程序把所选路径移动到缓存根目录。
4. 程序在原始路径创建符号链接。
5. 程序保存配置记录。

推荐添加项目：

- `RecommendedSyncItems` 定义内置预设，包含英文键、便携路径、预期项目类型和推荐理由键。
- 预设应优先覆盖广泛有用的开发工具、AI 编程工具、编辑器、终端、包管理器和小型应用状态路径。不要加入纯个人化应用选择，除非它们同时也是广泛常见用途。
- 推荐预设覆盖范围变化时，要同步更新 README、更新日志、发布说明和中文配套文档，说明推荐清单的来源和范围。
- 推荐项目界面文字和推荐理由必须放在 `LocalizationService`。
- 推荐窗口只显示本机存在，并且尚未出现在已启用配置记录中的路径。
- 如果某条已启用配置记录已经以父路径或子路径覆盖了预设路径，应隐藏该预设，避免重复管理。
- 选择推荐项目后，必须运行与手动选择路径相同的添加流程，不能绕过冲突处理、占用路径处理或安全检查。

添加项目时的占用路径处理：

- 始终先尝试正常移动。第一次移动失败前，不要预先扫描占用进程。
- 如果 Windows 对所选路径返回拒绝访问，打开 `LockingProcessesWindow`。
- 占用窗口在后台任务中扫描，避免主界面卡死。
- 占用检查改造自 `ShowWhatProcessLocksFile`，代码位于 `ThirdParty/ShowWhatProcessLocksFile`。
- 占用窗口列出进程名、进程号、用户、程序路径，以及所选文件或目录下被占用的文件。
- 用户可以刷新扫描、取消、通过右键菜单结束选中进程，或使用“结束全部并继续”。
- “结束全部并继续”会结束列表中的进程，短暂等待，用 `Continue` 结果关闭占用窗口，重新加载配置和表格状态，然后重试原添加项目移动流程。
- 如果重试仍因拒绝访问失败，再次显示占用窗口，而不是静默放弃。
- 检测或结束进程时出现错误，应显示在占用窗口中，不能导致主窗口崩溃。

恢复链接：

1. 读取配置。
2. 用当前机器的用户目录展开 `~`。
3. 从原始路径创建到缓存项目的链接。
4. 当目标内容已经存在时，显示冲突对话框并等待用户选择。
5. 逐个处理选中行；如果未选中任何行，则处理所有启用项目。

检查状态：

- 除保存规范化配置和清理已移除记录外，保持只读。
- 检查缓存项目是否存在。
- 检查目标路径是否存在。
- 检查目标路径是否为链接。
- 检查链接是否指向预期缓存项目。

搬回原位 / 撤销：

- 只处理选中行。
- 仅当原始路径链接指向预期缓存项目时，才删除该链接。
- 把缓存项目移动回原始路径。
- 移动成功后删除配置记录。
- 不得覆盖原始路径上的真实内容。
- 如果原始路径已有真实内容，或指向另一个链接目标，必须停止并给出清晰错误。

命令行模式：

```powershell
.\LinkShelf.exe check
.\LinkShelf.exe check --json
.\LinkShelf.exe check --verbose
.\LinkShelf.exe status
.\LinkShelf.exe cache-root
.\LinkShelf.exe version
.\LinkShelf.exe help
```

命令行检查应保持适合计划任务和自动化使用，不得移动、删除、覆盖、恢复或创建链接。

## 冲突决策

当前决策：

- `UseCacheDeleteTarget`：备份目标内容，然后把目标路径链接到缓存内容。
- `ImportTargetOverwriteCache`：把目标内容复制到缓存，备份目标内容，然后创建链接。
- `ImportTargetThenOverlayCacheBackup`：备份旧缓存内容，把目标内容导入为正式缓存，创建链接，然后叠加旧缓存内容。
- `BackupTargetAndSkip`：把目标内容复制到备份目录，然后跳过此项目。
- `Cancel`：不执行任何操作。

绝不能静默覆盖或删除目标内容。

## 国际化

界面语言由 `LocalizationService` 控制。

规则：

- 默认语言跟随 `CultureInfo.CurrentUICulture`。
- 主窗口必须允许在英文和中文之间切换。
- 界面字符串应通过 `LocalizationService` 获取。
- 内部状态码和配置值保持英文。
- 数据表格行应通过 `SyncItemRow` 显示本地化标签。

## 文档配对

修改 Markdown 文档时，保持英文和中文文档配对。

规则：

- 如果新增或实质修改 Markdown 文件，应保留同用途的 `.zh-CN.md` 配套文件；只有机器生成且不作为仓库文档阅读的文件可以例外。
- 配套文档应保持结构一致：用途、章节顺序、示例、发布链接、截图、警告和致谢都要对应。
- 对于 `README.md` 和 `README.zh-CN.md`，检查目录、主要章节、下载链接、截图、功能描述和致谢是否等价。
- 对于发布说明和更新日志，两种语言都要加入相同版本条目。
- 对于 GitHub issue 和 pull request 模板，实际可行时也提供中文配套。
- 提交 Markdown 修改前，列出所有 `.md` 文件，检查英文和中文配套文档是否仍然对应。

## 重要文件

- `MainWindow.xaml`：主图形界面布局。
- `MainWindow.xaml.cs`：图形界面行为、语言切换、多选操作、占用路径处理后的添加项目重试。
- `ConflictChoiceWindow.xaml`：冲突对话框布局。
- `ConflictChoiceWindow.xaml.cs`：冲突决策映射。
- `LockingProcessesWindow.xaml`：占用路径恢复窗口布局。
- `LockingProcessesWindow.xaml.cs`：占用路径扫描、进程列表、结束进程和继续/取消决策映射。
- `RecommendedItemsWindow.xaml`：推荐项目选择器布局。
- `RecommendedItemsWindow.xaml.cs`：推荐项目选择和添加/取消决策映射。
- `Models/SyncModels.cs`：配置模型、常量、表格行视图模型和枚举。
- `Models/RecommendedSyncItem.cs`：推荐项目模型和表格行模型。
- `Services/AppPaths.cs`：缓存根目录、配置路径、日志路径和备份路径。
- `Services/ConfigStore.cs`：配置加载、规范化和保存。
- `Services/FileOperations.cs`：移动、复制、链接、备份、恢复和冲突应用。
- `Services/RecommendedSyncItems.cs`：内置推荐路径和本机/配置过滤。
- `Services/StatusCheckService.cs`：只读健康检查。
- `Services/PathTools.cs`：路径规范化、`~` 展开和唯一缓存名。
- `Services/LocalizationService.cs`：英文和中文界面字符串。
- `Services/LogService.cs`：操作日志，以及添加项目问题排查用的诊断日志。
- `CommandLineMode.cs`：命令行入口和机器可读状态输出。
- `ThirdParty/ShowWhatProcessLocksFile`：改造后的占用检查和结束进程代码。
- `THIRD-PARTY-NOTICES.md`：复制或改造代码、工作流参考的第三方声明。

## 构建

构建：

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

发布：

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

发布后的可执行文件是 `dist\LinkShelf.exe`。

## GitHub 就绪状态

发布前优先确认：

- `README.md` 包含截图、安装说明、快速开始、命令行示例和限制。
- `README.md` 中有直接的最新下载链接，优先使用 `https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe`。
- `CONTRIBUTING.md` 存在。
- 存在真实许可证文件。
- 发布产物名是干净的 `LinkShelf.exe`。
- GitHub topics 包含 `windows`、`symlink`、`backup`、`syncthing`、`dotnet`、`wpf` 等。
- 首次公开发布后再补充 issue 模板。

## 安全边界

不要允许用户选择缓存根目录本身。

不要允许用户选择缓存根目录内的项目。

不要允许用户选择包含缓存根目录的父目录。

不要静默删除、覆盖或合并目标路径内容。

不要在 Link Shelf 中实现目录内部包含或排除规则。目录内部忽略规则应交给 Syncthing 或其他同步工具。

当 `~` 可以表示用户目录下路径时，不要在配置中保存用户目录绝对路径。
