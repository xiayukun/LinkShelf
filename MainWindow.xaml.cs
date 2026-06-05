using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LinkShelf.Models;
using LinkShelf.Services;
using Forms = System.Windows.Forms;

namespace LinkShelf;

public partial class MainWindow : Window
{
    private readonly AppPaths paths;
    private readonly ConfigStore store;
    private readonly LogService log;
    private readonly LocalizationService text = new();
    private SyncConfig config;
    private ObservableCollection<SyncItemRow> visibleItems = [];

    public MainWindow()
    {
        InitializeComponent();

        paths = new AppPaths(AppContext.BaseDirectory);
        paths.EnsureSystemDirectories();
        store = new ConfigStore(paths);
        config = store.Load();
        log = new LogService(paths);
        log.MessageAdded += AppendLog;

        SelectInitialLanguage();
        ApplyLanguage();
        ReloadGrid();
        AppendLog(text.T("log.started"));
    }

    private void SelectInitialLanguage()
    {
        RefreshLanguageSelectorLabels();
        var tag = text.Language == AppLanguage.Chinese ? "zh" : "en";
        foreach (ComboBoxItem item in LanguageSelector.Items)
        {
            if (string.Equals(item.Tag?.ToString(), tag, StringComparison.OrdinalIgnoreCase))
            {
                LanguageSelector.SelectedItem = item;
                break;
            }
        }
    }

    private void ApplyLanguage()
    {
        RefreshLanguageSelectorLabels();
        Title = text.T("app.title");
        TitleText.Text = text.T("app.title");
        CacheRootText.Text = text.F("main.cacheRoot", paths.CacheRoot);
        ConfigText.Text = text.F("main.configFile", paths.ConfigPath);
        LanguageLabel.Text = text.T("main.language");

        AddSyncButton.Content = text.T("main.add");
        AddRecommendedMenuItem.Header = text.T("main.addRecommended");
        AddDirectoryMenuItem.Header = text.T("main.addDirectory");
        AddFileMenuItem.Header = text.T("main.addFile");
        CheckButton.Content = text.T("main.check");
        RestoreButton.Content = text.T("main.restore");
        RevertButton.Content = text.T("main.revert");

        AddSyncButton.ToolTip = null;
        SetToolTip(AddRecommendedMenuItem, "help.addRecommended");
        SetToolTip(AddDirectoryMenuItem, "help.addDirectory");
        SetToolTip(AddFileMenuItem, "help.addFile");
        SetToolTip(CheckButton, "help.check");
        SetToolTip(RestoreButton, "help.restore");
        SetToolTip(RevertButton, "help.revert");

        CacheNameColumn.Header = text.T("grid.cacheName");
        OriginalPathColumn.Header = text.T("grid.originalPath");
        TypeColumn.Header = text.T("grid.type");
        StatusColumn.Header = text.T("grid.status");
        CheckResultColumn.Header = text.T("grid.checkResult");
        SourceMachineColumn.Header = text.T("grid.sourceMachine");
        LastOperationColumn.Header = text.T("grid.lastOperation");
        UpdatedAtColumn.Header = text.T("grid.updatedAt");
        CachePathColumn.Header = text.T("grid.cachePath");
        LocalTargetColumn.Header = text.T("grid.localTarget");
        LogTitleText.Text = text.T("main.log");
    }

    private void RefreshLanguageSelectorLabels()
    {
        foreach (ComboBoxItem item in LanguageSelector.Items)
        {
            item.Content = item.Tag?.ToString() == "zh" ? "中文" : "English";
        }
    }

    private void SetToolTip(FrameworkElement element, string key)
    {
        element.ToolTip = text.T(key);
    }

