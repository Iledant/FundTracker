using System.Collections.ObjectModel;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.Core.Services;
public class PortfoliosListService : IPortfoliosListService
{
    private readonly ObservableCollection<PortfolioItem> _allPortfolios;
    public PortfoliosListService()
    {
        var list = new List<PortfolioItem>()
        {
            new()
            {
                Id = 1,
                Name = "Portefeuille 1",
                Lines = new ObservableCollection<PortfolioLine>
                {
                    new() {
                        AveragePurchasePrice = 5.5,
                        Quantity = 2,
                        Evol = 0.43,
                        Fund = new FundItem
                        {
                            Id =1,
                            Name="Fond 1",
                            MSId="FD1"
                        }
                    },
                }
            },
            new()
            {
                Id= 2,
                Name = "Portfeuille 2",
                Lines = new ObservableCollection<PortfolioLine>
                {
                    new() {
                        AveragePurchasePrice = 4.5,
                        Quantity = 3,
                        Evol = -0.13,
                        Fund = new FundItem
                        {
                            Id =2,
                            Name="Fond 2",
                            MSId="FD2"
                        }
                    },
                    new() {
                        AveragePurchasePrice = 5.5,
                        Quantity = 1.5,
                        Evol = 0.2,
                        Fund = new FundItem
                        {
                            Id =3,
                            Name="Fond 3",
                            MSId="FD3"
                        }
                    },
                    new() {
                        AveragePurchasePrice = 8.5,
                        Quantity = 0.5,
                        Evol = -0.05,
                        Fund = new FundItem
                        {
                            Id =4,
                            Name="Fond 4",
                            MSId="FD4"
                        }
                    },
                }
            }
        };
        _allPortfolios = new ObservableCollection<PortfolioItem>(list);
    }

    public PortfolioItem Add(string name)
    {
        var newItem = new PortfolioItem() { 
            Name = name,
            Id = 5,
            Lines = new ObservableCollection<PortfolioLine>()
        };
        _allPortfolios.Add(newItem);
        return newItem;
    }

    public void Remove(PortfolioItem item)
    {
        _allPortfolios.Remove(item);
    }

    public ObservableCollection<PortfolioItem> PortfoliosList() => _allPortfolios;
}
