English: [Release notes](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v2.0.1.en.md)

Link Shelf 2.0.1 是构建流程维护与版本显示修复版本。

## 变更

- 构建和发布使用 checkout v7、setup-dotnet v6，继续使用 .NET 8 和 Windows x64 自包含单文件发布。
- CLI `version` 改为读取程序集版本，修复仍显示 1.1.6 的问题。
- 文件迁移、符号链接、硬链接与配置格式保持不变。

## 验证

- Release 构建、单文件发布和隔离配置 `check --json` 检查。
- 核对 CLI 版本与程序文件版本一致。

## 要求

- Windows x64；创建符号链接通常需要管理员权限，或启用允许当前用户创建链接的 Windows 开发者模式。
