using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightBot.Helpers
{
    internal class DeNivelAImagenSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nivel = (int)value;
            string ruta = "";
            if (nivel == 1)
            {
                ruta = "../Assets/Instrucciones1.png";
            }
            else if (nivel == 2)
            {
                ruta = "../Assets/Instrucciones2.png";
            }
            else if (nivel == 3)
            {
                ruta = "../Assets/Instrucciones3.png";
            }
            else if (nivel == 4)
            {
                ruta = "../Assets/Instrucciones4.png";
            }
            else if(nivel ==4)
            {
                ruta = "../Assets/Instrucciones4.png";
            }
            return ruta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
