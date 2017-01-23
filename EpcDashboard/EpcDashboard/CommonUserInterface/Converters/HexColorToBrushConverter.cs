using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EpcDashboard.CommonUserInterface.Converters
{
    class HexColorToBrushConverter : IValueConverter
    {
        public bool LightVersion { get; set; }
        public Color ColorBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String || value == null)
            {
                string hex = (string)value;
                if (!String.IsNullOrEmpty(hex))
                {
                    ColorBrush = (Color)(ColorConverter.ConvertFromString(hex));
                } else
                {
                    //Default color
                    ColorBrush = (Color)(ColorConverter.ConvertFromString(Constants.DefaultButtonColor));
                }
                if (LightVersion)
                {
                    ColorBrush = CreateLightVersion(ColorBrush, 0.7f);
                }
            }

            return ColorBrush;
        }

        private Color CreateLightVersion(Color color, float correctionFactor)
        {
            float red = (255 - color.R) * correctionFactor + color.R;
            float green = (255 - color.G) * correctionFactor + color.G;
            float blue = (255 - color.B) * correctionFactor + color.B;
            Color lighterColor = Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);

            return lighterColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
