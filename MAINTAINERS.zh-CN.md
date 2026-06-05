# 维护者

English: [MAINTAINERS.md](MAINTAINERS.md)

主要维护者：

- xiayukun

## 维护原则

- 保持首次运行流程简单。
- 保持命令行检查安全且只读。
- 对任何会移动、替换、链接或备份用户内容的操作，优先使用明确的用户确认。
- 配置键和运行时文件名保持英文。
- 中文界面文本集中放在 `Services/LocalizationService.cs`。
- Syncthing 是推荐工作流，不是唯一支持的工作流。

## 发布节奏

优先发布小型修复版本，避免一次合并太多高风险变更。一个发布版本应包含：

- 通过的 Windows 构建
- 新的 `LinkShelf.exe` 产物
- 简短发布说明
- 行为变化所需的 README 更新
