using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Actions.ShorthandActions;
using System;
using System.ComponentModel;
using EpcDashboard.Services.Interfaces;

namespace EpcDashboard.Actions.RDPs
{
    public class AddEditRDPActionViewModel : AddEditActionViewModelBase
    {
        #region fields
        private RDP _editingRDPAction = null;
        private SimpleRDP _RDPAction;
        private IMainRepository _repo;
        #endregion

        #region constructors
        public AddEditRDPActionViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }
        #endregion

        #region properties/commands

        public SimpleRDP RDPAction
        {
            get { return _RDPAction; }
            set { SetProperty(ref _RDPAction, value); }
        }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetAction(Site site, RDP rdpInfo)
        {
            Site = site;
            _contentHeader = "Remote desktop configuration for site " + Site.Name;
            _editingRDPAction = rdpInfo;
            if (RDPAction != null) RDPAction.ErrorsChanged -= RaiseCanExecuteChanged;
            RDPAction = new SimpleRDP();
            RDPAction.ErrorsChanged += RaiseCanExecuteChanged;
            CopyRDPAction(rdpInfo, RDPAction);
            if (!EditMode)
            {
                SetDefaultValues(RDPAction, "Run RDP", Site.IpAdress);
            }
        }

        private void CopyRDPAction(RDP source, SimpleRDP target)
        {
            target.ActionName = source.ActionName;
            target.Port = source.Port;
            target.Domain = source.Domain;
            target.IpAdress = source.IpAdress;
            target.ServerUserName = source.UserName;
            target.ServerPassword = source.Password;
        }

        private bool CanSave()
        {
            return !RDPAction.HasErrors;
        }

        private void OnSave()
        {
            UpdateRDPAction(_RDPAction, _editingRDPAction);
            _repo.AddEditRDPAction(Site, _editingRDPAction, EditMode);

            Done("Run RDP action configured"); ;
        }

        private void UpdateRDPAction(SimpleRDP source, RDP target)
        {
            target.ActionName = source.ActionName;
            target.Port = source.Port;
            target.Domain = source.Domain;
            target.IpAdress = source.IpAdress;
            target.UserName = source.ServerUserName;
            target.Password = source.ServerPassword;
            target.ActionType = Constants.TypeRDPTxt;
        }
        #endregion
    }
}
