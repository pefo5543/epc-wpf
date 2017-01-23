using System;
using System.Globalization;
using System.Windows.Data;

namespace EpcDashboard.CommonUserInterface.Converters
{
    public class StatusImageConverter : IValueConverter
    {
        public StatusImageConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = "";
            if (value.Equals(true))
            {
                //status online image
                path = "pack://application:,,,/Resources/status_green.png";
            } else
            {
                //status offline image
                path = "pack://application:,,,/Resources/status_red.png";
            }

            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
