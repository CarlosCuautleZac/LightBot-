using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightBot.Helpers
{
    internal class CommandToWinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string mensaje = value.ToString();

            if (mensaje.Contains("Felicidades"))
                return "SiguienteNivelCommand";
            else
                return "VerNivelesCommand";


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
