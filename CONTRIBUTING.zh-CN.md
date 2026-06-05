# 参与贡献

感谢你关注 Link Shelf。

## 开发环境

要求：

- Windows
- .NET SDK

构建：

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

从缓存根目录运行命令行检查：

```powershell
.\LinkShelf.exe check --json
```

## 拉取请求要求

- 文件操作要保持保守。
- 不要静默删除或覆盖用户数据。
- 除非有新的版本化迁移说明，否则保持当前英文配置架构稳定。
- 配置键、可执行文件名和运行时目录名保持英文。
- 界面字符串放在本地化服务中。
- 行为变化时更新 `README.md` 和 `README.zh-CN.md`。
- 使用 `dotnet build .\LinkShelf.csproj -c Release` 验证。

## 安全规则

任何会移动、删除、覆盖、备份或链接用户文件的变更，都必须在图形界面路径中给出明确的用户确认。命令行命令应保持只读，除非未来命令名称明确表示会修改状态。
