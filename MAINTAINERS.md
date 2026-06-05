# Maintainers

中文：[MAINTAINERS.zh-CN.md](MAINTAINERS.zh-CN.md)

Primary maintainer:

- xiayukun

## Maintenance Principles

- Keep the first-run workflow simple.
- Keep CLI checks safe and read-only.
- Prefer explicit user confirmation for any operation that moves, replaces, links, or backs up user content.
- Keep config keys and runtime file names in English.
- Keep Chinese UI text isolated in `Services/LocalizationService.cs`.
- Keep Syncthing as a recommended workflow, not the only supported workflow.

## Release Rhythm

Small bug-fix releases are preferred over large, risky batches. A release should include:

- a passing Windows build
- a fresh `LinkShelf.exe` artifact
- short release notes
- any README updates needed for changed behavior
