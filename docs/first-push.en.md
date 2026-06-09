# First Push Checklist

Chinese: [first-push.md](first-push.md)

Use this checklist after creating an empty GitHub repository named `LinkShelf`.

## GitHub Repository Settings

Create the repository with these options:

- Owner: your GitHub account or organization
- Repository name: `LinkShelf`
- Description: `A Windows desktop and CLI tool for moving scattered files and folders into a portable cache root, then restoring them with symbolic links.`
- Visibility: `Public`
- Add README: off
- Add .gitignore: `No .gitignore`
- Add license: `No license`

The local repository already contains `README.md`, `.gitignore`, and `LICENSE`, so GitHub should not generate those files.

## Local Git Settings

Set the commit identity for this repository:

```powershell
git config user.name "xiayukun"
git config user.email "YOUR_GITHUB_EMAIL"
```

If you do not want to expose a personal email address, use the GitHub-provided noreply email from GitHub email settings.

## First Commit

```powershell
git status --short
git diff --cached --check
git commit -m "Initial public release"
```

## Connect Remote

Replace the URL with the repository URL shown by GitHub:

```powershell
git remote add origin https://github.com/xiayukun/LinkShelf.git
git branch -M main
git push -u origin main
```

## After Push

- Confirm the README renders correctly on GitHub.
- Confirm screenshots render near the top of the README.
- Confirm the build workflow starts under GitHub Actions.
- Add the repository topics listed in `docs/github-launch-checklist.md`.
- Create the first release with the `release` GitHub Actions workflow. Use branch `main` and tag `v1.0.0`.
