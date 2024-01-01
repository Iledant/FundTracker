using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FundTracker.Core.Models;

/// <summary>
/// Classe contenant les données et les méthodes pour les persister dans un stream
/// </summary>
public class Repository
{
    private ObservableCollection<PortfolioItem> _portfolios = new();
    private List<FundItem> _funds = new();

    public ObservableCollection<PortfolioItem> Portfolios => _portfolios;
    public List<FundItem> Funds => _funds;
    private readonly ILogger _logger;

    public Repository(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sauvegarde le contenu du repository dans le stream en le convertissant en JSON
    /// </summary>
    /// <param name="stream">Le stream dans lequel le JSON sera écrit</param>
    public async void Save(Stream stream)
    {
        var jsonContainer = new JsonContainer(this);
        var json = JsonSerializer.Serialize(jsonContainer);
        var bytes = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(bytes);
    }

    /// <summary>
    /// Permet de remplacer le contenu du repository avec le contenu du stream qui doit être conforme au format de fichier
    /// </summary>
    /// <param name="stream">contient le JSON valide d'un repository</param>
    public async void Load(Stream stream)
    {      
        var jsonContainer = await JsonSerializer.DeserializeAsync<JsonContainer>(stream);

        if (jsonContainer == null)
        {
            _logger.Log(LogLevel.Warning, "Impossible de lire le fichier");
        }

        _portfolios = new ObservableCollection<PortfolioItem>(jsonContainer.PortfolioItems);
        _funds = jsonContainer.FundItems;
    }
}
