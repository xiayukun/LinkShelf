using LinkShelf.Models;

namespace LinkShelf.Services;

public static class RecommendedSyncItems
{
    [Flags]
    private enum PresetPlatforms
    {
        Windows = 1,
        MacOS = 2,
        Linux = 4
    }

    private sealed record Preset(string Id, string NameKey, string PortablePath, SyncItemKind Kind, string ReasonKey, PresetPlatforms Platforms);

    private static Preset Windows(string id, string nameKey, string portablePath, SyncItemKind kind, string reasonKey)
    {
        return new Preset(id, nameKey, portablePath, kind, reasonKey, PresetPlatforms.Windows);
    }

    private static Preset MacOS(string id, string nameKey, string portablePath, SyncItemKind kind, string reasonKey)
    {
        return new Preset(id, nameKey, portablePath, kind, reasonKey, PresetPlatforms.MacOS);
    }

    private static readonly Preset[] Presets =
    [
        Windows("codex", "preset.codex.name", @"~\.codex", SyncItemKind.Directory, "preset.codex.reason"),
        Windows("claude", "preset.claude.name", @"~\.claude", SyncItemKind.Directory, "preset.claude.reason"),
        Windows("claude-json", "preset.claudeJson.name", @"~\.claude.json", SyncItemKind.File, "preset.claudeJson.reason"),
        Windows("claude-desktop", "preset.claudeDesktop.name", @"~\AppData\Roaming\Claude", SyncItemKind.Directory, "preset.claudeDesktop.reason"),
        Windows("claude-desktop-msix", "preset.claudeDesktopMsix.name", @"~\AppData\Local\Packages\Claude_pzs8sxrjxfjjc\LocalCache\Roaming\Claude", SyncItemKind.Directory, "preset.claudeDesktopMsix.reason"),
        Windows("gemini", "preset.gemini.name", @"~\.gemini", SyncItemKind.Directory, "preset.gemini.reason"),
        Windows("continue", "preset.continue.name", @"~\.continue", SyncItemKind.Directory, "preset.continue.reason"),
        Windows("aider-config", "preset.aiderConfig.name", @"~\.aider.conf.yml", SyncItemKind.File, "preset.aiderConfig.reason"),
        Windows("windsurf-mcp", "preset.windsurfMcp.name", @"~\.codeium\windsurf", SyncItemKind.Directory, "preset.windsurfMcp.reason"),
        Windows("windsurf-user", "preset.windsurfUser.name", @"~\AppData\Roaming\Windsurf\User", SyncItemKind.Directory, "preset.windsurfUser.reason"),
        Windows("cursor-profile", "preset.cursorProfile.name", @"~\.cursor", SyncItemKind.Directory, "preset.cursorProfile.reason"),
        Windows("cursor-user", "preset.cursorUser.name", @"~\AppData\Roaming\Cursor\User", SyncItemKind.Directory, "preset.cursorUser.reason"),
        Windows("cursor-backups", "preset.cursorBackups.name", @"~\AppData\Roaming\Cursor\Backups", SyncItemKind.Directory, "preset.cursorBackups.reason"),
        Windows("cursor-workspaces", "preset.cursorWorkspaces.name", @"~\AppData\Roaming\Cursor\Workspaces", SyncItemKind.Directory, "preset.cursorWorkspaces.reason"),
        Windows("cursor-dictionaries", "preset.cursorDictionaries.name", @"~\AppData\Roaming\Cursor\Dictionaries", SyncItemKind.Directory, "preset.cursorDictionaries.reason"),
        Windows("cline-cursor", "preset.clineCursor.name", @"~\AppData\Roaming\Cursor\User\globalStorage\saoudrizwan.claude-dev", SyncItemKind.Directory, "preset.clineCursor.reason"),
        Windows("roo-cursor", "preset.rooCursor.name", @"~\AppData\Roaming\Cursor\User\globalStorage\rooveterinaryinc.roo-cline", SyncItemKind.Directory, "preset.rooCursor.reason"),
        Windows("vscode-profile", "preset.vscodeProfile.name", @"~\.vscode", SyncItemKind.Directory, "preset.vscodeProfile.reason"),
        Windows("vscode-user", "preset.vscodeUser.name", @"~\AppData\Roaming\Code\User", SyncItemKind.Directory, "preset.vscodeUser.reason"),
        Windows("vscode-insiders-profile", "preset.vscodeInsidersProfile.name", @"~\.vscode-insiders", SyncItemKind.Directory, "preset.vscodeInsidersProfile.reason"),
        Windows("vscode-insiders-user", "preset.vscodeInsidersUser.name", @"~\AppData\Roaming\Code - Insiders\User", SyncItemKind.Directory, "preset.vscodeInsidersUser.reason"),
        Windows("vscodium-profile", "preset.vscodiumProfile.name", @"~\.vscode-oss", SyncItemKind.Directory, "preset.vscodiumProfile.reason"),
        Windows("vscodium-user", "preset.vscodiumUser.name", @"~\AppData\Roaming\VSCodium\User", SyncItemKind.Directory, "preset.vscodiumUser.reason"),
        Windows("cline-vscode", "preset.clineVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\saoudrizwan.claude-dev", SyncItemKind.Directory, "preset.clineVsCode.reason"),
        Windows("roo-vscode", "preset.rooVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\rooveterinaryinc.roo-cline", SyncItemKind.Directory, "preset.rooVsCode.reason"),
        Windows("copilot-vscode", "preset.copilotVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\github.copilot", SyncItemKind.Directory, "preset.copilotVsCode.reason"),
        Windows("copilot-chat-vscode", "preset.copilotChatVsCode.name", @"~\AppData\Roaming\Code\User\globalStorage\github.copilot-chat", SyncItemKind.Directory, "preset.copilotChatVsCode.reason"),
        Windows("gitconfig", "preset.gitconfig.name", @"~\.gitconfig", SyncItemKind.File, "preset.gitconfig.reason"),
        Windows("gitignore-global", "preset.gitignoreGlobal.name", @"~\.gitignore_global", SyncItemKind.File, "preset.gitignoreGlobal.reason"),
        Windows("git-config-dir", "preset.gitConfigDir.name", @"~\.config\git", SyncItemKind.Directory, "preset.gitConfigDir.reason"),
        Windows("npmrc", "preset.npmrc.name", @"~\.npmrc", SyncItemKind.File, "preset.npmrc.reason"),
        Windows("yarnrc", "preset.yarnrc.name", @"~\.yarnrc", SyncItemKind.File, "preset.yarnrc.reason"),
        Windows("yarnrc-yml", "preset.yarnrcYml.name", @"~\.yarnrc.yml", SyncItemKind.File, "preset.yarnrcYml.reason"),
        Windows("pnpmrc", "preset.pnpmrc.name", @"~\.pnpmrc", SyncItemKind.File, "preset.pnpmrc.reason"),
        Windows("mcp-json", "preset.mcpJson.name", @"~\.mcp.json", SyncItemKind.File, "preset.mcpJson.reason"),
        Windows("pip-config", "preset.pipConfig.name", @"~\AppData\Roaming\pip\pip.ini", SyncItemKind.File, "preset.pipConfig.reason"),
        Windows("uv-config", "preset.uvConfig.name", @"~\AppData\Roaming\uv\uv.toml", SyncItemKind.File, "preset.uvConfig.reason"),
        Windows("nuget-config", "preset.nugetConfig.name", @"~\AppData\Roaming\NuGet\NuGet.Config", SyncItemKind.File, "preset.nugetConfig.reason"),
        Windows("maven-settings", "preset.mavenSettings.name", @"~\.m2\settings.xml", SyncItemKind.File, "preset.mavenSettings.reason"),
        Windows("gradle-properties", "preset.gradleProperties.name", @"~\.gradle\gradle.properties", SyncItemKind.File, "preset.gradleProperties.reason"),
        Windows("gradle-init", "preset.gradleInit.name", @"~\.gradle\init.d", SyncItemKind.Directory, "preset.gradleInit.reason"),
        Windows("cargo-config", "preset.cargoConfig.name", @"~\.cargo\config.toml", SyncItemKind.File, "preset.cargoConfig.reason"),
        Windows("bunfig", "preset.bunfig.name", @"~\.bunfig.toml", SyncItemKind.File, "preset.bunfig.reason"),
        Windows("powershell", "preset.powershell.name", @"~\Documents\PowerShell", SyncItemKind.Directory, "preset.powershell.reason"),
        Windows("windows-powershell", "preset.windowsPowerShell.name", @"~\Documents\WindowsPowerShell", SyncItemKind.Directory, "preset.windowsPowerShell.reason"),
        Windows("windows-terminal", "preset.windowsTerminal.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminal.reason"),
        Windows("windows-terminal-preview", "preset.windowsTerminalPreview.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminalPreview_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminalPreview.reason"),
        Windows("windows-terminal-canary", "preset.windowsTerminalCanary.name", @"~\AppData\Local\Packages\Microsoft.WindowsTerminalCanary_8wekyb3d8bbwe\LocalState", SyncItemKind.Directory, "preset.windowsTerminalCanary.reason"),
        Windows("windows-terminal-unpackaged", "preset.windowsTerminalUnpackaged.name", @"~\AppData\Local\Microsoft\Windows Terminal", SyncItemKind.Directory, "preset.windowsTerminalUnpackaged.reason"),
        Windows("neovim-config", "preset.neovimConfig.name", @"~\AppData\Local\nvim", SyncItemKind.Directory, "preset.neovimConfig.reason"),
        Windows("vimfiles", "preset.vimfiles.name", @"~\vimfiles", SyncItemKind.Directory, "preset.vimfiles.reason"),
        Windows("vimrc", "preset.vimrc.name", @"~\_vimrc", SyncItemKind.File, "preset.vimrc.reason"),
        Windows("dot-vimrc", "preset.dotVimrc.name", @"~\.vimrc", SyncItemKind.File, "preset.dotVimrc.reason"),
        Windows("starship", "preset.starship.name", @"~\.config\starship.toml", SyncItemKind.File, "preset.starship.reason"),
        Windows("alacritty", "preset.alacritty.name", @"~\AppData\Roaming\alacritty", SyncItemKind.Directory, "preset.alacritty.reason"),
        Windows("wezterm-lua", "preset.weztermLua.name", @"~\.wezterm.lua", SyncItemKind.File, "preset.weztermLua.reason"),
        Windows("wezterm-config", "preset.weztermConfig.name", @"~\.config\wezterm", SyncItemKind.Directory, "preset.weztermConfig.reason"),
        Windows("nushell", "preset.nushell.name", @"~\AppData\Roaming\nushell", SyncItemKind.Directory, "preset.nushell.reason"),
        Windows("jetbrains", "preset.jetbrains.name", @"~\AppData\Roaming\JetBrains", SyncItemKind.Directory, "preset.jetbrains.reason"),
        Windows("clash-verge", "preset.clashVerge.name", @"~\AppData\Roaming\io.github.clash-verge-rev.clash-verge-rev", SyncItemKind.Directory, "preset.clashVerge.reason"),
        MacOS("macos-codex", "preset.codex.name", "~/.codex", SyncItemKind.Directory, "preset.codex.reason"),
        MacOS("macos-claude", "preset.claude.name", "~/.claude", SyncItemKind.Directory, "preset.claude.reason"),
        MacOS("macos-claude-json", "preset.claudeJson.name", "~/.claude.json", SyncItemKind.File, "preset.claudeJson.reason"),
        MacOS("macos-gemini", "preset.gemini.name", "~/.gemini", SyncItemKind.Directory, "preset.gemini.reason"),
        MacOS("macos-continue", "preset.continue.name", "~/.continue", SyncItemKind.Directory, "preset.continue.reason"),
        MacOS("macos-aider-config", "preset.aiderConfig.name", "~/.aider.conf.yml", SyncItemKind.File, "preset.aiderConfig.reason"),
        MacOS("macos-cursor-profile", "preset.cursorProfile.name", "~/.cursor", SyncItemKind.Directory, "preset.cursorProfile.reason"),
        MacOS("macos-cursor-user", "preset.cursorUser.name", "~/Library/Application Support/Cursor/User", SyncItemKind.Directory, "preset.cursorUser.reason"),
        MacOS("macos-vscode-profile", "preset.vscodeProfile.name", "~/.vscode", SyncItemKind.Directory, "preset.vscodeProfile.reason"),
        MacOS("macos-vscode-user", "preset.vscodeUser.name", "~/Library/Application Support/Code/User", SyncItemKind.Directory, "preset.vscodeUser.reason"),
        MacOS("macos-gitconfig", "preset.gitconfig.name", "~/.gitconfig", SyncItemKind.File, "preset.gitconfig.reason"),
        MacOS("macos-gitignore-global", "preset.gitignoreGlobal.name", "~/.gitignore_global", SyncItemKind.File, "preset.gitignoreGlobal.reason"),
        MacOS("macos-git-config-dir", "preset.gitConfigDir.name", "~/.config/git", SyncItemKind.Directory, "preset.gitConfigDir.reason"),
        MacOS("macos-npmrc", "preset.npmrc.name", "~/.npmrc", SyncItemKind.File, "preset.npmrc.reason"),
        MacOS("macos-yarnrc", "preset.yarnrc.name", "~/.yarnrc", SyncItemKind.File, "preset.yarnrc.reason"),
        MacOS("macos-yarnrc-yml", "preset.yarnrcYml.name", "~/.yarnrc.yml", SyncItemKind.File, "preset.yarnrcYml.reason"),
        MacOS("macos-pnpmrc", "preset.pnpmrc.name", "~/.pnpmrc", SyncItemKind.File, "preset.pnpmrc.reason"),
        MacOS("macos-mcp-json", "preset.mcpJson.name", "~/.mcp.json", SyncItemKind.File, "preset.mcpJson.reason"),
        MacOS("macos-pip-config", "preset.pipConfig.name", "~/.config/pip/pip.conf", SyncItemKind.File, "preset.pipConfig.reason"),
        MacOS("macos-uv-config", "preset.uvConfig.name", "~/.config/uv/uv.toml", SyncItemKind.File, "preset.uvConfig.reason"),
        MacOS("macos-maven-settings", "preset.mavenSettings.name", "~/.m2/settings.xml", SyncItemKind.File, "preset.mavenSettings.reason"),
        MacOS("macos-gradle-properties", "preset.gradleProperties.name", "~/.gradle/gradle.properties", SyncItemKind.File, "preset.gradleProperties.reason"),
        MacOS("macos-gradle-init", "preset.gradleInit.name", "~/.gradle/init.d", SyncItemKind.Directory, "preset.gradleInit.reason"),
        MacOS("macos-cargo-config", "preset.cargoConfig.name", "~/.cargo/config.toml", SyncItemKind.File, "preset.cargoConfig.reason"),
        MacOS("macos-bunfig", "preset.bunfig.name", "~/.bunfig.toml", SyncItemKind.File, "preset.bunfig.reason"),
        MacOS("macos-powershell", "preset.powershell.name", "~/.config/powershell", SyncItemKind.Directory, "preset.powershell.reason"),
        MacOS("macos-neovim-config", "preset.neovimConfig.name", "~/.config/nvim", SyncItemKind.Directory, "preset.neovimConfig.reason"),
        MacOS("macos-vimfiles", "preset.vimfiles.name", "~/.vim", SyncItemKind.Directory, "preset.vimfiles.reason"),
        MacOS("macos-dot-vimrc", "preset.dotVimrc.name", "~/.vimrc", SyncItemKind.File, "preset.dotVimrc.reason"),
        MacOS("macos-starship", "preset.starship.name", "~/.config/starship.toml", SyncItemKind.File, "preset.starship.reason"),
        MacOS("macos-alacritty", "preset.alacritty.name", "~/.config/alacritty", SyncItemKind.Directory, "preset.alacritty.reason"),
        MacOS("macos-wezterm-lua", "preset.weztermLua.name", "~/.wezterm.lua", SyncItemKind.File, "preset.weztermLua.reason"),
        MacOS("macos-wezterm-config", "preset.weztermConfig.name", "~/.config/wezterm", SyncItemKind.Directory, "preset.weztermConfig.reason"),
        MacOS("macos-nushell", "preset.nushell.name", "~/Library/Application Support/nushell", SyncItemKind.Directory, "preset.nushell.reason"),
        MacOS("macos-jetbrains", "preset.jetbrains.name", "~/Library/Application Support/JetBrains", SyncItemKind.Directory, "preset.jetbrains.reason")
    ];

