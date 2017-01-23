using Epc.Data;
using Epc.Data.Models.ActionModels;

namespace EpcDashboard.ViewModelBases
{
    public class ViewModelBase : BindableBase
    {
        private bool _showAsLink;
        protected string _contentHeader;

        public bool ShowAsLink
        {
            get
            {
                return _showAsLink;
            }
            set { SetProperty(ref _showAsLink, value); }
        }

        protected bool CompareEBMS(EBMS ebms)
        {
            //Compare EBMS content with empty EBMS object - returns true if something differ
            return EpcBackgroundSync.EBMSCheck(ebms, new EBMS());
        }
    }
}
