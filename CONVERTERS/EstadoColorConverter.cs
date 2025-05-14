using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SYNKROAPP.CONVERTERS
{
    public class EstadoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = value?.ToString()?.ToLower();

            return estado switch
            {
                "Nuevo" => new SolidColorBrush(Colors.Green),
                "Usado" => new SolidColorBrush(Colors.OrangeRed),
                "Roto" => new SolidColorBrush(Colors.Red),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
