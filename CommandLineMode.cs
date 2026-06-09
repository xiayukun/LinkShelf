using System.Text.Encodings.Web;
using System.Text.Json;
using LinkShelf.Models;
using LinkShelf.Services;

namespace LinkShelf;

internal static class CommandLineMode
{
    private const string Version = "1.1.5";

    public static bool IsCommand(string[] args)
    {
        if (args.Length == 0)
        {
            return false;
        }

        var command = args[0].Trim().ToLowerInvariant();
        return command is "check" or "status" or "cache-root" or "version" or "help" or "--help" or "-h" or "-help";
    }

    public static int Run(string[] args)
    {
        try
        {
            var command = args.Length == 0 ? "help" : args[0].Trim().ToLowerInvariant();
            var options = args.Skip(1).Select(x => x.Trim().ToLowerInvariant()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var paths = new AppPaths(AppContext.BaseDirectory);

            return command switch
            {
                "check" => RunCheck(paths, options),
                "status" => RunCheck(paths, options),
                "cache-root" => WriteLine(paths.CacheRoot),
                "version" => WriteLine(Version),
                "help" => WriteHelp(),
                "--help" => WriteHelp(),
                "-h" => WriteHelp(),
                "-help" => WriteHelp(),
                _ => UnknownCommand(command)
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("ERROR");
            Console.Error.WriteLine(ex.Message);
            return 2;
        }
    }

    private static int RunCheck(AppPaths paths, HashSet<string> options)
    {
        var store = new ConfigStore(paths);
        var config = store.Load();
        var checker = new StatusCheckService(paths, config);
        var items = checker.CheckAll();
        store.Save(config);

        var unhealthy = items
            .Where(x => x.Status == SyncConstants.StatusEnabled && !StatusCheckService.IsHealthy(x))
            .ToList();
        var json = options.Contains("--json");
        var verbose = options.Contains("--verbose") || options.Contains("-v");

        if (json)
        {
            WriteJson(paths, items, unhealthy);
        }
        else
        {
            WriteText(paths, items, unhealthy, verbose);
        }

        return unhealthy.Count == 0 ? 0 : 1;
    }

    private static void WriteJson(AppPaths paths, List<SyncItem> items, List<SyncItem> unhealthy)
    {
        var payload = new
        {
            ok = unhealthy.Count == 0,
            cacheRoot = paths.CacheRoot,
            configPath = paths.ConfigPath,
            checkedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            total = items.Count,
            problemCount = unhealthy.Count,
            problems = unhealthy.Select(ToOutputItem).ToList(),
            items = items.Select(ToOutputItem).ToList()
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        Console.WriteLine(JsonSerializer.Serialize(payload, options));
    }

    private static object ToOutputItem(SyncItem item)
    {
        return new
        {
            cacheName = item.CacheName,
            originalPath = item.OriginalPath,
            expandedOriginalPath = item.ExpandedOriginalPath,
            cachePath = item.CachePath,
            type = item.ItemType,
            status = item.Status,
            checkResult = item.CheckResult,
            sourceMachine = item.SourceMachine,
            updatedAt = item.UpdatedAt
        };
    }

    private static void WriteText(AppPaths paths, List<SyncItem> items, List<SyncItem> unhealthy, bool verbose)
    {
        Console.WriteLine("Link Shelf Check");
        Console.WriteLine("Cache root: " + paths.CacheRoot);
        Console.WriteLine("Config: " + paths.ConfigPath);
        Console.WriteLine("Checked at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine("Total: " + items.Count);
        Console.WriteLine("Problems: " + unhealthy.Count);
        Console.WriteLine();

        var rows = verbose ? items : unhealthy;
        if (rows.Count == 0)
        {
            Console.WriteLine("OK");
            return;
        }

        foreach (var item in rows)
        {
            Console.WriteLine("- " + item.CacheName);
            Console.WriteLine("  result: " + item.CheckResult);
            Console.WriteLine("  target: " + item.ExpandedOriginalPath);
            Console.WriteLine("  cache:  " + item.CachePath);
        }
    }

    private static int WriteHelp()
    {
        Console.WriteLine("Link Shelf");
        Console.WriteLine("Move scattered local files or directories into this executable's folder,");
        Console.WriteLine("then restore the original paths with Windows symbolic links.");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  check              Check links and cache items");
        Console.WriteLine("  check --json       Print machine-readable JSON");
        Console.WriteLine("  check --verbose    Print every configured item");
        Console.WriteLine("  status             Alias of check");
        Console.WriteLine("  cache-root         Print current cache root");
        Console.WriteLine("  version            Print version");
        Console.WriteLine("  help, -h, -help, --help");
        Console.WriteLine("                     Print this help");
        Console.WriteLine();
        Console.WriteLine("AI and automation notes:");
        Console.WriteLine("  - The executable directory is the cache root.");
        Console.WriteLine("  - Use 'cache-root' before inspecting files.");
        Console.WriteLine("  - Use 'check --json' for health monitoring.");
        Console.WriteLine("  - Notify the user only when problemCount is greater than 0.");
        Console.WriteLine("  - CLI commands are read-only; GUI actions perform moves, links, restores, and undo.");
        return 0;
    }

    private static int WriteLine(string value)
    {
        Console.WriteLine(value);
        return 0;
    }

    private static int UnknownCommand(string command)
    {
        Console.Error.WriteLine("Unknown command: " + command);
        Console.Error.WriteLine("Run: LinkShelf.exe help");
        return 2;
    }
}
