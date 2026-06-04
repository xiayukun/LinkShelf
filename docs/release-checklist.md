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
- Include the first release notes from `docs/github-launch-checklist.md`.
- Update `CHANGELOG.md` before publishing another release.
