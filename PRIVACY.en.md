# Privacy

Chinese: [PRIVACY.md](PRIVACY.md)

Link Shelf is designed as a local Windows utility.

## Data It Reads

Link Shelf reads:

- the cache root where `LinkShelf.exe` is running
- `link-shelf.config.json`
- configured source paths and cache item paths
- file-system metadata needed to check links, conflicts, and missing items

## Data It Writes

Link Shelf writes:

- `link-shelf.config.json`
- `.link-shelf-logs`
- `.link-shelf-backups`
- symbolic links at configured target paths
- moved files or directories when the user adds an item or resolves a conflict

## Network Access

Link Shelf does not require network access for normal operation.

It does not upload files, paths, logs, configuration, or machine names to a remote service. If the cache root is stored in a cloud drive, sync tool, or backup tool, that external tool is responsible for any network transfer.

## Sensitive Paths

Configuration may contain local paths, machine names, and cache item names. Treat `link-shelf.config.json`, logs, backups, and screenshots as potentially sensitive before sharing them publicly.

## Automation

CLI commands such as `LinkShelf.exe check --json` are intended for local health checks. They print configured paths and status information to the local console so automation tools can notify the user when links break.
