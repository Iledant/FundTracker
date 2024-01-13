using Microsoft.UI.Xaml.Controls;

namespace FundTracker.ContentDialogs;

public sealed partial class PortfolioNameDialog : ContentDialog
{
    public string PortfolioName => NameTextBox.Text;

    public PortfolioNameDialog(string initialName = "")
    {
        InitializeComponent();
        NameTextBox.Text = initialName;
        PrimaryButtonText = initialName == "" ? "Créer" : "Modifier";
        SetPrimaryButtonEnabled();
    }

    private void SetPrimaryButtonEnabled()
    {
        IsPrimaryButtonEnabled = NameTextBox.Text.TrimEnd().Length > 0;
    }

    private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetPrimaryButtonEnabled();
    }
}
