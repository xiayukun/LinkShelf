# 发布说明模板

English template: [release-notes-template.en.md](release-notes-template.en.md)

把这个模板复制到 GitHub Release，然后按实际版本编辑。

English release notes: create `docs/release-notes-vX.Y.Z.en.md` and link it here after replacing `vX.Y.Z`.

## 亮点

- 把选中的文件或目录移动到可携带缓存根目录。
- 在另一台 Windows 机器上恢复符号链接。
- 从图形界面或命令行检查链接健康状态。

## 下载

- `LinkShelf.exe`

## 要求

- Windows
- 管理员权限，除非 Windows 开发者模式允许当前用户创建符号链接。

## 推荐工作流

1. 把 `LinkShelf.exe` 放到要作为缓存根目录的文件夹中。
2. 双击程序并添加文件或目录。
3. 使用你信任的工具备份、复制或同步整个缓存根目录。
4. 在另一台机器上，把 `LinkShelf.exe` 放到恢复后的缓存根目录，然后点击 `恢复链接`。

## 自动化

本地健康检查推荐使用：

```powershell
.\LinkShelf.exe check --json
```

只有 `problemCount` 大于 `0` 时才提醒用户。

## 安全说明

- Link Shelf 会移动文件并创建符号链接。
- 替换目标内容前，请仔细查看冲突提示。
- 首次运行前请备份重要数据。

## 检查项

- [ ] `dotnet build .\LinkShelf.csproj -c Release`
- [ ] `dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist`
- [ ] `.\dist\LinkShelf.exe check --json`
- [ ] 截图已更新
- [ ] GitHub 发布产物已上传
