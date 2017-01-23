using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EpcDashboard.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NumericValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string port = (String)value;

            return PortCheck(port);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }

        public bool PortCheck(string port)
        {
            int n;
            bool isNumeric = int.TryParse(port, out n);
            return isNumeric;
        }
    }
}
