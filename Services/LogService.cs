namespace LinkShelf.Services;

public sealed class LogService
{
    private readonly AppPaths paths;

    public LogService(AppPaths paths)
    {
        this.paths = paths;
        this.paths.EnsureSystemDirectories();
    }

    public event Action<string>? MessageAdded;

    public void Write(string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        File.AppendAllText(CurrentLogPath(), line + Environment.NewLine);
        MessageAdded?.Invoke(line);
    }

    private string CurrentLogPath()
    {
        return Path.Combine(paths.LogDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
    }
}
