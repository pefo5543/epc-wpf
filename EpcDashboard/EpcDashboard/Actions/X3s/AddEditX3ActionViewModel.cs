using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.Shared;
using System;
using System.ComponentModel;

namespace EpcDashboard.Actions.X3s
{
    public class AddEditX3ActionViewModel : AddEditActionViewModelBase
    {
        #region fields
        private EX3 _editingEX3 = null;
        private SimpleDbInfo _EX3;
        private IMainRepository _repo;
        #endregion

        #region constructors
        public AddEditX3ActionViewModel(IMainRepository repo)
        {
            _repo = repo;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }
        #endregion

        #region properties/commands

        public SimpleDbInfo X3
        {
            get { return _EX3; }
            set { SetProperty(ref _EX3, value); }
        }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetAction(Site site, EX3 x3Info)
        {
            Site = site;
            _contentHeader = "Starting X3 action configuration for site " + Site.Name;
            _editingEX3 = x3Info;
            if (X3 != null) X3.ErrorsChanged -= RaiseCanExecuteChanged;
            X3 = new SimpleDbInfo();
            X3.ErrorsChanged += RaiseCanExecuteChanged;
            CopyX3Action(x3Info, X3);
            if (!EditMode)
            {
                SetDefaultValues(X3, "Start Empiri X3", Site.IpAdress);
            }
        }

        private void CopyX3Action(EX3 source, SimpleDbInfo target)
        {
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.UserName = source.DbUserName;
            target.Name = source.DbName;
            target.Password = source.DbPassword;
        }

        private bool CanSave()
        {
            return !X3.HasErrors;
        }

        private void OnSave()
        {
            UpdateX3Action(X3, _editingEX3);
            _repo.AddEditX3Action(Site, _editingEX3, EditMode);

            Done("X3 Action configured"); ;
        }

        private void UpdateX3Action(SimpleDbInfo source, EX3 target)
        {
            target.ActionType = Constants.TypeX3Txt;
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.DbUserName = source.UserName;
            target.DbName = source.Name;
            target.DbPassword = source.Password;
        }

        #endregion
    }
}
