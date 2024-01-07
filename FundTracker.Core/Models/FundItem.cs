using System.Text.Json.Serialization;

namespace FundTracker.Core.Models;
public class FundItem
{
    [JsonPropertyName("Name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("MSId")]
    public string MSId
    {
        get; set;
    }

    [JsonPropertyName("DateValue")]
    public List<DateValue> DateValues
    {
        get; set;
    }  
}
