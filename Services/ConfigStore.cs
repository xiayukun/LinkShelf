using System.Text.Encodings.Web;
using System.Text.Json;
using LinkShelf.Models;

namespace LinkShelf.Services;

public sealed class ConfigStore
{
    private readonly AppPaths paths;
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public ConfigStore(AppPaths paths)
    {
        this.paths = paths;
    }

    public SyncConfig Load()
    {
        if (!File.Exists(paths.ConfigPath))
        {
            return new SyncConfig();
        }

        var text = File.ReadAllText(paths.ConfigPath);
        var config = JsonSerializer.Deserialize<SyncConfig>(text, options) ?? new SyncConfig();
        NormalizeConfig(config);
        return config;
    }

    public void Save(SyncConfig config)
    {
        NormalizeConfig(config);
        config.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var text = JsonSerializer.Serialize(config, options);
        File.WriteAllText(paths.ConfigPath, text);
    }

    private static void NormalizeConfig(SyncConfig config)
    {
        config.Version = Math.Max(config.Version, 2);
        foreach (var item in config.Items)
        {
            item.ItemType = NormalizeType(item.ItemType);
            item.LinkMode = SyncConstants.LinkModeSymbolic;
            item.Status = NormalizeStatus(item.Status);
        }
    }

    private static string NormalizeType(string value)
    {
        return value.Equals(SyncConstants.TypeFile, StringComparison.OrdinalIgnoreCase)
            ? SyncConstants.TypeFile
            : SyncConstants.TypeDirectory;
    }

    private static string NormalizeStatus(string value)
    {
        return value.Equals(SyncConstants.StatusRemoved, StringComparison.OrdinalIgnoreCase)
            ? SyncConstants.StatusRemoved
            : SyncConstants.StatusEnabled;
    }
}
