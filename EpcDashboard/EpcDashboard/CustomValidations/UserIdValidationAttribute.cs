using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EpcDashboard.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    public sealed class UserIdValidationAttribute : ValidationAttribute
    {

        // Internal field to hold the mask value. 
        readonly string _mask;

        public string Mask
        {
            get { return _mask; }
        }

        public UserIdValidationAttribute(string mask)
        {
            _mask = mask;
        }


        public override bool IsValid(object value)
        {
            var userId = (String)value;
            bool result = true;
            if (this.Mask != null)
            {
                result = MatchesMask(this.Mask, userId);
            }
            return result;
        }
 
        internal bool MatchesMask(string mask, string userId)
        {
            userId = userId.ToLower();
            mask = mask.ToLower();
            if (mask.Length != userId.Trim().Length)
            {
                // Length mismatch. 
                return false;
            }

            for (int i = 0; i < mask.Length; i++)
            {
                if (i == 0 || i == 1 || i == 2)
                {
                    //check first three characters - prefix - always identical
                    if (mask[i] != userId[i])
                    {
                        return false;
                    }
                } else
                {
                    if (mask[i] == 'd' || char.IsNumber(userId[i]) || !char.IsLetter(userId[i]))
                    {
                        // Letter expected at this position. 
                        return false;
                    }
                    //if (mask[i] == '_' && userId[i] != '_')
                    //{
                    //    // Spacing character expected at this position. 
                    //    return false;
                    //}

                }
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name, this.Mask);
        }
    }
}
