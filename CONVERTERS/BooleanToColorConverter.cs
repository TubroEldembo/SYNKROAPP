using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SYNKROAPP.CONVERTERS
{
    public class BooleanToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnVenta = value is bool b && b;
            return isEnVenta ? new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)) // Verde
                             : new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36)); // Rojo
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
