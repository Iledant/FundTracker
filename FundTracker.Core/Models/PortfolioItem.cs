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

    public List<PortfolioLine> Lines
    {
        get; set;
    }
}
