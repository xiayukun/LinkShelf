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
        new("claude-desktop", "preset.claudeDesktop.name", @"~\AppData\Roaming\Claude", SyncItemKind.Directory, "preset.claudeDesktop.reason"),
        new("claude-desktop-msix", "preset.claudeDesktopMsix.name", @"~\AppData\Local\Packages\Claude_pzs8sxrjxfjjc\LocalCache\Roaming\Claude", SyncItemKind.Directory, "preset.claudeDesktopMsix.reason"),
        new("gemini", "preset.gemini.name", @"~\.gemini", SyncItemKind.Directory, "preset.gemini.reason"),
        new("continue", "preset.continue.name", @"~\.continue", SyncItemKind.Directory, "preset.continue.reason"),
        new("aider-config", "preset.aiderConfig.name", @"~\.aider.conf.yml", SyncItemKind.File, "preset.aiderConfig.reason"),
        new("windsurf-mcp", "preset.windsurfMcp.name", @"~\.codeium\windsurf", SyncItemKind.Directory, "preset.windsurfMcp.reason"),
        new("windsurf-user", "preset.windsurfUser.name", @"~\AppData\Roaming\Windsurf\User", SyncItemKind.Directory, "preset.windsurfUser.reason"),
        new("cursor-profile", "preset.cursorProfile.name", @"~\.cursor", SyncItemKind.Directory, "preset.cursorProfile.reason"),
        new("cursor-user", "preset.cursorUser.name", @"~\AppData\Roaming\Cursor\User", SyncItemKind.Directory, "preset.cursorUser.reason"),
        new("cursor-backups", "preset.cursorBackups.name", @"~\AppData\Roaming\Cursor\Backups", SyncItemKind.Directory, "preset.cursorBackups.reason"),
        new("cursor-workspaces", "preset.cursorWorkspaces.name", @"~\AppData\Roaming\Cursor\Workspaces", SyncItemKind.Directory, "preset.cursorWorkspaces.reason"),
        new("cursor-dictionaries", "preset.cursorDictionaries.name", @"~\AppData\Roaming\Cursor\Dictionaries", SyncItemKind.Directory, "preset.cursorDictionaries.reason"),
        new("cline-cursor", "preset.clineCursor.name", @"~\AppData\Roaming\Cursor\User\globalStorage\saoudrizwan.claude-dev", SyncItemKind.Directory, "preset.clineCursor.reason"),
        new("roo-cursor", "preset.rooCursor.name", @"~\AppData\Roaming\Cursor\User\globalStorage\rooveterinaryinc.roo-cline", SyncItemKind.Directory, "preset.rooCursor.reason"),
        new("vscode-profile", "preset.vscodeProfile.name", @"~\.vscode", SyncItemKind.Directory, "preset.vscodeProfile.reason"),
        new("vscode-user", "preset.vscodeUser.name", @"~\AppData\Roaming\Code\User", SyncItemKind.Directory, "preset.vscodeUser.reason"),
        new("vscode-insiders-profile", "preset.vscodeInsidersProfile.name", @"~\.vscode-insiders", SyncItemKind.Directory, "preset.vscodeInsidersProfile.reason"),
        new("vscode-insiders-user", "preset.vscodeInsidersUser.name", @"~\AppData\Roaming\Code - Insiders\User", SyncItemKind.Directory, "preset.vscodeInsidersUser.reason"),
        new("vscodium-profile", "preset.vscodiumProfile.name", @"~\.vscode-oss", SyncItemKind.Directory, "preset.vscodiumProfile.reason"),
        new("vscodium-user", "preset.vscodiumUser.name", @"~\AppData\Roaming\VSCodium\User", SyncItemKind.Directory, "preset.vscodiumUser.reason"),
        new("cline-vscode", "preset.clineVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\saoudrizwan.claude-dev", SyncItemKind.Directory, "preset.clineVsCode.reason"),
        new("roo-vscode", "preset.rooVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\rooveterinaryinc.roo-cline", SyncItemKind.Directory, "preset.rooVsCode.reason"),
        new("copilot-vscode", "preset.copilotVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\github.copilot", SyncItemKind.Directory, "preset.copilotVsCode.reason"),
        new("copilot-chat-vscode", "preset.copilotChatVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\github.copilot-chat", SyncItemKind.Directory, "preset.copilotChatVsCode.reason"),
        new("gitconfig", "preset.gitconfig.name", @"~\.gitconfig", SyncItemKind.File, "preset.gitconfig.reason"),
        new("gitignore-global", "preset.gitignoreGlobal.name", @"~\.gitignore_global", SyncItemKind.File, "preset.gitignoreGlobal.reason"),
        new("git-config-dir", "preset.gitConfigDir.name", @"~\.config\git", SyncItemKind.Directory, "preset.gitConfigDir.reason"),
        new("npmrc", "preset.npmrc.name", @"~\.npmrc", SyncItemKind.File, "preset.npmrc.reason"),
        new("yarnrc", "preset.yarnrc.name", @"~\.yarnrc", SyncItemKind.File, "preset.yarnrc.reason"),
        new("yarnrc-yml", "preset.yarnrcYml.name", @"~\.yarnrc.yml", SyncItemKind.File, "preset.yarnrcYml.reason"),
        new("pnpmrc", "preset.pnpmrc.name", @"~\.pnpmrc", SyncItemKind.File, "preset.pnpmrc.reason"),
        new("mcp-json", "preset.mcpJson.name", @"~\.mcp.json", SyncItemKind.File, "preset.mcpJson.reason"),
        new("pip-config", "preset.pipConfig.name", @"~\AppData\Roaming\pip\pip.ini", SyncItemKind.File, "preset.pipConfig.reason"),
        new("uv-config", "preset.uvConfig.name", @"~\AppData\Roaming\uv\uv.toml", SyncItemKind.File, "preset.uvConfig.reason"),
        new("nuget-config", "preset.nugetConfig.name", @"~\AppData\Roaming\NuGet\NuGet.Config", SyncItemKind.File, "preset.nugetConfig.reason"),
        new("maven-settings", "preset.mavenSettings.name", @"~\.m2\settings.xml", SyncItemKind.File, "preset.mavenSettings.reason"),
        new("gradle-properties", "preset.gradleProperties.name", @"~\.gradle\gradle.properties", SyncItemKind.File, "preset.gradleProperties.reason"),
        new("gradle-init", "preset.gradleInit.name", @"~\.gradle\init.d", SyncItemKind.Directory, "preset.gradleInit.reason"),
        new("cargo-config", "preset.cargoConfig.name", @"~\.cargo\config.toml", SyncItemKind.File, "preset.cargoConfig.reason"),
        new("bunfig", "preset.bunfig.name", @"~\.bunfig.toml", SyncItemKind.File, "preset.bunfig.reason"),
        new("powershell", "preset.powershell.name", @"~\Documents\PowerShell", SyncItemKind.Directory, "preset.powershell.reason"),
        new("windows-powershell", "preset.windowsPowerShell.name", @"~\Documents\WindowsPowerShell", SyncItemKind.Directory, "preset.windowsPowerShell.reason"),
        new("windows-terminal", "preset.windowsTerminal.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminal.reason"),
        new("windows-terminal-preview", "preset.windowsTerminalPreview.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminalPreview_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminalPreview.reason"),
        new("windows-terminal-canary", "preset.windowsTerminalCanary.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminalCanary_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminalCanary.reason"),
        new("windows-terminal-unpackaged", "preset.windowsTerminalUnpackaged.name", @"~\AppData\Local\Microsoft\Windows Terminal", SyncItemKind.Directory, "preset.windowsTerminalUnpackaged.reason"),
        new("neovim-config", "preset.neovimConfig.name", @"~\AppData\Local\nvim", SyncItemKind.Directory, "preset.neovimConfig.reason"),
        new("vimfiles", "preset.vimfiles.name", @"~\vimfiles", SyncItemKind.Directory, "preset.vimfiles.reason"),
        new("vimrc", "preset.vimrc.name", @"~\_vimrc", SyncItemKind.File, "preset.vimrc.reason"),
        new("dot-vimrc", "preset.dotVimrc.name", @"~\.vimrc", SyncItemKind.File, "preset.dotVimrc.reason"),
        new("starship", "preset.starship.name", @"~\.config\starship.toml", SyncItemKind.File, "preset.starship.reason"),
        new("alacritty", "preset.alacritty.name", @"~\AppData\Roaming\alacritty", SyncItemKind.Directory, "preset.alacritty.reason"),
        new("wezterm-lua", "preset.weztermLua.name", @"~\.wezterm.lua", SyncItemKind.File, "preset.weztermLua.reason"),
        new("wezterm-config", "preset.weztermConfig.name", @"~\.config\wezterm", SyncItemKind.Directory, "preset.weztermConfig.reason"),
        new("nushell", "preset.nushell.name", @"~\AppData\Roaming\nushell", SyncItemKind.Directory, "preset.nushell.reason"),
        new("jetbrains", "preset.jetbrains.name", @"~\AppData\Roaming\JetBrains", SyncItemKind.Directory, "preset.jetbrains.reason"),
        new("clash-verge", "preset.clashVerge.name", @"~\AppData\Roaming\io.github.clash-verge-rev.clash-verge-rev", SyncItemKind.Directory, "preset.clashVerge.reason")
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
            if (configuredPaths.Any(configuredPath => PathsOverlap(expanded, configuredPath)))
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

    private static bool PathsOverlap(string first, string second)
    {
        return PathTools.IsSameOrChild(first, second) || PathTools.IsSameOrChild(second, first);
    }
}
