using Microsoft.UI.Xaml.Data;

namespace FundTracker.Helpers;
public class DoubleToPercentageConverter : IValueConverter
{
    private static readonly char[] charToTrim = { ' ', '%' };
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double val)
        {
            return string.Format("{0:P1}", val);
        }
        return "-";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string str)
        {
            return double.Parse(str.Trim(charToTrim));
        }
        return double.NaN;
    }
}
