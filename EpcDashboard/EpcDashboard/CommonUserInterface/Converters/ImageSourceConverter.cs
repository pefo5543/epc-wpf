using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EpcDashboard.CommonUserInterface.Converters
{
    public class ImageSourceConverter : IMultiValueConverter
    {
        public ImageSourceConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //array values should contain [0] - path, [1] - file name
            return LoadImage((string)values[0], (string)values[1]);
        }

        private BitmapImage LoadImage(string path, string fileName)
        {
            BitmapImage bitmap = null; 
            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(fileName))
            {
                var fullPath = Path.Combine(path, "Images", fileName);
                if (File.Exists(fullPath))
                {
                    var uri = new Uri(fullPath);
                    try
                    {
                        bitmap = new BitmapImage(uri);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caught Exception loading image [{0}]", e.Message);
                    }
                }
            }

            return bitmap;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
