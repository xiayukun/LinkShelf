Chinese release notes: [docs/release-notes-v1.1.1.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.1.1.md)

This release focuses on polishing recovery behavior and status clarity after the 1.1.0 app-state shelf update.

## Highlights

- The grid status column now shows a problem state when an enabled item has an unhealthy check result, so broken or conflicting items are easier to notice.
- The Chinese UI title now uses the product name `Link Shelf`.
- `Restore links` now reuses the locked-path recovery window when Windows reports `Access denied`.
- After resolving locked processes, restore retries the same item. If the path is still blocked, the recovery window can appear again.
- Locked-path recovery text is now operation-neutral for both add and restore flows.
- Changelog entries now link directly to the matching Chinese changelog entry and Chinese release notes file.

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
5. If restore reports a locked path, close or terminate the listed processes and continue.
6. Use `Move back / Undo` when an item should leave Link Shelf management.

## Safety Notes

- Locked-path recovery is only opened after Windows reports `Access denied`; Link Shelf still tries the normal operation first.
- Restore conflicts are still handled one item at a time.
- Link Shelf does not silently overwrite real content at the original path.
