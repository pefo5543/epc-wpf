using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using System;
using System.ComponentModel;

namespace EpcDashboard.Actions.EBMSs
{
    public class AddEditEBMSViewModel : AddEditActionViewModelBase
    {
        #region fields
        private string _context;
        private EBMS _editingEBMS = null;
        private IMainRepository _repo;
        private SimpleEditableEBMS _EBMS;
        private object _currentParent;

        #endregion

        #region constructors
        public AddEditEBMSViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }
        #endregion

        #region properties/commands
        //CurrentParent = Container object i.e. Customer/Site
        public object CurrentParent
        {
            get { return _currentParent; }
            set { SetProperty(ref _currentParent, value); }
        }

        public SimpleEditableEBMS EBMS
        {
            get { return _EBMS; }
            set { SetProperty(ref _EBMS, value); }
        }

        public event Action<string> Done = delegate { };
        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetEBMS(object obj, EBMS ebms, string context)
        {
            CurrentParent = obj;
            _context = context;
            string name = GetParentName(CurrentParent);
            _contentHeader = (EditMode ? "Edit " : "Add ") + "Website/EBMS Action configuration for " + _context + " " + name;
            _editingEBMS = ebms;
            if (EBMS != null) EBMS.ErrorsChanged -= RaiseCanExecuteChanged;
            EBMS = new SimpleEditableEBMS();
            EBMS.ErrorsChanged += RaiseCanExecuteChanged;
            CopyEBMS(ebms, EBMS);
            if (!EditMode)
            {
                SetDefaultValues(EBMS, "Open EBMS");
            }
        }

        private void CopyEBMS(EBMS source, SimpleEditableEBMS target)
        {
            target.ActionName = source.ActionName;
            target.Homepage = source.Homepage;
            //DbInfo
            target.Name = source.DbName;
            target.IpAdress = source.IpAdress;
            target.UserName = source.DbUserName;
            target.Password = source.DbPassword;
        }

        private bool CanSave()
        {
            return !EBMS.HasErrors;
        }

        private void OnSave()
        {
            UpdateEBMS(EBMS, _editingEBMS);
            _repo.GenericSave();

            Done(_context);
        }

        private void UpdateEBMS(SimpleEditableEBMS source, EBMS target)
        {
            target.ActionType = "OpenEMBS";
            target.ActionName = source.ActionName;
            target.Homepage = source.Homepage;
            target.IpAdress = source.IpAdress;
            //DbInfo
            target.DbName = source.Name;
            target.DbUserName = source.UserName;
            target.DbPassword = source.Password;
        }

        private string GetParentName(object parent)
        {
            string name = null;
            if (parent is Customer)
            {
                name = (parent as Customer).Name;
            }
            else if (parent is Site)
            {
                name = (parent as Site).Name;
            }

            return name;
        }
        #endregion
    }
}
