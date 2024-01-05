using System.Collections.ObjectModel;

namespace FundTracker.Core.Models;
public class PortfolioItem
{
    public string Name
    {
        get; set;
    }

    public ObservableCollection<PortfolioLine> Lines
    {
        get; set;
    }
}
