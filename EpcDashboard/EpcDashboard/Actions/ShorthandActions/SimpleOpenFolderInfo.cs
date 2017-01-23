using EpcDashboard.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class SimpleOpenFolderInfo : SimpleEditableServerBase
    {
        private string _serverUserName;
        private string _serverPassword;

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
    }
}