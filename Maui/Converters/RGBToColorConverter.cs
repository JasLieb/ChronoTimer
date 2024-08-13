using ChronoTimer.Core;
using System.Globalization;

namespace ChronoTimer.Maui;

public class RGBToColorConverter : IValueConverter
{
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            value is RGB rgb 
            ? Color.FromRgb(rgb.Red, rgb.Green, rgb.Blue) 
            : Colors.Transparent;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            new RGB(0,0,0);
}