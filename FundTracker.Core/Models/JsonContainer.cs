namespace FundTracker.Core.Models;
/// <summary>
/// Représente le contenu d'un Repository utiliser pour la séralisation et la désérialisation en JSON
/// </summary>
internal class JsonContainer
{
    public List<PortfolioItem> PortfolioItems
    {
        get; set;
    }
    public List<FundItem> FundItems
    {
        get; set;
    }
    public string FileVersion
    {
        get; set;
    }

    public JsonContainer(Repository repository)
    {
        FileVersion = "1.0";
        PortfolioItems = new List<PortfolioItem>(repository.Portfolios);
    }
}
