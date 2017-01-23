using EpcDashboard.MVVMHelpers;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class EditableBaseAction : ValidatableBindableBase
    {
        private string _actionName;

        [Required]
        [MaxLength(20, ErrorMessage = "ActionName must be 20 characters or less")]
        public string ActionName
        {
            get { return _actionName; }
            set { SetProperty(ref _actionName, value); }
        }
    }
}
