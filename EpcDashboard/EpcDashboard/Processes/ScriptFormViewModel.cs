using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.ViewModelBases;
using System;
using System.ComponentModel;

namespace EpcDashboard.Processes
{
    public class ScriptFormViewModel : ViewModelBase
    {
        #region fields
        private DbInfo _dbInfo = null;
        private Process _process = null;
        private ValidatableScript _script;

        #endregion

        #region constructors
        public ScriptFormViewModel()
        {
            CancelCommand = new RelayCommand(OnCancel);
            ExecuteCommand = new RelayCommand(OnExecute, CanSave);
        }
        #endregion

        #region properties/commands
        public DbInfo DbInfo
        {
            get { return _dbInfo; }
            set { SetProperty(ref _dbInfo, value); }
        }

        public Process Process
        {
            get { return _process; }
            set { SetProperty(ref _process, value); }
        }

        public ValidatableScript Script
        {
            get { return _script; }
            set { SetProperty(ref _script, value); }
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ExecuteCommand { get; private set; }

        public event Action<string> Done = delegate { };
        public event Action Cancel = delegate { };
        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ExecuteCommand.RaiseCanExecuteChanged();
        }

        public void SetScriptModel(Script s)
        {
            _dbInfo = s.DbInfo;
            _process = s.Process;
            if (Script != null) Script.ErrorsChanged -= RaiseCanExecuteChanged;
            Script = new ValidatableScript();
            Script.ErrorsChanged += RaiseCanExecuteChanged;
        }

        private bool CanSave()
        {
            return !Script.HasErrors;
        }

        private void OnExecute()
        {
            Done("Script executed message when implemented ");
        }

        private void OnCancel()
        {
            Cancel();
        }
        #endregion
    }
}
