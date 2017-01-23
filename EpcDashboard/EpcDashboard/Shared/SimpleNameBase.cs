using EpcDashboard.MVVMHelpers;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Shared
{
    public class SimpleNameBase : ValidatableBindableBase
    {
        private string _name;

        [Required]
        [MaxLength(25, ErrorMessage = "Name must be 25 characters or less")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}
