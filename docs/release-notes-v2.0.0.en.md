# Link Shelf 2.0.0

Chinese: [release-notes-v2.0.0.md](release-notes-v2.0.0.md)

This release moves the project to 2.0. The focus is groundwork for future platform frontends while preserving the current Windows behavior.

## Highlights

- Added the `LinkShelf.Core` class library for config I/O, path normalization, status checks, recommended-item filtering, and file/symbolic-link operations.
- Added `LinkShelf.Cli` as a cross-platform read-only command-line entry point that reuses Core.
- Added `Directory.Build.props` and `LinkShelf.slnx` to centralize version metadata and provide a multi-project build entry point.
- Added `LinkShelf.Core.Tests` for cross-platform CI validation of core behavior.
- Recommended items now have platform boundaries, with an initial macOS preset catalog.
- Added the read-only CLI command `platform` for confirming detected platform behavior.
- Added the read-only CLI command `recommended` / `recommended --json` for inspecting available recommended paths.
- The GitHub build workflow compiles the CLI on Windows, Linux, and macOS runners to help preserve the cross-platform boundary.
- Core path comparison is centralized through `PathTools.PathStringComparison` / `PathStringComparer`.
- Added the [macOS port plan](macos-port-plan.en.md), covering GUI, cache-root, permission, and validation requirements.
- The Windows WPF app remains the current downloadable build and now consumes the shared Core logic.
- The README stays as a short homepage, with full details kept in the user guide.
- Documentation now states that macOS support still needs a separate frontend, permission guidance, and real-device validation.

## Download

- `LinkShelf.exe`

## Requirements

- Windows 10 or later
- Permission to create symbolic links; run as administrator or enable Developer Mode if needed

## Verification

Before publishing, confirm:

- `dotnet build .\LinkShelf.slnx -c Release`
- `dotnet run --project .\LinkShelf.Core.Tests\LinkShelf.Core.Tests.csproj -c Release --no-build`
- `dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist`
- `.\dist\LinkShelf.exe version` prints `2.0.0`
- `.\dist\LinkShelf.exe check --json` remains a read-only health check
