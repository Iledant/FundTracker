namespace FundTracker.Core.Models;

public class PortfolioLine
{
    public FundItem Fund
    {
        get; set;
    }
    public double AveragePurchasePrice
    {
        get; set;
    }
    public double Quantity
    {
        get; set;
    }
    public double Evol
    {
        get; set;
    }
}