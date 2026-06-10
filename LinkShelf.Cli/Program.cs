using System.Reflection;
using LinkShelf;

var version = GetVersion();
return CommandLineRunner.Run(args, version, AppContext.BaseDirectory);

static string GetVersion()
{
    var informationalVersion = (Attribute.GetCustomAttribute(
        Assembly.GetExecutingAssembly(),
        typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute)?.InformationalVersion;
    if (!string.IsNullOrWhiteSpace(informationalVersion))
    {
        return informationalVersion.Split('+')[0];
    }

    return Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "unknown";
}
