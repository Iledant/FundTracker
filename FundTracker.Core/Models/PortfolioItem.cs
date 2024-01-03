using System.Collections.ObjectModel;

namespace FundTracker.Core.Models;
public class PortfolioItem
{
    public long Id
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public ObservableCollection<PortfolioLine> Lines
    {
        get; set;
    }
}
