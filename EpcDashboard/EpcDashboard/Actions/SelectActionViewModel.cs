using Epc.Data.Models;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using System;

namespace EpcDashboard.Actions
{
    public class SelectActionViewModel : ViewModelBase, IPageBaseViewModel
    {
        #region fields
        private Site _site;
        private string _selectedAction;

        #endregion

        #region constructors
        public SelectActionViewModel()
        {
            SelectionChangedCommand = new RelayCommand(OnSelectionChanged);
            CancelCommand = new RelayCommand(OnCancel);
            _contentHeader = "Select action";
        }
        #endregion

        #region properties/commands

        public RelayCommand CancelCommand { get; private set; }
        public event Action Cancel = delegate { };

        public Site Site
        {
            get { return _site; }
            set { SetProperty(ref _site, value); }
        }

        public string SelectedActionOption
        {
            get { return _selectedAction; }
            set { SetProperty(ref _selectedAction, value); }
        }

        public string ContentHeader
        {
            get
            {
                return _contentHeader;
            }
        }

        public RelayCommand SelectionChangedCommand { get; private set; }
        public event Action<string> ActionSelected = delegate { };

        #endregion

        #region methods

        private void OnSelectionChanged()
        {
            ActionSelected(_selectedAction);
        }

        private void OnCancel()
        {
            Cancel();
        }
        #endregion
    }
}
