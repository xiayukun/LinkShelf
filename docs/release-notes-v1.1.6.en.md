Chinese release notes: [docs/release-notes-v1.1.6.md](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v1.1.6.md)

This release backports the safest automation-friendly part of the 2.0 exploration to the current Windows build: read-only recommended-path commands for scripts and AI assistants, while keeping the homepage short and direct.

## Highlights

- Added `LinkShelf.exe recommended` to list currently available recommended paths in text form.
- Added `LinkShelf.exe recommended --json` for machine-readable output.
- Recommended-path commands are read-only; they do not move, delete, restore, or create links.
- `README.md` remains a short landing page, with full documentation in `docs/user-guide.md`.
- The homepage no longer frames one specific sync tool as a selling point; it uses general backup/sync risk guidance instead.
- The 2.0 cross-platform core split and macOS work remain on a separate branch, so this release does not pull in that structural change.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission is usually required for creating symbolic links unless Windows Developer Mode allows the current user to create them.

## Notes

- Link Shelf remains a local tool. It does not upload files, paths, logs, configuration, or machine names.
- `recommended --json` only lists recommended paths that exist locally and are not covered by enabled config records; moving content still requires explicit confirmation in the GUI.
