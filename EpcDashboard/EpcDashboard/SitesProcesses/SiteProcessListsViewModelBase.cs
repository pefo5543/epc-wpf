using EpcDashboard.MVVMHelpers;
using EpcDashboard.ViewModelBases;
using System;

namespace EpcDashboard.SitesProcesses
{
    /// <summary>
    /// Shared class for SiteListViewModel and ProcessListViewModel
    /// </summary>
    public class SiteProcessListsViewModelBase : ListViewModelBase
    {
        private bool _hasEBMS;

        public SiteProcessListsViewModelBase()
        {
            CopyCommand = new RelayCommand<object>(OnCopy);
        }

        public bool HasEBMS
        {
            get { return _hasEBMS; }
            set
            {
                SetProperty(ref _hasEBMS, value);
            }
        }

        public RelayCommand AddEBMSCommand { get; protected set; }
        public RelayCommand EditEBMSCommand { get; protected set; }
        public RelayCommand DeleteEBMSCommand { get; protected set; }
        public RelayCommand OpenEBMSCommand { get; protected set; }
        public RelayCommand<object> CopyCommand { get; private set; }

        public event Action<object> CopyRequest = delegate { };

        private void OnCopy(object entity)
        {
            CopyRequest(entity);
        }
    }
}
