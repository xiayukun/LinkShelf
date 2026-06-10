# Link Shelf 2.0.0

English: [release-notes-v2.0.0.en.md](release-notes-v2.0.0.en.md)

本版本把项目推进到 2.0，重点是为未来跨平台外壳打基础，同时继续保持当前 Windows 版行为不变。

## 重点

- 新增 `LinkShelf.Core` 类库，用于承载配置读写、路径规范化、状态检查、推荐项筛选和文件/符号链接操作。
- 新增 `LinkShelf.Cli`，作为复用 Core 的跨平台只读命令行入口。
- 新增 `Directory.Build.props` 和 `LinkShelf.slnx`，统一版本元数据和多项目构建入口。
- 新增 `LinkShelf.Core.Tests`，用于 CI 跨平台验证核心行为。
- 推荐项增加平台边界，并新增 macOS 起步预设。
- 新增只读 CLI 命令 `platform`，用于确认当前平台判定。
- 新增只读 CLI 命令 `recommended` / `recommended --json`，用于查看可添加的推荐路径。
- GitHub build workflow 会在 Windows、Linux 和 macOS runner 上编译 CLI，帮助守住跨平台边界。
- Core 路径比较集中到 `PathTools.PathStringComparison` / `PathStringComparer`。
- 新增 [macOS 移植计划](macos-port-plan.md)，说明 GUI、缓存根、权限和验证要求。
- Windows WPF 应用继续作为当前可下载版本，复用 Core 中的共享逻辑。
- README 保持短主页，完整说明继续放在用户指南中。
- 文档明确说明 macOS 版仍需要单独前端、权限引导和真机验证。

## 下载

- `LinkShelf.exe`

## 要求

- Windows 10 或更新版本
- 需要允许创建符号链接；必要时以管理员身份运行，或开启开发者模式

## 校验

发布前应确认：

- `dotnet build .\LinkShelf.slnx -c Release`
- `dotnet run --project .\LinkShelf.Core.Tests\LinkShelf.Core.Tests.csproj -c Release --no-build`
- `dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist`
- `.\dist\LinkShelf.exe version` 输出 `2.0.0`
- `.\dist\LinkShelf.exe check --json` 仍是只读健康检查
