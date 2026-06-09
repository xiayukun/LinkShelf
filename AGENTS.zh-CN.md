# Link Shelf 代理说明

English: [AGENTS.md](AGENTS.md)

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

1. 用户选择一个或多个文件或目录。
2. 程序拒绝缓存根目录本身、缓存根目录内的路径，以及包含缓存根目录的父目录。
3. 程序把所选路径移动到缓存根目录。
4. 程序在原始路径创建符号链接。
5. 程序保存配置记录。
6. 批量添加时，如果某个选中项被取消或失败，必须立刻停止，不得继续处理后面的选择。
7. 如果普通添加已经把源路径移入缓存，但在创建链接或保存配置记录前失败，应尽量把已移动内容回滚到原始路径。
8. 必须拒绝 Windows 快捷方式文件（`.lnk`），并显示本地化警告。它们被移动到缓存并链接回原位置后，行为不可靠。

投射程序：

- `投射程序` 按钮会在用户选择的目录中创建名为 `LinkShelf.exe` 的硬链接。
- 从该硬链接启动 Link Shelf 时，`AppContext.BaseDirectory` 会变成硬链接所在目录，因此该目录会成为独立缓存根目录。
- 投射功能使用硬链接，不要用快捷方式，也不要复制可执行文件。
- 重新生成 `dist\LinkShelf.exe` 后，应查询已有投射硬链接，并在需要时重建它们；发布/构建工具可能会替换文件对象，导致旧硬链接关系断开。
- Windows 硬链接不能跨卷，所以投射只能在同一盘符内进行。
- 如果目标目录中已经有 `LinkShelf.exe` 或其他同名文件系统项，不得覆盖。
- 投射功能界面文字必须放在 `LocalizationService`。

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
- 如果用户在添加流程中取消占用窗口，应停止本次添加操作以及后续批量选择。
- 检测或结束进程时出现错误，应显示在占用窗口中，不能导致主窗口崩溃。

恢复链接：

1. 读取配置。
2. 用当前机器的用户目录展开 `~`。
3. 从原始路径创建到缓存项目的链接。
4. 当目标内容已经存在时，显示冲突对话框并等待用户选择。
5. 逐个处理选中行；如果未选中任何行，则处理所有启用项目。
6. 如果恢复时因为拒绝访问失败，应针对目标路径打开 `LockingProcessesWindow`，让用户结束占用进程后重试同一个恢复项目。
7. 如果缓存项不存在，但原始路径有真实内容，应显示冲突对话框，并且只启用 `ImportTargetOverwriteCache`；使用缓存、合并和跳过选项必须置灰，因为没有缓存内容可用。
8. 如果选中行是未记录缓存项，应让用户选择原始路径，立刻创建并保存配置记录，然后对该项目执行正常恢复/冲突流程。

状态显示：

- 配置中的状态值保持英文，不要为了界面展示异常而改写配置状态。
- 已启用项目如果检查结果异常，表格状态列应显示本地化的 `problem` 标签，而不是继续显示 `enabled`。

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
- 如果搬回原位 / 撤销因为拒绝访问失败，应打开 `LockingProcessesWindow`，让用户结束占用进程后重试同一个项目。
- 搬回原位 / 撤销选择占用扫描路径时，原始路径仍存在就扫描原始路径；如果原路径链接已经被移除，则扫描缓存项路径。
- 如果缓存项不存在，应询问用户是否只删除配置记录；用户确认后，只删除 JSON 记录，不移动任何文件或目录。
- 如果选中行是未记录缓存项，应提示没有已知目标位置；用户确认后，从缓存根目录中删除该缓存项。

未记录缓存项：

- 缓存根目录中没有被已启用配置记录覆盖的子项，应在表格中显示为 `untracked-cache-item`。
- 忽略 Link Shelf 运行时项目：`LinkShelf.exe`、`link-shelf.config.json`、`.link-shelf-logs`、`.link-shelf-backups`。
- 未记录行只是临时视图模型，不得写入 JSON；只有当用户点击 `恢复链接` 并提供原始路径后，才写入配置。

命令行模式：

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
- 配套文档应在靠前位置互相链接，方便英文读者进入中文文档，也方便中文读者返回英文文档。
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
- `Services/ProjectionService.cs`：把当前可执行程序以硬链接方式投射到另一个缓存根目录。
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

修改代码后不要只停在 `dotnet build`。必须重新发布 `dist\LinkShelf.exe`，查询投射硬链接，并把需要指向新 `dist` 产物的 `LinkShelf.exe` 硬链接重建好。

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
