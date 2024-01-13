using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundTracker.Core.Models;

namespace FundTracker.Core.Contracts.Services;
public interface IRepositoryService
{
    /// <summary>
    /// Fournit la liste des portefeuilles du repository
    /// </summary>
    /// <returns>Liste des portefeuilles</returns>
    public ObservableCollection<PortfolioItem> Portfolios();

    /// <summary>
    /// Fournit la liste des fonds. Les fonds sont mutualisés entre les portefeuilles. Toute insertion ou suppression de lignes de portefeuille est gérée pour maintenir la liste des fonds présents dans tous les portefeuilles
    /// </summary>
    /// <returns>Liste des fonds</returns>
    public ObservableCollection<FundItem> Funds();

    /// <summary>
    /// Ajouter un nouveau portefeuille au repository. Il n'y a pas de vérification de l'unicité du nom du portefeuille.
    /// </summary>
    /// <param name="name">Nom du portefeuille à créer</param>
    public PortfolioItem AddPortfolio(string name);

    /// <summary>
    /// Renomme un portefeuille sans vérifier l'unicité du nouveau nom
    /// </summary>
    /// <param name="portfolio">Portefeuille à modifier</param>
    /// <param name="newName">Nouveau nom</param>
    public void RenamePortfolio(PortfolioItem portfolio, string newName);
    
    /// <summary>
    /// Supprime définitivement un portefeuille du repository. Supprime les fonds qui ne sont présents que dans ce portefeuille
    /// </summary>
    /// <param name="item">Lien vers le portefeuille à supprimer</param>
    public void RemovePortfolio(PortfolioItem item);

    /// <summary>
    /// Ajoute un fond à un portefeuille et récupère son historique
    /// </summary>
    /// <param name="portfolio">Portefeuille auquel le fond est ajouté</param>
    /// <param name="morningStarID">ID du site MorningStar du portefeuille</param>
    /// <param name="name">Nom du fond qui sera commun à tous les portefeuilles</param>
    /// <param name="quantity">Quantité (décimale) de fonds acquis</param>
    /// <param name="averagePurchasePrice">Valeur moyenne d'acquisition du fond. Utilisée pour le calcul de l'évolution du fond</param>
    public void AddToPortfolio(PortfolioItem portfolio, string morningStarID, string name, double quantity, double averagePurchasePrice);

    /// <summary>
    /// Supprime une ligne du portefeuille. Si le fond n'est plus présent dans les autres lignes ou portefeuilles, il est supprimé du repository
    /// </summary>
    /// <param name="item">Portefeuille à modifier</param>
    /// <param name="line">Ligne du portefeuille à supprimer</param>
    public void RemoveLineFromPortfolio(PortfolioItem item, PortfolioLine line);

    /// <summary>
    /// Sauvegarde le contenu du repository dans le stream en le convertissant en JSON
    /// </summary>
    /// <param name="stream">Le stream dans lequel le JSON sera écrit</param>
    public void Save(Stream stream);

    /// <summary>
    /// Permet de remplacer le contenu du repository avec le contenu du stream qui doit être conforme au format de fichier
    /// </summary>
    /// <param name="stream">contient le JSON valide d'un repository</param>
    public void Load(Stream stream);
}
