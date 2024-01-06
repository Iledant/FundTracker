using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
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

    public void AddFund(MorningStarFund fund, double quantity, double averagePurchasePrice)
    {
        var repositoryService = App.GetService<IRepositoryService>();
        repositoryService.AddToPortfolio(_portfolioItem, fund.MorningStarID, fund.Name, quantity, averagePurchasePrice);
    }

    public void RemoveFund(PortfolioLine? line)
    {
        if (line is null)
        {
            return;
        }
        var repositoryService = App.GetService<IRepositoryService>();
        repositoryService.RemoveLineFromPortfolio(_portfolioItem, line);
    }

    public void FundSelected(PortfolioLine selected)
    {
        Selected = selected;
    }

    public void OnNavigatedFrom() => throw new NotImplementedException();
}
