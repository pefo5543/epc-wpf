using EpcDashboard.Dialogs;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Interop;

namespace EpcDashboard.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        #region fields
        private string _name;
        private ISettingsRepository _repo;
        private Setting _Setting = null;
        private EditableSetting _editableSetting;
        #endregion

        #region constructors
        public SettingsViewModel(ISettingsRepository repo)
        {
            _repo = repo;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            BrowseCommand = new RelayCommand(OnBrowse);
        }

        public SettingsViewModel()
        {
        }
        #endregion

        #region properties/commands
        public string Name
        {
            get
            {
                return _name;
            }
            set { SetProperty(ref _name, value); }
        }

        public Setting Setting
        {
            get { return _Setting; }
            set { SetProperty(ref _Setting, value); }
        }

        public EditableSetting EditableSetting
        {
            get { return _editableSetting; }
            set { SetProperty(ref _editableSetting, value); }
        }

        #endregion

        #region methods
        public void SetSetting(Setting UserSettings)
        {
            Setting = UserSettings;
            if (EditableSetting != null) EditableSetting.ErrorsChanged -= RaiseCanExecuteChanged;
            EditableSetting = new EditableSetting();
            EditableSetting.ErrorsChanged += RaiseCanExecuteChanged;
            CopySetting(Setting, EditableSetting);
        }

        private void CopySetting(Setting source, EditableSetting target)
        {
            target.UserId = source.UserId;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.WinStart = source.WinStart;
            target.SourcePath = source.SourcePath;
        }

        public event Action Done = delegate { };
        public event Action DoneCancel = delegate { };

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand BrowseCommand { get; private set; }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            return !EditableSetting.HasErrors;
        }

        private void OnSave()
        {
            UpdateSetting(EditableSetting, Setting);
            try
            {
                _repo.UpdateSetting(Setting);
            }
            catch (Exception)
            {
                throw;
            }
            Done();
        }
        private void OnBrowse()
        {
            //Open Browse Folder dialogue
            System.Windows.Forms.FolderBrowserDialog dlg = new FolderBrowserDialog();
            IntPtr mainWindowPtr = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;

            dlg.SelectedPath = EditableSetting.SourcePath;
            dlg.ShowDialog(new OldWindow(mainWindowPtr));
            EditableSetting.SourcePath = dlg.SelectedPath;
        }

        private void UpdateSetting(EditableSetting source, Setting target)
        {
            if(source.SourcePath != target.SourcePath)
            {
                //sourcepath changed - delete old directory
                _repo.CleanupSourcePath(target.SourcePath);
            }
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.UserId = source.UserId;
            target.WinStart = source.WinStart;
            target.SourcePath = source.SourcePath;
        }

        private void OnCancel()
        {
            DoneCancel();
        }
        #endregion
    }
}
