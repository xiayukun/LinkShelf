中文：[发布说明](https://github.com/xiayukun/LinkShelf/blob/main/docs/release-notes-v2.0.1.md)

Link Shelf 2.0.1 maintains the build workflows and fixes version reporting.

## Changes

- Build and release now use checkout v7 and setup-dotnet v6, retaining .NET 8 and Windows x64 self-contained single-file publishing.
- The CLI `version` command reads the assembly version instead of incorrectly reporting 1.1.6.
- File migration, symbolic links, hard links, and configuration format remain unchanged.

## Verification

- Release build, single-file publishing, and `check --json` with isolated configuration.
- CLI version checked against the executable file version.

## Requirements

- Windows x64. Creating symbolic links usually requires administrator privileges or Windows Developer Mode allowing the current user to create links.
