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
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public bool Negate { get; set; }
        public Visibility FalseVisibility { get; set; }
        public EmptyStringToVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String || value == null)
            {
                string result = (string)value;
                bool bVal = String.IsNullOrEmpty(result);
                if (bVal && Negate) return Visibility.Visible;
                if (bVal && !Negate) return FalseVisibility;
                if (!bVal && Negate) return FalseVisibility;
                if (!bVal && !Negate) return Visibility.Visible;
                else return Visibility.Visible;
            }
            else return FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
