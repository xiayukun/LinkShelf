using LinkShelf.Models;

namespace LinkShelf.Services;

public static class PathTools
{
    public static string Normalize(string path)
    {
        return Path.GetFullPath(Environment.ExpandEnvironmentVariables(path))
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    public static string ToPortablePath(string path)
    {
        return ToPortablePath(path, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }

    public static string ToPortablePath(string path, string userHome)
    {
        var normalized = Normalize(path);
        var home = Normalize(userHome);
        if (IsSameOrChild(normalized, home))
        {
            var relative = Path.GetRelativePath(home, normalized);
            return relative == "." ? "~" : "~" + Path.DirectorySeparatorChar + relative;
        }

        return normalized;
    }

    public static string ExpandPortablePath(string path)
    {
        return ExpandPortablePath(path, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }

    public static string ExpandPortablePath(string path, string userHome)
    {
        if (path == "~")
        {
            return Normalize(userHome);
        }

        if (path.StartsWith("~\\", StringComparison.Ordinal) || path.StartsWith("~/", StringComparison.Ordinal))
        {
            return Normalize(Path.Combine(userHome, path[2..]));
        }

        return Normalize(path);
    }

    public static bool IsSameOrChild(string path, string parent)
    {
        var fullPath = Normalize(path) + Path.DirectorySeparatorChar;
        var fullParent = Normalize(parent) + Path.DirectorySeparatorChar;
        return fullPath.StartsWith(fullParent, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsSamePath(string left, string right)
    {
        return string.Equals(Normalize(left), Normalize(right), StringComparison.OrdinalIgnoreCase);
    }

    public static string KindText(SyncItemKind kind)
    {
        return kind == SyncItemKind.Directory ? SyncConstants.TypeDirectory : SyncConstants.TypeFile;
    }

    public static SyncItemKind KindFromText(string text)
    {
        return text.Equals(SyncConstants.TypeFile, StringComparison.OrdinalIgnoreCase)
            ? SyncItemKind.File
            : SyncItemKind.Directory;
    }

    public static string MakeUniqueName(string desiredName, Func<string, bool> exists)
    {
        if (!exists(desiredName))
        {
            return desiredName;
        }

        for (var index = 2; index < 10000; index++)
        {
            var candidate = desiredName + index;
            if (!exists(candidate))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException("Unable to generate a unique cache name.");
    }
}
