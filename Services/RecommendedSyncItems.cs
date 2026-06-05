using LinkShelf.Models;

namespace LinkShelf.Services;

public static class RecommendedSyncItems
{
    private sealed record Preset(string Id, string NameKey, string PortablePath, SyncItemKind Kind, string ReasonKey);

    private static readonly Preset[] Presets =
    [
        new("codex", "preset.codex.name", @"~\.codex", SyncItemKind.Directory, "preset.codex.reason"),
        new("claude", "preset.claude.name", @"~\.claude", SyncItemKind.Directory, "preset.claude.reason"),
        new("claude-json", "preset.claudeJson.name", @"~\.claude.json", SyncItemKind.File, "preset.claudeJson.reason"),
        new("gemini", "preset.gemini.name", @"~\.gemini", SyncItemKind.Directory, "preset.gemini.reason"),
        new("cursor-profile", "preset.cursorProfile.name", @"~\.cursor", SyncItemKind.Directory, "preset.cursorProfile.reason"),
        new("cursor-user", "preset.cursorUser.name", @"~\AppData\Roaming\Cursor\User", SyncItemKind.Directory, "preset.cursorUser.reason"),
        new("cursor-backups", "preset.cursorBackups.name", @"~\AppData\Roaming\Cursor\Backups", SyncItemKind.Directory, "preset.cursorBackups.reason"),
        new("cursor-workspaces", "preset.cursorWorkspaces.name", @"~\AppData\Roaming\Cursor\Workspaces", SyncItemKind.Directory, "preset.cursorWorkspaces.reason"),
        new("cursor-dictionaries", "preset.cursorDictionaries.name", @"~\AppData\Roaming\Cursor\Dictionaries", SyncItemKind.Directory, "preset.cursorDictionaries.reason"),
        new("vscode-profile", "preset.vscodeProfile.name", @"~\.vscode", SyncItemKind.Directory, "preset.vscodeProfile.reason"),
        new("vscode-user", "preset.vscodeUser.name", @"~\AppData\Roaming\Code\User", SyncItemKind.Directory, "preset.vscodeUser.reason"),
        new("gitconfig", "preset.gitconfig.name", @"~\.gitconfig", SyncItemKind.File, "preset.gitconfig.reason"),
        new("npmrc", "preset.npmrc.name", @"~\.npmrc", SyncItemKind.File, "preset.npmrc.reason"),
        new("yarnrc", "preset.yarnrc.name", @"~\.yarnrc", SyncItemKind.File, "preset.yarnrc.reason"),
        new("mcp-json", "preset.mcpJson.name", @"~\.mcp.json", SyncItemKind.File, "preset.mcpJson.reason"),
        new("powershell", "preset.powershell.name", @"~\Documents\PowerShell", SyncItemKind.Directory, "preset.powershell.reason"),
        new("windows-powershell", "preset.windowsPowerShell.name", @"~\Documents\WindowsPowerShell", SyncItemKind.Directory, "preset.windowsPowerShell.reason"),
        new("windows-terminal", "preset.windowsTerminal.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminal.reason"),
        new("jetbrains", "preset.jetbrains.name", @"~\AppData\Roaming\JetBrains", SyncItemKind.Directory, "preset.jetbrains.reason"),
        new("clash-verge", "preset.clashVerge.name", @"~\AppData\Roaming\io.github.clash-verge-rev.clash-verge-rev", SyncItemKind.Directory, "preset.clashVerge.reason"),
        new("clash-win", "preset.clashWin.name", @"~\AppData\Roaming\clash_win", SyncItemKind.Directory, "preset.clashWin.reason"),
        new("ditto", "preset.ditto.name", @"~\AppData\Roaming\Ditto", SyncItemKind.Directory, "preset.ditto.reason"),
        new("wiz", "preset.wiz.name", @"~\AppData\Roaming\Wiz", SyncItemKind.Directory, "preset.wiz.reason"),
        new("lx-music", "preset.lxMusic.name", @"~\AppData\Roaming\lx-music-desktop", SyncItemKind.Directory, "preset.lxMusic.reason")
    ];

    public static IReadOnlyList<RecommendedSyncItem> GetAvailable(AppPaths paths, SyncConfig config)
    {
        var configuredPaths = config.Items
            .Where(item => item.Status == SyncConstants.StatusEnabled)
            .Select(item => PathTools.Normalize(PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome)))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var result = new List<RecommendedSyncItem>();
        foreach (var preset in Presets)
        {
            var expanded = PathTools.ExpandPortablePath(preset.PortablePath, paths.UserHome);
            if (configuredPaths.Contains(PathTools.Normalize(expanded)))
            {
                continue;
            }

            if (!PathExistsWithKind(expanded, preset.Kind))
            {
                continue;
            }

            if (PathTools.IsSameOrChild(expanded, paths.CacheRoot) || PathTools.IsSameOrChild(paths.CacheRoot, expanded))
            {
                continue;
            }

            result.Add(new RecommendedSyncItem
            {
                Id = preset.Id,
                NameKey = preset.NameKey,
                PortablePath = preset.PortablePath,
                ExpandedPath = expanded,
                Kind = preset.Kind,
                ReasonKey = preset.ReasonKey
            });
        }

        return result;
    }

    private static bool PathExistsWithKind(string path, SyncItemKind kind)
    {
        try
        {
            var attributes = File.GetAttributes(path);
            var isDirectory = attributes.HasFlag(FileAttributes.Directory);
            return kind == SyncItemKind.Directory ? isDirectory : !isDirectory;
        }
        catch
        {
            return false;
        }
    }
}
