using LinkShelf.Models;

namespace LinkShelf.Services;

public sealed class FileOperations
{
    private readonly AppPaths paths;
    private readonly SyncConfig config;
    private readonly ConfigStore store;
    private readonly LogService log;
    private readonly Func<SyncItem, string, string, string, ConflictDecision> resolveConflict;

    public FileOperations(
        AppPaths paths,
        SyncConfig config,
        ConfigStore store,
        LogService log,
        Func<SyncItem, string, string, string, ConflictDecision> resolveConflict)
    {
        this.paths = paths;
        this.config = config;
        this.store = store;
        this.log = log;
        this.resolveConflict = resolveConflict;
    }

    public SyncItem AddSyncItem(string sourcePath, SyncItemKind kind)
    {
        var source = PathTools.Normalize(sourcePath);
        EnsureCanUsePath(source);

        if (!PathExists(source))
        {
            throw new InvalidOperationException("Target path does not exist.");
        }

        var portable = PathTools.ToPortablePath(source, paths.UserHome);
        var linkTarget = TryGetLinkTarget(source);
        if (linkTarget is not null)
        {
            if (PathTools.IsSameOrChild(linkTarget, paths.CacheRoot))
            {
                var existing = EnsureConfigForExistingLink(source, linkTarget, kind);
                store.Save(config);
                log.Write($"Target is already linked to this cache: {source}");
                return existing;
            }

            throw new InvalidOperationException("The target path is already a link, but it does not point to this cache root.");
        }

        var desiredName = Path.GetFileName(source);
        if (string.IsNullOrWhiteSpace(desiredName))
        {
            throw new InvalidOperationException("Unable to detect the target name.");
        }

        var sameRecord = config.Items.FirstOrDefault(x =>
            string.Equals(x.OriginalPath, portable, StringComparison.OrdinalIgnoreCase) &&
            x.Status == SyncConstants.StatusEnabled);

        var cacheName = sameRecord?.CacheName ?? ChooseCacheName(desiredName, portable);
        var item = sameRecord ?? CreateItem(cacheName, portable, kind);
        var cachePath = Path.Combine(paths.CacheRoot, cacheName);
        var movedSourceToCache = false;

        try
        {
            if (PathExists(cachePath))
            {
                var decision = resolveConflict(item, source, cachePath, "cache-already-exists");
                if (!ApplyConflictDecision(item, source, cachePath, decision))
                {
                    throw new InvalidOperationException("The conflict was canceled or skipped.");
                }
            }
            else
            {
                MovePath(source, cachePath, kind);
                movedSourceToCache = true;
                log.Write($"Moved into cache: {source} -> {cachePath}");
            }

            CreateLink(source, cachePath, kind);
            item.LastOperation = "add-sync-item";
            item.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!config.Items.Contains(item))
            {
                config.Items.Add(item);
            }

            store.Save(config);
            log.Write($"Created link: {source} -> {cachePath}");
            return item;
        }
        catch
        {
            TryRollbackMovedAdd(source, cachePath, kind, movedSourceToCache);
            throw;
        }
    }

    public void RestoreItem(SyncItem item)
    {
        var cachePath = Path.Combine(paths.CacheRoot, item.CacheName);
        var targetPath = PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome);
        var kind = PathTools.KindFromText(item.ItemType);

        if (!PathExists(cachePath))
        {
            item.CheckResult = SyncConstants.CheckCacheMissing;
            log.Write($"Cache item missing: {cachePath}");
            return;
        }

        EnsureCanUsePath(targetPath);
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath) ?? paths.CacheRoot);

        var linkTarget = TryGetLinkTarget(targetPath);
        if (linkTarget is not null)
        {
            if (PathTools.IsSamePath(linkTarget, cachePath))
            {
                item.CheckResult = SyncConstants.CheckOk;
                log.Write($"Link is healthy: {targetPath}");
                return;
            }

            var decision = resolveConflict(item, targetPath, cachePath, "target-is-another-link");
            if (!ApplyConflictDecision(item, targetPath, cachePath, decision))
            {
                item.CheckResult = SyncConstants.CheckSkipped;
                return;
            }
        }
        else if (PathExists(targetPath))
        {
            var decision = resolveConflict(item, targetPath, cachePath, "target-has-real-content");
            if (!ApplyConflictDecision(item, targetPath, cachePath, decision))
            {
                item.CheckResult = SyncConstants.CheckSkipped;
                return;
            }
        }
        else
        {
            CreateLink(targetPath, cachePath, kind);
        }

        item.LastOperation = "restore-local-link";
        item.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        item.CheckResult = SyncConstants.CheckRestored;
        log.Write($"Restored link: {targetPath} -> {cachePath}");
    }

    public void RevertItem(SyncItem item)
    {
        var cachePath = Path.Combine(paths.CacheRoot, item.CacheName);
        var targetPath = PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome);
        var kind = PathTools.KindFromText(item.ItemType);

        if (!PathExists(cachePath))
        {
            throw new InvalidOperationException($"Cache item missing: {cachePath}");
        }

        EnsureCanUsePath(targetPath);
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath) ?? paths.CacheRoot);

        var linkTarget = TryGetLinkTarget(targetPath);
        if (linkTarget is not null)
        {
            if (!PathTools.IsSamePath(linkTarget, cachePath))
            {
                throw new InvalidOperationException($"The original path is a link to another location: {targetPath}");
            }

            DeletePath(targetPath);
            log.Write($"Removed original link before revert: {targetPath}");
        }
        else if (PathExists(targetPath))
        {
            throw new InvalidOperationException($"The original path has real content. Revert skipped to avoid overwriting it: {targetPath}");
        }

        MovePath(cachePath, targetPath, kind);
        config.Items.Remove(item);
        store.Save(config);
        log.Write($"Reverted item to original path and removed config record: {cachePath} -> {targetPath}");
    }

    public void CheckAll()
    {
        var checker = new StatusCheckService(paths, config);
        checker.CheckAll();
        store.Save(config);
    }

    public void CheckItem(SyncItem item)
    {
        var checker = new StatusCheckService(paths, config);
        checker.CheckItem(item);
    }

    private SyncItem EnsureConfigForExistingLink(string source, string linkTarget, SyncItemKind kind)
    {
        var cacheName = Path.GetFileName(linkTarget);
        var portable = PathTools.ToPortablePath(source, paths.UserHome);
        var item = config.Items.FirstOrDefault(x =>
            string.Equals(x.CacheName, cacheName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(x.OriginalPath, portable, StringComparison.OrdinalIgnoreCase));

        if (item is not null)
        {
            item.CacheName = cacheName;
            item.OriginalPath = portable;
            item.ItemType = PathTools.KindText(kind);
            item.Status = SyncConstants.StatusEnabled;
            item.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            item.LastOperation = "complete-existing-link";
            return item;
        }

        item = CreateItem(cacheName, portable, kind);
        item.LastOperation = "complete-existing-link";
        config.Items.Add(item);
        return item;
    }

    private static SyncItem CreateItem(string cacheName, string portable, SyncItemKind kind)
    {
        return new SyncItem
        {
            CacheName = cacheName,
            OriginalPath = portable,
            ItemType = PathTools.KindText(kind),
            LinkMode = SyncConstants.LinkModeSymbolic,
            Status = SyncConstants.StatusEnabled,
            SourceMachine = Environment.MachineName,
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private string ChooseCacheName(string desiredName, string portable)
    {
        var sameRecord = config.Items.FirstOrDefault(x =>
            string.Equals(x.OriginalPath, portable, StringComparison.OrdinalIgnoreCase) &&
            x.Status == SyncConstants.StatusEnabled);
        if (sameRecord is not null)
        {
            return sameRecord.CacheName;
        }

        return PathTools.MakeUniqueName(desiredName, name =>
        {
            var full = Path.Combine(paths.CacheRoot, name);
            var usedByOtherRecord = config.Items.Any(x =>
                string.Equals(x.CacheName, name, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(x.OriginalPath, portable, StringComparison.OrdinalIgnoreCase) &&
                x.Status == SyncConstants.StatusEnabled);
            return PathExists(full) || usedByOtherRecord;
        });
    }

    private bool ApplyConflictDecision(SyncItem item, string targetPath, string cachePath, ConflictDecision decision)
    {
        var kind = PathTools.KindFromText(item.ItemType);

        switch (decision)
        {
            case ConflictDecision.UseCacheDeleteTarget:
                MoveTargetToBackup(targetPath, "target-before-cache-link");
                CreateLink(targetPath, cachePath, kind);
                return true;

            case ConflictDecision.ImportTargetOverwriteCache:
                CopyPath(targetPath, cachePath, overwrite: true);
                MoveTargetToBackup(targetPath, "target-after-import");
                CreateLink(targetPath, cachePath, kind);
                log.Write($"Imported target into cache: {targetPath} -> {cachePath}");
                return true;

            case ConflictDecision.ImportTargetThenOverlayCacheBackup:
                var backupCache = MakeBackupPath(Path.GetFileName(cachePath), "previous-cache-content");
                MovePath(cachePath, backupCache, kind);
                CopyPath(targetPath, cachePath, overwrite: true);
                MoveTargetToBackup(targetPath, "target-after-import");
                CreateLink(targetPath, cachePath, kind);
                CopyPath(backupCache, cachePath, overwrite: true);
                log.Write($"Merged previous cache content: {backupCache} -> {cachePath}");
                return true;

            case ConflictDecision.BackupTargetAndSkip:
                CopyTargetToBackup(targetPath, "target-before-skip");
                log.Write($"Backed up and skipped: {targetPath}");
                return false;

            default:
                return false;
        }
    }

    private void EnsureCanUsePath(string path)
    {
        var normalized = PathTools.Normalize(path);
        var cacheRoot = PathTools.Normalize(paths.CacheRoot);

        if (PathTools.IsSamePath(normalized, cacheRoot))
        {
            throw new InvalidOperationException("The cache root itself cannot be selected.");
        }

        if (PathTools.IsSameOrChild(normalized, cacheRoot))
        {
            throw new InvalidOperationException("Items inside the cache root cannot be selected.");
        }

        if (PathTools.IsSameOrChild(cacheRoot, normalized))
        {
            throw new InvalidOperationException("A parent folder that contains the cache root cannot be selected.");
        }
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

    private static void MovePath(string source, string destination, SyncItemKind kind)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destination) ?? ".");
        if (kind == SyncItemKind.Directory)
        {
            Directory.Move(source, destination);
        }
        else
        {
            File.Move(source, destination, overwrite: false);
        }
    }

    private void TryRollbackMovedAdd(string source, string cachePath, SyncItemKind kind, bool movedSourceToCache)
    {
        if (!movedSourceToCache || !PathExists(cachePath) || PathExists(source))
        {
            return;
        }

        try
        {
            MovePath(cachePath, source, kind);
            log.Write($"Rolled back add move after failure: {cachePath} -> {source}");
        }
        catch (Exception rollbackError)
        {
            log.Write($"Failed to roll back add move after failure: {cachePath} -> {source}. {rollbackError.Message}");
        }
    }

    private void CopyPath(string source, string destination, bool overwrite)
    {
        var attributes = File.GetAttributes(source);
        if (attributes.HasFlag(FileAttributes.Directory))
        {
            CopyDirectory(source, destination, overwrite);
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination) ?? ".");
            File.Copy(source, destination, overwrite);
        }
    }

    private void CopyDirectory(string source, string destination, bool overwrite)
    {
        Directory.CreateDirectory(destination);

        foreach (var file in Directory.GetFiles(source))
        {
            var target = Path.Combine(destination, Path.GetFileName(file));
            File.Copy(file, target, overwrite);
        }

        foreach (var directory in Directory.GetDirectories(source))
        {
            var target = Path.Combine(destination, Path.GetFileName(directory));
            CopyDirectory(directory, target, overwrite);
        }
    }

    private void CreateLink(string linkPath, string targetPath, SyncItemKind kind)
    {
        if (PathExists(linkPath))
        {
            DeletePath(linkPath);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(linkPath) ?? ".");
        if (kind == SyncItemKind.Directory)
        {
            Directory.CreateSymbolicLink(linkPath, targetPath);
        }
        else
        {
            File.CreateSymbolicLink(linkPath, targetPath);
        }
    }

    private static void DeletePath(string path)
    {
        var attributes = File.GetAttributes(path);
        if (attributes.HasFlag(FileAttributes.Directory))
        {
            Directory.Delete(path, recursive: !attributes.HasFlag(FileAttributes.ReparsePoint));
        }
        else
        {
            File.Delete(path);
        }
    }

    private void MoveTargetToBackup(string targetPath, string reason)
    {
        if (!PathExists(targetPath))
        {
            return;
        }

        var backup = MakeBackupPath(Path.GetFileName(targetPath), reason);
        var attributes = File.GetAttributes(targetPath);
        var kind = attributes.HasFlag(FileAttributes.Directory) ? SyncItemKind.Directory : SyncItemKind.File;
        MovePath(targetPath, backup, kind);
        log.Write($"Backed up target: {targetPath} -> {backup}");
    }

    private void CopyTargetToBackup(string targetPath, string reason)
    {
        if (!PathExists(targetPath))
        {
            return;
        }

        var backup = MakeBackupPath(Path.GetFileName(targetPath), reason);
        CopyPath(targetPath, backup, overwrite: true);
        log.Write($"Copied target backup: {targetPath} -> {backup}");
    }

    private string MakeBackupPath(string name, string reason)
    {
        var safeReason = string.Concat(reason.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch));
        var machine = string.Concat(Environment.MachineName.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch));
        var folder = $"{DateTime.Now:yyyyMMdd_HHmmss}_{machine}_{safeReason}";
        return Path.Combine(paths.BackupDirectory, folder, name);
    }
}
