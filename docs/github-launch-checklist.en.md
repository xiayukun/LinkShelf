# GitHub Launch Checklist

Chinese: [github-launch-checklist.md](github-launch-checklist.md)

This checklist turns Link Shelf from a local utility into a repository that is easier to trust, try, and star.

## Positioning

Lead with the broadest useful promise:

> Move scattered local paths into one portable folder, then keep apps working through symbolic links.

Syncthing should be presented as the recommended sync companion, not as the only reason the project exists.

Good GitHub topics:

- `windows`
- `symlink`
- `backup`
- `portable`
- `sync`
- `syncthing`
- `dotnet`
- `wpf`

## Before Publishing

- Replace `Assets/app-preview.png` with a real screenshot of the main window.
- Confirm the release artifact is named `LinkShelf.exe`.
- Confirm `README.md` opens with the problem and the quick start.
- Confirm `LICENSE` exists.
- Confirm `PRIVACY.md` exists and explains local-only behavior.
- Follow `docs/first-push.md` for the empty GitHub repository and first push.
- Confirm `.github/workflows/build.yml` passes on GitHub Actions.
- Create the first release with a short changelog.
- Add the repository profile from `docs/repository-profile.md`.

## What Popular Repositories Usually Do Well

Clear first screen:

- The README explains the problem in one or two sentences.
- The primary screenshot appears near the top.
- Install and quick-start instructions are visible without hunting.

Low trial cost:

- Users can download one artifact.
- The first command or first click is obvious.
- Failure modes and limitations are documented.

Trust signals:

- License is present.
- Build workflow is visible.
- Issues have templates.
- Releases are named and versioned.
- Dangerous behavior is documented honestly.

Community readiness:

- `CONTRIBUTING.md` exists.
- Security reporting path exists.
- The maintainer responds quickly to early issues.
- The roadmap is specific but not bloated.

## Useful References

- GitHub Open Source Guides: https://opensource.guide/building-community/
- GitHub README research: https://arxiv.org/abs/2206.10772
- README structure and popularity study: https://enoei.github.io/papers/liu2022readme.pdf

## First Release Notes Draft

```text
Link Shelf 1.0.0

Initial public release.

Highlights:
- Windows GUI for moving files and folders into a portable cache root.
- Symbolic link restoration on another machine.
- CLI health checks with JSON output.
- English and Chinese UI with automatic language selection.
- English config schema with English runtime folder names.
- Syncthing-friendly workflow without requiring Syncthing.
```
