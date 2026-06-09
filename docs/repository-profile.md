# Repository Profile

中文：[repository-profile.zh-CN.md](repository-profile.zh-CN.md)

Suggested GitHub repository name:

```text
link-shelf
```

Suggested description:

```text
Portable Windows app-state and dotfiles manager: move scattered config into one syncable cache root, then restore original paths with symlinks.
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
sync
dotfiles
dotfiles-manager
syncthing
wpf
dotnet
ai-tools
developer-tools
config-management
```

Suggested social preview text:

```text
Link Shelf turns scattered Windows app state, dotfiles, developer settings, and AI coding tool config into one portable cache root while apps keep using their original paths.
```

Short launch post:

```text
I built Link Shelf, a small Windows utility for collecting scattered app state, dotfiles, developer settings, and AI coding tool config into one syncable cache root while apps keep using their original paths through symbolic links. It can add recommended local config paths, restore links on a new machine, check link health from the CLI, and move managed items back when you want to undo them. Syncthing works great with it, but any backup or sync workflow can use the same idea.
```

## Promotion Notes

- Keep `README.md` as the default GitHub landing page, but make it Chinese-first because the current launch audience is Chinese. Preserve the full English version in `README.en.md` for GitHub search, global users, and external references.
- Do not rename every Markdown file into a Chinese-default / `*-en.md` pair yet. That would create wide link churn across release notes, templates, and external pages. Apply the Chinese-first model first to the default README, where the promotion impact is highest.
- Use AI positioning carefully. The strongest angle is not "AI sells this tool"; it is "Link Shelf helps users sync and restore local AI coding tool configuration safely."
- Good search phrases for README, releases, topics, and launch posts: Windows symlink manager, symbolic link, hard link, dotfiles manager, config sync, app state backup, Syncthing, AI coding tools, Codex, Claude, Gemini, Cursor.
