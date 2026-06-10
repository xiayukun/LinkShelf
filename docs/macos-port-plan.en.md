# macOS Port Plan

Chinese: [macos-port-plan.md](macos-port-plan.md)

This document describes the macOS port direction after Link Shelf 2.0. It is for maintainers and AI coding agents. The goal is to keep Windows behavior stable while expanding macOS support behind clear boundaries.

## Current State

- The Windows WPF app remains the current official downloadable build.
- `LinkShelf.Core` has been split out, targets `net8.0`, and owns config, paths, status checks, recommended-item filtering, file/symbolic-link operations, and the shared command runner.
- `LinkShelf.Cli` exists as a cross-platform read-only command-line entry point that reuses Core outside WPF.
- The GitHub build workflow compiles the CLI and runs Core tests on Windows, Linux, and macOS runners to protect the shared layer from accidental Windows-only dependencies.

## Parts Not Reused Directly

These pieces remain Windows-shell responsibilities and should not move into Core directly:

- WPF windows and Windows Forms pickers.
- Windows elevation, UAC, and Developer Mode guidance.
- Windows hard-link projection service.
- Windows shortcut file `.lnk` handling.
- Locked-process detection based on ShowWhatProcessLocksFile.

## macOS Phase One

The first phase is not a full GUI. It should prove that core behavior can run safely on macOS:

1. Keep compiling `LinkShelf.Cli` on macOS runners.
2. Run CLI `version`, `cache-root`, `platform`, `check --json`, and `recommended --json` on a real macOS machine.
3. Manually verify file and directory symlink create/check/restore semantics in temporary folders.
4. Record paths that need Full Disk Access, terminal permissions, or explicit user authorization.

## macOS GUI Direction

Evaluate Avalonia first because it can reuse C#, MVVM-style structure, and Core. If the experience is not good enough, consider a native SwiftUI shell that calls Core or the CLI.

A macOS GUI cannot blindly copy the Windows interaction where the executable directory is the cache root. For a `.app` bundle, `AppContext.BaseDirectory` usually points inside the app package, which is not a suitable user cache root. Possible directions:

- CLI/portable mode continues to use the binary directory as the cache root.
- GUI mode asks the user to choose a cache root on first launch and stores that choice in app settings.
- If a user places a portable GUI inside a cache root, the app must clearly display the actual cache-root path so it does not accidentally use `.app/Contents/MacOS`.

That changes shell entry behavior, so it needs a separate design. Do not rush that change into the Windows app or the core contract.

## Platform Differences

- macOS can create file and directory symbolic links, but system permissions, protected locations, and user authorization matter.
- Unix-like systems generally cannot hard-link directories; hard-link projection should only be evaluated as a file-level capability.
- APFS is usually case-insensitive by default, but can be case-sensitive; Core path comparison is now centralized through `PathTools.PathStringComparison` so it can be tightened by platform later.
- Recommended paths now have platform boundaries and an initial macOS preset catalog; continue validating path existence, privacy risk, and copy on a real macOS environment. Do not show Windows `AppData` presets to macOS users.
- Locked-process detection needs a new macOS implementation; the current Windows third-party code cannot be reused.

## Required Development Environment

I can continue maintaining Core, CLI, docs, and CI from the current Windows environment. To validate or ship a macOS GUI later, you will need to provide:

- A macOS machine, or access to a macOS CI runner.
- .NET 8 SDK.
- Xcode Command Line Tools.
- If using Avalonia: Avalonia templates and package restore access to NuGet.
- For public distribution: Apple Developer account, Developer ID certificate, and notarization credentials.

## Acceptance Bar

macOS support should not be declared complete from a Windows-only build. It needs at least:

- CLI build passing on macOS.
- Basic symlink flows verified on a real macOS filesystem.
- GUI cache-root selection that does not fall into an incorrect `.app` internal path.
- Windows WPF build, publish, hard-link projection, and `check --json` still passing.
- English and Chinese docs updated in pairs.
