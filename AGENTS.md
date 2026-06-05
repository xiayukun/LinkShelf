# Link Shelf Agent Notes

This file is for AI coding agents that work on this repository. Human-facing product documentation belongs in `README.md`.

Before changing the project, also read `docs/session-handoff.md`. It records the current local setup, GitHub release state, automation behavior, and important lessons from the setup conversation.

## Product Goal

Link Shelf is a Windows utility that moves selected files or directories into the executable's own folder, then creates symbolic links at the original locations.

The broad use case is path relocation into a portable cache root:

- backup one managed folder instead of scattered paths
- sync selected app or developer-tool state across computers
- move local configuration into a portable directory
- restore links after setting up a new machine
- run a CLI health check for automation

Syncthing is the recommended companion, not the only supported scenario. Syncthing can sync the cache root between machines while Link Shelf handles local symbolic links.

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

1. User selects a file or directory.
2. The app rejects the cache root itself, paths inside the cache root, and parent directories that contain the cache root.
3. The app moves the selected path into the cache root.
4. The app creates a symbolic link at the original path.
5. The app saves a config record.

Recommended add item:

- `RecommendedSyncItems` defines built-in presets with English keys, portable paths, expected item kind, and reason keys.
- Presets should favor broadly useful developer, AI coding, editor, terminal, package-manager, and small app-state paths. Avoid adding purely personal application choices unless they are also broadly common.
- When preset coverage changes, update README, changelog, release notes, and Chinese companions to describe the source and scope of the recommendation list.
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
- Detection or termination errors should be shown in the lock window and should not crash the main window.

Restore links:

1. Read config.
2. Expand `~` with the current machine's user profile.
3. Create links from original paths to cache items.
4. When target content already exists, show the conflict dialog and wait for the user's choice.
5. Process selected rows one by one; if no rows are selected, process all enabled items.

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

CLI mode:

```powershell
.\LinkShelf.exe check
.\LinkShelf.exe check --json
.\LinkShelf.exe check --verbose
.\LinkShelf.exe status
.\LinkShelf.exe cache-root
.\LinkShelf.exe version
.\LinkShelf.exe help
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
- If a Markdown file is added or materially changed, keep a same-purpose `.zh-CN.md` companion unless it is machine-generated and not intended to be read as repository documentation.
- Keep paired documents structurally aligned: matching purpose, section order, examples, release links, screenshots, warnings, and acknowledgements.
- For `README.md` and `README.zh-CN.md`, verify the table of contents, major sections, download links, screenshots, feature descriptions, and acknowledgements stay equivalent.
- For release notes and changelogs, add the same version entries to both languages.
- For GitHub issue and pull request templates, provide Chinese companions when practical.
- Before committing Markdown changes, list all `.md` files and check that paired English/Chinese documents still correspond.

## Important Files

- `MainWindow.xaml`: main GUI layout.
- `MainWindow.xaml.cs`: GUI behavior, language switching, multi-select operations, add-item retry after locked-path handling.
- `ConflictChoiceWindow.xaml`: conflict dialog layout.
- `ConflictChoiceWindow.xaml.cs`: conflict decision mapping.
- `LockingProcessesWindow.xaml`: locked-path recovery window layout.
- `LockingProcessesWindow.xaml.cs`: locked-path scan, process list, process termination, and continue/cancel decision mapping.
- `RecommendedItemsWindow.xaml`: recommended item picker layout.
- `RecommendedItemsWindow.xaml.cs`: recommended item selection and add/cancel decision mapping.
- `Models/SyncModels.cs`: config models, constants, row view model, enums.
- `Models/RecommendedSyncItem.cs`: recommended item models and grid row model.
- `Services/AppPaths.cs`: cache root, config path, log path, backup path.
- `Services/ConfigStore.cs`: config load, normalization, save.
- `Services/FileOperations.cs`: moving, copying, linking, backups, restore, conflict application.
- `Services/RecommendedSyncItems.cs`: built-in recommended paths and local/config filtering.
- `Services/StatusCheckService.cs`: read-only health checks.
- `Services/PathTools.cs`: path normalization, `~` expansion, unique cache names.
- `Services/LocalizationService.cs`: English and Chinese UI strings.
- `Services/LogService.cs`: operation logs and diagnostic logs for add-item troubleshooting.
- `CommandLineMode.cs`: CLI entry points and machine-readable status output.
- `ThirdParty/ShowWhatProcessLocksFile`: adapted lock inspection and process termination code.
- `THIRD-PARTY-NOTICES.md`: third-party notices for copied or adapted code and workflow references.

## Build

Build:

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

Publish:

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

The published executable is `dist\LinkShelf.exe`.

## GitHub Readiness

Before publishing, prefer:

- `README.md` with screenshot, install, quick start, CLI examples, and limitations
- a direct latest download link in `README.md`, preferably `https://github.com/xiayukun/LinkShelf/releases/latest/download/LinkShelf.exe`
- `CONTRIBUTING.md`
- a real license file
- a clean release artifact named `LinkShelf.exe`
- GitHub topics such as `windows`, `symlink`, `backup`, `syncthing`, `dotnet`, `wpf`
- issue templates after the first public release

## Safety Boundaries

Do not allow users to select the cache root itself.

Do not allow users to select items inside the cache root.

Do not allow users to select a parent directory that contains the cache root.

Do not silently delete, overwrite, or merge target-path content.

Do not implement directory-internal include or exclude rules in Link Shelf. Pair with Syncthing or another sync tool for internal ignore rules.

Do not save user-profile absolute paths in config when `~` can represent them.
