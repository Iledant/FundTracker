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
        get; private set;
    }

    /// <summary>
    /// Calcule la dernière évolution du fond en fonction de l'historique et de AveragePurchasePrice. Mise à 0 par défaut.
    /// </summary>
    public void SetEvol()
    {
        var lastDateValue = Fund.DateValues.LastOrDefault();

        if (lastDateValue is null || lastDateValue.Value ==0)
        {
            Evol = 0;
            return;
        }

        Evol = (lastDateValue.Value - AveragePurchasePrice) / AveragePurchasePrice - 1;
    }
}