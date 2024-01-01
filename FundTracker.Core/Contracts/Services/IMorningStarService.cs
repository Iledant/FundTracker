using FundTracker.Core.Models;

namespace FundTracker.Core.Contracts.Services;
public interface IMorningStarService
{
    public Task<List<DateValue>> FetchHistorical(string MorningStarID, DateTime? beginDate = null, DateTime? endDate = null);
    public Task<List<MorningStarFund>> FetchFunds(string content);
}
