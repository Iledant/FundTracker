using FundTracker.Core.Models;

namespace FundTracker.Core.Contracts.Services;
public interface IMorningStarService
{
    /// <summary>
    /// Fournit l'historique des valeurs d'un fond ou d'une action entre deux dates
    /// </summary>
    /// <param name="MorningStarID">Identifiant unique MornigStar utilisé pour retrouver l'historique des valeurs</param>
    /// <param name="beginDate">Début de l'historique recherché. Si le début n'est pas précisé, la date la plus ancienne disponible sur le site</param>
    /// <param name="endDate">Fin de l'historique recherché. Si elle n'est pas précisée, la date du jour</param>
    /// <returns>Liste classée de l'historique des valeurs utilisant l'objet DateValue</returns>
    public Task<List<DateValue>> FetchHistorical(string MorningStarID, DateTime? beginDate = null, DateTime? endDate = null);

    /// <summary>
    /// Recherche les fonds disponibles sur le site MorningStar à partir de son nom. Retourne la liste de fonds ou actions dont le nom contient la chaîne transport
    /// </summary>
    /// <param name="namePattern">partie du nom du fond ou de l'action recherchée</param>
    /// <returns>Liste de fonds correspondant à la recherche avec une structure comportant l'ID, le nom complet, le type (action, fond...), la bourse et l'abbréviation</returns>
    public Task<List<MorningStarFund>> FetchFunds(string namePattern);

    /// <summary>
    /// Get the date of historical begin
    /// </summary>
    /// <returns>The default date used to fetch historical values</returns>
    public DateTime HistoricalBeginDate();
}
