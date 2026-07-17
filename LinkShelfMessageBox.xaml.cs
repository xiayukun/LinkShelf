using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace LinkShelf;

public partial class LinkShelfMessageBox : Wpf.Ui.Controls.FluentWindow
{
    public bool Confirmed { get; private set; }

    public static MessageBoxResult Show(Window? owner, string message, string title,
        MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
    {
        var dialog = new LinkShelfMessageBox(message, title, button == MessageBoxButton.OKCancel);
        dialog.Owner = owner;
        dialog.ShowDialog();
        return dialog.Confirmed ? MessageBoxResult.OK : MessageBoxResult.Cancel;
    }

    public LinkShelfMessageBox(string message, string title, bool showCancel = false)
    {
        ExtendsContentIntoTitleBar = true;
        InitializeComponent();
        TitleBlock.Text = title;
        MessageBlock.Text = message;
        CancelBtn.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var accent = ApplicationAccentColorManager.PrimaryAccent;
        OkBtn.Background = new SolidColorBrush(accent);
    }

    private void OkBtn_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = true;
        DialogResult = true;
        Close();
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = false;
        DialogResult = false;
        Close();
    }
}
