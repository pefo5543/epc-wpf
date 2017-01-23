using Epc.Data.Models;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EpcDashboard.Processes
{
    public class AddEditProcessViewModel : AddEditViewModelBase
    {
        #region fields
        private Process _editingProcess = null;
        private IMainRepository _repo;
        private ImageSource _selectedIcon = null;
        private SimpleEditableProcess _Process;
        private Site _currentSite;

        #endregion

        #region constructors
        public AddEditProcessViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            BrowseCommand = new RelayCommand(OnBrowse);
        }
        #endregion

        #region properties/commands
        public Site CurrentSite
        {
            get { return _currentSite; }
            set { SetProperty(ref _currentSite, value); }
        }

        public ImageSource SelectedIcon
        {
            get
            {
                return _selectedIcon;
            }
            set { SetProperty(ref _selectedIcon, value); }
        }

        public SimpleEditableProcess Process
        {
            get { return _Process; }
            set { SetProperty(ref _Process, value); }
        }
        public RelayCommand BrowseCommand { get; private set; }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetProcess(Site site, Process process)
        {
            _currentSite = site;
            _contentHeader = "Add new process to site " + CurrentSite.Name;
            _editingProcess = process;
            if (Process != null) Process.ErrorsChanged -= RaiseCanExecuteChanged;
            Process = new SimpleEditableProcess();
            Process.ErrorsChanged += RaiseCanExecuteChanged;
            CopyProcess(process, Process);
        }

        private void CopyProcess(Process source, SimpleEditableProcess target)
        {
            target.Name = source.Name;
            target.ProcessIcon = source.ProcessIcon;
            target.SetProcessIconPath();
        }

        private bool CanSave()
        {
            return !Process.HasErrors;
        }

        private void OnSave()
        {
            UpdateProcess(Process, _editingProcess);
            string fileName = Path.GetFileName(_editingProcess.ProcessIcon);
            if (SelectedIcon != null)
            {
                //Copy image to server and update icon to name of image 
                _editingProcess.ProcessIcon = _repo.SaveImageOnServerAndLocal(SelectedIcon, _editingProcess.ProcessIcon, fileName);
                //reset selectedicon
                SelectedIcon = null;
            } else
            {
                //set the iconproperty to filename  not the whole path
                _editingProcess.ProcessIcon = fileName;
            }
            _repo.AddEditProcess(CurrentSite,_editingProcess, EditMode);
            Done(Constants.StrProcess + " updated");
        }

        private void UpdateProcess(SimpleEditableProcess source, Process target)
        {
            target.Name = source.Name.Trim();
            target.ProcessIcon = source.ProcessIconPath;
        }

        private void OnBrowse()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an icon";
            op.Filter = "\"Image files (*.png, *.ico)|*.png;*.ico\"";
            if (op.ShowDialog() == true)
            {
                SelectedIcon = new BitmapImage(new Uri(op.FileName));
                Process.ProcessIcon = op.FileName;
                Process.ProcessIconPath = op.FileName;
            }
        }
        #endregion
    }
}
