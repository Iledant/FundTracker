using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.ViewModels;

public partial class PortfoliosViewModel : ObservableRecipient
{
    private readonly IPortfoliosListService _portfoliosListService;

    public ObservableCollection<PortfolioItem> PortfoliosList => _portfoliosListService.PortfoliosList();

    private ObservableCollection<PortfolioLine> _portfolioContent = new();

    public ObservableCollection<PortfolioLine> PortfolioContent => _portfolioContent;

    public PortfoliosViewModel(IPortfoliosListService portfoliosListService)
    {
        _portfoliosListService = portfoliosListService;
    }

    public void GetPortfolioContent(PortfolioItem portfolioItem)
    {
        _portfolioContent = portfolioItem.Lines;
    }

    public void ClearPortfolioContent()
    {
        PortfolioContent.Clear();
    }

    public PortfolioItem AddPortfolio(string name) => _portfoliosListService.Add(name);
}
