Chinese release notes: [docs/release-notes-v2.0.0.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v2.0.0.md)

This release upgrades the Link Shelf GUI across the board: every manager window migrated to WPF-UI `FluentWindow` with dark theme and system accent colour support. It is a milestone focused on delivery quality and visual consistency.

## Highlights

- **FluentWindow UI upgrade**: Conflict choice, Locking processes, Recommended items, and Main window all converted to WPF-UI `FluentWindow` with `ExtendsContentIntoTitleBar` modern title bar.
- **Unified dark theme**: WPF-UI Dark theme integrated, with consistent dark foreground colours across all windows, text blocks, and labels.
- **Custom LinkShelfMessageBox**: All `System.Windows.MessageBox` calls replaced with a custom dark-themed message box for a cohesive look.
- **WPF-UI package dependency**: Added `WPF-UI` v4.3.0 as the UI foundation for future iterations.
- **Updated main screenshot**: The GitHub README hero image is now a feature-summary screenshot that better communicates the product's core capabilities.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission is usually required for creating symbolic links unless Windows Developer Mode allows the current user to create them.

## Notes

- Link Shelf remains a local tool. It does not upload files, paths, logs, configuration, or machine names.
- This upgrade is a UI-layer refactor only. All CLI commands, services, config structure, and data models remain unchanged.
