using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Models;
using Microsoft.UI.Xaml.Navigation;

namespace FundTracker.ViewModels;
public partial class FundsViewModel : ObservableRecipient, INavigationAware
{
    private PortfolioItem? _portfolioItem;
    public ObservableCollection<PortfolioLine> Lines = new();

    public void OnNavigatedTo(object parameter)
    {
        _portfolioItem = (PortfolioItem)e.Parameter;
        if (_portfolioItem != null)
        {
            Lines = _portfolioItem.Lines;
        }
    }

    public void OnNavigatedFrom() => throw new NotImplementedException();
}
