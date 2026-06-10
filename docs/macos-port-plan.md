# macOS 移植计划

English: [macos-port-plan.en.md](macos-port-plan.en.md)

本文是 Link Shelf 2.0 之后的 macOS 移植设计说明。它面向维护者和 AI 编码代理，目标是保证 Windows 功能不退化，同时让 macOS 支持按清晰边界推进。

## 当前状态

- Windows WPF 应用仍是当前正式可下载版本。
- `LinkShelf.Core` 已拆出，面向 `net8.0`，承载配置、路径、状态检查、推荐项筛选、文件/符号链接操作和共用命令行运行器。
- `LinkShelf.Cli` 已新增为跨平台只读命令行入口，可在非 WPF 环境中复用 Core。
- GitHub build workflow 会在 Windows、Linux 和 macOS runner 上编译 CLI，并运行 Core 测试，用来守住共享层的跨平台边界。

## 不直接复用的部分

以下能力仍属于 Windows 外壳，不应直接搬到 Core：

- WPF 窗口和 Windows Forms 选择器。
- Windows 提权、UAC 和开发者模式提示。
- Windows 硬链接投射服务。
- Windows 快捷方式文件 `.lnk` 处理。
- 基于 ShowWhatProcessLocksFile 的占用进程检测。

## macOS 第一阶段

第一阶段目标不是完整 GUI，而是确认核心行为能在 macOS 上安全运行：

1. 在 macOS runner 上持续编译 `LinkShelf.Cli`。
2. 在真实 macOS 机器上运行 CLI 的 `version`、`cache-root`、`platform`、`check --json` 和 `recommended --json`。
3. 用临时目录手工验证文件和目录符号链接的创建、检查和恢复语义。
4. 记录需要 Full Disk Access、终端权限或用户授权的路径场景。

## macOS GUI 设计方向

推荐先评估 Avalonia，因为它可以复用 C#、MVVM 思路和 Core；如果体验不足，再考虑原生 SwiftUI 外壳调用 Core/CLI。

macOS GUI 不能简单照搬 Windows 的“可执行文件所在目录就是缓存根目录”交互。对于 `.app` bundle，`AppContext.BaseDirectory` 通常位于应用包内部，不适合作为用户缓存根目录。可选方向：

- CLI/便携模式继续使用二进制所在目录作为缓存根目录。
- GUI 模式首次启动时显式选择缓存根目录，并把选择保存到应用设置。
- 如果用户把便携 GUI 放进缓存根目录，必须明确显示实际缓存根路径，避免误把 `.app/Contents/MacOS` 当成缓存根。

这会改变 GUI 外壳的入口行为，因此需要单独设计，不要在 Windows 版里仓促改动核心约定。

## 平台差异

- macOS 可以创建文件和目录符号链接，但受系统权限、隐私保护目录和用户授权影响。
- Unix-like 系统一般不能给目录创建硬链接；硬链接投射只能作为文件级能力评估。
- APFS 默认通常不区分大小写，但可以格式化为区分大小写；Core 的路径比较已集中到 `PathTools.PathStringComparison`，后续可按平台继续收紧。
- 推荐路径已经增加平台边界和一组 macOS 起步预设；后续需要在真实 macOS 环境中继续验证目录存在性、隐私风险和展示文案。不要把 Windows `AppData` 预设展示给 macOS 用户。
- 占用进程检测需要新的 macOS 实现，不能复用当前 Windows 第三方代码。

## 需要的开发环境

我可以在当前 Windows 环境继续维护 Core、CLI、文档和 CI。要验证或发布 macOS GUI，需要你后续提供：

- 一台 macOS 机器，或可访问的 macOS CI runner。
- .NET 8 SDK。
- Xcode Command Line Tools。
- 如果走 Avalonia：Avalonia 模板和相关 workload/package restore 能正常访问 NuGet。
- 如果要正式分发：Apple Developer 账号、Developer ID 证书、公证所需凭据。

## 验收门槛

macOS 支持不能只靠 Windows 本机编译宣布完成。至少需要：

- macOS 上 CLI 构建通过。
- macOS 上基本符号链接流程经过真实文件系统验证。
- GUI 缓存根选择不会落到 `.app` 内部的错误路径。
- Windows WPF 构建、发布、硬链接投射和 `check --json` 仍然通过。
- 中英文文档继续成对更新。
