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
        ["main.addDirectory"] = "Add directory",
        ["main.addFile"] = "Add file",
        ["main.restore"] = "Restore links",
        ["main.check"] = "Check status",
        ["main.unsync"] = "Remove",
        ["main.language"] = "Language",
        ["language.en"] = "English",
        ["language.zh"] = "Chinese",
        ["main.log"] = "Activity log",
        ["help.add"] = "Move a selected file or directory into this cache root, then create a link at its original path.",
        ["help.addDirectory"] = "Move a selected directory into this cache root, then link it back to its original path.",
        ["help.addFile"] = "Move a selected file into this cache root, then link it back to its original path.",
        ["help.restore"] = "Create local links from the saved configuration. If rows are selected, only those rows are processed.",
        ["help.check"] = "Check the configuration, cache items, and target links without moving files.",
        ["help.unsync"] = "Remove selected records from the configuration. Files and existing links are not moved.",

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
        ["dialog.unsyncConfirm"] = "Remove {0} selected item(s) from the configuration. Files and existing links will not be moved.",

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
        ["op.addDirectory"] = "add-directory",
        ["op.addFile"] = "add-file",
        ["op.restoreLinks"] = "restore-links",
        ["op.checkStatus"] = "check-status",
        ["op.removeRecords"] = "remove-records",

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
        ["code.remove-record"] = "Remove record",
        ["code.cache-already-exists"] = "A cache item with this name already exists.",
        ["code.target-is-another-link"] = "The target path is already a link to another location.",
        ["code.target-has-real-content"] = "The target path already has real content."
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
        ["main.addDirectory"] = "添加目录",
        ["main.addFile"] = "添加文件",
        ["main.restore"] = "恢复链接",
        ["main.check"] = "检查状态",
        ["main.unsync"] = "移除记录",
        ["main.language"] = "语言",
        ["language.en"] = "英语",
        ["language.zh"] = "中文",
        ["main.log"] = "操作日志",
        ["help.add"] = "把选择的文件或目录移动到当前缓存根目录，并在原位置创建链接。",
        ["help.addDirectory"] = "把选择的目录移动到当前缓存根目录，并在原位置创建链接。",
        ["help.addFile"] = "把选择的文件移动到当前缓存根目录，并在原位置创建链接。",
        ["help.restore"] = "根据配置创建本机链接。选中多行时，只处理选中的项目。",
        ["help.check"] = "只检查配置、缓存项目和目标链接是否匹配，不移动文件。",
        ["help.unsync"] = "从配置中删除选中的记录，不移动文件，也不删除已有链接。",

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
        ["dialog.unsyncConfirm"] = "将从配置中删除选中的 {0} 项。不移动文件，也不删除已有链接。",

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
        ["op.addDirectory"] = "添加目录",
        ["op.addFile"] = "添加文件",
        ["op.restoreLinks"] = "恢复链接",
        ["op.checkStatus"] = "检查状态",
        ["op.removeRecords"] = "移除记录",

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
        ["code.remove-record"] = "删除配置记录",
        ["code.cache-already-exists"] = "缓存目录中已经存在同名项目。",
        ["code.target-is-another-link"] = "目标位置已经是指向其他位置的链接。",
        ["code.target-has-real-content"] = "目标位置已经有真实内容。"
    };
}
