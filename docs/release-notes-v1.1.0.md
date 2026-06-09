English release notes: [docs/release-notes-v1.1.0.en.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.1.0.en.md)

这个版本让 Link Shelf 的定位更清楚：它是 Windows 上的便携应用状态收纳工具。本版本增加了常见本地配置路径的推荐项目，并把原来的“只移除记录”改为真正的“搬回原位/撤销”流程。

## 亮点

- 可以从本机检测到的路径中添加推荐项目。已经在配置中的路径、本机不存在的路径会被隐藏。
- 推荐预设来自作者本人日常 Windows 环境，以及 AI 联网调研后认为很多人会使用的开发工具、AI 编程工具、编辑器、终端和包管理器配置路径。
- 推荐预设包括 Cursor、VS Code、VS Code Insiders、VSCodium、Codex、Claude、Claude Desktop、Gemini、Continue、aider、Windsurf、Cline、Roo Code、GitHub Copilot、Git、npm、Yarn、pnpm、pip、uv、NuGet、Maven、Gradle、Cargo、Bun、PowerShell、Windows Terminal、Neovim、Vim、Starship、Alacritty、WezTerm、Nushell、JetBrains、Clash Verge。
- 可以把选中的项目搬回原始路径，并删除配置记录。
- 撤销流程不会覆盖原始路径上的真实内容。
- 主按钮顺序调整为：`添加项目`、`检查状态`、`恢复链接`、`搬回原位/撤销`。
- 添加菜单改为点击展开，不再鼠标悬停自动展开，避免提示或菜单遮挡下拉区域。

![推荐项目窗口](https://raw.githubusercontent.com/xiayukun/LinkShelf/v1.1.0/Assets/recommended-items-window-cn.png)

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 推荐工作流

1. 把 `LinkShelf.exe` 放到要作为缓存根目录的文件夹中。
2. 点击 `添加项目`，选择 `添加推荐项目`、`添加目录` 或 `添加文件`。
3. 使用 `检查状态` 做只读健康检查。
4. 缓存根目录同步或恢复到另一台机器后，使用 `恢复链接`。
5. 某个项目不再需要 Link Shelf 管理时，使用 `搬回原位/撤销`。

## 安全说明

- 搬回原位/撤销只会在原始路径不存在，或仍然是指向该缓存项的正确链接时执行。
- 如果原始路径已经有真实内容，Link Shelf 会停止而不是覆盖它。
- 推荐项目只是同一个添加流程的快捷入口，不会绕过冲突处理或文件占用处理。
- 部分推荐路径可能包含账号名、令牌、本地历史或服务商设置等私有状态。把缓存根目录交给其他工具同步前，请先检查这些内容。
