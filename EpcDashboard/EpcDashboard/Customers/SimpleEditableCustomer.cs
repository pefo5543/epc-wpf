using EpcDashboard.CustomValidations;
using EpcDashboard.Shared;
using System;
using System.IO;

namespace EpcDashboard.Customers
{
    public class SimpleEditableCustomer : SimpleNameBase
    {
        private string _customerIcon;
        private string _customerIconPath;
        private string _color;

        public SimpleEditableCustomer()
        {
            Color = Constants.DefaultButtonColor;
        }
        public string CustomerIcon
        {
            get { return _customerIcon; }
            set { SetProperty(ref _customerIcon, value); }
        }
        [FileValidation(new string[] { "png", "ico" },
        ErrorMessage = "{0} must be of type .png or .ico")]
        [FileSourceValidation(50000, ErrorMessage = "{0} path does not exist or image is to big")] //Max size limit set to 50kB
        public string CustomerIconPath
        {
            get { return _customerIconPath; }
            set { SetProperty(ref _customerIconPath, value); }
        }

        public void SetCustomerIconPath()
        {
            if(!String.IsNullOrEmpty(_customerIcon))
            {
                CustomerIconPath = Path.Combine(UserSettings.Default.SourcePath, "Images", _customerIcon);
            } else
            {
                CustomerIconPath = _customerIcon;
            }
        }

        public string Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }
    }
}
