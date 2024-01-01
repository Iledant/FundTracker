using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTracker.Core.Models;
public class DateValue
{
    private readonly DateTime _date;
    private readonly double _value;

    public double Value => _value;
    public DateTime Date => _date;

    public DateValue(double value, DateTime date)
    {
        _value = value;
        _date = date;
    }
 }