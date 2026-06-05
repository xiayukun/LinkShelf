namespace LinkShelf.Services;

public sealed class LogService
{
    private readonly AppPaths paths;
    private readonly object fileLock = new();

    public LogService(AppPaths paths)
    {
        this.paths = paths;
        this.paths.EnsureSystemDirectories();
    }

    public event Action<string>? MessageAdded;

    public void Write(string message)
    {
        var line = WriteLine(message);
        MessageAdded?.Invoke(line);
    }

    public void WriteDiagnostic(string message)
    {
        WriteLine("DIAGNOSTIC " + message);
    }

    private string WriteLine(string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
        lock (fileLock)
        {
            File.AppendAllText(CurrentLogPath(), line + Environment.NewLine);
        }

        return line;
    }

    private string CurrentLogPath()
    {
        return Path.Combine(paths.LogDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
    }
}
