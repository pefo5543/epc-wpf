using EpcDashboard.MVVMHelpers;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Processes
{
    public class ValidatableScript : ValidatableBindableBase
    {
        private string _script;

        [Required]
        public string Script
        {
            get { return _script; }
            set { SetProperty(ref _script, value); }
        }
    }
}
