using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.Core.Services;
public class PortfoliosListService : IPortfoliosListService
{
    private List<PortfolioItem> _allPortfolios;
    private readonly List<FundItem> _allFunds;
    private IEnumerable<PortfolioItem> AllPortfolios()
    {
        return new List<PortfolioItem>()
        {
            new PortfolioItem()
            {
                Id = 1,
                Name = "Portefeuille 1",
                Lines = new List<PortfolioLine>
                {
                    new PortfolioLine
                    {
                        AveragePurchasePrice = 5.5,
                        Quantity = 2,
                        Evol = 0.43,
                        Fund = _allFunds.First(f => f.Id == 1)
                    },
                }
            },
            new PortfolioItem()
            {
                Id= 2,
                Name = "Portfeuille 2",
                Lines = new List<PortfolioLine>
                {
                    new PortfolioLine
                    {
                        AveragePurchasePrice = 4.5,
                        Quantity = 3,
                        Evol = -0.13,
                        Fund = _allFunds.First(f => f.Id == 2)
                    },
                    new PortfolioLine
                    {
                        AveragePurchasePrice = 5.5,
                        Quantity = 1.5,
                        Evol = 0.2,
                        Fund = _allFunds.First(f => f.Id == 3)
                    },
                    new PortfolioLine
                    {
                        AveragePurchasePrice = 8.5,
                        Quantity = 0.5,
                        Evol = -0.05,
                        Fund = _allFunds.First(f => f.Id == 4)
                    },
                }
            }
        };
    }

    PortfoliosListService()
    {
        _allFunds = new List<FundItem>()
        {
            new FundItem()
            {
                Id =1,
                Name="Fond 1",
                MSId="FD1"
            },
            new FundItem()
            {
                Id =2,
                Name="Fond 2",
                MSId="FD2"
            },
            new FundItem()
            {
                Id =3,
                Name="Fond 3",
                MSId="FD3"
            },
            new FundItem()
            {
                Id =4,
                Name="Fond 4",
                MSId="FD4"
            }
        };
    }
    public async Task<IEnumerable<PortfolioItem>> GetPortfoliosListAsync()
    {
        _allPortfolios ??= new List<PortfolioItem>(AllPortfolios());
        await Task.CompletedTask;

        return _allPortfolios;
    }
}
