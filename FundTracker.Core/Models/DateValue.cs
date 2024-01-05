using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FundTracker.Core.Models;
public class DateValue
{
    [JsonPropertyName("Value")]
    public double Value
    {
        get; set;
    }

    [JsonPropertyName("Date")]
    public DateTime Date
    {
        get; set;
    }
 }