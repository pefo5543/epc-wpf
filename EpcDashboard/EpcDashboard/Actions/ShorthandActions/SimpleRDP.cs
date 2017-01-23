using EpcDashboard.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class SimpleRDP : SimpleEditableServerBase
    {
        private string _serverUserName;
        private string _serverPassword;
        private string _port;
        private string _domain;

        [Required]
        [MaxLength(25, ErrorMessage = "User name must be 25 characters or less")]
        public string ServerUserName
        {
            get
            {
                return _serverUserName;
            }
            set
            {
                SetProperty(ref _serverUserName, value);
            }
        }

        [Required]
        [MaxLength(25, ErrorMessage = "Password must be 25 characters or less")]
        public string ServerPassword
        {
            get
            {
                return _serverPassword;
            }
            set
            {
                SetProperty(ref _serverPassword, value);
            }
        }

        [Required]
        [MaxLength(4, ErrorMessage = "Port must be a number between 1-9999")]
        [NumericValidation(ErrorMessage = "Port property must be numeric")]
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                SetProperty(ref _port, value);
            }
        }

        [Required]
        [MaxLength(25, ErrorMessage = "Domain can be 25 characters or less")]
        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                SetProperty(ref _domain, value);
            }
        }

    }
}
