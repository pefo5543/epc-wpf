using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Actions.ShorthandActions;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;

namespace EpcDashboard.Actions.EXEs
{
    public class AddEditExeActionViewModel : AddEditActionViewModelBase
    {
        #region fields
        private Exe _editingExeAction = null;
        private SimpleExe _ExeAction;
        private IMainRepository _repo;
        private FileInfo _selectedFile = null;
        #endregion

        #region constructors
        public AddEditExeActionViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            BrowseCommand = new RelayCommand(OnBrowse);
        }
        #endregion

        #region properties/commands

        public SimpleExe ExeAction
        {
            get { return _ExeAction; }
            set { SetProperty(ref _ExeAction, value); }
        }

        public FileInfo SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            set { SetProperty(ref _selectedFile, value); }
        }

        public RelayCommand BrowseCommand { get; private set; }
        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetAction(Site site, Exe ExeInfo)
        {
            Site = site;
            _contentHeader = "Exe configuration for site " + Site.Name;
            _editingExeAction = ExeInfo;
            if (ExeAction != null) ExeAction.ErrorsChanged -= RaiseCanExecuteChanged;
            ExeAction = new SimpleExe();
            ExeAction.ErrorsChanged += RaiseCanExecuteChanged;
            CopyExeAction(ExeInfo, ExeAction);
        }

        private void CopyExeAction(Exe source, SimpleExe target)
        {
            target.ActionName = source.ActionName;
            target.Arguments = source.Arguments;
            if (EditMode && !String.IsNullOrEmpty(source.FileName))
            {
                target.FileName = Path.Combine(UserSettings.Default.SourcePath, Constants.ExecutablesFolder, source.FileName);
            } else
            {
                target.FileName = source.FileName;
            }
        }

        private bool CanSave()
        {
            return !ExeAction.HasErrors;
        }

        private void OnSave()
        {
            UpdateExeAction(_ExeAction, _editingExeAction);
            if (SelectedFile != null && SelectedFile.FullName == _editingExeAction.FileName)
            {
                //Copy file to server and return filename - we dont want the whole path 
                _editingExeAction.FileName = _repo.SaveExeOnServerAndLocal(SelectedFile);
                //reset selectedfile
                SelectedFile = null;
            } else
            {
                SelectedFile = null;
            }

            _repo.AddEditExeAction(Site, _editingExeAction, EditMode);

            Done("Run Exe action configured"); ;
        }

        private void UpdateExeAction(SimpleExe source, Exe target)
        {
            target.ActionName = source.ActionName;
            target.ActionType = Constants.TypeEXETxt;
            target.Arguments = source.Arguments;
            if (EditMode && !String.IsNullOrEmpty(source.FileName))
            {
                target.FileName = Path.GetFileName(source.FileName);
            } else
            {
                target.FileName = source.FileName;
            }
        }

        private void OnBrowse()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an executable file";
            op.Filter = "\"Executable files (*.exe)|*.exe;\"";
            if (op.ShowDialog() == true)
            {
                SelectedFile = new FileInfo(op.FileName);
                ExeAction.FileName = op.FileName;
            }
        }
        #endregion
    }
}
