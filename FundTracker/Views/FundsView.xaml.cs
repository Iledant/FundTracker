using FundTracker.ViewModels;
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

    private void AddFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void DeleteFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Selected = (Core.Models.PortfolioLine?)DataGrid.SelectedItem;
    }
}
