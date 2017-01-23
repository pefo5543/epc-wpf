using EpcDashboard.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class SimpleEditableServerBase : EditableBaseAction
    {
        private string _ipAdress;

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
