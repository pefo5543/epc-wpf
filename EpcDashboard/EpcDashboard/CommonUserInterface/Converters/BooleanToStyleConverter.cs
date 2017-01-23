using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EpcDashboard.CommonUserInterface.Converters
{
    public class BooleanToStyleConverter : IMultiValueConverter
    {
        public BooleanToStyleConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement targetElement = values[0] as FrameworkElement;
            bool? isOnline = values[1] as bool?;
            string onlineStyleName = "buttonEPC";
            string offlineStyleName = "offlineButton";
            Style style = null;

            if (isOnline == true)
            {
                style = (Style)targetElement.TryFindResource(onlineStyleName);
            } else if(isOnline == false)
            {
                style = (Style)targetElement.TryFindResource(offlineStyleName);
            }

            return style;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
