# Session Handoff

Chinese: [session-handoff.md](session-handoff.md)

This document preserves high-value project context for future Codex sessions. A new session should read `AGENTS.md` first, then this file.

## Current Project

- Project name: `Link Shelf`
- Remote repository: `git@github.com:xiayukun/LinkShelf.git`
- GitHub page: `https://github.com/xiayukun/LinkShelf`
- Latest GitHub Release: `v1.1.5`
- Current source and local runtime version: `1.1.5`
- Release page: `https://github.com/xiayukun/LinkShelf/releases/tag/v1.1.5`
- Download URL: `https://github.com/xiayukun/LinkShelf/releases/download/v1.1.5/LinkShelf.exe`
- Main branch: `main`
- Current Git HEAD: `Prepare Link Shelf 1.1.5` unless newer documentation-only commits have been added.
- Current worktree note: `v1.1.5` is already released; documentation is being converted to Chinese-default `.md` files with `.en.md` English companions.

## Local Layout

- Cache root: see the Chinese handoff file for the exact localized path under `C:\Users\11467\AppData\Local`.
- Source repository: see the Chinese handoff file for the exact localized path under `C:\git`.
- Runtime executable: `LinkShelf.exe` inside the cache root.
- Config file: `link-shelf.config.json` inside the cache root.
- Runtime logs: `.link-shelf-logs` inside the cache root.
- Conflict backups: `.link-shelf-backups` inside the cache root.

If the source repository is migrated, copy the whole `LinkShelf` folder including `.git` and any uncommitted changes. Do not rely on a fresh GitHub clone while local fixes are still unpublished.

If the runtime cache root is migrated, remember that the directory containing `LinkShelf.exe` is the cache root. Move the executable together with config, logs, backups, and cache items.

## Product Purpose

Link Shelf moves selected local files or directories into the executable's cache root, then creates symbolic links at the original paths so applications can keep using those paths.

Typical flow:

1. Put `LinkShelf.exe` in the cache root.
2. Select files or directories to sync or back up.
3. Link Shelf moves them into the cache root.
4. Link Shelf creates symbolic links at the original paths.
5. A trusted external tool backs up, copies, or syncs the cache root.
6. On another computer, run Link Shelf from that cache root and restore links.

Backup and sync tools are optional companions; Link Shelf itself only handles local relocation and links.

## Key Decisions

- The executable directory is the cache root.
- The same `LinkShelf.exe` supports GUI and CLI mode.
- Config keys, runtime directory names, executable names, and operation codes stay English.
- The GUI supports Chinese and English.
- The language dropdown must always show `English` and the Chinese-language option.
- Current config version is `2`.
- Old Chinese-key config files are no longer supported.
- Paths under the user profile should be stored with `~`.
- The smallest managed unit is a whole file or a whole directory. Directory-internal ignore rules belong in an external backup or sync tool.
- Add file and add directory support multi-select. A canceled or failed item must stop the remaining batch.
- `Project app` creates a same-drive hard link named `LinkShelf.exe` in another folder. Launching that hard link makes the hard-link folder an independent cache root.
- Windows shortcut files (`.lnk`) are rejected.
- Missing-cache restore and undo paths must stay explicit and safe.
- Extra cache-root children not tracked by JSON appear as untracked cache items. Restore asks for an original path first; undo can delete the extra cache item after confirmation.

## Published State

GitHub automation exists:

- Build workflow: `.github/workflows/build.yml`
- Release workflow: `.github/workflows/release.yml`
- First release notes: `docs/release-notes-v1.0.0.md`
- Latest release notes: `docs/release-notes-v1.1.5.md`

The `release` workflow creates or updates a GitHub Release and uploads `LinkShelf.exe`.

The first public release was run manually:

- Branch: `main`
- Tag: `v1.0.0`
- Result: successful

## v1.1.4 Notes

This release focused on edge cases:

- Reject Windows shortcut files (`.lnk`).
- If a JSON record exists but the cache item is missing:
  - `Restore links` only enables importing the original target content into the cache when real target content exists.
  - `Move back / Undo` can remove only the JSON record after confirmation.
