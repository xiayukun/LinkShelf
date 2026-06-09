# 隐私

English: [PRIVACY.en.md](PRIVACY.en.md)

Link Shelf 被设计为本地 Windows 工具。

## 读取的数据

Link Shelf 会读取：

- `LinkShelf.exe` 所在的缓存根目录
- `link-shelf.config.json`
- 已配置的源路径和缓存项路径
- 检查链接、冲突和缺失项所需的文件系统元数据

## 写入的数据

Link Shelf 会写入：

- `link-shelf.config.json`
- `.link-shelf-logs`
- `.link-shelf-backups`
- 配置目标路径处的符号链接
- 用户添加项目或处理冲突时被移动的文件或目录

## 网络访问

Link Shelf 正常运行不需要网络访问。

它不会把文件、路径、日志、配置或机器名上传到远程服务。如果缓存根目录放在 Syncthing、云盘或其他同步工具中，网络传输由对应外部工具负责。

## 敏感路径

配置可能包含本地路径、机器名和缓存项名称。公开分享 `link-shelf.config.json`、日志、备份或截图前，请把它们当作可能包含敏感信息处理。

## 自动化

`LinkShelf.exe check --json` 等命令行命令用于本地健康检查。它们会把已配置路径和状态信息打印到本地控制台，方便自动化工具在链接失效时提醒用户。
