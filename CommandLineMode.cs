namespace LinkShelf;

internal static class CommandLineMode
{
    private static string Version
    {
        get
        {
            var informationalVersion = (Attribute.GetCustomAttribute(
                typeof(CommandLineMode).Assembly,
                typeof(System.Reflection.AssemblyInformationalVersionAttribute)) as System.Reflection.AssemblyInformationalVersionAttribute)?.InformationalVersion;
            if (!string.IsNullOrWhiteSpace(informationalVersion))
            {
                return informationalVersion.Split('+')[0];
            }

            return typeof(CommandLineMode).Assembly.GetName().Version?.ToString(3) ?? "unknown";
        }
    }

    public static bool IsCommand(string[] args) => CommandLineRunner.IsCommand(args);

    public static int Run(string[] args) => CommandLineRunner.Run(args, Version, AppContext.BaseDirectory);
}
