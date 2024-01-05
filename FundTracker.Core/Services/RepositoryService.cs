using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using Microsoft.Extensions.Logging;

namespace FundTracker.Core.Services;
public class RepositoryService : IRepositoryService
{
    private ObservableCollection<PortfolioItem> _portfolios = new();

    private ObservableCollection<FundItem> _funds = new();

    private readonly ILogger<RepositoryService> _logger;

    private readonly IMorningStarService _morningStarService;

    public RepositoryService(ILogger<RepositoryService> logger, IMorningStarService morningStarService)
    {
        _logger = logger;
        _morningStarService = morningStarService;
    }

    public ObservableCollection<PortfolioItem> Portfolios() => _portfolios;

    public ObservableCollection<FundItem> Funds() => _funds;

    public void AddPortfolio(string name)
    {
        _portfolios.Add(new PortfolioItem { Name = name, Lines = new() });
    }

    public async void AddToPortfolio(PortfolioItem portfolio, string morningStarID, string name, double quantity, double averagePurchasePrice)
    {
        var insertedFund = _funds.FirstOrDefault<FundItem>(fund => fund.MSId == morningStarID, null);

        if (insertedFund == null)
        {
            insertedFund = new FundItem { MSId = morningStarID, Name = name, DateValues = new() };
            insertedFund.DateValues = await _morningStarService.FetchHistorical(insertedFund.MSId);
            _funds.Add(insertedFund);
        }

        var line = new PortfolioLine { AveragePurchasePrice = averagePurchasePrice, Fund = insertedFund, Quantity = quantity };
        line.SetEvol();
        portfolio.Lines.Add(line);
    }

    public void RemoveLineFromPortfolio(PortfolioItem item, PortfolioLine line)
    {
        item.Lines.Remove(line);

        if (!IsFundInPortfolios(line.Fund)) {
            _funds.Remove(line.Fund);
        }
    }

    public void RemovePortfolio(PortfolioItem item)
    {
        _portfolios.Remove(item);

        foreach (var line in item.Lines)
        {
            if (!IsFundInPortfolios(line.Fund))
            {
                _funds.Remove(line.Fund);
            }
        }
    }

    public async void Save(Stream stream)
    {
        _logger?.LogInformation("Sérialisation des données et sauvegarde");
        var jsonContainer = new JSONContainer(this);
        var json = JsonSerializer.SerializeToUtf8Bytes(jsonContainer);
        await stream.WriteAsync(json);
        _logger?.LogInformation("Sauvegarde terminée");
    }

    public async void Load(Stream stream)
    {
        _logger?.LogInformation("Lecture et désérialisation");
        var jsonContainer = await JsonSerializer.DeserializeAsync<JSONContainer>(stream);

        if (jsonContainer == null)
        {
            _logger?.LogError("Impossible de lire le fichier");
        }

        _portfolios = jsonContainer.GetPortfolioItems();
        _funds = new ObservableCollection<FundItem>(jsonContainer.Funds);
        _logger?.LogInformation("Lecture terminée");
    }

    private bool IsFundInPortfolios(FundItem searchedFund)
    {
        bool containsSearchedFund(PortfolioItem portfolio) => portfolio.Lines.Any<PortfolioLine>(line => line.Fund.MSId == searchedFund.MSId);

        return _portfolios.Any<PortfolioItem>(portfolio => containsSearchedFund(portfolio));
    }
}
