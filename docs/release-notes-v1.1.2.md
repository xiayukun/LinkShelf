Chinese release notes: [docs/release-notes-v1.1.2.zh-CN.md](https://github.com/xiayukun/LinkShelf/blob/v1.1.2/docs/release-notes-v1.1.2.zh-CN.md)

This release completes the locked-path recovery coverage for the move back / undo workflow.

## Highlights

- `Move back / Undo` now opens the locked-path recovery window when Windows reports `Access denied`.
- After closing or terminating locking processes, Link Shelf retries the same move back / undo item.
- If the original link still exists, Link Shelf scans the original path for locking processes.
- If the original link was already removed and the cache item move is blocked, Link Shelf scans the cache item path.
- Add, restore, and move back / undo now share the same locked-path recovery behavior.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.

## Recommended Workflow

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Click `Add item` and choose `Add recommended items`, `Add directory`, or `Add file`.
3. Use `Check status` for read-only health checks.
4. Use `Restore links` on another machine after syncing or restoring the cache root.
5. Use `Move back / Undo` when an item should leave Link Shelf management.
6. If a locked-path window appears, close or terminate the listed processes and continue.

## Safety Notes

- Link Shelf still refuses to overwrite real content at the original path.
- Move back / undo still removes only links that point to the expected cache item.
- Locked-path recovery is only opened after Windows reports `Access denied`; Link Shelf still tries the normal operation first.
