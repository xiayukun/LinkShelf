# Release Notes Template

Copy this template into a GitHub release and edit it for the version being published.

## Link Shelf vX.Y.Z

### Highlights

- Move selected files or directories into a portable cache root.
- Restore symbolic links on another Windows machine.
- Check link health from the GUI or CLI.

### Download

- `LinkShelf.exe`

### Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.

### Recommended Workflow

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Double-click it and add files or directories.
3. Sync or back up the whole cache root with Syncthing or another tool.
4. On another machine, put `LinkShelf.exe` in the restored cache root and click `Restore links`.

### Automation

Use this command for local health checks:

```powershell
.\LinkShelf.exe check --json
```

Notify the user only when `problemCount` is greater than `0`.

### Safety Notes

- Link Shelf moves files and creates symbolic links.
- Review conflict prompts before replacing target content.
- Back up important data before the first run.

### Checks

- [ ] `dotnet build .\LinkShelf.csproj -c Release`
- [ ] `dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist`
- [ ] `.\dist\LinkShelf.exe check --json`
- [ ] Screenshots updated
- [ ] GitHub release asset uploaded
