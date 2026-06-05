# Link Shelf 1.0.1

This release improves the add-item flow when the selected path cannot be moved because Windows reports `Access denied`, usually because a running program is using the selected directory or one of its files.

## Highlights

- Link Shelf now tries the normal move first and scans for locking processes only after Windows blocks the move.
- The GUI shows a locked-path recovery window with process names, process IDs, users, executable paths, and locked files.
- Users can terminate selected processes from the context menu or choose `Terminate all and continue`.
- After terminating processes, Link Shelf waits briefly and retries the original move operation. If the path is still blocked, it opens the recovery window again.
- The implementation adapts lock inspection code from `ShowWhatProcessLocksFile` and documents the related acknowledgements.
- CLI health checks remain read-only and unchanged.

![Locked path recovery window](https://raw.githubusercontent.com/xiayukun/LinkShelf/v1.0.1/Assets/lock-resolution-window-cn.png)

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.

## Recommended Workflow

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Double-click it and add files or directories.
3. If the add operation reports a blocked path, review the listed processes.
4. Close them manually, terminate selected processes, or choose `Terminate all and continue`.
5. Sync or back up the whole cache root with Syncthing or another tool.
6. On another machine, put `LinkShelf.exe` in the restored cache root and click `Restore links`.

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
