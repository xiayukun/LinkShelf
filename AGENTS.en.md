# Link Shelf Agent Notes

Chinese: [AGENTS.md](AGENTS.md)

This file is for AI coding agents that work on this repository. The Chinese-first human-facing product documentation belongs in `README.md`; the full English companion belongs in `README.en.md`.

Before changing the project, also read `docs/session-handoff.md`. It records the current local setup, GitHub release state, automation behavior, and important lessons from the setup conversation.

## Product Goal

Link Shelf is a Windows utility that moves selected files or directories into the executable's own folder, then creates symbolic links at the original locations.

The broad use case is path relocation into a portable cache root:

- backup one managed folder instead of scattered paths
- sync selected app or developer-tool state across computers
- move local configuration into a portable directory
- restore links after setting up a new machine
- run a CLI health check for automation

Backup or sync tools are optional companions, not the product itself. Link Shelf handles local relocation and symbolic links; users should choose external tools carefully and avoid blindly sharing caches, databases, or high-churn directories across machines.

## Core Contract

The executable directory is the cache root.

Primary runtime files and folders:

- `LinkShelf.exe`
- `link-shelf.config.json`
- `.link-shelf-logs`
- `.link-shelf-backups`

Do not create Chinese file names, directory names, config keys, or executable names. Chinese UI text is allowed only inside `Services/LocalizationService.cs` and Chinese documentation files.

## Configuration

Current config schema uses English keys:

```json
{
  "version": 2,
  "cacheId": "generated-id",
  "updatedAt": "2026-06-04 19:00:00",
  "items": [
    {
      "cacheName": ".codex",
      "originalPath": "~\\.codex",
      "type": "directory",
      "linkMode": "symbolic-link",
      "status": "enabled",
      "sourceMachine": "DESKTOP",
      "createdAt": "2026-06-04 19:00:00",
      "updatedAt": "2026-06-04 19:00:00",
      "lastOperation": "add-sync-item",
      "note": ""
    }
  ]
}
```

`ConfigStore` only reads the current English schema. Old Chinese-key config files are not supported.

Paths under the current user profile must be stored with `~` so that configs can work across different Windows user folders.

## Main Features

Add item:

1. User selects one or more files or directories.
2. The app rejects the cache root itself, paths inside the cache root, and parent directories that contain the cache root.
3. The app moves the selected path into the cache root.
4. The app creates a symbolic link at the original path.
5. The app saves a config record.
6. Batch add must stop immediately when a selected item is canceled or fails; it must not continue processing later selections.
7. If a normal add has moved the source into the cache but fails before the link/config record is completed, try to roll the moved content back to the original path.
8. Windows shortcut files (`.lnk`) must be rejected with a localized warning. They do not work reliably after being moved into the cache and linked back.

Project app:

- The `Project app` button creates a hard link named `LinkShelf.exe` in a user-selected directory.
- Starting Link Shelf from that hard link uses the hard-link directory as `AppContext.BaseDirectory`, so that directory becomes an independent cache root.
- Use a hard link for projection, not a shortcut and not a copied executable.
- After regenerating `dist\LinkShelf.exe`, inspect existing projected hard links and recreate them if needed, because publish/build tools may replace the file object and break the old hard-link relationship.
- Projection is same-drive only because Windows hard links cannot cross volumes.
- Do not overwrite an existing `LinkShelf.exe` or any other file system entry in the target directory.
- Projection UI text must stay in `LocalizationService`.

Recommended add item:

- `RecommendedSyncItems` defines built-in presets with English keys, portable paths, expected item kind, reason keys, and supported platform.
- Presets should favor broadly useful developer, AI coding, editor, terminal, package-manager, and small app-state paths. Avoid adding purely personal application choices unless they are also broadly common.
- Windows presets must not be shown on macOS or Linux; macOS presets must use Unix-style portable paths such as `~/.gitconfig` or `~/Library/Application Support/...`.
- When preset coverage changes, update README, changelog, release notes, and English companions to describe the source and scope of the recommendation list.
- Recommended UI text and reasons must stay in `LocalizationService`.
- The recommendation window must only show paths that exist on the current machine and are not already present in enabled config records.
- If an enabled config record already covers a preset path as a parent or child path, hide the preset to avoid duplicate management.
- Selecting recommended items runs the same add-item workflow as manually selecting those paths. It must not bypass conflict handling, locked-path handling, or safety checks.

Locked-path handling during add item:

- Always try the normal move first. Do not scan for locking processes before the first move attempt fails.
- If Windows reports access denied for the selected path, open `LockingProcessesWindow`.
- The lock window scans on a background task so the main UI does not freeze.
- Lock inspection is adapted from `ShowWhatProcessLocksFile` and lives under `ThirdParty/ShowWhatProcessLocksFile`.
- The lock window lists process name, process ID, user, executable path, and locked files under the selected file or directory.
- The user can refresh the scan, cancel, terminate selected processes from the context menu, or use "terminate all and continue".
- "Terminate all and continue" terminates the listed processes, waits briefly, closes the lock window with `Continue`, reloads config/grid state, and retries the original add-item move.
- If the retry still fails with access denied, show the lock window again instead of silently giving up.
- If the user cancels the lock window during add, stop that add operation and any remaining batch selections.
- Detection or termination errors should be shown in the lock window and should not crash the main window.

