using System.Globalization;

namespace LinkShelf.Services;

public enum AppLanguage
{
    English,
    Chinese
}

public sealed class LocalizationService
{
    private AppLanguage language;

    public LocalizationService()
    {
        language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("zh", StringComparison.OrdinalIgnoreCase)
            ? AppLanguage.Chinese
            : AppLanguage.English;
    }

    public AppLanguage Language
    {
        get => language;
        set => language = value;
    }

    public string T(string key)
    {
        var map = language == AppLanguage.Chinese ? Zh : En;
        return map.TryGetValue(key, out var value) ? value : key;
    }

    public string F(string key, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, T(key), args);
    }

    public string Code(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return "";
        }

        return T("code." + NormalizeCode(code));
    }

    private static string NormalizeCode(string code)
    {
        var normalized = code.StartsWith("code.", StringComparison.OrdinalIgnoreCase)
            ? code[5..]
            : code;

        return normalized switch
        {
            "导入现有缓存" => "complete-existing-link",
            "瀵煎叆鐜版湁缂撳瓨" => "complete-existing-link",
            _ => normalized
        };
    }

    private static readonly Dictionary<string, string> En = new()
    {
        ["app.title"] = "Link Shelf",
        ["app.startupFailed"] = "The application failed to start. A startup error log was written.",
        ["app.cannotLocateSelf"] = "The current executable cannot be located, so administrator elevation cannot be requested automatically.",
        ["app.adminRequired"] = "Administrator permission is required to move paths and create links.",

        ["main.cacheRoot"] = "Cache root: {0}",
        ["main.configFile"] = "Config file: {0}",
        ["main.add"] = "Add item",
        ["main.addRecommended"] = "Add recommended items",
        ["main.addDirectory"] = "Add directory",
        ["main.addFile"] = "Add file",
        ["main.restore"] = "Restore links",
        ["main.check"] = "Check status",
        ["main.revert"] = "Move back / Undo",
        ["main.language"] = "Language",
        ["language.en"] = "English",
        ["language.zh"] = "Chinese",
        ["main.log"] = "Activity log",
        ["help.add"] = "Move a selected file or directory into this cache root, then create a link at its original path.",
        ["help.addRecommended"] = "Choose from detected common configuration and state paths on this computer.",
        ["help.addDirectory"] = "Move a selected directory into this cache root, then link it back to its original path.",
        ["help.addFile"] = "Move a selected file into this cache root, then link it back to its original path.",
        ["help.restore"] = "Create local links from the saved configuration. If rows are selected, only those rows are processed.",
        ["help.check"] = "Check the configuration, cache items, and target links without moving files.",
        ["help.revert"] = "Move selected cache items back to their original paths, remove their links, delete them from the cache, and remove their config records.",

        ["grid.cacheName"] = "Cache name",
        ["grid.originalPath"] = "Original path",
        ["grid.type"] = "Type",
        ["grid.status"] = "Status",
        ["grid.checkResult"] = "Check result",
        ["grid.sourceMachine"] = "Source machine",
        ["grid.lastOperation"] = "Last operation",
        ["grid.updatedAt"] = "Updated at",
        ["grid.cachePath"] = "Cache path",
        ["grid.localTarget"] = "Local target",

        ["dialog.pickDirectory"] = "Choose a directory to move into the cache",
        ["dialog.pickFile"] = "Choose a file to move into the cache",
        ["dialog.noItems"] = "There are no sync items in the configuration.",
        ["dialog.noSelection"] = "Select one or more items first.",
        ["dialog.restoreSelected"] = "Restore {0} selected item(s).",
        ["dialog.restoreAll"] = "Restore all enabled items.",
        ["dialog.restoreConfirmSuffix"] = "Conflicts will be handled one item at a time.",
        ["dialog.revertConfirm"] = "Move {0} selected item(s) back to their original paths and remove their config records? This will delete the corresponding cache entries after they are moved back.",
        ["recommended.title"] = "Recommended items",
        ["recommended.heading"] = "Add recommended items",
        ["recommended.description"] = "Only paths that exist on this computer and are not already in the configuration are shown.",
        ["recommended.select"] = "Add",
        ["recommended.name"] = "Name",
        ["recommended.path"] = "Path",
        ["recommended.type"] = "Type",
        ["recommended.reason"] = "Why",
        ["recommended.addSelected"] = "Add selected",
        ["recommended.noItems"] = "No recommended item is currently available. Existing configured paths and missing local paths are hidden.",
        ["recommended.noSelection"] = "Select one or more recommended items first.",
        ["dialog.lockedPathTitle"] = "Path is in use",
        ["dialog.lockedPathMessage"] = "The add operation failed because this path may be used by another program.{0}{0}Path:{0}{1}{0}{0}Error:{0}{2}{0}{0}Detected programs:{0}{3}{0}{0}Close the listed software, then choose OK to scan again and continue. Choose Cancel to stop this add operation.",
        ["dialog.lockedPathPreflightMessage"] = "This path appears to be used by another program.{0}{0}Path:{0}{1}{0}{0}Detected programs:{0}{2}{0}{0}Close the listed software, then choose OK to scan again and continue. Choose Cancel to stop this add operation.",
        ["dialog.lockedPathNoProcesses"] = "No locking program was detected. The path may still be blocked by permissions, security software, or a running program.",
        ["dialog.lockedPathTimedOut"] = "The lock scan timed out before a complete result was available.",
        ["dialog.accessDeniedTitle"] = "Access denied",
        ["dialog.accessDeniedMessage"] = "Windows denied access while moving this path.{0}{0}Path:{0}{1}{0}{0}Error:{0}{2}{0}{0}This usually means the directory has protected permissions, a service is guarding it, or Windows refuses to move part of its contents. Close related software and services, or move a smaller subdirectory instead.",
        ["lockWindow.title"] = "Resolve file usage",
        ["lockWindow.heading"] = "Move failed. Detecting processes that use this path.",
        ["lockWindow.path"] = "Path: {0}",
        ["lockWindow.error"] = "Windows error: {0}",
        ["lockWindow.scanning"] = "Getting locking information",
        ["lockWindow.noProcesses"] = "No locking process was found.",
        ["lockWindow.foundProcesses"] = "Found {0} locking process(es).",
        ["lockWindow.scanFailed"] = "Failed to get locking information: {0}",
        ["lockWindow.processName"] = "Process",
        ["lockWindow.processId"] = "PID",
        ["lockWindow.user"] = "User",
        ["lockWindow.executable"] = "Executable",
        ["lockWindow.terminateSelected"] = "Terminate selected processes",
        ["lockWindow.refresh"] = "Refresh",
        ["lockWindow.terminateAllContinue"] = "Terminate all and continue",
        ["lockWindow.terminating"] = "Terminating processes",
        ["lockWindow.terminateConfirmTitle"] = "Terminate processes",
        ["lockWindow.terminateConfirm"] = "Terminate {0} selected process(es)? Unsaved work in those programs may be lost.",
        ["lockWindow.terminateFailed"] = "Failed to terminate processes: {0}",

        ["conflict.title"] = "Resolve conflict",
        ["conflict.heading"] = "The target path has a conflict",
        ["conflict.target"] = "Target path: {0}",
        ["conflict.cache"] = "Cache path: {0}",
        ["conflict.useCache"] = "Use the content in this cache",
        ["conflict.useTarget"] = "Use the content at the original path",
        ["conflict.merge"] = "Keep both sides first, then merge what can be merged",
        ["conflict.skip"] = "Back up the original content and skip this item",
        ["conflict.cancel"] = "Cancel",

        ["log.started"] = "Application started.",
        ["log.begin"] = "Begin: {0}",
        ["log.done"] = "Done: {0}",
        ["log.failed"] = "Failed: {0}, {1}",
        ["log.retryLockedPath"] = "Retry after locked path prompt: {0}",
        ["log.lockedPathCanceled"] = "Canceled locked path add operation: {0}",
        ["op.addDirectory"] = "add-directory",
        ["op.addFile"] = "add-file",
        ["op.addRecommendedItem"] = "add-recommended-item: {0}",
        ["op.restoreLinks"] = "restore-links",
        ["op.checkStatus"] = "check-status",
        ["op.revertItems"] = "move-back-undo",

        ["code.directory"] = "Directory",
        ["code.file"] = "File",
        ["code.enabled"] = "Enabled",
        ["code.removed"] = "Removed",
        ["code.symbolic-link"] = "Symbolic link",
        ["code.ok"] = "OK",
        ["code.cache-missing"] = "Cache item missing",
        ["code.target-has-content"] = "Target has real content",
        ["code.target-missing-link"] = "Target link missing",
        ["code.link-wrong-target"] = "Link points elsewhere",
        ["code.skipped"] = "Skipped",
        ["code.restored"] = "Restored",
        ["code.add-sync-item"] = "Add sync item",
        ["code.complete-existing-link"] = "Complete existing link",
        ["code.restore-local-link"] = "Restore local link",
        ["code.revert-item"] = "Move back / undo",
        ["code.cache-already-exists"] = "A cache item with this name already exists.",
        ["code.target-is-another-link"] = "The target path is already a link to another location.",
        ["code.target-has-real-content"] = "The target path already has real content.",

        ["preset.codex.name"] = "Codex",
        ["preset.codex.reason"] = "Codex settings and local state for agent workflows.",
        ["preset.claude.name"] = "Claude",
        ["preset.claude.reason"] = "Claude Code settings and local state.",
        ["preset.claudeJson.name"] = "Claude JSON",
        ["preset.claudeJson.reason"] = "Claude Code user-level configuration file.",
        ["preset.gemini.name"] = "Gemini",
        ["preset.gemini.reason"] = "Gemini CLI settings and local state.",
        ["preset.cursorProfile.name"] = "Cursor profile",
        ["preset.cursorProfile.reason"] = "Cursor profile data under the user folder.",
        ["preset.cursorUser.name"] = "Cursor user settings",
        ["preset.cursorUser.reason"] = "Cursor settings, keybindings, snippets, and user state.",
        ["preset.cursorBackups.name"] = "Cursor backups",
        ["preset.cursorBackups.reason"] = "Cursor workspace backup data.",
        ["preset.cursorWorkspaces.name"] = "Cursor workspaces",
        ["preset.cursorWorkspaces.reason"] = "Cursor workspace metadata.",
        ["preset.cursorDictionaries.name"] = "Cursor dictionaries",
        ["preset.cursorDictionaries.reason"] = "Cursor custom dictionaries.",
        ["preset.vscodeProfile.name"] = "VS Code profile",
        ["preset.vscodeProfile.reason"] = "VS Code extensions and profile data under the user folder.",
        ["preset.vscodeUser.name"] = "VS Code user settings",
        ["preset.vscodeUser.reason"] = "VS Code settings, keybindings, snippets, and user state.",
        ["preset.gitconfig.name"] = "Git config",
        ["preset.gitconfig.reason"] = "User-level Git configuration.",
        ["preset.npmrc.name"] = "npm config",
        ["preset.npmrc.reason"] = "User-level npm configuration.",
        ["preset.yarnrc.name"] = "Yarn config",
        ["preset.yarnrc.reason"] = "User-level Yarn configuration.",
        ["preset.mcpJson.name"] = "MCP config",
        ["preset.mcpJson.reason"] = "User-level MCP server configuration.",
        ["preset.powershell.name"] = "PowerShell profile",
        ["preset.powershell.reason"] = "PowerShell profile scripts and module configuration.",
        ["preset.windowsPowerShell.name"] = "Windows PowerShell profile",
        ["preset.windowsPowerShell.reason"] = "Windows PowerShell profile scripts and module configuration.",
        ["preset.windowsTerminal.name"] = "Windows Terminal",
        ["preset.windowsTerminal.reason"] = "Windows Terminal settings and local state.",
        ["preset.jetbrains.name"] = "JetBrains",
        ["preset.jetbrains.reason"] = "JetBrains IDE settings, plugins, and application state.",
        ["preset.clashVerge.name"] = "Clash Verge",
        ["preset.clashVerge.reason"] = "Clash Verge profiles and application configuration.",
        ["preset.clashWin.name"] = "Clash for Windows",
        ["preset.clashWin.reason"] = "Clash for Windows profiles and application configuration.",
        ["preset.ditto.name"] = "Ditto",
        ["preset.ditto.reason"] = "Ditto clipboard manager settings and database.",
        ["preset.wiz.name"] = "Wiz",
        ["preset.wiz.reason"] = "Wiz note application settings and local state.",
        ["preset.lxMusic.name"] = "LX Music",
        ["preset.lxMusic.reason"] = "LX Music desktop settings and local state."
    };

    private static readonly Dictionary<string, string> Zh = new()
    {
        ["app.title"] = "链接缓存架",
        ["app.startupFailed"] = "程序启动失败，已写入启动错误日志。",
        ["app.cannotLocateSelf"] = "无法定位当前程序，不能自动请求管理员权限。",
        ["app.adminRequired"] = "需要管理员权限才能移动目录和创建链接。",

        ["main.cacheRoot"] = "缓存根目录：{0}",
        ["main.configFile"] = "配置文件：{0}",
        ["main.add"] = "添加项目",
        ["main.addRecommended"] = "添加推荐项目",
        ["main.addDirectory"] = "添加目录",
        ["main.addFile"] = "添加文件",
        ["main.restore"] = "恢复链接",
        ["main.check"] = "检查状态",
        ["main.revert"] = "搬回原位/撤销",
        ["main.language"] = "语言",
        ["language.en"] = "英语",
        ["language.zh"] = "中文",
        ["main.log"] = "操作日志",
        ["help.add"] = "把选择的文件或目录移动到当前缓存根目录，并在原位置创建链接。",
        ["help.addRecommended"] = "从本机已检测到的常见配置和状态路径中选择要加入的项目。",
        ["help.addDirectory"] = "把选择的目录移动到当前缓存根目录，并在原位置创建链接。",
        ["help.addFile"] = "把选择的文件移动到当前缓存根目录，并在原位置创建链接。",
        ["help.restore"] = "根据配置创建本机链接。选中多行时，只处理选中的项目。",
        ["help.check"] = "只检查配置、缓存项目和目标链接是否匹配，不移动文件。",
        ["help.revert"] = "把选中的缓存项目搬回原始位置，移除原路径链接，从缓存中删除对应项目，并删除配置记录。",

        ["grid.cacheName"] = "缓存名称",
        ["grid.originalPath"] = "原始位置",
        ["grid.type"] = "类型",
        ["grid.status"] = "状态",
        ["grid.checkResult"] = "检查结果",
        ["grid.sourceMachine"] = "来源机器",
        ["grid.lastOperation"] = "最近操作",
        ["grid.updatedAt"] = "更新时间",
        ["grid.cachePath"] = "缓存路径",
        ["grid.localTarget"] = "本机目标",

        ["dialog.pickDirectory"] = "选择要移入缓存的目录",
        ["dialog.pickFile"] = "选择要移入缓存的文件",
        ["dialog.noItems"] = "配置文件中还没有同步项目。",
        ["dialog.noSelection"] = "请先选择一个或多个项目。",
        ["dialog.restoreSelected"] = "将恢复选中的 {0} 项。",
        ["dialog.restoreAll"] = "将恢复全部启用项。",
        ["dialog.restoreConfirmSuffix"] = "遇到冲突时会逐项询问。",
        ["dialog.revertConfirm"] = "确定将选中的 {0} 项搬回原始位置并删除配置记录吗？搬回后，对应缓存项会从缓存目录中消失。",
        ["recommended.title"] = "推荐项目",
        ["recommended.heading"] = "添加推荐项目",
        ["recommended.description"] = "这里只显示本机存在、并且还没有写入配置文件的常见配置和状态路径。",
        ["recommended.select"] = "添加",
        ["recommended.name"] = "名称",
        ["recommended.path"] = "路径",
        ["recommended.type"] = "类型",
        ["recommended.reason"] = "推荐理由",
        ["recommended.addSelected"] = "添加选中项",
        ["recommended.noItems"] = "当前没有可添加的推荐项目。已配置的路径和本机不存在的路径会被自动隐藏。",
        ["recommended.noSelection"] = "请先选择一个或多个推荐项目。",
        ["dialog.lockedPathTitle"] = "路径被占用",
        ["dialog.lockedPathMessage"] = "添加操作失败，这个路径可能正在被其他程序占用。{0}{0}路径：{0}{1}{0}{0}错误：{0}{2}{0}{0}检测到的程序：{0}{3}{0}{0}请先关闭上方列出的软件，然后点击“确定”重新扫描并继续。点击“取消”会停止本次添加。",
        ["dialog.lockedPathPreflightMessage"] = "这个路径看起来正在被其他程序占用。{0}{0}路径：{0}{1}{0}{0}检测到的程序：{0}{2}{0}{0}请先关闭上方列出的软件，然后点击“确定”重新扫描并继续。点击“取消”会停止本次添加。",
        ["dialog.lockedPathNoProcesses"] = "没有检测到明确的占用程序。这个路径仍可能被权限、安全软件或正在运行的程序阻止。",
        ["dialog.lockedPathTimedOut"] = "占用扫描超时，尚未得到完整结果。",
        ["dialog.accessDeniedTitle"] = "访问被拒绝",
        ["dialog.accessDeniedMessage"] = "Windows 在移动这个路径时拒绝访问。{0}{0}路径：{0}{1}{0}{0}错误：{0}{2}{0}{0}这通常表示目录有受保护权限、相关服务正在保护它，或 Windows 拒绝移动其中的某些内容。请关闭相关软件和服务，或者改为移动更小的子目录。",
        ["lockWindow.title"] = "处理文件占用",
        ["lockWindow.heading"] = "移动失败，正在检测占用这个路径的进程。",
        ["lockWindow.path"] = "路径：{0}",
        ["lockWindow.error"] = "Windows 错误：{0}",
        ["lockWindow.scanning"] = "正在获取占用信息",
        ["lockWindow.noProcesses"] = "没有找到占用进程。",
        ["lockWindow.foundProcesses"] = "找到 {0} 个占用进程。",
        ["lockWindow.scanFailed"] = "获取占用信息失败：{0}",
        ["lockWindow.processName"] = "进程",
        ["lockWindow.processId"] = "进程号",
        ["lockWindow.user"] = "用户",
        ["lockWindow.executable"] = "程序路径",
        ["lockWindow.terminateSelected"] = "结束选中进程",
        ["lockWindow.refresh"] = "重新检测",
        ["lockWindow.terminateAllContinue"] = "结束全部并继续",
        ["lockWindow.terminating"] = "正在结束进程",
        ["lockWindow.terminateConfirmTitle"] = "结束进程",
        ["lockWindow.terminateConfirm"] = "确定结束 {0} 个进程吗？这些程序中未保存的内容可能会丢失。",
        ["lockWindow.terminateFailed"] = "结束进程失败：{0}",

        ["conflict.title"] = "处理冲突",
        ["conflict.heading"] = "目标位置存在冲突",
        ["conflict.target"] = "目标位置：{0}",
        ["conflict.cache"] = "缓存位置：{0}",
        ["conflict.useCache"] = "使用缓存目录里的内容",
        ["conflict.useTarget"] = "使用原位置里的内容",
        ["conflict.merge"] = "先保留两边内容，再尽量合并",
        ["conflict.skip"] = "先备份原位置内容，这次不处理",
        ["conflict.cancel"] = "取消",

        ["log.started"] = "程序已启动。",
        ["log.begin"] = "开始：{0}",
        ["log.done"] = "完成：{0}",
        ["log.failed"] = "失败：{0}，{1}",
        ["log.retryLockedPath"] = "用户处理占用后重试：{0}",
        ["log.lockedPathCanceled"] = "已取消被占用路径的添加操作：{0}",
        ["op.addDirectory"] = "添加目录",
        ["op.addFile"] = "添加文件",
        ["op.addRecommendedItem"] = "添加推荐项目：{0}",
        ["op.restoreLinks"] = "恢复链接",
        ["op.checkStatus"] = "检查状态",
        ["op.revertItems"] = "搬回原位/撤销",

        ["code.directory"] = "目录",
        ["code.file"] = "文件",
        ["code.enabled"] = "启用",
        ["code.removed"] = "已移除",
        ["code.symbolic-link"] = "符号链接",
        ["code.ok"] = "链接正常",
        ["code.cache-missing"] = "缓存项不存在",
        ["code.target-has-content"] = "目标被真实内容占用",
        ["code.target-missing-link"] = "目标缺少链接",
        ["code.link-wrong-target"] = "链接指向其他位置",
        ["code.skipped"] = "已跳过",
        ["code.restored"] = "已恢复",
        ["code.add-sync-item"] = "添加同步项",
        ["code.complete-existing-link"] = "补全配置",
        ["code.restore-local-link"] = "恢复本机链接",
        ["code.revert-item"] = "搬回原位/撤销",
        ["code.cache-already-exists"] = "缓存目录中已经存在同名项目。",
        ["code.target-is-another-link"] = "目标位置已经是指向其他位置的链接。",
        ["code.target-has-real-content"] = "目标位置已经有真实内容。",

        ["preset.codex.name"] = "Codex",
        ["preset.codex.reason"] = "Codex 的设置和代理工作流本地状态。",
        ["preset.claude.name"] = "Claude",
        ["preset.claude.reason"] = "Claude Code 的设置和本地状态。",
        ["preset.claudeJson.name"] = "Claude JSON",
        ["preset.claudeJson.reason"] = "Claude Code 的用户级配置文件。",
        ["preset.gemini.name"] = "Gemini",
        ["preset.gemini.reason"] = "Gemini CLI 的设置和本地状态。",
        ["preset.cursorProfile.name"] = "Cursor 配置目录",
        ["preset.cursorProfile.reason"] = "用户目录下的 Cursor 配置和状态数据。",
        ["preset.cursorUser.name"] = "Cursor 用户设置",
        ["preset.cursorUser.reason"] = "Cursor 的设置、快捷键、代码片段和用户状态。",
        ["preset.cursorBackups.name"] = "Cursor 备份",
        ["preset.cursorBackups.reason"] = "Cursor 的工作区备份数据。",
        ["preset.cursorWorkspaces.name"] = "Cursor 工作区",
        ["preset.cursorWorkspaces.reason"] = "Cursor 的工作区元数据。",
        ["preset.cursorDictionaries.name"] = "Cursor 词典",
        ["preset.cursorDictionaries.reason"] = "Cursor 的自定义词典。",
        ["preset.vscodeProfile.name"] = "VS Code 配置目录",
        ["preset.vscodeProfile.reason"] = "用户目录下的 VS Code 扩展和配置数据。",
        ["preset.vscodeUser.name"] = "VS Code 用户设置",
        ["preset.vscodeUser.reason"] = "VS Code 的设置、快捷键、代码片段和用户状态。",
        ["preset.gitconfig.name"] = "Git 配置",
        ["preset.gitconfig.reason"] = "用户级 Git 配置。",
        ["preset.npmrc.name"] = "npm 配置",
        ["preset.npmrc.reason"] = "用户级 npm 配置。",
        ["preset.yarnrc.name"] = "Yarn 配置",
        ["preset.yarnrc.reason"] = "用户级 Yarn 配置。",
        ["preset.mcpJson.name"] = "MCP 配置",
        ["preset.mcpJson.reason"] = "用户级 MCP 服务器配置。",
        ["preset.powershell.name"] = "PowerShell 配置",
        ["preset.powershell.reason"] = "PowerShell 配置脚本和模块配置。",
        ["preset.windowsPowerShell.name"] = "Windows PowerShell 配置",
        ["preset.windowsPowerShell.reason"] = "Windows PowerShell 配置脚本和模块配置。",
        ["preset.windowsTerminal.name"] = "Windows Terminal",
        ["preset.windowsTerminal.reason"] = "Windows Terminal 的设置和本地状态。",
        ["preset.jetbrains.name"] = "JetBrains",
        ["preset.jetbrains.reason"] = "JetBrains IDE 的设置、插件和应用状态。",
        ["preset.clashVerge.name"] = "Clash Verge",
        ["preset.clashVerge.reason"] = "Clash Verge 的订阅、配置和应用状态。",
        ["preset.clashWin.name"] = "Clash for Windows",
        ["preset.clashWin.reason"] = "Clash for Windows 的订阅、配置和应用状态。",
        ["preset.ditto.name"] = "Ditto",
        ["preset.ditto.reason"] = "Ditto 剪贴板管理器的设置和数据库。",
        ["preset.wiz.name"] = "Wiz",
        ["preset.wiz.reason"] = "Wiz 笔记应用的设置和本地状态。",
        ["preset.lxMusic.name"] = "LX Music",
        ["preset.lxMusic.reason"] = "LX Music 桌面版的设置和本地状态。"
    };
}
