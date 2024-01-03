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
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFundSelected))]
    private PortfolioLine? _selected = null;
    public bool IsFundSelected => Selected is not null;

    public void OnNavigatedTo(object parameter)
    {
        _portfolioItem = (PortfolioItem)parameter;
        if (_portfolioItem != null)
        {
            Lines = _portfolioItem.Lines;
        }
    }

    public void FundSelected(PortfolioLine selected)
    {
        Selected = selected;
    }

    public void OnNavigatedFrom() => throw new NotImplementedException();
}
