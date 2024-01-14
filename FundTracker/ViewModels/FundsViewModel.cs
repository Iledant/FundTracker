using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.WinUI.UI.Controls;
using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml.Navigation;
using Windows.AI.MachineLearning;

namespace FundTracker.ViewModels;

public partial class FundsViewModel : ObservableRecipient, INavigationAware
{
    private PortfolioItem? _portfolioItem;

    public ObservableCollection<PortfolioLine> Lines = new();

    public string NameColumn => "Nom";
    public string QuantityColumn => "Qté";
    public string EvolColumn => "Evol";

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

    public void SortLines(string columnName, DataGridSortDirection direction)
    {
        if (columnName == null || _portfolioItem is null) {
            return;
        }

        if (columnName == NameColumn)
        {
            if (direction == DataGridSortDirection.Ascending)
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Fund.Name ascending select item);
            }
            else
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Fund.Name descending select item);
            }
        }

        if (columnName == QuantityColumn)
        {
            if (direction == DataGridSortDirection.Ascending)
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Quantity ascending select item);
            }
            else
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Quantity descending select item);
            }
        }

        if (columnName == EvolColumn)
        {
            if (direction == DataGridSortDirection.Ascending)
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Evol ascending select item);
            }
            else
            {
                Lines = new ObservableCollection<PortfolioLine>(from item in _portfolioItem.Lines orderby item.Evol descending select item);
            }
        }

    }
}
