using System.ComponentModel.DataAnnotations;
using EpcDashboard.Actions.ShorthandActions;

namespace EpcDashboard.Shared
{
    public class SimpleDbInfo : SimpleEditableServerBase
    {
        private string _userName;
        private string _password;
        private string _name;

        [Required]
        [MaxLength(50, ErrorMessage = "Name must be 50 characters or less")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [Required]
        [MaxLength(50, ErrorMessage = "Username must be 50 characters or less")]
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        [Required]
        [MaxLength(50, ErrorMessage = "Password must be 50 characters or less")]
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
    }
}