    public static IReadOnlyList<RecommendedSyncItem> GetAvailable(AppPaths paths, SyncConfig config)
    {
        return GetAvailable(paths, config, CurrentPlatform());
    }

    public static IReadOnlyList<RecommendedSyncItem> GetAvailable(AppPaths paths, SyncConfig config, RecommendedPlatform platform)
    {
        var configuredPaths = config.Items
            .Where(item => item.Status == SyncConstants.StatusEnabled)
            .Select(item => PathTools.Normalize(PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome)))
            .ToHashSet(PathTools.PathStringComparer);

        var result = new List<RecommendedSyncItem>();
        foreach (var preset in Presets)
        {
            if (!SupportsPlatform(preset, platform))
            {
                continue;
            }

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
                ReasonKey = preset.ReasonKey,
                Platform = platform
            });
        }

        return result;
    }

    private static RecommendedPlatform CurrentPlatform()
    {
        if (OperatingSystem.IsMacOS())
        {
            return RecommendedPlatform.MacOS;
        }

        if (OperatingSystem.IsLinux())
        {
            return RecommendedPlatform.Linux;
        }

        return RecommendedPlatform.Windows;
    }

    private static bool SupportsPlatform(Preset preset, RecommendedPlatform platform)
    {
        var flag = platform switch
        {
            RecommendedPlatform.MacOS => PresetPlatforms.MacOS,
            RecommendedPlatform.Linux => PresetPlatforms.Linux,
            _ => PresetPlatforms.Windows
        };
        return preset.Platforms.HasFlag(flag);
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
