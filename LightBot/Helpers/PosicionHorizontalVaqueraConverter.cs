using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LightBot.Models;
using LightBot.ViewModels;

namespace LightBot.Helpers
{
    public class PosicionHorizontalVaqueraConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            char juego = (char)value;
           


            if (juego == 'A')
                return 0;
            else if (juego == 'B')
                return 1;
            else if (juego == 'C')
                return 2;
            else if (juego == 'D')
                return 3;
            else /*if (juego.Posicion[1] == 'E')*/
                return 4;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
