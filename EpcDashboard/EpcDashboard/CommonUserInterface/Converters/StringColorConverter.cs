using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EpcDashboard.CommonUserInterface.Converters
{
    class StringColorConverter : IValueConverter
    {
        public Color ColorBrush { get; set; }
        public StringColorConverter()
        {
            //set default color
            ColorBrush = (Color)ColorConverter.ConvertFromString(Constants.DefaultButtonColor);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String)
            {
                string hex = (string)value;
                if (!String.IsNullOrEmpty(hex))
                {
                    ColorBrush = (Color)ColorConverter.ConvertFromString(hex);
                }
            }

            return ColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
            {
                Color c = (Color)value;
                return ToHexColor(c, true);
            }
            else
            {

                return Constants.DefaultButtonColor;
            }
        }

        public static string ToHexColor(Color color, bool alphaChannel)
        {
            return String.Format("#{0}{1}{2}{3}",
                                 alphaChannel ? color.A.ToString("X2") : String.Empty,
                                 color.R.ToString("X2"),
                                 color.G.ToString("X2"),
                                 color.B.ToString("X2"));
        }
    }
}
