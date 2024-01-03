
using System.Collections.ObjectModel;
using FundTracker.Core.Models;

namespace FundTracker.Core.Contracts.Services;
public interface IPortfoliosListService
{
    ObservableCollection<PortfolioItem> PortfoliosList();

    PortfolioItem Add(string name);
    void Remove(PortfolioItem item);
}
