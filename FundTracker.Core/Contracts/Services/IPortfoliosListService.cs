
using FundTracker.Core.Models;

namespace FundTracker.Core.Contracts.Services;
public interface IPortfoliosListService
{
    Task<IEnumerable<PortfolioItem>> GetPortfoliosListAsync();

}
