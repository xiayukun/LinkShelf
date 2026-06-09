Chinese release notes: [docs/release-notes-v1.0.0.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.0.0.md)

Initial public release.

## Highlights

- Windows GUI for moving selected files and directories into a portable cache root.
- Symbolic link restoration on another Windows machine.
- CLI health checks with plain text and JSON output.
- English and Chinese UI.
- Portable config paths with `~` for user-profile compatibility across machines.
- Backup/sync-tool-friendly workflow without requiring a specific tool.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.

## Quick Start

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Double-click it and add files or directories.
3. Back up, copy, or sync the whole cache root with a tool you trust.
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
