using System.Text.Encodings.Web;
using System.Text.Json;
using LinkShelf.Models;
using LinkShelf.Services;

namespace LinkShelf;

public static class CommandLineRunner
{
    public static bool IsCommand(string[] args)
    {
        if (args.Length == 0)
        {
            return false;
        }

        var command = args[0].Trim().ToLowerInvariant();
        return command is "check" or "status" or "recommended" or "cache-root" or "platform" or "version" or "help" or "--help" or "-h" or "-help";
    }

    public static int Run(string[] args, string version, string baseDirectory, string? userHome = null)
    {
        try
        {
            var command = args.Length == 0 ? "help" : args[0].Trim().ToLowerInvariant();
            var optionList = args.Skip(1).Select(x => x.Trim().ToLowerInvariant()).ToList();
            var options = optionList.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var paths = new AppPaths(baseDirectory, userHome);

            return command switch
            {
                "check" => RunCheck(paths, options),
                "status" => RunCheck(paths, options),
                "recommended" => RunRecommended(paths, options, optionList),
                "cache-root" => WriteLine(paths.CacheRoot),
                "platform" => WriteLine(PlatformText(CurrentRecommendedPlatform())),
                "version" => WriteLine(version),
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

    private static int RunRecommended(AppPaths paths, HashSet<string> options, IReadOnlyList<string> optionList)
    {
        var platform = ParseRecommendedPlatform(optionList);
        var store = new ConfigStore(paths);
        var config = store.Load();
        var items = RecommendedSyncItems.GetAvailable(paths, config, platform);
        var json = options.Contains("--json");

        if (json)
        {
            var payload = new
            {
                ok = true,
                cacheRoot = paths.CacheRoot,
                configPath = paths.ConfigPath,
                platform = PlatformText(platform),
                total = items.Count,
                items = items.Select(ToRecommendedOutputItem).ToList()
            };
            WriteJsonPayload(payload);
        }
        else
        {
            Console.WriteLine("Link Shelf Recommended Items");
            Console.WriteLine("Cache root: " + paths.CacheRoot);
            Console.WriteLine("Platform: " + PlatformText(platform));
            Console.WriteLine("Total: " + items.Count);
            Console.WriteLine();

            if (items.Count == 0)
            {
                Console.WriteLine("No recommended item is currently available.");
                return 0;
            }

            foreach (var item in items)
            {
                Console.WriteLine("- " + item.Id);
                Console.WriteLine("  path: " + item.PortablePath);
                Console.WriteLine("  expanded: " + item.ExpandedPath);
                Console.WriteLine("  type: " + PathTools.KindText(item.Kind));
            }
        }

        return 0;
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

        WriteJsonPayload(payload);
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

    private static object ToRecommendedOutputItem(RecommendedSyncItem item)
    {
        return new
        {
            id = item.Id,
            portablePath = item.PortablePath,
            expandedPath = item.ExpandedPath,
            type = PathTools.KindText(item.Kind),
            platform = PlatformText(item.Platform),
            nameKey = item.NameKey,
            reasonKey = item.ReasonKey
        };
    }

    private static void WriteJsonPayload(object payload)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        Console.WriteLine(JsonSerializer.Serialize(payload, options));
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
        Console.WriteLine("then restore the original paths with symbolic links.");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  check              Check links and cache items");
        Console.WriteLine("  check --json       Print machine-readable JSON");
        Console.WriteLine("  check --verbose    Print every configured item");
        Console.WriteLine("  status             Alias of check");
        Console.WriteLine("  recommended        List available recommended paths");
        Console.WriteLine("  recommended --json Print recommended paths as JSON");
        Console.WriteLine("  recommended --platform windows|macos|linux");
        Console.WriteLine("                     Override platform filtering");
        Console.WriteLine("  cache-root         Print current cache root");
        Console.WriteLine("  platform           Print detected platform");
        Console.WriteLine("  version            Print version");
        Console.WriteLine("  help, -h, -help, --help");
        Console.WriteLine("                     Print this help");
        Console.WriteLine();
        Console.WriteLine("AI and automation notes:");
        Console.WriteLine("  - The executable directory is the cache root.");
        Console.WriteLine("  - Use 'cache-root' before inspecting files.");
        Console.WriteLine("  - Use 'platform' before validating platform-specific behavior.");
        Console.WriteLine("  - Use 'check --json' for health monitoring.");
        Console.WriteLine("  - Use 'recommended --json' to inspect available recommended paths.");
        Console.WriteLine("  - Notify the user only when problemCount is greater than 0.");
        Console.WriteLine("  - CLI commands are read-only; GUI actions perform moves, links, restores, and undo.");
        return 0;
    }

    private static RecommendedPlatform ParseRecommendedPlatform(IReadOnlyList<string> options)
    {
        for (var index = 0; index < options.Count; index++)
        {
            var option = options[index];
            if (!option.StartsWith("--platform", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var value = "";
            var separatorIndex = option.IndexOf('=', StringComparison.Ordinal);
            if (separatorIndex >= 0)
            {
                value = option[(separatorIndex + 1)..];
            }
            else
            {
                value = option["--platform".Length..].TrimStart(':');
                if (string.IsNullOrWhiteSpace(value) && index + 1 < options.Count)
                {
                    value = options[index + 1];
                }
            }

            return value switch
            {
                "mac" or "macos" or "osx" => RecommendedPlatform.MacOS,
                "linux" => RecommendedPlatform.Linux,
                "win" or "windows" => RecommendedPlatform.Windows,
                "" => CurrentRecommendedPlatform(),
                _ => throw new InvalidOperationException("Unknown recommended platform: " + value)
            };
        }

        return CurrentRecommendedPlatform();
    }

    private static RecommendedPlatform CurrentRecommendedPlatform()
    {
        if (OperatingSystem.IsMacOS())
        {
            return RecommendedPlatform.MacOS;
        }

        if (OperatingSystem.IsLinux())
        {
            return RecommendedPlatform.Linux;
        }

        return RecommendedPlatform.Windows;
    }

    private static string PlatformText(RecommendedPlatform platform)
    {
        return platform switch
        {
            RecommendedPlatform.MacOS => "macos",
            RecommendedPlatform.Linux => "linux",
            _ => "windows"
        };
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
