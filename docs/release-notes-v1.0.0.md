# Link Shelf 1.0.0

Chinese release notes: [docs/release-notes-v1.0.0.zh-CN.md](https://github.com/xiayukun/LinkShelf/blob/v1.0.0/docs/release-notes-v1.0.0.zh-CN.md)

Initial public release.

## Highlights

- Windows GUI for moving selected files and directories into a portable cache root.
- Symbolic link restoration on another Windows machine.
- CLI health checks with plain text and JSON output.
- English and Chinese UI.
- Portable config paths with `~` for user-profile compatibility across machines.
- Syncthing-friendly workflow without requiring Syncthing.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.

## Quick Start

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Double-click it and add files or directories.
3. Sync or back up the whole cache root with Syncthing or another tool.
4. On another machine, put `LinkShelf.exe` in the restored cache root and click `Restore links`.

## Automation

Use this command for local health checks:

```powershell
.\LinkShelf.exe check --json
```

Notify the user only when `problemCount` is greater than `0`.

## Safety Notes

- Link Shelf moves files and creates symbolic links.
- Review conflict prompts before replacing target content.
- Back up important data before the first run.
