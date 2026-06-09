中文发布说明：[docs/release-notes-v1.1.4.zh-CN.md](https://github.com/xiayukun/LinkShelf/blob/v1.1.4/docs/release-notes-v1.1.4.zh-CN.md)

This release tightens recovery behavior for edge cases around extra cache items, missing cache content, and Windows shortcut files.

## Highlights

- Extra cache items now appear in the grid when the cache root contains files or folders that are not covered by enabled config records.
- `Restore links` can complete an extra cache item after you choose its original path mapping.
- `Move back / Undo` can delete an extra cache item from the cache root after a clear confirmation.
- Windows shortcut files (`.lnk`) are rejected with a localized warning because they do not work reliably after being moved and linked back.
- When a config record exists but its cache item is missing, restore and undo flows now present safer choices instead of silently failing or offering impossible actions.
- Extra cache-item dialogs and cleanup paths include additional diagnostics for troubleshooting.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.
- `Project app` uses a Windows hard link, so the target folder must be on the same drive as the current `LinkShelf.exe`.

## Recommended Workflow

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Click `Add item` and choose `Add recommended items`, `Add directory`, or `Add file`.
3. Use `Check status` for read-only health checks.
4. Use `Restore links` after syncing or restoring the cache root on another machine.
5. Use `Move back / Undo` when an item should leave Link Shelf management.

## Safety Notes

- Extra cache items are temporary view rows until you explicitly choose an original path for restore.
- Deleting an extra cache item removes that cache-root entry; Link Shelf does not know an external destination for it.
- Link Shelf still refuses to overwrite real content at the original path without a conflict decision.
