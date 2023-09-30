using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.ViewModels;

public partial class PortfoliosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPortfoliosListService _portfoliosListService;

    public ObservableCollection<PortfolioItem> PortfoliosList { get; } = new ObservableCollection<PortfolioItem>();

    public ObservableCollection<PortfolioLine> PortfolioContent
    {
        get;
    } = new ObservableCollection<PortfolioLine>();

    public PortfoliosViewModel(IPortfoliosListService portfoliosListService)
    {
        _portfoliosListService = portfoliosListService;
    }

    public void GetPortfolioContent(PortfolioItem portfolioItem)
    {
        PortfolioContent.Clear();
        foreach (var  item in portfolioItem.Lines)
        {
            PortfolioContent.Add(item);
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        PortfoliosList.Clear();

        var data = await _portfoliosListService.GetPortfoliosListAsync();

        foreach (var item in data)
        {
            PortfoliosList.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
