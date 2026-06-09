# 发布清单

English: [release-checklist.en.md](release-checklist.en.md)

## 构建

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

预期产物：

```text
dist\LinkShelf.exe
```

## 本地验证

从缓存根目录运行：

```powershell
.\LinkShelf.exe check --json
```

预期结果：

```json
{
  "ok": true,
  "problemCount": 0
}
```

## 手动图形界面验证

- 双击 `LinkShelf.exe`。
- 确认需要时会出现 Windows 提权提示。
- 确认主窗口能打开。
- 确认语言选择器可以在英文和中文之间切换。
- 确认切换语言后表格标题和按钮会更新。
- 确认关闭图形界面后 `check --json` 仍可工作。

## GitHub 发布

- 标签：`v1.0.0`
- 标题：`Link Shelf 1.0.0`
- 产物：`LinkShelf.exe`
- 优先使用 `release` GitHub Actions 工作流。
- 首个发布版本：打开 Actions，选择 `release`，点击运行工作流，分支保持 `main`，标签设为 `v1.0.0`。
- 后续发布版本：推送新的 `v*` 标签，让工作流自动运行。
- 使用 `docs/release-notes-v1.0.0.md` 和 `docs/release-notes-v1.0.0.en.md` 准备发布说明。
- 发布新版本前更新 `CHANGELOG.md` 和 `CHANGELOG.en.md`。
- 确认两种语言的更新日志中每个版本都能跳转到对应语言的更新日志条目和发布说明文件。
