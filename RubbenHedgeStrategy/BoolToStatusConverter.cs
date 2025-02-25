using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RubbenHedgeStrategy
{
    public class BoolToStatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool isActive)
            {
                return isActive ? "Strategy is active." : "Strategy is not active.";
            }
            return "Status unknown.";
        }

        public object ConvertBack(object value, Type targettype, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
