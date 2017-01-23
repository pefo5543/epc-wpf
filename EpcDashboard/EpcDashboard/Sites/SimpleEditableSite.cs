using EpcDashboard.CustomValidations;
using EpcDashboard.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace EpcDashboard.Sites
{
    public class SimpleEditableSite : SimpleNameBase
    {
        private string _siteIcon;
        private string _siteIconPath;
        private string _ipAdress;


        public SimpleEditableSite()
        {
        }

        public string SiteIcon
        {
            get { return _siteIcon; }
            set { SetProperty(ref _siteIcon, value); }
        }
        [FileValidation(new string[] { "png", "ico" },
        ErrorMessage = "{0} must be of type .png or .ico")]
        [FileSourceValidation(50000, ErrorMessage = "{0} path does not exist or image is to big")]
        public string SiteIconPath
        {
            get { return _siteIconPath; }
            set { SetProperty(ref _siteIconPath, value); }
        }

        public void SetSiteIconPath()
        {
            if (!String.IsNullOrEmpty(_siteIcon))
            {
                SiteIconPath = Path.Combine(UserSettings.Default.SourcePath, "Images", _siteIcon);
            }
            else
            {
                SiteIconPath = _siteIcon;
            }
        }

        [Required]
        [MaxLength(39, ErrorMessage = "IP adress can be max 39 characters (IPv6)")]
        [IPAdressValidation(ErrorMessage = "Incorrect IP adress")]
        public string IpAdress
        {
            get { return _ipAdress; }
            set { SetProperty(ref _ipAdress, value); }
        }
    }
}