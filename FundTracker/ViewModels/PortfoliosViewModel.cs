using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.ViewModels;

public partial class PortfoliosViewModel : ObservableRecipient
{
    private readonly IRepositoryService _repositoryService;

    public ObservableCollection<PortfolioItem> PortfoliosList => _repositoryService.Portfolios();

    private ObservableCollection<PortfolioLine> _portfolioContent = new();

    public ObservableCollection<PortfolioLine> PortfolioContent => _portfolioContent;

    public PortfoliosViewModel(IRepositoryService repositoryService)
    {
        _repositoryService = repositoryService;
    }

    public void GetPortfolioContent(PortfolioItem portfolioItem)
    {
        _portfolioContent = portfolioItem.Lines;
    }

    public void ClearPortfolioContent()
    {
        PortfolioContent.Clear();
    }

    public PortfolioItem AddPortfolio(string name) => _repositoryService.AddPortfolio(name);

    public void RemovePortfolio(PortfolioItem item)
    {
        _repositoryService.RemovePortfolio(item);
    }
}
