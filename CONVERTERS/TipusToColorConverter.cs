using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SYNKROAPP.CONVERTERS
{
    public class TipusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value?.ToString())
            {
                case "Entrada":
                    return new SolidColorBrush(Color.FromRgb(76, 175, 80)); // verde
                case "Sortida":
                    return new SolidColorBrush(Color.FromRgb(244, 67, 54)); // rojo
                case "TrasllatIntern":
                    return new SolidColorBrush(Color.FromRgb(33, 150, 243)); // azul
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Para conversores de un solo sentido
            return DependencyProperty.UnsetValue;
        }
    }

}
