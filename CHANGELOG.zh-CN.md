# 更新日志

Link Shelf 的重要变更会记录在这里。

本项目的公开版本遵循语义化版本。

English changelog: [CHANGELOG.md](CHANGELOG.md)

## 1.1.5 - 2026-06-09

English: [Changelog](CHANGELOG.md#115---2026-06-09) | [Release notes](docs/release-notes-v1.1.5.md)

### 新增

- 增加 `-help` 作为命令行帮助别名，并扩展面向 AI 助手和自动化的帮助输出。
- 发布工作流通过 `workflow_dispatch` 手动重跑时，现在会 checkout 用户指定的 tag。

### 变更

- 更新 README 定位、关键词、GitHub 仓库资料建议和 AI 助手提示词，强化 Windows 符号链接、dotfiles、应用状态备份和 AI 编程工具配置同步场景。
- 将默认 GitHub 主页 `README.md` 调整为中文优先，并新增 `README.en.md` 作为完整英文配套文档。
- 将 `README.zh-CN.md` 保留为旧链接兼容入口。

## 1.1.4 - 2026-06-09

English: [Changelog](CHANGELOG.md#114---2026-06-09) | [Release notes](docs/release-notes-v1.1.4.md)

### 新增

- 缓存根目录中没有被已启用配置记录覆盖的子项，现在会在表格中显示为多出缓存项。
- 多出缓存项可以先选择原始路径映射后恢复链接，也可以通过“搬回原位/撤销”从缓存根目录中删除。
- 为多出缓存项清理和图形界面未捕获异常增加了更安全的处理和诊断日志。

### 修复

- Windows 快捷方式文件（`.lnk`）现在会被本地化警告拒绝，不再移动到缓存中。
- 当配置记录存在、缓存项缺失且原始路径有真实内容时，恢复链接现在只允许把原始内容导入回缓存。
- 当缓存项缺失时，“搬回原位/撤销”可以只移除残留配置记录。
- 多出缓存项确认对话框现在使用正确的格式化参数，不会在用户确认删除前报错。

## 1.1.3 - 2026-06-09

English: [Changelog](CHANGELOG.md#113---2026-06-09) | [Release notes](docs/release-notes-v1.1.3.md)

### 新增

- 添加目录和添加文件现在支持在同一个选择窗口里一次选择多个路径。
- 增加 `投射程序`：可以在另一个目录中创建 `LinkShelf.exe` 的硬链接。从该链接程序启动时，会把链接所在目录作为独立缓存根目录，而不需要复制完整可执行文件。

### 修复

- 批量添加过程中，如果用户在文件占用处理窗口点击取消，Link Shelf 会停止后续批量项目，不再继续处理后面的选择。
- 如果添加操作已经把内容移入缓存，但在创建链接或保存配置前失败，Link Shelf 现在会尽量把这次移动回滚到原始路径。

## 1.1.2 - 2026-06-08

English: [Changelog](CHANGELOG.md#112---2026-06-08) | [Release notes](docs/release-notes-v1.1.2.md)

### 修复

- 搬回原位/撤销时如果 Windows 返回拒绝访问，现在会打开文件占用处理窗口。
- 用户处理占用进程后，搬回原位/撤销会重试同一个项目，不再直接失败。

### 调整

- 被占用路径取消日志改为通用操作文案，因为添加、恢复链接和搬回原位/撤销现在共用同一套恢复流程。

## 1.1.1 - 2026-06-07

English: [Changelog](CHANGELOG.md#111---2026-06-07) | [Release notes](docs/release-notes-v1.1.1.md)

### 修复

- 当已启用项目的检查结果异常时，表格状态列现在会显示异常状态，不再继续显示“启用”。
- 中文界面标题现在使用产品名 `Link Shelf`，不再使用旧中文名称。
- 恢复链接时如果 Windows 返回拒绝访问，现在也会打开文件占用处理窗口，与添加项目时的处理流程一致。

### 调整

- 文件占用处理窗口的标题文案改为通用操作失败文案，适配添加和恢复两种操作。
- 更新日志条目现在会同时链接到对应的中文更新日志条目和中文发布说明文件。

## 1.1.0 - 2026-06-06

English: [Changelog](CHANGELOG.md#110---2026-06-06) | [Release notes](docs/release-notes-v1.1.0.md)

### 新增

- 增加推荐项目流程，可添加本机检测到的常见配置、状态和小型缓存路径。
- 内置推荐预设来自作者本人日常 Windows 环境，以及 AI 联网调研后认为很多人会使用的目录，覆盖常见开发工具、AI 编程工具、编辑器、终端和包管理器配置路径，例如 Cursor、VS Code、VS Code Insiders、VSCodium、Codex、Claude、Claude Desktop、Gemini、Continue、aider、Windsurf、Cline、Roo Code、GitHub Copilot、Git、npm、Yarn、pnpm、pip、uv、NuGet、Maven、Gradle、Cargo、Bun、PowerShell、Windows Terminal、Neovim、Vim、Starship、Alacritty、WezTerm、Nushell、JetBrains、Clash Verge。
- 增加“搬回原位/撤销”操作，可把选中的缓存项搬回原始路径，移除原路径链接，删除缓存项，并删除配置记录。
- 补充 Windows 签名发布说明和新的产品定位文档。

### 调整

- 项目定位调整为“Windows 上的便携应用状态收纳工具”，不再强调成普通符号链接工具。
- 主按钮顺序调整为：添加项目、检查状态、恢复链接、搬回原位/撤销。
- 添加项目不再鼠标悬停自动展开菜单，避免提示或菜单遮挡下拉区域。
- 推荐项目过滤现在会隐藏父子路径重复项；如果配置中已有更大的父目录，就不会再显示它下面的子目录预设。

## 1.0.1 - 2026-06-05

English: [Changelog](CHANGELOG.md#101---2026-06-05) | [Release notes](docs/release-notes-v1.0.1.md)

### 新增

- 添加项目因为 Windows 返回拒绝访问而移动失败时，会打开文件占用处理窗口。
- 借鉴并改写 `ShowWhatProcessLocksFile` 的检测实现，可列出占用目录中具体文件的进程。
- 增加“结束全部并继续”流程：结束占用进程后短暂等待，再重新执行原来的移动操作。
- 为文件占用处理流程补充截图和使用说明。
- 为维护文档和发布说明补充中文配套文档。

### 调整

- Link Shelf 现在会先尝试正常移动，只有移动失败后才扫描占用进程，避免对正常路径做多余扫描。
- 添加项目流程会在专门窗口里显示占用进程，不再只显示原始 Windows 错误或让界面卡住。

## 1.0.0 - 2026-06-04

English: [Changelog](CHANGELOG.md#100---2026-06-04) | [Release notes](docs/release-notes-v1.0.0.md)

首个公开就绪版本。

### 新增

- Windows 桌面程序，可把文件和目录移动到可携带的缓存根目录。
- 在原始位置创建符号链接。
- 支持为已同步或已恢复的缓存根目录恢复链接。
- 提供带备份优先选项的冲突处理。
- 提供文本和 JSON 输出的命令行健康检查。
- 支持英文和中文界面，并可按系统语言自动选择。
- 使用英文配置架构。
- 配置、日志、备份和运行时目录使用英文名称。
- 支持 Windows x64 单文件发布工作流。
- 提供 GitHub Actions 构建工作流。
