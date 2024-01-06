using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using FundTracker.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;


namespace FundTracker.Views;
/// <summary>
/// Page affichant les lignes d'un portefeuille
/// </summary>
public sealed partial class FundsView : Page
{
    public FundsViewModel ViewModel
    {
        get;
    }

    public FundsView()
    {
        ViewModel = App.GetService<FundsViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.OnNavigatedTo(e.Parameter);

    private async void AddFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FundAddDialog content = new();
        ContentDialog dialog = new()
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Ajouter un fond",
            PrimaryButtonText = "Ajouter",
            CloseButtonText = "Annuler",
            DefaultButton = ContentDialogButton.Close,
            Content = content,
            IsPrimaryButtonEnabled = false
        };

        content.OnChanged = (bool b) => dialog.IsPrimaryButtonEnabled = b;
        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary && content.SelectedFund is not null)
        {
            var fund = content.SelectedFund;

            ToggleProgressVisibility(true);
            ViewModel.AddFund(fund, content.Quantity, content.AveragePurchasePrice);
            ToggleProgressVisibility(false);
        }
    }

    private void DeleteFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RemoveFund(DataGrid.SelectedItem as PortfolioLine);
    }

    private void ToggleProgressVisibility(bool isProgressVisible)
    {
        ProgressStackPanel.Visibility = isProgressVisible ? Visibility.Visible : Visibility.Collapsed;
        DataGrid.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
        AppBarStackPanel.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Selected = (Core.Models.PortfolioLine?)DataGrid.SelectedItem;
    }
}
