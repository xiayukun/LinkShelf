using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LinkShelf.Models;
using LinkShelf.Services;
using Wpf.Ui.Controls;
namespace LinkShelf;

public partial class RecommendedItemsWindow : Wpf.Ui.Controls.FluentWindow
{
    private readonly LocalizationService text;
    private readonly ObservableCollection<RecommendedSyncItemRow> rows;

    public RecommendedItemsWindow(LocalizationService text, IReadOnlyList<RecommendedSyncItem> items)
    {
        this.text = text;
        rows = new ObservableCollection<RecommendedSyncItemRow>(
            items.Select(item => new RecommendedSyncItemRow
            {
                Item = item,
                Name = text.T(item.NameKey),
                PortablePath = item.PortablePath,
                ItemType = text.Code(PathTools.KindText(item.Kind)),
                Reason = text.T(item.ReasonKey)
            }));

        ExtendsContentIntoTitleBar = true;
        InitializeComponent();
        ApplyLanguage();
        RecommendedGrid.ItemsSource = rows;
    }

    public IReadOnlyList<RecommendedSyncItem> SelectedItems { get; private set; } = [];

    private void ApplyLanguage()
    {
        Title = text.T("recommended.title");
        HeadingText.Text = text.T("recommended.heading");
        DescriptionText.Text = text.T("recommended.description");
        SelectColumn.Header = text.T("recommended.select");
        NameColumn.Header = text.T("recommended.name");
        PathColumn.Header = text.T("recommended.path");
        TypeColumn.Header = text.T("recommended.type");
        ReasonColumn.Header = text.T("recommended.reason");
        AddSelectedButton.Content = text.T("recommended.addSelected");
        CancelButton.Content = text.T("conflict.cancel");
    }

    private void AddSelectedButton_Click(object sender, RoutedEventArgs e)
    {
        RecommendedGrid.CommitEdit();
        RecommendedGrid.CommitEdit(DataGridEditingUnit.Row, true);
        SelectedItems = rows.Where(row => row.IsSelected).Select(row => row.Item).ToList();
        if (SelectedItems.Count == 0)
        {
            LinkShelfMessageBox.Show(this, text.T("recommended.noSelection"), text.T("recommended.title"), System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
