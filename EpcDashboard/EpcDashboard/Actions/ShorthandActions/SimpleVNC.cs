using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class SimpleVNC : SimpleEditableServerBase
    {
        private string _vncPassword;

        [Required]
        [MaxLength(25, ErrorMessage = "Password must be 25 characters or less")]
        public string VNCPassword
        {
            get
            {
                return _vncPassword;
            }
            set
            {
                SetProperty(ref _vncPassword, value);
            }
        }
    }
}
