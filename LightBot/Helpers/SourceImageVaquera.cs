using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightBot.Helpers
{
    internal class SourceImageVaquera : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ruta = "";
            if (value is bool == true)
            {
                ruta = "/Assets/cowGirlGolpeada.png";
            }
            else if (value is byte == false)
            {
                ruta = "/Assets/cowGirl.png";
            }
            return ruta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