- If the cache root contains entries not tracked by JSON:
  - The grid shows them as extra cache items.
  - `Restore links` asks for an original path and saves a config record first.
  - `Move back / Undo` confirms deletion of that cache item.
- The `v1.1.4` release body removed the top-level version heading to avoid duplicate headings on GitHub Release pages.

## v1.1.5 Promotion Work

The project moved toward Chinese-first public presentation with English companions:

- `README.md` is the Chinese-first GitHub landing page.
- `README.en.md` is the full English companion.
- Old Chinese-named compatibility files should not be kept.
- Reader-facing Markdown uses Chinese default `.md` files and `.en.md` English companions.
- Release-note language links should point at `main` when they reference files renamed after older tags.
- GitHub description: `Windows 配置迁移和符号链接工具 / Windows config mover: collect app settings, dotfiles, and small state folders, then restore paths with symlinks.`
- GitHub topics: `windows`, `symlink`, `symbolic-link`, `hardlink`, `backup`, `app-state`, `config-backup`, `config-migration`, `dotfiles`, `dotfiles-manager`, `wpf`, `dotnet`, `ai-tools`, `developer-tools`, `config-management`.
- `CommandLineMode` supports `-help` and has CLI help written for AI assistants and automation.
- `v1.1.5` release notes live in `docs/release-notes-v1.1.5.md` and `docs/release-notes-v1.1.5.en.md`.

## Current Automation Check

A Codex heartbeat automation was created:

- Automation ID: `link-shelf`
- Name: `Link Shelf 链接健康巡检`
- Frequency: every 30 minutes
- Command: `& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" check --json`
- Runs without administrator elevation.
- Stays quiet when `problemCount` is `0`.
- Warns the user and lists abnormal items when `problemCount` is greater than `0`.

It runs without elevation to avoid recurring administrator approval prompts.

## Known Cache Items

The cache has contained these tool or app-state entries:

- `.claude`
- `.claude.json`
- `.codex`
- `.cursor`
- `.gemini`
- `.vscode`
- `.mcp.json`
- Cursor subdirectories such as `Backups`, `Workspaces`, `Dictionaries`, and `User`
- Clash Verge config directory: `io.github.clash-verge-rev.clash-verge-rev`

Do not treat this list as final truth. Use `LinkShelf.exe check --json` and the config file as the source of truth.

## Important Lessons

`.claude.json` may be recreated by related tools. If the original path becomes a real file instead of a link, health checks report `target-has-content`; resolve it through the restore conflict flow.

The Clash Verge config directory can fail to move even with administrator rights because processes may hold files open. Stop `clash-verge`, `clash-verge-service`, and `verge-mihomo` first.

Restore and undo can hit access denied when target paths are in use. The GUI should open the locking-process recovery window, let the user terminate blocking processes, and retry the same item.

Projection hard-link behavior has been verified: launching `LinkShelf.exe cache-root` from the hard-link location returns the hard-link directory, not the original executable directory. Hard links cannot cross volumes.

The Windows account name may be `xiayukun`, but many real paths are under `C:\Users\11467`. Use program or system API output rather than guessing user folders.

## Common Commands

Health check:

```powershell
& "C:\Users\11467\AppData\Local\同步缓存\LinkShelf.exe" check --json
```

Build:

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

Publish:

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

Git status:

```powershell
git status --short --branch
git log --oneline --decorate -5
```

## Recommended Next-Session Prompt

```text
请先读取 AGENTS.md 和 docs/session-handoff.md，然后检查当前项目状态。这个项目是 Link Shelf，用于把分散的本地文件或目录移动到同步缓存根目录，并通过符号链接恢复原路径。项目已经发布到 GitHub，最新公开版本是 v1.1.5。请先不要重构，先确认当前仓库路径、运行缓存根目录、Git 状态、Release 状态、Markdown 中英文配对、自动化巡检和配置健康状态，再继续后续开发或迁移。
```

## Maintenance Rules

- Update this file when release state, automation behavior, cache-root paths, or major design decisions change.
- Do not write secrets, tokens, sensitive config, or private file contents into this file.
- This handoff file does not replace `README.md`, `AGENTS.md`, or release documentation.