Restore links:

1. Read config.
2. Expand `~` with the current machine's user profile.
3. Create links from original paths to cache items.
4. When target content already exists, show the conflict dialog and wait for the user's choice.
5. Process selected rows one by one; if no rows are selected, process all enabled items.
6. If restore fails with access denied, open `LockingProcessesWindow` for the target path and let the user terminate blocking processes before retrying the same restore item.
7. If the cache item is missing but the original path has real content, show the conflict dialog with only `ImportTargetOverwriteCache` enabled; cache-based, merge, and skip choices must be disabled because there is no cache content to use.
8. If the selected row is an untracked cache item, prompt the user for the original path, immediately create and save a config record, then run the normal restore/conflict flow for that item.

Status display:

- Config status values stay English and should not be changed for display-only problems.
- When an enabled item has an unhealthy check result, the grid status should display the localized `problem` label instead of `enabled`.

Check status:

- read-only except for saving normalized config and clearing removed records
- checks cache item existence
- checks whether target path exists
- checks whether target path is a link
- checks whether the link points to the expected cache item

Move back / undo:

- processes selected rows only
- removes the original-path link only when it points to the expected cache item
- moves the cache item back to the original path
- removes the config record after the move succeeds
- must not overwrite real content at the original path
- must stop with a clear error if the original path has real content or points to another link target
- if move back / undo fails with access denied, open `LockingProcessesWindow`, let the user terminate blocking processes, and retry the same item
- when choosing the lock scan path for move back / undo, scan the original path while it still exists; if the original link has already been removed, scan the cache item path
- if the cache item is missing, ask the user whether to remove only the config record; on confirmation, delete the JSON record without moving files or folders
- if the selected row is an untracked cache item, warn that there is no known destination; on confirmation, delete that cache item from the cache root

Untracked cache items:

- Cache root children that are not covered by enabled config records should appear in the grid as `untracked-cache-item`.
- Ignore Link Shelf runtime entries: `LinkShelf.exe`, `link-shelf.config.json`, `.link-shelf-logs`, and `.link-shelf-backups`.
- Untracked rows are temporary view models and must not be saved to JSON until the user chooses `Restore links` and provides an original path.

CLI mode:

```powershell
.\LinkShelf.exe check
.\LinkShelf.exe check --json
.\LinkShelf.exe check --verbose
.\LinkShelf.exe status
.\LinkShelf.exe recommended
.\LinkShelf.exe recommended --json
.\LinkShelf.exe recommended --platform macos --json
.\LinkShelf.exe cache-root
.\LinkShelf.exe platform
.\LinkShelf.exe version
.\LinkShelf.exe help
.\LinkShelf.exe -help
```

CLI checks should stay safe for scheduled automation and should not move, delete, overwrite, restore, or link files.

## Conflict Decisions

Current decisions:

- `UseCacheDeleteTarget`: back up target content, then link target path to cache content.
- `ImportTargetOverwriteCache`: copy target content into cache, back up target content, then create the link.
- `ImportTargetThenOverlayCacheBackup`: back up old cache content, import target as the official cache, create the link, then overlay old cache content.
- `BackupTargetAndSkip`: copy target content to backup and skip this item.
- `Cancel`: do nothing.

Never silently overwrite or delete target content.

## Internationalization

UI language is controlled by `LocalizationService`.

Rules:

- Default language follows `CultureInfo.CurrentUICulture`.
- The main window must allow switching between English and Chinese.
- UI strings should be retrieved through `LocalizationService`.
- Internal status codes and config values stay English.
- Data grid rows should display localized labels through `SyncItemRow`.

## Documentation Pairing

When changing Markdown documentation, keep English and Chinese documents paired.

Rules:

- If an English user-facing Markdown file has a Chinese companion, update both files in the same change.
- If a Markdown file is added or materially changed, keep a same-purpose `.en.md` English companion unless it is machine-generated and not intended to be read as repository documentation.
- Keep paired documents structurally aligned: matching purpose, section order, examples, release links, screenshots, warnings, and acknowledgements.
- Paired documents should link to each other near the top so English readers can reach Chinese docs and Chinese readers can return to English docs.
- For `README.md` and `README.en.md`, verify the table of contents, major sections, download links, screenshots, feature descriptions, and acknowledgements stay equivalent.
- For release notes and changelogs, add the same version entries to both languages.
- For GitHub issue and pull request templates, provide `.en.md` English companions when practical.
- Before committing Markdown changes, list all `.md` files and check that paired English/Chinese documents still correspond.

## Architecture Boundary

Starting in 2.0, the project has a cross-platform core library and a Windows WPF shell:

- `LinkShelf.Core` targets `net8.0` and owns config models, path tools, config I/O, status checks, recommended-item filtering, and file/symbolic-link operations. The core layer must not reference WPF, Windows Forms, Win32 P/Invoke, or localized UI copy.
- `Directory.Build.props` is the shared source for version and assembly metadata; change it first when bumping versions.
- `LinkShelf.slnx` is the local multi-project entry point and includes the Windows WPF app, Core, CLI, and Core tests.
- The root `LinkShelf.csproj` remains the current Windows WPF app and owns windows, the Windows entry point, language switching, Windows hard-link projection, and locked-process recovery.
- `LinkShelf.Cli` is the cross-platform read-only CLI entry point and reuses `CommandLineRunner` from `LinkShelf.Core`.
- A future macOS build should add a separate platform shell that reuses `LinkShelf.Core`; do not place macOS UI or permission guidance inside the Windows WPF project. See `docs/macos-port-plan.en.md` for design boundaries.

## Important Files

- `Directory.Build.props`: shared version and assembly metadata.
- `LinkShelf.slnx`: multi-project entry point containing the Windows WPF app, Core, and CLI.
- `MainWindow.xaml`: main GUI layout.
- `MainWindow.xaml.cs`: GUI behavior, language switching, multi-select operations, add-item retry after locked-path handling.
- `ConflictChoiceWindow.xaml`: conflict dialog layout.
- `ConflictChoiceWindow.xaml.cs`: conflict decision mapping.
- `LockingProcessesWindow.xaml`: locked-path recovery window layout.
- `LockingProcessesWindow.xaml.cs`: locked-path scan, process list, process termination, and continue/cancel decision mapping.
- `RecommendedItemsWindow.xaml`: recommended item picker layout.
- `RecommendedItemsWindow.xaml.cs`: recommended item selection and add/cancel decision mapping.
- `LinkShelf.Core/Models/SyncModels.cs`: config models, constants, row view model, enums.
- `LinkShelf.Core/Models/RecommendedSyncItem.cs`: recommended item models and grid row model.
- `LinkShelf.Core/Services/AppPaths.cs`: cache root, config path, log path, backup path.
- `LinkShelf.Core/Services/ConfigStore.cs`: config load, normalization, save.
- `LinkShelf.Core/Services/FileOperations.cs`: moving, copying, linking, backups, restore, conflict application.
- `LinkShelf.Core/Services/RecommendedSyncItems.cs`: built-in recommended paths and local/config filtering.
- `LinkShelf.Core/Services/StatusCheckService.cs`: read-only health checks.
- `LinkShelf.Core/Services/PathTools.cs`: path normalization, `~` expansion, unique cache names.
- `LinkShelf.Core/Services/LogService.cs`: operation logs and diagnostic logs for add-item troubleshooting.
- `LinkShelf.Core/CommandLineRunner.cs`: shared read-only CLI logic used by the GUI app and cross-platform CLI.
- `LinkShelf.Cli/Program.cs`: cross-platform CLI entry point.
- `LinkShelf.Core.Tests/Program.cs`: dependency-free Core behavior test entry point for cross-platform CI validation.
- `Services/ProjectionService.cs`: Windows hard-link projection.
- `Services/LocalizationService.cs`: English and Chinese UI strings.
- `CommandLineMode.cs`: CLI entry points and machine-readable status output.
- `ThirdParty/ShowWhatProcessLocksFile`: adapted lock inspection and process termination code.
- `THIRD-PARTY-NOTICES.md`: third-party notices for copied or adapted code and workflow references.

## Build

Build:

```powershell
dotnet build .\LinkShelf.slnx -c Release
dotnet run --project .\LinkShelf.Core.Tests\LinkShelf.Core.Tests.csproj -c Release --no-build
```

After changing code, do not stop at `dotnet build`. Re-publish `dist\LinkShelf.exe`, inspect the projected hard links, and recreate any projected `LinkShelf.exe` hard links that should point at the new `dist` artifact.

Publish:

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

The published executable is `dist\LinkShelf.exe`.

## GitHub Readiness

Before publishing, prefer:

- Chinese-first `README.md` with screenshot, install, quick start, CLI examples, and limitations
- `README.en.md` with equivalent English content
- a direct latest download link in `README.md`, preferably `https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe`
- `CONTRIBUTING.md`
- a real license file
- a clean release artifact named `LinkShelf.exe`
- GitHub topics such as `windows`, `symlink`, `backup`, `config-migration`, `config-backup`, `dotnet`, `wpf`
- issue templates after the first public release

## Safety Boundaries

Do not allow users to select the cache root itself.

Do not allow users to select items inside the cache root.

Do not allow users to select a parent directory that contains the cache root.

Do not silently delete, overwrite, or merge target-path content.

Do not implement directory-internal include or exclude rules in Link Shelf. Pair with an external backup or sync tool for internal ignore rules.

Do not save user-profile absolute paths in config when `~` can represent them.
