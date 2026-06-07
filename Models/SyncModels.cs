using System.Text.Json.Serialization;

namespace LinkShelf.Models;

public static class SyncConstants
{
    public const string TypeDirectory = "directory";
    public const string TypeFile = "file";
    public const string StatusEnabled = "enabled";
    public const string StatusRemoved = "removed";
    public const string StatusProblem = "problem";
    public const string LinkModeSymbolic = "symbolic-link";

    public const string CheckUnknown = "";
    public const string CheckOk = "ok";
    public const string CheckCacheMissing = "cache-missing";
    public const string CheckTargetHasContent = "target-has-content";
    public const string CheckTargetMissingLink = "target-missing-link";
    public const string CheckLinkWrongTarget = "link-wrong-target";
    public const string CheckSkipped = "skipped";
    public const string CheckRestored = "restored";
}

public sealed class SyncConfig
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 2;

    [JsonPropertyName("cacheId")]
    public string CacheId { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [JsonPropertyName("items")]
    public List<SyncItem> Items { get; set; } = [];
}

public sealed class SyncItem
{
    [JsonPropertyName("cacheName")]
    public string CacheName { get; set; } = "";

    [JsonPropertyName("originalPath")]
    public string OriginalPath { get; set; } = "";

    [JsonPropertyName("type")]
    public string ItemType { get; set; } = SyncConstants.TypeDirectory;

    [JsonPropertyName("linkMode")]
    public string LinkMode { get; set; } = SyncConstants.LinkModeSymbolic;

    [JsonPropertyName("status")]
    public string Status { get; set; } = SyncConstants.StatusEnabled;

    [JsonPropertyName("sourceMachine")]
    public string SourceMachine { get; set; } = Environment.MachineName;

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [JsonPropertyName("lastOperation")]
    public string LastOperation { get; set; } = "";

    [JsonPropertyName("note")]
    public string Note { get; set; } = "";

    [JsonIgnore]
    public string CheckResult { get; set; } = SyncConstants.CheckUnknown;

    [JsonIgnore]
    public string CachePath { get; set; } = "";

    [JsonIgnore]
    public string ExpandedOriginalPath { get; set; } = "";
}

public sealed class SyncItemRow
{
    public SyncItemRow(SyncItem item, Func<string, string> localizeCode)
    {
        Item = item;
        CacheName = item.CacheName;
        OriginalPath = item.OriginalPath;
        ItemType = localizeCode(item.ItemType);
        Status = localizeCode(DisplayStatusCode(item));
        CheckResult = localizeCode(item.CheckResult);
        SourceMachine = item.SourceMachine;
        LastOperation = localizeCode(item.LastOperation);
        UpdatedAt = item.UpdatedAt;
        CachePath = item.CachePath;
        ExpandedOriginalPath = item.ExpandedOriginalPath;
    }

    public SyncItem Item { get; }
    public string CacheName { get; }
    public string OriginalPath { get; }
    public string ItemType { get; }
    public string Status { get; }
    public string CheckResult { get; }
    public string SourceMachine { get; }
    public string LastOperation { get; }
    public string UpdatedAt { get; }
    public string CachePath { get; }
    public string ExpandedOriginalPath { get; }

    private static string DisplayStatusCode(SyncItem item)
    {
        if (item.Status != SyncConstants.StatusEnabled)
        {
            return item.Status;
        }

        return item.CheckResult is SyncConstants.CheckCacheMissing or
            SyncConstants.CheckTargetHasContent or
            SyncConstants.CheckTargetMissingLink or
            SyncConstants.CheckLinkWrongTarget
            ? SyncConstants.StatusProblem
            : item.Status;
    }
}

public enum SyncItemKind
{
    Directory,
    File
}

public enum ConflictDecision
{
    UseCacheDeleteTarget,
    ImportTargetOverwriteCache,
    ImportTargetThenOverlayCacheBackup,
    BackupTargetAndSkip,
    Cancel
}
