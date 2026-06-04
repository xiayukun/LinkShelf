# Link Shelf Agent Notes

This file is for AI coding agents that work on this repository. Human-facing product documentation belongs in `README.md`.

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

Remove records:

- removes selected records from config only
- does not move files
- does not delete existing links

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

## Important Files

- `MainWindow.xaml`: main GUI layout.
- `MainWindow.xaml.cs`: GUI behavior, language switching, multi-select operations.
- `ConflictChoiceWindow.xaml`: conflict dialog layout.
- `ConflictChoiceWindow.xaml.cs`: conflict decision mapping.
- `Models/SyncModels.cs`: config models, constants, row view model, enums.
- `Services/AppPaths.cs`: cache root, config path, log path, backup path.
- `Services/ConfigStore.cs`: config load, normalization, save.
- `Services/FileOperations.cs`: moving, copying, linking, backups, restore, conflict application.
- `Services/StatusCheckService.cs`: read-only health checks.
- `Services/PathTools.cs`: path normalization, `~` expansion, unique cache names.
- `Services/LocalizationService.cs`: English and Chinese UI strings.
- `CommandLineMode.cs`: CLI entry points and machine-readable status output.

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
