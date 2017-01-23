using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace EpcDashboard.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property |
      AttributeTargets.Field, AllowMultiple = false)]
    public sealed class FileValidationAttribute : ValidationAttribute
    {
        // Internal field to hold the mask value. 
        readonly string[] _mask;

        public string[] Mask
        {
            get { return _mask; }
        }

        public FileValidationAttribute(string[] mask)
        {
            _mask = mask;
        }


        public override bool IsValid(object value)
        {
            //Validation of string path - if user manually inputs a path - check that file extension is a legal one
            var icon = (String)value;
            bool result = false;
            if (this.Mask != null)
            {
                foreach (string m in Mask)
                {
                    if(MatchesMask(m, icon))
                    {
                        result = true;
                        break;
                    } else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        internal bool MatchesMask(string mask, string icon)
        {
            if(icon == null)
            {
                return true;
            } else
            {
                int iconLength = icon.Count();
                int maskLength = mask.Count();
                bool result = false;
                if (iconLength > maskLength)
                {
                    string fileExtension = icon.Split('.').LastOrDefault();
                    if (fileExtension == mask)
                    {
                        result = true;
                    }
                }
                else if (iconLength == 0)
                {
                    //No filepath set
                    result = true;
                }

                return result;
            }          
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name, this.Mask);
        }
    }
}
