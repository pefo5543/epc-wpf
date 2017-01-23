using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Actions.ShorthandActions;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using System;
using System.ComponentModel;

namespace EpcDashboard.Actions.VNCs
{
    public class AddEditVNCActionViewModel : AddEditActionViewModelBase
    {
        #region fields
        private VNC _editingVNCAction = null;
        private SimpleVNC _VNCAction;
        private IMainRepository _repo;
        #endregion

        #region constructors
        public AddEditVNCActionViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }
        #endregion

        #region properties/commands

        public SimpleVNC VNCAction
        {
            get { return _VNCAction; }
            set { SetProperty(ref _VNCAction, value); }
        }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetAction(Site site, VNC VNCInfo)
        {
            Site = site;
            _contentHeader = "VNC configuration for site " + Site.Name;
            _editingVNCAction = VNCInfo;
            if (VNCAction != null) VNCAction.ErrorsChanged -= RaiseCanExecuteChanged;
            VNCAction = new SimpleVNC();
            VNCAction.ErrorsChanged += RaiseCanExecuteChanged;
            CopyVNCAction(VNCInfo, VNCAction);
            if (!EditMode)
            {
                SetDefaultValues(VNCAction, "Run VNC", Site.IpAdress);
                VNCAction.VNCPassword = "xtel";
            }
        }

        private void CopyVNCAction(VNC source, SimpleVNC target)
        {
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.VNCPassword = source.VNCPassword;
        }

        private bool CanSave()
        {
            return !VNCAction.HasErrors;
        }

        private void OnSave()
        {
            UpdateVNCAction(_VNCAction, _editingVNCAction);
            _repo.AddEditVNCAction(Site, _editingVNCAction, EditMode);

            Done("Run VNC action configured"); ;
        }

        private void UpdateVNCAction(SimpleVNC source, VNC target)
        {
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.VNCPassword = source.VNCPassword;
            target.ActionType = Constants.TypeVNCTxt;
        }
        #endregion
    }
}
