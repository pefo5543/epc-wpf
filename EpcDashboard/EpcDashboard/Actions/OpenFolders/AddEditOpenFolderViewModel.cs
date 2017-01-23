using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Actions.ShorthandActions;
using System;
using System.ComponentModel;
using EpcDashboard.Services.Interfaces;

namespace EpcDashboard.Actions.OpenFolders
{
    public class AddEditOpenFolderViewModel : AddEditActionViewModelBase
    {
        #region fields
        private OpenFolder _editingFolderAction = null;
        private SimpleOpenFolderInfo _FolderAction;
        private IMainRepository _repo;
        #endregion

        #region constructors
        public AddEditOpenFolderViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }
        #endregion

        #region properties/commands

        public SimpleOpenFolderInfo FolderAction
        {
            get { return _FolderAction; }
            set { SetProperty(ref _FolderAction, value); }
        }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetAction(Site site, OpenFolder serverInfo)
        {
            Site = site;
            _contentHeader = "Open folder action configuration for site " + Site.Name;
            _editingFolderAction = serverInfo;
            if (FolderAction != null) FolderAction.ErrorsChanged -= RaiseCanExecuteChanged;
            FolderAction = new SimpleOpenFolderInfo();
            FolderAction.ErrorsChanged += RaiseCanExecuteChanged;
            CopyFolderAction(serverInfo, FolderAction);
             if(!EditMode)
            {
                SetDefaultValues(FolderAction, "Open server folder", Site.IpAdress);
            }
        }

        private void CopyFolderAction(OpenFolder source, SimpleOpenFolderInfo target)
        {
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.ServerUserName = source.UserName;
            target.ServerPassword = source.Password;
        }

        private bool CanSave()
        {
            return !FolderAction.HasErrors;
        }

        private void OnSave()
        {
            UpdateFolderAction(FolderAction, _editingFolderAction);
            _repo.AddEditOpenFolderAction(Site, _editingFolderAction, EditMode);

            Done("Open folder action configured"); ;
        }

        private void UpdateFolderAction(SimpleOpenFolderInfo source, OpenFolder target)
        {
            target.ActionName = source.ActionName;
            target.IpAdress = source.IpAdress;
            target.UserName = source.ServerUserName;
            target.Password = source.ServerPassword;
            target.ActionType = Constants.TypeOpenFolderTxt;
        }
        #endregion
    }
}
