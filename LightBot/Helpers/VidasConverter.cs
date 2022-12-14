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
    internal class VidasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "2")
            {
                return "&#x2764; &#x2764;";
            }
            if (value.ToString() == "1")
            {
                return "&#x2764; &#x274C;";
            }
            if (value.ToString() == "0")
            {
                return "&#x274C; &#x274C;";
            }
            else if (value.ToString() == "0")
            {
                return "&#x274C; &#x274C;";
            }



            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
