# Changelog

All notable changes to Link Shelf will be documented in this file.

The project follows semantic versioning for public releases.

## 1.1.0 - 2026-06-06

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
