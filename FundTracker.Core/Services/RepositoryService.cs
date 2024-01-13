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

    public PortfolioItem AddPortfolio(string name)
    {
        var newItem = new PortfolioItem { Name = name, Lines = new() };
        _portfolios.Add(newItem);
        return newItem;
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

    public void RenamePortfolio(PortfolioItem portfolio, string newName)
    {
        if (portfolio is null)
        {
            return;
        }

        portfolio.Name = newName;
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

    private async void UpdateFunds(JSONContainer container)
    {
        foreach (var fund in container.Funds) {
            var lastDate = _morningStarService.HistoricalBeginDate();
            if (fund.DateValues.Count > 0)
            {
                lastDate = fund.DateValues.Last().Date.AddDays(1);
            }
            var newValues = await _morningStarService.FetchHistorical(fund.MSId, lastDate);
            fund.DateValues.AddRange(newValues);
        }
    }

    public async void Load(Stream stream)
    {
        _logger?.LogInformation("Lecture et désérialisation");
        if (stream.Length == 0)
        {
            _logger?.LogInformation("Fichier vide");
            return;
        }
        var jsonContainer = await JsonSerializer.DeserializeAsync<JSONContainer>(stream);

        if (jsonContainer == null)
        {
            _logger?.LogError("Impossible de lire le fichier");
        }

        _portfolios = jsonContainer.GetPortfolioItems();
        await Task.Run( () => UpdateFunds(jsonContainer));
        _funds = new ObservableCollection<FundItem>(jsonContainer.Funds);
        _logger?.LogInformation("Lecture terminée");
    }

    private bool IsFundInPortfolios(FundItem searchedFund)
    {
        bool containsSearchedFund(PortfolioItem portfolio) => portfolio.Lines.Any<PortfolioLine>(line => line.Fund.MSId == searchedFund.MSId);

        return _portfolios.Any<PortfolioItem>(portfolio => containsSearchedFund(portfolio));
    }

}
