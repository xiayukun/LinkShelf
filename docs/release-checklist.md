# Release Checklist

## Build

```powershell
dotnet publish .\LinkShelf.csproj -t:Rebuild -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\dist
```

Expected artifact:

```text
dist\LinkShelf.exe
```

## Local Verification

From a cache root:

```powershell
.\LinkShelf.exe check --json
```

Expected result:

```json
{
  "ok": true,
  "problemCount": 0
}
```

## Manual GUI Verification

- Double-click `LinkShelf.exe`.
- Confirm Windows elevation prompt appears when needed.
- Confirm the main window opens.
- Confirm the language selector can switch between English and Chinese.
- Confirm table headers and buttons update after language switching.
- Confirm `check --json` still works after closing the GUI.

## GitHub Release

- Tag: `v1.0.0`
- Title: `Link Shelf 1.0.0`
- Artifact: `LinkShelf.exe`
- Prefer the `release` GitHub Actions workflow.
- For the first release, open Actions, choose `release`, click `Run workflow`, keep branch `main`, and set tag to `v1.0.0`.
- For later releases, push a new `v*` tag and let the workflow run automatically.
- Use release notes from `docs/release-notes-v1.0.0.md`.
- Update `CHANGELOG.md` before publishing another release.
