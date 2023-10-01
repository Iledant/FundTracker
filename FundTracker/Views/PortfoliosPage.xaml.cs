using FundTracker.ContentDialogs;
using FundTracker.Core.Models;
using FundTracker.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FundTracker.Views;

public sealed partial class PortfoliosPage : Page
{
    public PortfoliosViewModel ViewModel
    {
        get;
    }

    public PortfoliosPage()
    {
        ViewModel = App.GetService<PortfoliosViewModel>();
        InitializeComponent();
    }

    private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is PortfolioItem portfolio)
        {
            ViewModel.GetPortfolioContent(portfolio);
            PortfolioDeleteBtn.IsEnabled = true;
        }
        else
        {
            ViewModel.ClearPortfolioContent();
            PortfolioDeleteBtn.IsEnabled = false;
        }
    }

    private async void PortfolioAddBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        AddPortfolioContentDialog addContentDialog = new AddPortfolioContentDialog();
        addContentDialog.XamlRoot = this.Content.XamlRoot;

        var result = await addContentDialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Primary)
        {
            ViewModel.AddPortfolio(addContentDialog.PortfolioName);
        }
    }

    private void PortfolioDeleteBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (NavLinksList.SelectedItem is PortfolioItem item)
        {
            ViewModel.PortfoliosList.Remove(item);
            ViewModel.PortfolioContent.Clear();
            PortfolioDeleteBtn.IsEnabled = false;
        }
    }
}
