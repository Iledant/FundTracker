using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTracker.Core.Models;
public class FundItem
{
    public int Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string MSId
    {
        get; set;
    }

    public List<DateValue> DateValues
    {
        get; set;
    }  
}
