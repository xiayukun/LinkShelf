# Link Shelf 1.1.0

This release turns Link Shelf more clearly into a portable app-state shelf for Windows. It adds recommended items for common local configuration paths and replaces record-only removal with a real move back / undo flow.

## Highlights

- Add recommended items from detected local paths. Paths already in the config and paths missing on the current machine are hidden.
- Recommended presets include Cursor, VS Code, Git, npm, Codex, Claude, JetBrains, Clash, Ditto, Wiz, LX Music, PowerShell, Windows Terminal, and related configuration files.
- Move selected items back to their original paths and remove their config records.
- The undo flow refuses to overwrite real content at the original path.
- The main button order is now `Add item`, `Check status`, `Restore links`, `Move back / Undo`.
- The add menu opens on click instead of hover, avoiding tooltip/menu overlap.

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

## Safety Notes

- Move back / undo only proceeds when the original path is missing or is still the expected link to the cache item.
- If real content exists at the original path, Link Shelf stops instead of overwriting it.
- Recommended items are only shortcuts into the same add-item workflow; they do not bypass conflict or locked-path handling.
