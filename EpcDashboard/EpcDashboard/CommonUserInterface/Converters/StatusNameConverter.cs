using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EpcDashboard.CommonUserInterface.Converters
{
    public class StatusNameConverter : IMultiValueConverter
    {
        public StatusNameConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            try
            {
                //array values should contain [0] - first name, [1] - last name, [2] - userid
                result = CreateFullNameLabel((string)values[0], (string)values[1], (string)values[2]);
            }
            catch(Exception e)
            {
                Console.WriteLine("StatusNameConverter: String conversion problem...{0}", e.Message);
                result = "";
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string CreateFullNameLabel(string first, string last, string id)
        {
            if(!String.IsNullOrEmpty(first))
            {
                try
                {
                    first = FirstCharToUpper((string)first);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception during string conversion, msg: {0}", e.Message);
                }
            }
            if (!String.IsNullOrEmpty(last))
            {
                try
                {
                    last = FirstCharToUpper((string)last);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception during string conversion, msg: {0}", e.Message);
                }
            }
            if (id == null)
            {
                id = "";
            }
            string res = first + " " + last + " (" + id + ")";

            return res;
        }

        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Parameter must be a string");
            int count = input.Count();
            if (count > 1)
            {
                return input.First().ToString().ToUpper() + input.Substring(1);
            } else
            {
                return input.First().ToString().ToUpper();
            }
            
        }
    }
}
