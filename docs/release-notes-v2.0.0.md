English release notes: [docs/release-notes-v2.0.0.en.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v2.0.0.en.md)

这个版本全面升级了 Link Shelf 的图形界面：所有管理窗口迁移到 WPF-UI `FluentWindow` 现代界面，支持深色主题和系统主题色选中效果。这是一个专注于交付质量和视觉一致性的里程碑版本。

## 亮点

- **FluentWindow 界面升级**：冲突选择、文件占用处理、推荐项目、主窗口全部迁移为 WPF-UI `FluentWindow`，启用 `ExtendsContentIntoTitleBar` 现代标题栏。
- **深色主题统一**：集成 WPF-UI Dark 主题，窗口、文本框、标签统一深色前景色。
- **LinkShelfMessageBox 自定义消息框**：所有 `System.Windows.MessageBox` 替换为自定义深色主题消息框，界面风格更统一。
- **WPF-UI 包依赖**：新增 `WPF-UI` v4.3.0，为后续界面迭代提供现代 UI 基础。
- **主页主截图更新**：GitHub README 主图更换为功能汇总 Hero 图，更直观展示产品核心能力。

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 创建符号链接通常需要管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 说明

- Link Shelf 仍然是本地工具。它不会上传文件、路径、日志、配置或机器名。
- 本次升级仅涉及界面层重构，所有 CLI 命令、服务、配置结构和项目模型保持不变。
