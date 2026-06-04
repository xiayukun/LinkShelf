using System.Windows;
using LinkShelf.Models;
using LinkShelf.Services;

namespace LinkShelf;

public partial class ConflictChoiceWindow : Window
{
    public ConflictChoiceWindow(LocalizationService text, string reason, string targetPath, string cachePath)
    {
        InitializeComponent();
        Title = text.T("conflict.title");
        HeadingText.Text = text.T("conflict.heading");
        ReasonText.Text = text.Code(reason);
        TargetText.Text = text.F("conflict.target", targetPath);
        CacheText.Text = text.F("conflict.cache", cachePath);
        UseCacheButton.Content = text.T("conflict.useCache");
        ImportButton.Content = text.T("conflict.useTarget");
        MergeButton.Content = text.T("conflict.merge");
        SkipButton.Content = text.T("conflict.skip");
        CancelButton.Content = text.T("conflict.cancel");
    }

    public ConflictDecision Decision { get; private set; } = ConflictDecision.Cancel;

    private void UseCacheButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = ConflictDecision.UseCacheDeleteTarget;
        DialogResult = true;
    }

    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = ConflictDecision.ImportTargetOverwriteCache;
        DialogResult = true;
    }

    private void MergeButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = ConflictDecision.ImportTargetThenOverlayCacheBackup;
        DialogResult = true;
    }

    private void SkipButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = ConflictDecision.BackupTargetAndSkip;
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Decision = ConflictDecision.Cancel;
        DialogResult = false;
    }
}
