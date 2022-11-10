using LightBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightBot.Helpers
{
    public class PosicionVaqueraConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             char juego = (char)value;
            if (juego == '1')
                return 0;
            else if (juego == '2')
                return 1;
            else if (juego == '3')
                return 2;
            else if (juego == '4')
                return 3;
            else 
                return 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
