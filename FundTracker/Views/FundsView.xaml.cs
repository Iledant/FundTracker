using FundTracker.ContentDialogs;
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
    private readonly FundsViewModel ViewModel;

    public FundsView()
    {
        ViewModel = App.GetService<FundsViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.OnNavigatedTo(e.Parameter);

    private async void AddFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var addContentDialog = new AddFundContentDialog
        {
            XamlRoot = Content.XamlRoot
        };

        var result = await addContentDialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Primary && addContentDialog.SelectedFund is not null)
        {
            ToggleProgressVisibility(true);
            ViewModel.AddFund(addContentDialog.SelectedFund, addContentDialog.Quantity, addContentDialog.AveragePurchasePrice);
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
