using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.ViewModels;

public partial class PortfoliosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPortfoliosListService _portfoliosListService;

    public ObservableCollection<PortfolioItem> PortfoliosList => _portfoliosListService.PortfoliosList();

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

    public void ClearPortfolioContent()
    {
        PortfolioContent.Clear();
    }

    public void AddPortfolio(string name)
    {
        _portfoliosListService.Add(name);
    }

    public async void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }
}
