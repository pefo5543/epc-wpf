using EpcDashboard.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpcDashboard.ViewModelBases
{
    public class AddEditViewModelBase : ViewModelBase
    {
        protected bool _EditMode;

        public AddEditViewModelBase()
        {
            CancelCommand = new RelayCommand(OnCancel);
        }
        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }

        public string ContentHeader
        {
            get
            {
                return _contentHeader;
            }
        }

        public RelayCommand CancelCommand { get; protected set; }
        public RelayCommand SaveCommand { get; protected set; }

        public event Action Cancel = delegate { };

        protected void OnCancel()
        {
            Cancel();
        }
    }
}
