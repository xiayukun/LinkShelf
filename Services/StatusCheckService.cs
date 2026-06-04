using LinkShelf.Models;

namespace LinkShelf.Services;

public sealed class StatusCheckService
{
    private readonly AppPaths paths;
    private readonly SyncConfig config;

    public StatusCheckService(AppPaths paths, SyncConfig config)
    {
        this.paths = paths;
        this.config = config;
    }

    public List<SyncItem> CheckAll()
    {
        config.Items.RemoveAll(x => x.Status == SyncConstants.StatusRemoved);

        foreach (var item in config.Items)
        {
            CheckItem(item);
        }

        return config.Items;
    }

    public void CheckItem(SyncItem item)
    {
        var cachePath = Path.Combine(paths.CacheRoot, item.CacheName);
        var targetPath = PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome);
        item.CachePath = cachePath;
        item.ExpandedOriginalPath = targetPath;

        if (!PathExists(cachePath))
        {
            item.CheckResult = SyncConstants.CheckCacheMissing;
            return;
        }

        var linkTarget = TryGetLinkTarget(targetPath);
        if (linkTarget is null)
        {
            item.CheckResult = PathExists(targetPath)
                ? SyncConstants.CheckTargetHasContent
                : SyncConstants.CheckTargetMissingLink;
            return;
        }

        item.CheckResult = PathTools.IsSamePath(linkTarget, cachePath)
            ? SyncConstants.CheckOk
            : SyncConstants.CheckLinkWrongTarget;
    }

    public static bool IsHealthy(SyncItem item)
    {
        return item.CheckResult == SyncConstants.CheckOk;
    }

    private static bool PathExists(string path)
    {
        try
        {
            _ = File.GetAttributes(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string? TryGetLinkTarget(string path)
    {
        try
        {
            var attributes = File.GetAttributes(path);
            if (!attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                return null;
            }

            FileSystemInfo info = attributes.HasFlag(FileAttributes.Directory)
                ? new DirectoryInfo(path)
                : new FileInfo(path);
            var target = info.ResolveLinkTarget(returnFinalTarget: true)?.FullName ?? info.LinkTarget;
            return string.IsNullOrWhiteSpace(target) ? null : PathTools.Normalize(target);
        }
        catch
        {
            return null;
        }
    }
}
