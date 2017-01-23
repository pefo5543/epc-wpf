using EpcDashboard.CustomValidations;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace EpcDashboard.Processes
{
    public class SimpleEditableProcess : SimpleNameBase
    {
        private string _processIcon;
        private string _processIconPath;

        public string ProcessIcon
        {
            get { return _processIcon; }
            set { SetProperty(ref _processIcon, value); }
        }
        [FileValidation(new string[] { "png", "ico" },
        ErrorMessage = "{0} must be of type .png or .ico")]
        [FileSourceValidation(50000, ErrorMessage = "{0} path does not exist or image is to big")]
        public string ProcessIconPath
        {
            get { return _processIconPath; }
            set { SetProperty(ref _processIconPath, value); }
        }

        public void SetProcessIconPath()
        {
            if (!String.IsNullOrEmpty(_processIcon))
            {
                ProcessIconPath = Path.Combine(UserSettings.Default.SourcePath, "Images", _processIcon);
            }
            else
            {
                ProcessIconPath = _processIcon;
            }         
        }
    }
}