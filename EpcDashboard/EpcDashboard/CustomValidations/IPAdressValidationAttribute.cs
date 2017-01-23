using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;

namespace EpcDashboard.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IPAdressValidationAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            string ipAdress = (String)value;

            return IpCheck(ipAdress);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }

        public bool IpCheck(string strIP)
        {
            IPAddress adress;
            if (IPAddress.TryParse(strIP, out adress)) return true;
            else return false;
        }

    }
}
