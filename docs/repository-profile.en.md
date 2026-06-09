# Repository Profile

Chinese: [repository-profile.md](repository-profile.md)

Suggested GitHub repository name:

```text
link-shelf
```

Suggested description:

```text
Windows 配置迁移和符号链接工具 / Windows config mover: collect app settings, dotfiles, and small state folders, then restore paths with symlinks.
```

Suggested website:

```text
https://github.com/xiayukun/LinkShelf/releases/latest
```

Suggested topics:

```text
windows
symlink
symbolic-link
hardlink
backup
app-state
config-backup
config-migration
dotfiles
dotfiles-manager
wpf
dotnet
ai-tools
developer-tools
config-management
```

Suggested social preview text:

```text
Link Shelf collects scattered Windows app settings, dotfiles, developer config, and AI coding tool state into one cache root, then restores original paths with symlinks.
```

Short launch post:

```text
I built Link Shelf, a Windows config mover and symlink tool. It collects scattered app settings, dotfiles, developer config, and AI coding tool state into one cache root, then restores the original paths with symbolic links. It can add recommended local config paths, restore links on a new machine, check link health from the CLI, and move managed items back when you want to undo them. Backup, copy, or sync the cache root with a tool you trust, but do not blindly share large caches, databases, or high-churn directories across machines.
```

## Promotion Notes

- Keep `README.md` as the default GitHub landing page and make it Chinese-first because the current launch audience is Chinese. Preserve the full English version in `README.en.md` for GitHub search, global users, and external references.
- Reader-facing Markdown now uses Chinese as the default `.md` document and English companions as `.en.md`. Do not keep the old Chinese-named compatibility files; release-note language links should point at the current `main` branch so renamed companion files do not break when older tags lack them.
- Keep `LICENSE` as the standard English MIT text so GitHub can detect the license and the legal text stays canonical; other explanatory pages use Chinese default documents.
- Use AI positioning carefully. The strongest angle is not "AI sells this tool"; it is "Link Shelf helps users organize, back up, and restore local AI coding tool configuration."
- Do not make any specific sync tool the primary homepage selling point. External tools should appear only as optional companions, with warnings against blindly sharing caches, databases, and high-churn directories.
- Good search phrases for README, releases, topics, and launch posts: Windows symlink manager, symbolic link, hard link, dotfiles manager, config migration, config backup, app state backup, AI coding tools, Codex, Claude, Gemini, Cursor.
