# Changelog

All notable changes to Link Shelf will be documented in this file.

The project follows semantic versioning for public releases.

中文更新日志：[CHANGELOG.zh-CN.md](CHANGELOG.zh-CN.md)

## 1.1.5 - 2026-06-09

中文：[更新日志](CHANGELOG.zh-CN.md#115---2026-06-09) | [发布说明](docs/release-notes-v1.1.5.zh-CN.md)

### Added

- Added `-help` as a CLI help alias and expanded help output for AI assistants and automation.
- The release workflow now checks out the requested tag when manually rerun through `workflow_dispatch`.

### Changed

- Updated README positioning, keywords, GitHub repository profile guidance, and AI assistant prompts for Windows symlink, dotfiles, app-state backup, and AI coding tool configuration use cases.
- Made `README.md` Chinese-first for the default GitHub landing page and added `README.en.md` as the full English companion.
- Kept `README.zh-CN.md` as a short compatibility entry for older links.

## 1.1.4 - 2026-06-09

中文：[更新日志](CHANGELOG.zh-CN.md#114---2026-06-09) | [发布说明](docs/release-notes-v1.1.4.zh-CN.md)

### Added

- Cache root entries that are not covered by enabled config records now appear in the grid as extra cache items.
- Extra cache items can be restored after choosing an original path mapping, or removed from the cache root through Move back / Undo.
- Added safer handling and diagnostics for extra cache-item cleanup and unexpected GUI exceptions.

### Fixed

- Windows shortcut files (`.lnk`) are now rejected with a localized warning instead of being moved into the cache.
- Restore links now handles the case where a config record exists, the cache item is missing, and the original path has real content by only allowing the original content to be imported back into the cache.
- Move back / Undo can remove stale config records when the cache item is missing.
- Extra cache-item confirmation dialogs now use correct formatting arguments and no longer fail before the user can confirm removal.

## 1.1.3 - 2026-06-09

中文：[更新日志](CHANGELOG.zh-CN.md#113---2026-06-09) | [发布说明](docs/release-notes-v1.1.3.zh-CN.md)

### Added

- Add directory and add file now support selecting multiple paths in one dialog.
- Added `Project app`, which creates a hard link to `LinkShelf.exe` in another folder. Launching that linked executable uses the linked folder as a separate cache root without copying the full executable.

### Fixed

- If the locked-path recovery window is canceled during a batch add, Link Shelf stops the remaining batch instead of continuing with later selections.
- If an add operation has already moved content into the cache but then fails before the link/config record is completed, Link Shelf now tries to roll that move back to the original path.

## 1.1.2 - 2026-06-08

中文：[更新日志](CHANGELOG.zh-CN.md#112---2026-06-08) | [发布说明](docs/release-notes-v1.1.2.zh-CN.md)

### Fixed

- Move back / undo now opens the locked-path recovery window when Windows reports `Access denied`.
- After the user handles locking processes, move back / undo retries the same item instead of failing immediately.

### Changed

- Locked-path cancellation log text is now operation-neutral because add, restore, and move back / undo all share the same recovery flow.

## 1.1.1 - 2026-06-07

中文：[更新日志](CHANGELOG.zh-CN.md#111---2026-06-07) | [发布说明](docs/release-notes-v1.1.1.zh-CN.md)

### Fixed

- Grid status now shows a problem state when an enabled item has an unhealthy check result, instead of continuing to display `Enabled`.
- The Chinese UI title now uses the product name `Link Shelf` instead of the older Chinese name.
- Restore links now opens the locked-path recovery window when Windows reports `Access denied`, matching the add-item recovery flow.

### Changed

- Locked-path recovery wording is now operation-neutral so it fits both add and restore operations.
- Changelog entries now link to the matching Chinese release notes file as well as the Chinese changelog entry.

## 1.1.0 - 2026-06-06

中文：[更新日志](CHANGELOG.zh-CN.md#110---2026-06-06) | [发布说明](docs/release-notes-v1.1.0.zh-CN.md)

### Added

- Recommended items flow for detected local configuration, state, and small cache paths.
- Built-in recommended presets curated from the author's daily Windows setup and AI-assisted web research, covering common developer tools, AI coding tools, editors, terminals, and package-manager configuration paths such as Cursor, VS Code, VS Code Insiders, VSCodium, Codex, Claude, Claude Desktop, Gemini, Continue, aider, Windsurf, Cline, Roo Code, GitHub Copilot, Git, npm, Yarn, pnpm, pip, uv, NuGet, Maven, Gradle, Cargo, Bun, PowerShell, Windows Terminal, Neovim, Vim, Starship, Alacritty, WezTerm, Nushell, JetBrains, and Clash Verge.
- Move back / undo action that moves selected cache items back to their original paths, removes original links, deletes the cache entries, and removes config records.
- Documentation for Windows code signing options and the updated product positioning.

### Changed

- Repository positioning now describes Link Shelf as a portable app-state shelf for Windows instead of a generic symbolic-link helper.
- Main button order is now Add item, Check status, Restore links, Move back / Undo.
- Add item no longer opens its menu on hover, avoiding tooltip/menu overlap.
- Recommended item filtering now hides parent/child duplicates when a broader configured path already covers a preset path.

## 1.0.1 - 2026-06-05

中文：[更新日志](CHANGELOG.zh-CN.md#101---2026-06-05) | [发布说明](docs/release-notes-v1.0.1.zh-CN.md)

### Added

- Locked-path recovery window for add operations that fail because Windows reports access denied.
- Process detection adapted from `ShowWhatProcessLocksFile`, including locked file details for selected directories.
- One-click terminate-all-and-continue flow that waits briefly, then retries the original move operation.
- Screenshot and user documentation for the locked-path recovery workflow.
- English and Chinese documentation companions for maintainer docs and release notes.

### Changed

- Link Shelf now tries the normal move first and scans for locking processes only after the move fails, avoiding unnecessary scans for healthy paths.
- The add-item flow reports blocking processes in a dedicated window instead of freezing or showing only the raw Windows error.

## 1.0.0 - 2026-06-04

中文：[更新日志](CHANGELOG.zh-CN.md#100---2026-06-04) | [发布说明](docs/release-notes-v1.0.0.zh-CN.md)

Initial public-ready release.

### Added

- Windows desktop app for moving files and directories into a portable cache root.
- Symbolic link creation at original paths.
- Link restoration for synced or restored cache roots.
- Conflict handling with backup-first choices.
- Command-line health checks with text and JSON output.
- English and Chinese UI with automatic language selection.
- English configuration schema.
- English runtime names for config, logs, and backups.
- Single-file Windows x64 publish workflow.
- GitHub Actions build workflow.
- README, contributing guide, security policy, license, issue templates, and release checklists.
