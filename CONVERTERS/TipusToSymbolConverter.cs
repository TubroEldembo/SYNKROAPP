using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SYNKROAPP.CONVERTERS
{
    public class TipusToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value?.ToString())
            {
                case "Entrada":
                    return "↓";
                case "Sortida":
                    return "↑";
                case "TrasllatIntern":
                    return "⇄";
                default:
                    return "?";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Para conversores de un solo sentido
            return DependencyProperty.UnsetValue;
        }
    }

}
