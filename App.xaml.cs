using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;
using LinkShelf.Services;

namespace LinkShelf;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var text = new LocalizationService();
        InstallUnhandledExceptionLogging(text);

        try
        {
            base.OnStartup(e);

            if (CommandLineMode.IsCommand(e.Args))
            {
                Shutdown(CommandLineMode.Run(e.Args));
                return;
            }

            ConsoleTools.HideConsoleForGui();

            if (!IsAdministrator())
            {
                RelaunchAsAdministrator(text);
                Shutdown();
                return;
            }

            var window = new MainWindow();
            window.Show();
        }
        catch (Exception ex)
        {
            WriteStartupError(ex);
            System.Windows.MessageBox.Show(text.T("app.startupFailed"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private void InstallUnhandledExceptionLogging(LocalizationService text)
    {
        DispatcherUnhandledException += (_, e) =>
        {
            WriteUnhandledError("dispatcher-unhandled", e.Exception);
            try
            {
                System.Windows.MessageBox.Show(e.Exception.Message, text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
            }

            e.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                WriteUnhandledError("appdomain-unhandled", ex);
            }
            else
            {
                WriteUnhandledText("appdomain-unhandled", e.ExceptionObject?.ToString() ?? "Unknown exception object.");
            }
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            WriteUnhandledError("task-unobserved", e.Exception);
            e.SetObserved();
        };
    }

    private static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static void RelaunchAsAdministrator(LocalizationService text)
    {
        var executable = Environment.ProcessPath;
        if (string.IsNullOrWhiteSpace(executable))
        {
            System.Windows.MessageBox.Show(text.T("app.cannotLocateSelf"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            var info = new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = AppContext.BaseDirectory
            };
            Process.Start(info);
        }
        catch
        {
            System.Windows.MessageBox.Show(text.T("app.adminRequired"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private static void WriteStartupError(Exception ex)
    {
        try
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, AppPaths.LogDirectoryName);
            Directory.CreateDirectory(logDirectory);
            var logPath = Path.Combine(logDirectory, "startup-error.log");
            File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]{Environment.NewLine}{ex}{Environment.NewLine}{Environment.NewLine}");
        }
        catch
        {
        }
    }

    private static void WriteUnhandledError(string source, Exception ex)
    {
        WriteUnhandledText(source, ex.ToString());
    }

    private static void WriteUnhandledText(string source, string detail)
    {
        try
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, AppPaths.LogDirectoryName);
            Directory.CreateDirectory(logDirectory);
            var logPath = Path.Combine(logDirectory, "unhandled-error.log");
            File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {source}{Environment.NewLine}{detail}{Environment.NewLine}{Environment.NewLine}");
        }
        catch
        {
        }
    }
}

internal static class ConsoleTools
{
    [DllImport("kernel32.dll")]
    private static extern bool FreeConsole();

    public static void HideConsoleForGui()
    {
        try
        {
            FreeConsole();
        }
        catch
        {
        }
    }
}
