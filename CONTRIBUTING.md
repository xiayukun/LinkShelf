# Contributing

中文：[CONTRIBUTING.zh-CN.md](CONTRIBUTING.zh-CN.md)

Thanks for taking a look at Link Shelf.

## Development Setup

Requirements:

- Windows
- .NET SDK

Build:

```powershell
dotnet build .\LinkShelf.csproj -c Release
```

Run CLI checks from a cache-root folder:

```powershell
.\LinkShelf.exe check --json
```

## Pull Request Guidelines

- Keep file operations conservative.
- Do not silently delete or overwrite user data.
- Keep the current English configuration schema stable unless a new versioned migration is documented.
- Keep config keys, executable names, and runtime directory names in English.
- Keep UI strings in the localization service.
- Update `README.md` and `README.en.md` when behavior changes.
- Verify with `dotnet build -c Release`.

## Safety Rules

Any change that moves, deletes, overwrites, backs up, or links user files must have clear user confirmation in the GUI path. CLI commands should remain read-only unless a future command name makes mutation explicit.
