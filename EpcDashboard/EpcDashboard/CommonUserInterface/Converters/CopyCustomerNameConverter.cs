using Epc.Data.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EpcDashboard.CommonUserInterface.Converters
{
    public class CopyCustomerNameConverter : IMultiValueConverter
    {
        public CopyCustomerNameConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            try
            {
                //array values should contain [0] - boolean IsSite, [1] - NameBaseModel object
                result = GetCustomerName((bool)values[0], (NameBaseModel)values[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("CustomerNameConverter error: {0}", e.Message);
                result = "";
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetCustomerName(bool isSite, NameBaseModel obj)
        {
            string customerName;
            if (isSite)
            {
                customerName = ((Customer)obj).Name;
            } else
            {
                customerName = ((Site)obj).CustomerName;
            }


            return customerName;
        }

        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Parameter must be a string");
            int count = input.Count();
            if (count > 1)
            {
                return input.First().ToString().ToUpper() + input.Substring(1);
            }
            else
            {
                return input.First().ToString().ToUpper();
            }

        }
    }
}
