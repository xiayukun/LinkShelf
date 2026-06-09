using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LinkShelf.Services;

public sealed class ProjectionException : Exception
{
    public ProjectionException(string textKey, string? detail = null)
        : base(textKey)
    {
        TextKey = textKey;
        Detail = detail;
    }

    public string TextKey { get; }

    public string? Detail { get; }
}

public static class ProjectionService
{
    public static string ProjectExecutableToDirectory(string executablePath, string targetDirectory)
    {
        var source = PathTools.Normalize(executablePath);
        var directory = PathTools.Normalize(targetDirectory);
        var projectionPath = Path.Combine(directory, Path.GetFileName(source));

        if (PathTools.IsSamePath(source, projectionPath))
        {
            throw new ProjectionException("projection.error.sameExecutable");
        }

        if (File.Exists(projectionPath) || Directory.Exists(projectionPath))
        {
            throw new ProjectionException("projection.error.targetExists");
        }

        var sourceRoot = Path.GetPathRoot(source);
        var targetRoot = Path.GetPathRoot(projectionPath);
        if (!string.Equals(sourceRoot, targetRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new ProjectionException("projection.error.differentDrive");
        }

        Directory.CreateDirectory(directory);
        if (!CreateHardLink(projectionPath, source, IntPtr.Zero))
        {
            var error = new Win32Exception(Marshal.GetLastWin32Error());
            throw new ProjectionException("projection.error.nativeFailure", error.Message);
        }

        return projectionPath;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
}
