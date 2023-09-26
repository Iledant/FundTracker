using FundTracker.Core.Models;
using FundTracker.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FundTracker.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
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
        }
    }

    private void PortfolioAddBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }
}
