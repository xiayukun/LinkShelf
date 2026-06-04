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
        AddDirectoryMenuItem.Header = text.T("main.addDirectory");
        AddFileMenuItem.Header = text.T("main.addFile");
        RestoreButton.Content = text.T("main.restore");
        CheckButton.Content = text.T("main.check");
        UnsyncButton.Content = text.T("main.unsync");

        SetToolTip(AddSyncButton, "help.add");
        SetToolTip(AddDirectoryMenuItem, "help.addDirectory");
        SetToolTip(AddFileMenuItem, "help.addFile");
        SetToolTip(RestoreButton, "help.restore");
        SetToolTip(CheckButton, "help.check");
        SetToolTip(UnsyncButton, "help.unsync");

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

    private void AddDirectoryButton_Click(object sender, RoutedEventArgs e)
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

        RunOperation(text.T("op.addDirectory"), () =>
        {
            var operations = CreateOperations();
            operations.AddSyncItem(dialog.SelectedPath, SyncItemKind.Directory);
            operations.CheckAll();
        });
    }

    private void AddSyncButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        AddSyncButton.ContextMenu.PlacementTarget = AddSyncButton;
        AddSyncButton.ContextMenu.IsOpen = true;
    }

    private void AddSyncButton_Click(object sender, RoutedEventArgs e)
    {
        AddSyncButton.ContextMenu.PlacementTarget = AddSyncButton;
        AddSyncButton.ContextMenu.IsOpen = true;
    }

    private void AddDirectoryMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AddDirectoryButton_Click(sender, e);
    }

    private void AddFileMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AddFileButton_Click(sender, e);
    }

    private void AddFileButton_Click(object sender, RoutedEventArgs e)
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

        RunOperation(text.T("op.addFile"), () =>
        {
            var operations = CreateOperations();
            operations.AddSyncItem(dialog.FileName, SyncItemKind.File);
            operations.CheckAll();
        });
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

    private void UnsyncButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = GetSelectedItems();
        if (selectedItems.Count == 0)
        {
            System.Windows.MessageBox.Show(this, text.T("dialog.noSelection"), text.T("app.title"), MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = System.Windows.MessageBox.Show(
            this,
            text.F("dialog.unsyncConfirm", selectedItems.Count),
            text.T("main.unsync"),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.OK)
        {
            return;
        }

        RunOperation(text.T("op.removeRecords"), () =>
        {
            foreach (var item in selectedItems)
            {
                config.Items.Remove(item);
                log.Write($"Removed config record: {item.CacheName}");
            }
            store.Save(config);
            var operations = CreateOperations();
            operations.CheckAll();
        });
    }

    private FileOperations CreateOperations()
    {
        return new FileOperations(paths, config, store, log, ResolveConflict);
    }

    private ConflictDecision ResolveConflict(SyncItem item, string targetPath, string cachePath, string reason)
    {
        var window = new ConflictChoiceWindow(text, reason, targetPath, cachePath)
        {
            Owner = this
        };

        return window.ShowDialog() == true ? window.Decision : ConflictDecision.Cancel;
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
