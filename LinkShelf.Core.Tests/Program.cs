using LinkShelf;
using LinkShelf.Models;
using LinkShelf.Services;

var tests = new (string Name, Action Run)[]
{
    ("PathTools round-trips user-profile portable paths", PathToolsRoundTripsPortablePaths),
    ("PathTools uses platform path comparison", PathToolsUsesPlatformPathComparison),
    ("ConfigStore normalizes saved config", ConfigStoreNormalizesSavedConfig),
    ("RecommendedSyncItems filters by requested platform", RecommendedSyncItemsFiltersByPlatform),
    ("CommandLineRunner lists platform recommended items", CommandLineRunnerListsPlatformRecommendedItems),
    ("CommandLineRunner prints detected platform", CommandLineRunnerPrintsDetectedPlatform),
    ("CommandLineRunner prints supplied version", CommandLineRunnerPrintsSuppliedVersion)
};

var failed = 0;
foreach (var test in tests)
{
    try
    {
        test.Run();
        Console.WriteLine("PASS " + test.Name);
    }
    catch (Exception ex)
    {
        failed++;
        Console.Error.WriteLine("FAIL " + test.Name);
        Console.Error.WriteLine(ex.Message);
    }
}

if (failed > 0)
{
    Console.Error.WriteLine($"{failed} test(s) failed.");
    return 1;
}

Console.WriteLine($"{tests.Length} test(s) passed.");
return 0;

static void PathToolsRoundTripsPortablePaths()
{
    var userHome = Path.Combine(Path.GetTempPath(), "link-shelf-tests-user");
    var original = Path.Combine(userHome, ".config", "tool", "settings.json");
    var portable = PathTools.ToPortablePath(original, userHome);

    AssertEqual("~" + Path.DirectorySeparatorChar + Path.Combine(".config", "tool", "settings.json"), portable);
    AssertEqual(PathTools.Normalize(original), PathTools.ExpandPortablePath(portable, userHome));
    AssertEqual(PathTools.Normalize(Path.Combine(userHome, "child")), PathTools.ExpandPortablePath("~/child", userHome));
    AssertEqual(PathTools.Normalize(Path.Combine(userHome, "child")), PathTools.ExpandPortablePath(@"~\child", userHome));
}

static void PathToolsUsesPlatformPathComparison()
{
    var root = Path.Combine(Path.GetTempPath(), "LinkShelfCaseRoot");
    var left = Path.Combine(root, "Folder");
    var right = Path.Combine(root, "folder");
    var shouldIgnoreCase = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS();

    AssertEqual(shouldIgnoreCase, PathTools.IsSamePath(left, right));
}

static void ConfigStoreNormalizesSavedConfig()
{
    var tempRoot = CreateTempDirectory();
    try
    {
        var paths = new AppPaths(tempRoot);
        var store = new ConfigStore(paths);
        var config = new SyncConfig
        {
            Version = 0,
            Items =
            [
                new SyncItem
                {
                    CacheName = "demo",
                    OriginalPath = "~/demo",
                    ItemType = "FILE",
                    LinkMode = "bad-link-mode",
                    Status = "REMOVED"
                }
            ]
        };

        store.Save(config);
        var loaded = store.Load();

        AssertEqual(2, loaded.Version);
        AssertEqual(SyncConstants.TypeFile, loaded.Items[0].ItemType);
        AssertEqual(SyncConstants.LinkModeSymbolic, loaded.Items[0].LinkMode);
        AssertEqual(SyncConstants.StatusRemoved, loaded.Items[0].Status);
    }
    finally
    {
        DeleteTempDirectory(tempRoot);
    }
}

