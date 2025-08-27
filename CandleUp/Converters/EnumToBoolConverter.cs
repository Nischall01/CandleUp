using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace CandleUp.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return false;
        return value.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool and true ? parameter : AvaloniaProperty.UnsetValue;
    }
}