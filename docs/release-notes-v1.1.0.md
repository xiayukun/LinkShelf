# Link Shelf 1.1.0

This release turns Link Shelf more clearly into a portable app-state shelf for Windows. It adds recommended items for common local configuration paths and replaces record-only removal with a real move back / undo flow.

## Highlights

- Add recommended items from detected local paths. Paths already in the config and paths missing on the current machine are hidden.
- Recommended presets are curated from the author's daily Windows setup and AI-assisted web research into common developer, AI coding, editor, terminal, and package-manager configuration paths.
- Presets include paths for Cursor, VS Code, VS Code Insiders, VSCodium, Codex, Claude, Claude Desktop, Gemini, Continue, aider, Windsurf, Cline, Roo Code, GitHub Copilot, Git, npm, Yarn, pnpm, pip, uv, NuGet, Maven, Gradle, Cargo, Bun, PowerShell, Windows Terminal, Neovim, Vim, Starship, Alacritty, WezTerm, Nushell, JetBrains, and Clash Verge.
- Move selected items back to their original paths and remove their config records.
- The undo flow refuses to overwrite real content at the original path.
- The main button order is now `Add item`, `Check status`, `Restore links`, `Move back / Undo`.
- The add menu opens on click instead of hover, avoiding tooltip/menu overlap.

![Recommended items window](https://raw.githubusercontent.com/xiayukun/LinkShelf/v1.1.0/Assets/recommended-items-window-cn.png)

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
- Some recommended paths can contain private state such as account names, tokens, local history, or provider settings. Review them before syncing the cache root with another tool.