static void RecommendedSyncItemsFiltersByPlatform()
{
    var tempRoot = CreateTempDirectory();
    try
    {
        var userHome = Path.Combine(tempRoot, "home");
        var cacheRoot = Path.Combine(tempRoot, "cache");
        Directory.CreateDirectory(cacheRoot);

        var macGitConfig = Path.Combine(userHome, ".gitconfig");
        Directory.CreateDirectory(Path.GetDirectoryName(macGitConfig) ?? userHome);
        File.WriteAllText(macGitConfig, "[user]");

        var windowsTerminalState = Path.Combine(
            userHome,
            "AppData",
            "Local",
            "Packages",
            "Microsoft.WindowsTerminal_8wekyb3d8bbwe",
            "LocalState");
        Directory.CreateDirectory(windowsTerminalState);

        var paths = new AppPaths(cacheRoot, userHome);
        var config = new SyncConfig();

        var macItems = RecommendedSyncItems.GetAvailable(paths, config, RecommendedPlatform.MacOS);
        AssertEqual(true, macItems.Any(item => item.Id == "macos-gitconfig"));
        AssertEqual(false, macItems.Any(item => item.Id == "windows-terminal"));

        var windowsItems = RecommendedSyncItems.GetAvailable(paths, config, RecommendedPlatform.Windows);
        AssertEqual(true, windowsItems.Any(item => item.Id == "windows-terminal"));
        AssertEqual(false, windowsItems.Any(item => item.Id == "macos-gitconfig"));
    }
    finally
    {
        DeleteTempDirectory(tempRoot);
    }
}

static void CommandLineRunnerPrintsSuppliedVersion()
{
    var previousOut = Console.Out;
    using var writer = new StringWriter();
    try
    {
        Console.SetOut(writer);
        var exitCode = CommandLineRunner.Run(["version"], "9.8.7-test", AppContext.BaseDirectory);

        AssertEqual(0, exitCode);
        AssertEqual("9.8.7-test", writer.ToString().Trim());
    }
    finally
    {
        Console.SetOut(previousOut);
    }
}

static void CommandLineRunnerPrintsDetectedPlatform()
{
    var previousOut = Console.Out;
    using var writer = new StringWriter();
    try
    {
        Console.SetOut(writer);
        var exitCode = CommandLineRunner.Run(["platform"], "9.8.7-test", AppContext.BaseDirectory);
        var expected = OperatingSystem.IsMacOS()
            ? "macos"
            : OperatingSystem.IsLinux()
                ? "linux"
                : "windows";

        AssertEqual(0, exitCode);
        AssertEqual(expected, writer.ToString().Trim());
    }
    finally
    {
        Console.SetOut(previousOut);
    }
}

static void CommandLineRunnerListsPlatformRecommendedItems()
{
    var tempRoot = CreateTempDirectory();
    var previousOut = Console.Out;
    using var writer = new StringWriter();
    try
    {
        var userHome = Path.Combine(tempRoot, "home");
        var cacheRoot = Path.Combine(tempRoot, "cache");
        Directory.CreateDirectory(cacheRoot);

        var macGitConfig = Path.Combine(userHome, ".gitconfig");
        Directory.CreateDirectory(Path.GetDirectoryName(macGitConfig) ?? userHome);
        File.WriteAllText(macGitConfig, "[user]");

        var windowsTerminalState = Path.Combine(
            userHome,
            "AppData",
            "Local",
            "Packages",
            "Microsoft.WindowsTerminal_8wekyb3d8bbwe",
            "LocalState");
        Directory.CreateDirectory(windowsTerminalState);

        Console.SetOut(writer);
        var exitCode = CommandLineRunner.Run(["recommended", "--json", "--platform", "macos"], "9.8.7-test", cacheRoot, userHome);
        var output = writer.ToString();

        AssertEqual(0, exitCode);
        AssertEqual(true, output.Contains("\"platform\": \"macos\"", StringComparison.Ordinal));
        AssertEqual(true, output.Contains("\"id\": \"macos-gitconfig\"", StringComparison.Ordinal));
        AssertEqual(false, output.Contains("\"id\": \"windows-terminal\"", StringComparison.Ordinal));
    }
    finally
    {
        Console.SetOut(previousOut);
        DeleteTempDirectory(tempRoot);
    }
}

static string CreateTempDirectory()
{
    var path = Path.Combine(Path.GetTempPath(), "link-shelf-core-tests-" + Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(path);
    return path;
}

static void DeleteTempDirectory(string path)
{
    try
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
    catch
    {
    }
}

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected: {expected}; actual: {actual}");
    }
}
