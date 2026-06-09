using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using LinkShelf.Models;
using LinkShelf.Services;

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
        ProjectButton.Content = text.T("main.project");

        AddSyncButton.ToolTip = null;
        SetToolTip(AddRecommendedMenuItem, "help.addRecommended");
        SetToolTip(AddDirectoryMenuItem, "help.addDirectory");
        SetToolTip(AddFileMenuItem, "help.addFile");
        SetToolTip(CheckButton, "help.check");
        SetToolTip(RestoreButton, "help.restore");
        SetToolTip(RevertButton, "help.revert");
        SetToolTip(ProjectButton, "help.project");

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
        var dialog = new Microsoft.Win32.OpenFolderDialog
        {
            Title = text.T("dialog.pickDirectory"),
            Multiselect = true
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        foreach (var selectedPath in dialog.FolderNames)
        {
            log.WriteDiagnostic($"add-directory selected path={selectedPath}");
            if (!await RunAddOperationAsync(text.T("op.addDirectory"), selectedPath, SyncItemKind.Directory))
            {
                return;
            }
        }
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
            if (!await RunAddOperationAsync(text.F("op.addRecommendedItem", text.T(item.NameKey)), item.ExpandedPath, item.Kind))
            {
                return;
            }
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
            DereferenceLinks = false,
            Multiselect = true
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var shortcutFiles = dialog.FileNames.Where(IsWindowsShortcut).ToList();
        if (shortcutFiles.Count > 0)
        {
            var message = text.F("dialog.shortcutUnsupported", Environment.NewLine, string.Join(Environment.NewLine, shortcutFiles));
            AppendLog(text.F("log.skippedShortcuts", shortcutFiles.Count));
            System.Windows.MessageBox.Show(this, message, text.T("main.addFile"), MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        foreach (var fileName in dialog.FileNames)
        {
            if (IsWindowsShortcut(fileName))
            {
                continue;
            }

            log.WriteDiagnostic($"add-file selected path={fileName}");
            if (!await RunAddOperationAsync(text.T("op.addFile"), fileName, SyncItemKind.File))
            {
                return;
            }
        }
    }

    private void ProjectButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFolderDialog
        {
            Title = text.T("dialog.pickProjectionDirectory"),
            Multiselect = false
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            var executablePath = Environment.ProcessPath;
            if (string.IsNullOrWhiteSpace(executablePath))
            {
                executablePath = Process.GetCurrentProcess().MainModule?.FileName;
            }

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                throw new InvalidOperationException(text.T("projection.cannotLocateSelf"));
            }

            var projectionPath = ProjectionService.ProjectExecutableToDirectory(executablePath, dialog.FolderName);
            AppendLog(text.F("log.projected", projectionPath));
            System.Windows.MessageBox.Show(this, text.F("projection.created", projectionPath), text.T("main.project"), MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (ProjectionException ex)
        {
            var message = ex.Detail is null ? text.T(ex.TextKey) : text.F(ex.TextKey, ex.Detail);
            AppendLog(text.F("log.failed", text.T("main.project"), message));
            System.Windows.MessageBox.Show(this, message, text.T("main.project"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            AppendLog(text.F("log.failed", text.T("main.project"), ex.Message));
            System.Windows.MessageBox.Show(this, ex.Message, text.T("main.project"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void RestoreButton_Click(object sender, RoutedEventArgs e)
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

        var untrackedItemCount = selectedItems.Count(IsUntrackedCacheItem);
        var hasUntrackedItems = untrackedItemCount > 0;
        var scopeText = hasUntrackedItems
            ? untrackedItemCount == selectedItems.Count
                ? text.F("dialog.restoreUntrackedConfirm", untrackedItemCount)
                : text.F("dialog.restoreMixedUntrackedConfirm", selectedItems.Count, untrackedItemCount)
            : selectedItems.Count > 0
                ? text.F("dialog.restoreSelected", selectedItems.Count)
                : text.T("dialog.restoreAll");
        var result = System.Windows.MessageBox.Show(
            this,
            scopeText + Environment.NewLine + text.T(hasUntrackedItems ? "dialog.restoreUntrackedConfirmSuffix" : "dialog.restoreConfirmSuffix"),
            text.T("main.restore"),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.OK)
        {
            return;
        }

        await RunRestoreOperationAsync(text.T("op.restoreLinks"), itemsToRestore);
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
        RunOperation(text.T("op.checkStatus"), () =>
        {
            var operations = CreateOperations();
            operations.CheckAll();
        });
    }

    private async void RevertButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            log.WriteDiagnostic("revert-click stage=begin");
            var selectedItems = GetSelectedItems();
            log.WriteDiagnostic($"revert-click stage=selected count={selectedItems.Count} names={string.Join("|", selectedItems.Select(x => x.CacheName))}");
            if (selectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show(this, text.T("dialog.noSelection"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var hasUntrackedItems = selectedItems.Any(IsUntrackedCacheItem);
            MessageBoxResult result;
            if (hasUntrackedItems && selectedItems.All(IsUntrackedCacheItem))
            {
                log.WriteDiagnostic("revert-click stage=skip-generic-confirm all-untracked=true");
                result = MessageBoxResult.OK;
            }
            else
            {
                var message = hasUntrackedItems
                    ? text.F("dialog.revertMixedUntrackedConfirm", selectedItems.Count, selectedItems.Count(IsUntrackedCacheItem))
                    : text.F("dialog.revertConfirm", selectedItems.Count);
                log.WriteDiagnostic($"revert-click stage=generic-confirm-show hasUntracked={hasUntrackedItems}");
                result = System.Windows.MessageBox.Show(
                    this,
                    message,
                    text.T("main.revert"),
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                log.WriteDiagnostic($"revert-click stage=generic-confirm-closed result={result}");
            }

            if (result != MessageBoxResult.OK)
            {
                log.WriteDiagnostic("revert-click stage=canceled");
                return;
            }

            log.WriteDiagnostic("revert-click stage=run-operation");
            await RunRevertOperationAsync(text.T("op.revertItems"), selectedItems);
            log.WriteDiagnostic("revert-click stage=done");
        }
        catch (Exception ex)
        {
            log.WriteDiagnostic($"revert-click stage=exception type={ex.GetType().FullName} message={ex.Message} stack={ex}");
            AppendLog(text.F("log.failed", text.T("main.revert"), ex.Message));
            System.Windows.MessageBox.Show(this, ex.Message, text.T("main.revert"), MessageBoxButton.OK, MessageBoxImage.Error);
            config = store.Load();
            ReloadGrid();
        }
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

    private async Task<bool> RunAddOperationAsync(string name, string sourcePath, SyncItemKind kind)
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
                return true;
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
                    return false;
                }

                AppendLog(text.F("log.failed", name, ex.Message));
                System.Windows.MessageBox.Show(this, ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
                config = store.Load();
                ReloadGrid();
                return false;
            }
        }
    }

    private async Task RunRestoreOperationAsync(string name, IReadOnlyList<SyncItem> itemsToRestore)
    {
        AppendLog(text.F("log.begin", name));

        foreach (var item in itemsToRestore)
        {
            var restoreItem = item;
            if (IsUntrackedCacheItem(restoreItem))
            {
                restoreItem = CreateConfigRecordForUntrackedCacheItem(restoreItem);
                if (restoreItem is null)
                {
                    config = store.Load();
                    ReloadGrid();
                    return;
                }
            }

            var targetPath = PathTools.ExpandPortablePath(restoreItem.OriginalPath, paths.UserHome);
            var attempt = 0;

            while (true)
            {
                attempt++;
                try
                {
                    log.WriteDiagnostic($"restore attempt={attempt} stage=begin name={name} target={targetPath} cacheName={restoreItem.CacheName}");
                    await Task.Run(() =>
                    {
                        var operations = CreateOperations();
                        operations.RestoreItem(restoreItem);
                    });
                    store.Save(config);
                    log.WriteDiagnostic($"restore attempt={attempt} stage=item-finished target={targetPath}");
                    break;
                }
                catch (Exception ex)
                {
                    log.WriteDiagnostic($"restore attempt={attempt} stage=exception target={targetPath} type={ex.GetType().FullName} message={ex.Message} stack={ex}");
                    if (IsAccessDenied(ex))
                    {
                        log.WriteDiagnostic($"restore attempt={attempt} stage=access-denied-lock-window-show target={targetPath}");
                        var decision = ShowLockingProcessesWindow(targetPath, ex.Message);
                        log.WriteDiagnostic($"restore attempt={attempt} stage=access-denied-lock-window-closed decision={decision} target={targetPath}");

                        if (decision == LockingProcessesDecision.Continue)
                        {
                            AppendLog(text.F("log.retryLockedPath", targetPath));
                            continue;
                        }

                        AppendLog(text.F("log.lockedPathCanceled", targetPath));
                        config = store.Load();
                        ReloadGrid();
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

        await Task.Run(() =>
        {
            var operations = CreateOperations();
            operations.CheckAll();
        });
        config = store.Load();
        ReloadGrid();
        AppendLog(text.F("log.done", name));
    }

    private async Task RunRevertOperationAsync(string name, IReadOnlyList<SyncItem> itemsToRevert)
    {
        AppendLog(text.F("log.begin", name));

        foreach (var item in itemsToRevert)
        {
            if (IsUntrackedCacheItem(item))
            {
                var cachePathForUntracked = Path.Combine(paths.CacheRoot, item.CacheName);
                var confirmResult = System.Windows.MessageBox.Show(
                    this,
                    text.F("dialog.untrackedCacheDeleteConfirm", Environment.NewLine, cachePathForUntracked),
                    text.T("main.revert"),
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);

                if (confirmResult != MessageBoxResult.OK)
                {
                    return;
                }

                var untrackedAttempt = 0;
                while (true)
                {
                    untrackedAttempt++;
                    try
                    {
                        log.WriteDiagnostic($"revert-untracked attempt={untrackedAttempt} stage=delete-begin name={name} cache={cachePathForUntracked} cacheName={item.CacheName}");
                        await Task.Run(() => DeletePath(cachePathForUntracked, log, $"revert-untracked attempt={untrackedAttempt} cacheName={item.CacheName}"));
                        log.WriteDiagnostic($"revert-untracked attempt={untrackedAttempt} stage=delete-finished cache={cachePathForUntracked}");
                        AppendLog(text.F("log.removedUntrackedCacheItem", item.CacheName));
                        break;
                    }
                    catch (Exception ex)
                    {
                        log.WriteDiagnostic($"revert-untracked attempt={untrackedAttempt} stage=exception cache={cachePathForUntracked} type={ex.GetType().FullName} message={ex.Message} stack={ex}");
                        if (IsAccessDenied(ex))
                        {
                            var decision = ShowLockingProcessesWindow(cachePathForUntracked, ex.Message);
                            log.WriteDiagnostic($"revert-untracked attempt={untrackedAttempt} stage=access-denied-lock-window-closed decision={decision} cache={cachePathForUntracked}");

                            if (decision == LockingProcessesDecision.Continue)
                            {
                                AppendLog(text.F("log.retryLockedPath", cachePathForUntracked));
                                continue;
                            }

                            AppendLog(text.F("log.lockedPathCanceled", cachePathForUntracked));
                            config = store.Load();
                            ReloadGrid();
                            return;
                        }

                        AppendLog(text.F("log.failed", name, ex.Message));
                        System.Windows.MessageBox.Show(this, ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
                        config = store.Load();
                        ReloadGrid();
                        return;
                    }
                }

                continue;
            }

            var targetPath = PathTools.ExpandPortablePath(item.OriginalPath, paths.UserHome);
            var cachePath = Path.Combine(paths.CacheRoot, item.CacheName);
            var attempt = 0;

            if (!PathExistsForLockScan(cachePath))
            {
                var result = System.Windows.MessageBox.Show(
                    this,
                    text.F("dialog.revertMissingCacheConfirm", Environment.NewLine, cachePath, item.OriginalPath),
                    text.T("main.revert"),
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.OK)
                {
                    return;
                }

                config.Items.Remove(item);
                store.Save(config);
                AppendLog(text.F("log.removedMissingCacheRecord", item.CacheName));
                continue;
            }

            while (true)
            {
                attempt++;
                try
                {
                    log.WriteDiagnostic($"revert attempt={attempt} stage=begin name={name} target={targetPath} cache={cachePath} cacheName={item.CacheName}");
                    await Task.Run(() =>
                    {
                        var operations = CreateOperations();
                        operations.RevertItem(item);
                    });
                    store.Save(config);
                    log.WriteDiagnostic($"revert attempt={attempt} stage=item-finished target={targetPath} cache={cachePath}");
                    break;
                }
                catch (Exception ex)
                {
                    log.WriteDiagnostic($"revert attempt={attempt} stage=exception target={targetPath} cache={cachePath} type={ex.GetType().FullName} message={ex.Message} stack={ex}");
                    if (IsAccessDenied(ex))
                    {
                        var scanPath = PathExistsForLockScan(targetPath) ? targetPath : cachePath;
                        log.WriteDiagnostic($"revert attempt={attempt} stage=access-denied-lock-window-show scanPath={scanPath}");
                        var decision = ShowLockingProcessesWindow(scanPath, ex.Message);
                        log.WriteDiagnostic($"revert attempt={attempt} stage=access-denied-lock-window-closed decision={decision} scanPath={scanPath}");

                        if (decision == LockingProcessesDecision.Continue)
                        {
                            AppendLog(text.F("log.retryLockedPath", scanPath));
                            continue;
                        }

                        AppendLog(text.F("log.lockedPathCanceled", scanPath));
                        config = store.Load();
                        ReloadGrid();
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

        await Task.Run(() =>
        {
            var operations = CreateOperations();
            operations.CheckAll();
        });
        config = store.Load();
        ReloadGrid();
        AppendLog(text.F("log.done", name));
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

    private static bool PathExistsForLockScan(string path)
    {
        return Directory.Exists(path) || File.Exists(path);
    }

    private static bool IsWindowsShortcut(string path)
    {
        return string.Equals(Path.GetExtension(path), ".lnk", StringComparison.OrdinalIgnoreCase);
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
                .Concat(GetUntrackedCacheItems())
                .Select(x => new SyncItemRow(x, text.Code)));
        ItemsGrid.ItemsSource = visibleItems;
        ItemsGrid.Items.Refresh();
    }

    private List<SyncItem> GetUntrackedCacheItems()
    {
        var configuredNames = config.Items
            .Where(x => x.Status == SyncConstants.StatusEnabled)
            .Select(x => x.CacheName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var ignoredNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            AppPaths.ConfigFileName,
            AppPaths.LogDirectoryName,
            AppPaths.BackupDirectoryName,
            Path.GetFileName(Environment.ProcessPath ?? "LinkShelf.exe"),
            "LinkShelf.exe"
        };

        var items = new List<SyncItem>();
        foreach (var entry in Directory.EnumerateFileSystemEntries(paths.CacheRoot))
        {
            var name = Path.GetFileName(entry);
            if (string.IsNullOrWhiteSpace(name) || ignoredNames.Contains(name) || configuredNames.Contains(name))
            {
                continue;
            }

            try
            {
                var attributes = File.GetAttributes(entry);
                var kind = attributes.HasFlag(FileAttributes.Directory) ? SyncItemKind.Directory : SyncItemKind.File;
                items.Add(new SyncItem
                {
                    CacheName = name,
                    OriginalPath = "",
                    ItemType = PathTools.KindText(kind),
                    Status = SyncConstants.StatusUntrackedCacheItem,
                    SourceMachine = Environment.MachineName,
                    UpdatedAt = File.GetLastWriteTime(entry).ToString("yyyy-MM-dd HH:mm:ss"),
                    LastOperation = SyncConstants.CheckUntrackedCacheItem,
                    CheckResult = SyncConstants.CheckUntrackedCacheItem,
                    CachePath = entry,
                    ExpandedOriginalPath = ""
                });
            }
            catch
            {
                // Skip entries that disappear or cannot be inspected while refreshing the grid.
            }
        }

        return items.OrderBy(x => x.CacheName, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private SyncItem? CreateConfigRecordForUntrackedCacheItem(SyncItem untrackedItem)
    {
        var targetPath = ChooseOriginalPathForUntrackedCacheItem(untrackedItem);
        if (targetPath is null)
        {
            return null;
        }

        if (!CanUseOriginalPath(targetPath))
        {
            System.Windows.MessageBox.Show(this, text.T("dialog.invalidOriginalPath"), text.T("main.restore"), MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        var portable = PathTools.ToPortablePath(targetPath, paths.UserHome);
        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var item = new SyncItem
        {
            CacheName = untrackedItem.CacheName,
            OriginalPath = portable,
            ItemType = untrackedItem.ItemType,
            LinkMode = SyncConstants.LinkModeSymbolic,
            Status = SyncConstants.StatusEnabled,
            SourceMachine = Environment.MachineName,
            CreatedAt = now,
            UpdatedAt = now,
            LastOperation = "complete-untracked-cache-item"
        };

        config.Items.RemoveAll(x =>
            string.Equals(x.CacheName, item.CacheName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(x.OriginalPath, item.OriginalPath, StringComparison.OrdinalIgnoreCase));
        config.Items.Add(item);
        store.Save(config);
        AppendLog(text.F("log.completedUntrackedCacheRecord", item.CacheName, item.OriginalPath));
        return item;
    }

    private string? ChooseOriginalPathForUntrackedCacheItem(SyncItem item)
    {
        var kind = PathTools.KindFromText(item.ItemType);
        if (kind == SyncItemKind.Directory)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog
            {
                Title = text.F("dialog.pickUntrackedDirectoryParent", item.CacheName),
                Multiselect = false
            };

            return dialog.ShowDialog(this) == true
                ? Path.Combine(dialog.FolderName, item.CacheName)
                : null;
        }

        var fileDialog = new Microsoft.Win32.SaveFileDialog
        {
            Title = text.F("dialog.pickUntrackedFilePath", item.CacheName),
            FileName = item.CacheName,
            Filter = text.T("dialog.allFilesFilter"),
            CheckPathExists = true,
            OverwritePrompt = false
        };

        return fileDialog.ShowDialog(this) == true ? fileDialog.FileName : null;
    }

    private bool CanUseOriginalPath(string path)
    {
        var normalized = PathTools.Normalize(path);
        var cacheRoot = PathTools.Normalize(paths.CacheRoot);
        return !PathTools.IsSamePath(normalized, cacheRoot) &&
               !PathTools.IsSameOrChild(normalized, cacheRoot) &&
               !PathTools.IsSameOrChild(cacheRoot, normalized);
    }

    private List<SyncItem> GetSelectedItems()
    {
        return ItemsGrid.SelectedItems.OfType<SyncItemRow>().Select(x => x.Item).ToList();
    }

    private static bool IsUntrackedCacheItem(SyncItem item)
    {
        return string.Equals(item.Status, SyncConstants.StatusUntrackedCacheItem, StringComparison.OrdinalIgnoreCase);
    }

    private static void DeletePath(string path, LogService? log = null, string context = "delete")
    {
        var stopwatch = Stopwatch.StartNew();
        log?.WriteDiagnostic($"{context} stage=inspect-begin path={path} existsFile={File.Exists(path)} existsDirectory={Directory.Exists(path)}");
        var attributes = File.GetAttributes(path);
        log?.WriteDiagnostic($"{context} stage=inspect-finished path={path} attributes={attributes}");

        if (attributes.HasFlag(FileAttributes.Directory))
        {
            DeleteDirectory(path, attributes, log, context, depth: 0);
        }
        else
        {
            var info = new FileInfo(path);
            log?.WriteDiagnostic($"{context} stage=file-delete-begin path={path} length={info.Length} lastWrite={info.LastWriteTime:yyyy-MM-dd HH:mm:ss.fff}");
            File.Delete(path);
            log?.WriteDiagnostic($"{context} stage=file-delete-finished path={path} elapsedMs={stopwatch.ElapsedMilliseconds}");
        }
    }

    private static void DeleteDirectory(string path, FileAttributes attributes, LogService? log, string context, int depth)
    {
        var stopwatch = Stopwatch.StartNew();
        log?.WriteDiagnostic($"{context} stage=directory-delete-begin depth={depth} path={path} attributes={attributes}");

        if (attributes.HasFlag(FileAttributes.ReparsePoint))
        {
            log?.WriteDiagnostic($"{context} stage=directory-reparse-delete-begin depth={depth} path={path}");
            Directory.Delete(path, recursive: false);
            log?.WriteDiagnostic($"{context} stage=directory-reparse-delete-finished depth={depth} path={path} elapsedMs={stopwatch.ElapsedMilliseconds}");
            return;
        }

        var entryCount = 0;
        log?.WriteDiagnostic($"{context} stage=directory-enumerate-begin depth={depth} path={path}");
        foreach (var entry in Directory.EnumerateFileSystemEntries(path))
        {
            entryCount++;
            log?.WriteDiagnostic($"{context} stage=directory-entry depth={depth} index={entryCount} path={entry}");
            var entryAttributes = File.GetAttributes(entry);
            if (entryAttributes.HasFlag(FileAttributes.Directory))
            {
                DeleteDirectory(entry, entryAttributes, log, context, depth + 1);
                continue;
            }

            var info = new FileInfo(entry);
            log?.WriteDiagnostic($"{context} stage=file-delete-begin depth={depth} path={entry} length={info.Length} lastWrite={info.LastWriteTime:yyyy-MM-dd HH:mm:ss.fff}");
            File.Delete(entry);
            log?.WriteDiagnostic($"{context} stage=file-delete-finished depth={depth} path={entry}");
        }

        log?.WriteDiagnostic($"{context} stage=directory-enumerate-finished depth={depth} path={path} entries={entryCount}");
        log?.WriteDiagnostic($"{context} stage=directory-remove-empty-begin depth={depth} path={path}");
        Directory.Delete(path, recursive: false);
        log?.WriteDiagnostic($"{context} stage=directory-remove-empty-finished depth={depth} path={path} elapsedMs={stopwatch.ElapsedMilliseconds}");
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
