using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using FundTracker.Core.Services;

namespace FundTracker.Core.Models;

internal class JSONPortfolioLine
{
    [JsonPropertyName("MSId")]
    public string MSId
    {
    get; set;
    }

    [JsonPropertyName("APP")]
    public double AveragePurchasePrice
    {
        get; set;
    }

    [JsonPropertyName("Quantity")]
    public double Quantity
    {
        get; set;
    }
    
    public JSONPortfolioLine(PortfolioLine line)
    {
        MSId = line.Fund.MSId;
        AveragePurchasePrice = line.AveragePurchasePrice;
        Quantity = line.Quantity;
    }
    public JSONPortfolioLine()
    {
    }
}

internal class JSONPortfolioItem
{
    [JsonPropertyName("Name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("Line")]
    public List<JSONPortfolioLine> Lines
    {
        get; set;
    }

    public JSONPortfolioItem(PortfolioItem item)
    {
        Name = item.Name;
        Lines = new();
        foreach (var  line in item.Lines)
        {
            Lines.Add(new JSONPortfolioLine(line));
        }
    }

    public JSONPortfolioItem()
    {
        Lines = new();
    }
}

/// <summary>
/// Encapsule un Repository pour permettre sa sauvegarde en JSON en gérant les liens entre les lignes de portefeuille et la base de fonds
/// </summary>
internal class JSONContainer
{
    [JsonPropertyName("Portfolio")]
    public List<JSONPortfolioItem> Portfolios
    {
        get; set;
    }

    [JsonPropertyName("Fund")]
    public  List<FundItem> Funds
    {
        get; set;
    }

    [JsonPropertyName("FileVersion")]
    public string FileVersion
    {
        get; set;
    }

    public JSONContainer()
    {
        Portfolios = new();
        Funds= new();
        FileVersion= string.Empty;
    }

    public JSONContainer(RepositoryService repository)
    {
        FileVersion = "1.0";
        Funds = new List<FundItem>(repository.Funds());
        Portfolios = new();
        foreach (var porfolio in repository.Portfolios())
        {
            Portfolios.Add(new JSONPortfolioItem(porfolio));
        }
    }

    public ObservableCollection<PortfolioItem> GetPortfolioItems()
    {
        ObservableCollection<PortfolioItem> portfolioItems = new();
        foreach (var portfolio in Portfolios)
        {
            var portfolioItem = new PortfolioItem
            {
                Name = portfolio.Name,
                Lines = new ObservableCollection<PortfolioLine>()
            };

            foreach(var line in portfolio.Lines)
            {
                var newLine = new PortfolioLine
                {
                    AveragePurchasePrice = line.AveragePurchasePrice,
                    Quantity = line.Quantity,
                    Fund = Funds.FirstOrDefault(f => f.MSId == line.MSId)
                };
                newLine.SetEvol();
                portfolioItem.Lines.Add(newLine);
            }
            portfolioItems.Add(portfolioItem);
        }
        return portfolioItems;
    }
}
