namespace LinkShelf.Services;

public sealed class AppPaths
{
    public const string ConfigFileName = "link-shelf.config.json";
    public const string LogDirectoryName = ".link-shelf-logs";
    public const string BackupDirectoryName = ".link-shelf-backups";

    public AppPaths(string baseDirectory, string? userHome = null)
    {
        CacheRoot = Path.GetFullPath(baseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        UserHome = userHome is null ? InferUserHome(CacheRoot) : Path.GetFullPath(userHome);
        ConfigPath = Path.Combine(CacheRoot, ConfigFileName);
        LogDirectory = Path.Combine(CacheRoot, LogDirectoryName);
        BackupDirectory = Path.Combine(CacheRoot, BackupDirectoryName);
    }

    public string CacheRoot { get; }
    public string UserHome { get; }
    public string ConfigPath { get; }
    public string LogDirectory { get; }
    public string BackupDirectory { get; }

    public void EnsureSystemDirectories()
    {
        Directory.CreateDirectory(LogDirectory);
        Directory.CreateDirectory(BackupDirectory);
    }

    private static string InferUserHome(string cacheRoot)
    {
        var marker = Path.DirectorySeparatorChar + "AppData" + Path.DirectorySeparatorChar + "Local" + Path.DirectorySeparatorChar;
        var index = cacheRoot.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index > 0)
        {
            return cacheRoot[..index];
        }

        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}
