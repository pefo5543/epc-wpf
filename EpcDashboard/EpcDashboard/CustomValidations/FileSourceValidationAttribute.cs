using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;

namespace EpcDashboard.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property |
      AttributeTargets.Field, AllowMultiple = false)]
    public sealed class FileSourceValidationAttribute : ValidationAttribute
    {
        // Internal field to hold the mask value. 
        readonly int _sizeLimit = 50000; //50kB

        public int SizeLimit
        {
            get { return _sizeLimit; }
        }
        public FileSourceValidationAttribute(int limit)
        {
            _sizeLimit = limit;
        }


        public override bool IsValid(object value)
        {
            var icon = (String)value;
            bool result = false;
            result = FileSizeCheck(icon);

            return result;
        }

        private bool FileSizeCheck(string icon)
        {
            bool result = true;
            //A file is not required, so check that path isnt null or empty string
            if (!String.IsNullOrEmpty(icon))
            {
                if (File.Exists(icon))
                {
                    FileInfo SelectedIcon = new FileInfo(@icon);
                    //Is the file too big to upload?
                    long fileSize = SelectedIcon.Length;
                    if (fileSize > (_sizeLimit))
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}
