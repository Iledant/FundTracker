using System.Text;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using FundTracker.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FundTracker.Tests.MSTest;

[TestClass]
public class TestRepository
{
    private static readonly Mock<ILogger<RepositoryService>> mockLogger = new();
    private static readonly ILogger<RepositoryService> logger = mockLogger.Object;
    private static readonly Mock<IMorningStarService> mockMSService = new();
    private static readonly IMorningStarService morningstarService = mockMSService.Object;
    private static readonly string portfolioTestName1 = "Portefeuille 1";
    private static readonly string portfolioTestName2 = "Portefeuille 2";
    private static readonly string msIdTestName1 = "MSID 1";
    private static readonly string FundTestName1 = "Fond 1";
    private static readonly double FundTestQuantity1 = 3.5;
    private static readonly double FundTestQuantity2 = 4.0;
    private static readonly double FundTestAPP1 = 15.7;
    private static readonly double FundTestAPP2 = 23.8;
    private static readonly DateTime DateTest1 = new (2023, 12, 1);
    private static readonly double ValueTest1 = 12.3;
    private static readonly string jsonTest = """{"Portfolio":[{"Name":"Portefeuille 1","Line":[{"MSId":"MSID 1","APP":15.7,"Quantity":3.5}]}],"Fund":[{"Name":"Fond 1","MSId":"MSID 1","DateValue":[{"Value":12.3,"Date":"2023-12-01T00:00:00"}]}],"FileVersion":"1.0"}""";

    private static RepositoryService CreateRepositoryService() => new(logger, morningstarService);

    private RepositoryService CreateRepositoryServiceWithOnePorfolio()
    {
        var repositoryService = CreateRepositoryService();
        repositoryService.AddPortfolio(portfolioTestName1);
        return repositoryService;
    }

    private RepositoryService CreateRepositoryServiceWithOneFund()
    {
        mockMSService.Setup(foo => foo.FetchHistorical(msIdTestName1,null,null).Result).Returns(new List<DateValue>() { new() { Date= DateTest1, Value=ValueTest1 } });
        var repositoryService = CreateRepositoryServiceWithOnePorfolio();
        var firstPortfolio = repositoryService.Portfolios().First();
        repositoryService.AddToPortfolio(firstPortfolio, msIdTestName1, FundTestName1, FundTestQuantity1, FundTestAPP1);
        var firstFund = repositoryService.Funds().First();
        return repositoryService;
    }

    private RepositoryService CreateRepositoryServiceWith2PortfolioAnd1Fund()
    {
        var repositoryService = CreateRepositoryServiceWithOneFund();
        repositoryService.AddPortfolio(portfolioTestName2);
        var secondPortfolio = repositoryService.Portfolios().Last();
        repositoryService.AddToPortfolio(secondPortfolio, msIdTestName1, FundTestName1, FundTestQuantity2, FundTestAPP2);
        return repositoryService;
    }

    [TestMethod]
    public void TestAddPortfolio()
    {
        var repositoryService = CreateRepositoryService();

        repositoryService.AddPortfolio(portfolioTestName1);
        var portfolios = repositoryService.Portfolios();

        Assert.AreEqual(1, portfolios.Count);
        Assert.AreEqual(portfolioTestName1, portfolios[0].Name);
    }

    [TestMethod]
    public void TestAddToPortfolio()
    {
        var repositoryService = CreateRepositoryServiceWithOneFund();

        var funds = repositoryService.Funds();
        Assert.AreEqual(1, funds.Count);

        var firstFund = funds.First();
        Assert.AreEqual(msIdTestName1, firstFund.MSId);
        Assert.AreEqual(FundTestName1, firstFund.Name);

        var firstLine = repositoryService.Portfolios().First().Lines.First();
        Assert.AreEqual(FundTestQuantity1, firstLine.Quantity);
        Assert.AreEqual(FundTestAPP1, firstLine.AveragePurchasePrice);
        var evol = (ValueTest1 - firstLine.AveragePurchasePrice)/firstLine.AveragePurchasePrice - 1;
        Assert.AreEqual(evol, firstLine.Evol);
    }

    [TestMethod]
    public void TestRemoveLineFromPortfolio()
    {
        var repositoryService = CreateRepositoryServiceWithOneFund();

        var firstPortfolio = repositoryService.Portfolios().First();
        var firstLine = firstPortfolio.Lines.First();

        repositoryService.RemoveLineFromPortfolio(firstPortfolio, firstLine);

        var funds = repositoryService.Funds();

        Assert.AreEqual(0, funds.Count);
        Assert.AreEqual(0, firstPortfolio.Lines.Count);
    }

    [TestMethod]
    public void TestRemovePortfolio()
    {
        var repositoryService = CreateRepositoryServiceWithOneFund();

        var firstPortfolio = repositoryService.Portfolios().First();

        repositoryService.RemovePortfolio(firstPortfolio);

        var funds = repositoryService.Funds();
        var portfolios = repositoryService.Portfolios();

        Assert.AreEqual(0,funds.Count);
        Assert.AreEqual(0,portfolios.Count);
    }

    [TestMethod]
    public void TestSave()
    {
        var repositoryService = CreateRepositoryServiceWithOneFund();
        var testStream = new MemoryStream();

        repositoryService.Save(testStream);

        var jsonResult = Encoding.UTF8.GetString(testStream.ToArray());
    
        Assert.AreEqual(jsonTest, jsonResult);
    }

    [TestMethod]
    public void TestLoad()
    {
        var repositoryService = CreateRepositoryService();

        var testStream = new MemoryStream();
        var bytes = Encoding.UTF8.GetBytes(jsonTest);
        testStream.Write(bytes, 0, bytes.Length);
        testStream.Flush();
        testStream.Position = 0;

        repositoryService.Load(testStream);

        Assert.AreEqual(1,repositoryService.Portfolios().Count);
        Assert.AreEqual(1, repositoryService.Funds().Count);
    }

    [TestMethod]
    public void TestCommonFund()
    {
        var repositoryService = CreateRepositoryServiceWith2PortfolioAnd1Fund();

        Assert.AreEqual(1, repositoryService.Funds().Count());

        var lastPortfolio = repositoryService.Portfolios().Last();

        repositoryService.RemovePortfolio(lastPortfolio);

        Assert.AreEqual(1, repositoryService.Portfolios().Count());
        Assert.AreEqual(1, repositoryService.Funds().Count());
    }
}
