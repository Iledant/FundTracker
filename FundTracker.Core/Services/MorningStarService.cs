using System.Globalization;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using Microsoft.Extensions.Logging;

namespace FundTracker.Core.Services;
public class MorningStarService : IMorningStarService
{
    private static readonly HttpClient httpClient = new();
    private static readonly CultureInfo usCulture = new("en-US");
    private static readonly string[] filteredCategories = { "PEA", "Fonds", "Actions", "ETFs" };
    private static readonly string searchUrl = "https://www.morningstar.fr/fr/util/SecuritySearch.ashx?source=nav&moduleId=6&ifIncludeAds=False&usrtType=v";
    private static readonly DateTime historicalBeginDate = new(2010, 1, 1);
    private readonly ILogger<MorningStarService> _logger;

    public MorningStarService(ILogger<MorningStarService> logger)
    {
        _logger = logger;
    }

    private static List<MorningStarFund> ParseMorningstarResponse(string content)
    {
        char[] charToStrim = { '\n', '\r' };
        var lines = content.Split("\n");
        List<MorningStarFund> pickStocks = new();
        foreach (var line in lines)
        {
            var fields = line.Split('|');
            if (fields.Length < 6)
            {
                continue;
            }

            var left = fields[1].IndexOf("\"i\":\"") + 5;
            var right = fields[1].IndexOf("\",\"");
            if (left == -1 || right == -1 || right <= left)
            {
                continue;
            }

            var category = fields[5].Trim(charToStrim);

            for (var i = 0; i < filteredCategories.Length; i++)
            {
                if (category == filteredCategories[i])
                {
                    pickStocks.Add(new MorningStarFund
                    {
                        Name = fields[0].Trim(charToStrim),
                        MorningStarID = fields[1][left..right],
                        Category = category,
                        Place = fields[4].Trim(charToStrim),
                        Abbreviation = fields[3].Trim(charToStrim)
                    });
                    break;
                }
            }
        }
        return pickStocks;
    }

    public async Task<List<MorningStarFund>> FetchFunds(string content)
    {
        FormUrlEncodedContent formContent = new(new[]
        {
            new KeyValuePair<string, string>("q", content),
            new KeyValuePair<string, string>("limit", "100")
        });

        try
        {
            var response = await httpClient.PostAsync(searchUrl, formContent);
            var responseAsString = await response.Content.ReadAsStringAsync();
            return ParseMorningstarResponse(responseAsString);
        }
        catch (Exception)
        {
            return new List<MorningStarFund>();
        }

    }
    public async Task<List<DateValue>> FetchHistorical(string MorningStarID, DateTime? beginDate = null, DateTime? endDate = null)
    {
        var endPattern = (endDate ?? DateTime.Now).ToString("yyyy-MM-dd");
        var beginPattern = (beginDate ?? historicalBeginDate).ToString("yyyy-MM-dd");
        var url = $"https://tools.morningstar.fr/api/rest.svc/timeseries_price/ok91jeenoo?" +
            $"id={MorningStarID}%5D22%5D1%5D&currencyId=EUR&idtype=Morningstar&frequency=daily&" +
            $"startDate={beginPattern}&endDate={endPattern}&outputType=COMPACTJSON";
        List<DateValue> values = new();
        char[] leadingCharToTrim = { ',', '[' };

        try
        {
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var innerList = content[1..^1]; // trim external square brackets
            var dateValues = innerList.Split(']');

            for (var i = 0; i < dateValues.Length - 1; i++)
            {
                var dateAndValue = dateValues[i].Trim(leadingCharToTrim).Split(',');

                if (dateAndValue.Length != 2)
                {
                    throw new Exception("Erreur de format de réponse");
                }

                if (!double.TryParse(dateAndValue[1], NumberStyles.Number, usCulture, out var value))
                {
                    throw new Exception("Erreur de format de réponse");
                }

                if (!long.TryParse(dateAndValue[0], out var dateInMilliseconds))
                {
                    throw new Exception("Erreur de format de réponse");
                }

                values.Add(new DateValue { Value = value, Date = new DateTime(1970, 1, 1).AddMilliseconds(dateInMilliseconds) });
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception dans FetchHistorical provoquée par {Source} avec le message {Message} ", e.Source, e.Message);
        }
        values.Sort((v1,v2) => DateTime.Compare(v1.Date,v2.Date));
        return values;
    }

    public DateTime HistoricalBeginDate() => historicalBeginDate;
}