    private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsInitialized || LanguageSelector.SelectedItem is not ComboBoxItem item)
        {
            return;
        }

        text.Language = item.Tag?.ToString() == "zh" ? AppLanguage.Chinese : AppLanguage.English;
        ApplyLanguage();
        ReloadGrid();
    }

    private async void AddDirectoryButton_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new Forms.FolderBrowserDialog
        {
            Description = text.T("dialog.pickDirectory"),
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        if (dialog.ShowDialog() != Forms.DialogResult.OK)
        {
            return;
        }

        log.WriteDiagnostic($"add-directory selected path={dialog.SelectedPath}");
        await RunAddOperationAsync(text.T("op.addDirectory"), dialog.SelectedPath, SyncItemKind.Directory);
    }

    private void AddSyncButton_Click(object sender, RoutedEventArgs e)
    {
        AddSyncButton.ContextMenu.PlacementTarget = AddSyncButton;
        AddSyncButton.ContextMenu.IsOpen = true;
    }

    private async void AddRecommendedMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var candidates = RecommendedSyncItems.GetAvailable(paths, config);
        if (candidates.Count == 0)
        {
            System.Windows.MessageBox.Show(this, text.T("recommended.noItems"), text.T("recommended.title"), MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new RecommendedItemsWindow(text, candidates)
        {
            Owner = this
        };

        if (window.ShowDialog() != true)
        {
            return;
        }

        foreach (var item in window.SelectedItems)
        {
            await RunAddOperationAsync(text.F("op.addRecommendedItem", text.T(item.NameKey)), item.ExpandedPath, item.Kind);
        }
    }

    private void AddDirectoryMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AddDirectoryButton_Click(sender, e);
    }

    private void AddFileMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AddFileButton_Click(sender, e);
    }

    private async void AddFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = text.T("dialog.pickFile"),
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        log.WriteDiagnostic($"add-file selected path={dialog.FileName}");
        await RunAddOperationAsync(text.T("op.addFile"), dialog.FileName, SyncItemKind.File);
    }

    private void RestoreButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = GetSelectedItems();
        var itemsToRestore = selectedItems.Count > 0
            ? selectedItems
            : config.Items.Where(x => x.Status == SyncConstants.StatusEnabled).ToList();

        if (itemsToRestore.Count == 0)
        {
            System.Windows.MessageBox.Show(this, text.T("dialog.noItems"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var scopeText = selectedItems.Count > 0
            ? text.F("dialog.restoreSelected", selectedItems.Count)
            : text.T("dialog.restoreAll");
        var result = System.Windows.MessageBox.Show(
            this,
            scopeText + Environment.NewLine + text.T("dialog.restoreConfirmSuffix"),
            text.T("main.restore"),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.OK)
        {
            return;
        }

        RunOperation(text.T("op.restoreLinks"), () =>
        {
            var operations = CreateOperations();
            foreach (var item in itemsToRestore)
            {
                operations.RestoreItem(item);
            }
            store.Save(config);
            operations.CheckAll();
        });
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
        RunOperation(text.T("op.checkStatus"), () =>
        {
            var operations = CreateOperations();
            operations.CheckAll();
        });
    }

    private void RevertButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = GetSelectedItems();
        if (selectedItems.Count == 0)
        {
            System.Windows.MessageBox.Show(this, text.T("dialog.noSelection"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = System.Windows.MessageBox.Show(
            this,
            text.F("dialog.revertConfirm", selectedItems.Count),
            text.T("main.revert"),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.OK)
        {
            return;
        }

        RunOperation(text.T("op.revertItems"), () =>
        {
            var operations = CreateOperations();
            foreach (var item in selectedItems)
            {
                operations.RevertItem(item);
            }
            store.Save(config);
            operations.CheckAll();
        });
    }

    private FileOperations CreateOperations()
    {
        return new FileOperations(paths, config, store, log, ResolveConflict);
    }

    private ConflictDecision ResolveConflict(SyncItem item, string targetPath, string cachePath, string reason)
    {
        if (!Dispatcher.CheckAccess())
        {
            return Dispatcher.Invoke(() => ResolveConflict(item, targetPath, cachePath, reason));
        }

        var window = new ConflictChoiceWindow(text, reason, targetPath, cachePath)
        {
            Owner = this
        };

        return window.ShowDialog() == true ? window.Decision : ConflictDecision.Cancel;
    }

    private async Task RunAddOperationAsync(string name, string sourcePath, SyncItemKind kind)
    {
        var attempt = 0;
        while (true)
        {
            attempt++;
            try
            {
                log.WriteDiagnostic($"add attempt={attempt} stage=begin name={name} kind={kind} source={sourcePath}");
                AppendLog(text.F("log.begin", name));
                log.WriteDiagnostic($"add attempt={attempt} stage=operation-task-start");
                await Task.Run(() =>
                {
                    log.WriteDiagnostic($"add attempt={attempt} stage=create-operations");
                    var operations = CreateOperations();
                    log.WriteDiagnostic($"add attempt={attempt} stage=add-sync-item-start");
                    operations.AddSyncItem(sourcePath, kind);
                    log.WriteDiagnostic($"add attempt={attempt} stage=check-all-start");
                    operations.CheckAll();
                    log.WriteDiagnostic($"add attempt={attempt} stage=operation-task-finished");
                });
                log.WriteDiagnostic($"add attempt={attempt} stage=reload-config-start");
                config = store.Load();
                log.WriteDiagnostic($"add attempt={attempt} stage=reload-grid-start");
                ReloadGrid();
                log.WriteDiagnostic($"add attempt={attempt} stage=done");
                AppendLog(text.F("log.done", name));
                return;
            }
            catch (Exception ex)
            {
                log.WriteDiagnostic($"add attempt={attempt} stage=exception type={ex.GetType().FullName} message={ex.Message} stack={ex}");
                if (IsAccessDenied(ex))
                {
                    log.WriteDiagnostic($"add attempt={attempt} stage=access-denied-lock-window-show");
                    var decision = ShowLockingProcessesWindow(sourcePath, ex.Message);
                    log.WriteDiagnostic($"add attempt={attempt} stage=access-denied-lock-window-closed decision={decision}");
                    config = store.Load();
                    ReloadGrid();

                    if (decision == LockingProcessesDecision.Continue)
                    {
                        AppendLog(text.F("log.retryLockedPath", sourcePath));
                        continue;
                    }

                    AppendLog(text.F("log.lockedPathCanceled", sourcePath));
                    return;
                }

                AppendLog(text.F("log.failed", name, ex.Message));
                System.Windows.MessageBox.Show(this, ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
                config = store.Load();
                ReloadGrid();
                return;
            }
        }
    }

    private static bool IsAccessDenied(Exception ex)
    {
        if (ex is UnauthorizedAccessException)
        {
            return true;
        }

        return ex.HResult == unchecked((int)0x80070005) ||
               ex.Message.Contains("access to the path", StringComparison.OrdinalIgnoreCase) &&
               ex.Message.Contains("denied", StringComparison.OrdinalIgnoreCase);
    }

    private LockingProcessesDecision ShowLockingProcessesWindow(string sourcePath, string errorMessage)
    {
        var window = new LockingProcessesWindow(text, sourcePath, errorMessage)
        {
            Owner = this
        };

        return window.ShowDialog() == true ? window.Decision : LockingProcessesDecision.Cancel;
    }

    private void RunOperation(string name, Action action)
    {
        try
        {
            AppendLog(text.F("log.begin", name));
            action();
            config = store.Load();
            ReloadGrid();
            AppendLog(text.F("log.done", name));
        }
        catch (Exception ex)
        {
            AppendLog(text.F("log.failed", name, ex.Message));
            System.Windows.MessageBox.Show(this, ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            config = store.Load();
            ReloadGrid();
        }
    }

    private void ReloadGrid()
    {
        var operations = CreateOperations();
        foreach (var item in config.Items)
        {
            operations.CheckItem(item);
        }

        visibleItems = new ObservableCollection<SyncItemRow>(
            config.Items
                .Where(x => x.Status == SyncConstants.StatusEnabled)
                .Select(x => new SyncItemRow(x, text.Code)));
        ItemsGrid.ItemsSource = visibleItems;
        ItemsGrid.Items.Refresh();
    }

    private List<SyncItem> GetSelectedItems()
    {
        return ItemsGrid.SelectedItems.OfType<SyncItemRow>().Select(x => x.Item).ToList();
    }

    private void AppendLog(string message)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => AppendLog(message));
            return;
        }

        var line = message.StartsWith("[", StringComparison.Ordinal) ? message : $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        LogBox.AppendText(line + Environment.NewLine);
        LogBox.ScrollToEnd();
    }
}
