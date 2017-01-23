using EpcDashboard.Shared;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.EBMSs
{
    public class SimpleEditableEBMS : SimpleDbInfo
    {
        private string _homepage;

        [Required]
        [MaxLength(50, ErrorMessage = "Max 50 characters...")]
        public string Homepage
        {
            get { return _homepage; }
            set { SetProperty(ref _homepage, value); }
        }
    }
}
