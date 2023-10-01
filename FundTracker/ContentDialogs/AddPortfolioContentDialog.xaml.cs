using Microsoft.UI.Xaml.Controls;

namespace FundTracker.ContentDialogs;

public sealed partial class AddPortfolioContentDialog : ContentDialog
{
    public string PortfolioName => NameTextBox.Text;

    public AddPortfolioContentDialog()
    {
        this.InitializeComponent();
    }

    private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        IsPrimaryButtonEnabled = NameTextBox.Text.TrimEnd().Length > 0;
    }
}
