using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml.Navigation;

namespace FundTracker.ViewModels;
public partial class FundsViewModel : ObservableRecipient, INavigationAware
{
    private PortfolioItem? _portfolioItem;

    public ObservableCollection<PortfolioLine> Lines = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFundSelected))]
    private PortfolioLine? _selected = null;

    public readonly List<DateTimePoint> ChartValues = new();

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

    partial void OnSelectedChanged(PortfolioLine? oldValue, PortfolioLine? newValue)
    {
        ChartValues.Clear();

        if (newValue == null)
        {
            return;
        }

        foreach (var val in newValue.Fund.DateValues)
        {
            ChartValues.Add(new DateTimePoint(val.Date, val.Value));
        }
    }

    public void OnNavigatedFrom() => throw new NotImplementedException();
}
