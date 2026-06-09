中文发布说明：[docs/release-notes-v1.1.5.zh-CN.md](https://github.com/xiayukun/LinkShelf/blob/v1.1.5/docs/release-notes-v1.1.5.zh-CN.md)

This release improves the public GitHub presentation for Chinese-first promotion, adds a full English companion README, and makes the CLI help more useful for AI assistants and automation.

## Highlights

- `README.md` is now the Chinese-first GitHub landing page.
- `README.en.md` contains the full English documentation.
- `README.zh-CN.md` remains as a short compatibility entry for older links.
- The README now foregrounds Windows symbolic links, symlink backup, hard-link projection, dotfiles, app-state backup, Syncthing, and AI coding tool configuration sync.
- The AI and automation section now includes a copy-ready prompt for a user's AI assistant.
- `LinkShelf.exe -help` now works as a CLI help alias.
- CLI help now explains the cache-root model, `check --json`, and read-only automation behavior.
- The release workflow now checks out the requested tag when manually rerun through `workflow_dispatch`.

## Download

- `LinkShelf.exe`

## Requirements

- Windows
- Administrator permission is usually required for creating symbolic links unless Windows Developer Mode allows the current user to create them.

## Notes

- Link Shelf remains a local tool. It does not upload files, paths, logs, configuration, or machine names.
- Review AI tool settings and other recommended paths before syncing them, because they can contain tokens, account names, local history, or other private state.
