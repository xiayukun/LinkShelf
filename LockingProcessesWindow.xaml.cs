using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LinkShelf.Services;
using ShowWhatProcessLocksFile.LockFinding;
using ShowWhatProcessLocksFile.LockFinding.Utils;
using ShowWhatProcessLocksFile.Utils;

namespace LinkShelf;

public enum LockingProcessesDecision
{
    Cancel,
    Continue
}

public sealed class LockingProcessRow
{
    public LockingProcessRow(ProcessInfo process)
    {
        Process = process;
    }

    public ProcessInfo Process { get; }
    public string? ProcessName => Process.ProcessName;
    public int ProcessId => Process.Pid;
    public string? UserName => Process.DomainAndUserName;
    public string? ExecutablePath => Process.ProcessExecutableFullName;
    public string LockedFilesText => string.Join(Environment.NewLine, Process.LockedFileFullNames);
}

public partial class LockingProcessesWindow : Window
{
    private readonly LocalizationService text;
    private readonly string sourcePath;
    private readonly string errorMessage;
    private ObservableCollection<LockingProcessRow> rows = [];

    public LockingProcessesWindow(LocalizationService text, string sourcePath, string errorMessage)
    {
        this.text = text;
        this.sourcePath = sourcePath;
        this.errorMessage = errorMessage;

        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) => await RefreshProcessesAsync();
    }

    public LockingProcessesDecision Decision { get; private set; } = LockingProcessesDecision.Cancel;

    private void ApplyLanguage()
    {
        Title = text.T("lockWindow.title");
        TitleText.Text = text.T("lockWindow.heading");
        PathText.Text = text.F("lockWindow.path", sourcePath);
        ErrorText.Text = text.F("lockWindow.error", errorMessage);
        StatusText.Text = text.T("lockWindow.scanning");
        ProcessNameColumn.Header = text.T("lockWindow.processName");
        ProcessIdColumn.Header = text.T("lockWindow.processId");
        UserColumn.Header = text.T("lockWindow.user");
        ExecutableColumn.Header = text.T("lockWindow.executable");
        TerminateSelectedMenuItem.Header = text.T("lockWindow.terminateSelected");
        RefreshButton.Content = text.T("lockWindow.refresh");
        TerminateAllAndContinueButton.Content = text.T("lockWindow.terminateAllContinue");
        CancelButton.Content = text.T("conflict.cancel");
    }

    private async Task RefreshProcessesAsync()
    {
        SetBusy(true, text.T("lockWindow.scanning"));
        try
        {
            var processes = await Task.Run(() => LockFinder.FindWhatProcessesLockPath(new CanonicalPath(sourcePath)).ToList());
            rows = new ObservableCollection<LockingProcessRow>(processes.Select(x => new LockingProcessRow(x)));
            ProcessGrid.ItemsSource = rows;
            LockedFilesBox.Text = "";
            StatusText.Text = rows.Count == 0
                ? text.T("lockWindow.noProcesses")
                : text.F("lockWindow.foundProcesses", rows.Count);
        }
        catch (Exception ex)
        {
            StatusText.Text = text.F("lockWindow.scanFailed", ex.Message);
        }
        finally
        {
            SetBusy(false, StatusText.Text);
        }
    }

    private void SetBusy(bool busy, string status)
    {
        Progress.Visibility = busy ? Visibility.Visible : Visibility.Collapsed;
        StatusText.Text = status;
        RefreshButton.IsEnabled = !busy;
        TerminateAllAndContinueButton.IsEnabled = !busy && rows.Count > 0;
        ProcessGrid.IsEnabled = !busy;
    }

    private void ProcessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ProcessGrid.SelectedItems.OfType<LockingProcessRow>().ToList();
        if (selected.Count == 0)
        {
            LockedFilesBox.Text = "";
            return;
        }

        LockedFilesBox.Text = string.Join(
            Environment.NewLine + Environment.NewLine,
            selected.Select(x => $"{x.ProcessName} (PID {x.ProcessId}){Environment.NewLine}{x.LockedFilesText}"));
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        await RefreshProcessesAsync();
    }

    private async void TerminateSelectedMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selected = ProcessGrid.SelectedItems.OfType<LockingProcessRow>().Select(x => x.Process).ToList();
        if (selected.Count == 0)
        {
            return;
        }

        await TerminateProcessesAsync(selected, continueAfterTerminate: false);
    }

    private async void TerminateAllAndContinueButton_Click(object sender, RoutedEventArgs e)
    {
        await TerminateProcessesAsync(rows.Select(x => x.Process).ToList(), continueAfterTerminate: true);
    }

    private async Task TerminateProcessesAsync(IReadOnlyList<ProcessInfo> processes, bool continueAfterTerminate)
    {
        if (processes.Count == 0)
        {
            return;
        }

        var result = System.Windows.MessageBox.Show(
            this,
            text.F("lockWindow.terminateConfirm", processes.Count),
            text.T("lockWindow.terminateConfirmTitle"),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Warning);
        if (result != MessageBoxResult.OK)
        {
            return;
        }

        SetBusy(true, text.T("lockWindow.terminating"));
        try
        {
            await Task.Run(() => ProcessKiller.Kill(processes));
            if (continueAfterTerminate)
            {
                await Task.Delay(800);
                Decision = LockingProcessesDecision.Continue;
                DialogResult = true;
                Close();
                return;
            }

            await RefreshProcessesAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(this, text.F("lockWindow.terminateFailed", ex.Message), text.T("lockWindow.title"), MessageBoxButton.OK, MessageBoxImage.Error);
            await RefreshProcessesAsync();
        }
        finally
        {
            if (IsVisible)
            {
                SetBusy(false, StatusText.Text);
            }
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = LockingProcessesDecision.Cancel;
        DialogResult = false;
        Close();
    }
}
