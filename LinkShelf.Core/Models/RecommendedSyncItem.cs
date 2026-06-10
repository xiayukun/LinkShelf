namespace LinkShelf.Models;

public sealed class RecommendedSyncItem
{
    public required string Id { get; init; }
    public required string NameKey { get; init; }
    public required string PortablePath { get; init; }
    public required string ExpandedPath { get; init; }
    public required SyncItemKind Kind { get; init; }
    public required string ReasonKey { get; init; }
    public required RecommendedPlatform Platform { get; init; }
}

public enum RecommendedPlatform
{
    Windows,
    MacOS,
    Linux
}

public sealed class RecommendedSyncItemRow
{
    public required RecommendedSyncItem Item { get; init; }
    public bool IsSelected { get; set; }
    public required string Name { get; init; }
    public required string PortablePath { get; init; }
    public required string ItemType { get; init; }
    public required string Reason { get; init; }
}
