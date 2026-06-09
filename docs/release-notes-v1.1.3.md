中文发布说明：[docs/release-notes-v1.1.3.zh-CN.md](https://github.com/xiayukun/LinkShelf/blob/v1.1.3/docs/release-notes-v1.1.3.zh-CN.md)

This release improves batch adding and adds a lightweight way to reuse the same executable from another cache root.

## Highlights

- `Add directory` and `Add file` now support selecting multiple paths at once.
- If a locked-path recovery window is canceled during a batch add, Link Shelf stops the remaining batch immediately.
- If a normal add has already moved content into the cache but fails before the link and config record are completed, Link Shelf tries to roll the moved content back to its original path.
- `Project app` creates a hard link to `LinkShelf.exe` in another folder.
- Launching the projected executable uses the projected folder as its own cache root, so separate cache roots do not need full duplicate copies of the executable.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission, unless Windows Developer Mode allows symbolic link creation for the current user.
- `Project app` uses a Windows hard link, so the target folder must be on the same drive as the current `LinkShelf.exe`.

## Recommended Workflow

1. Put `LinkShelf.exe` inside the folder that should act as the cache root.
2. Click `Add item` and choose `Add recommended items`, `Add directory`, or `Add file`.
3. Select one or more paths when adding directories or files.
4. Use `Project app` when another same-drive folder should have its own Link Shelf entry point without copying the full executable.
5. Use `Check status` for read-only health checks.
6. Use `Restore links` on another machine after syncing or restoring the cache root.
7. Use `Move back / Undo` when an item should leave Link Shelf management.

## Safety Notes

- Batch add stops on cancel or failure instead of continuing unexpectedly.
- Link Shelf still tries the normal move before opening locked-path recovery.
- Link Shelf still refuses to overwrite real content at the original path.
- `Project app` will not overwrite an existing `LinkShelf.exe` in the selected folder.
