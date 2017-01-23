using Epc.Data;
using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Actions;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.ActionServices;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.SitesProcesses;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace EpcDashboard.Processes
{
    public class ProcessListViewModel : SiteProcessListsViewModelBase, IPageViewModel
    {
        #region fields
        private IMainRepository _repo;
        private ActionListViewModel _actions;
        private AsyncObservableCollection<Process> _processes;
        private Site _site;
        private AsyncObservableCollection<Process> _allProcesses;
        private EBMS _ebmsAction;
        private BackgroundWorker _startX3Worker = new BackgroundWorker();
        private bool _isBusy = false;
        private int _progressCount = 0;
        private string _progressMessage;
        #endregion

        #region constructors
        public ProcessListViewModel(IMainRepository repo)
        {
            _repo = repo;
            _actions = new ActionListViewModel();
            OpenScriptBoxCommand = new RelayCommand<Process>(OnOpenScriptBox);
            AddCommand = new RelayCommand(OnAddProcess);
            EditProcessCommand = new RelayCommand<Process>(OnEditProcess);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            DeleteCommand = new RelayCommand<Process>(OnDelete);
            AddActionCommand = new RelayCommand(OnAddAction);
            //EBMS
            AddEBMSCommand = new RelayCommand(OnAddEBMS);
            EditEBMSCommand = new RelayCommand(OnEditEBMS);
            OpenEBMSCommand = new RelayCommand(OnOpenEBMS);
            DeleteEBMSCommand = new RelayCommand(OnDeleteEBMS);
            DeleteActionCommand = new RelayCommand<BaseAction>(OnDeleteAction);

            _startX3Worker.DoWork += _startX3Worker_DoWork;
            _startX3Worker.RunWorkerCompleted += _startX3Worker_RunWorkerCompleted;
            _actions.StartEX3Request += StartEX3;
        }
        #endregion

        #region properties/commands
        public ActionListViewModel Actions
        {
            get
            {
                return _actions;
            }
        }
        public AsyncObservableCollection<Process> Processes
        {
            get
            { return _processes; }
            set { SetProperty(ref _processes, value); }
        }
        public AsyncObservableCollection<Process> AllProcesses
        {
            get
            { return _allProcesses; }
            set { SetProperty(ref _allProcesses, value); }
        }

        public string MenuName
        {
            get
            {
                return _menuName;
            }
            set { SetProperty(ref _menuName, value); }
        }
        public Site Site
        {
            get { return _site; }
            set { SetProperty(ref _site, value); }
        }

        public EBMS EBMSAction
        {
            get
            { return _ebmsAction; }
            set { SetProperty(ref _ebmsAction, value); }
        }

        public string ContentHeader
        {
            get
            {
                return _contentHeader;
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set { SetProperty(ref _isBusy, value); }
        }

        public int ProgressCount
        {
            get
            {
                return _progressCount;
            }
            set { SetProperty(ref _progressCount, value); }
        }

        public string ProgressMessage
        {
            get
            {
                return _progressMessage;
            }
            set { SetProperty(ref _progressMessage, value); }
        }

        public RelayCommand<Process> OpenScriptBoxCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<Process> EditProcessCommand { get; private set; }
        public RelayCommand ClearSearchCommand { get; private set; }
        public RelayCommand<Process> DeleteCommand { get; private set; }

        public RelayCommand AddActionCommand { get; private set; }
        public RelayCommand<BaseAction> DeleteActionCommand { get; private set; }

        public event Action<Process> AddProcessRequest = delegate { };
        public event Action<Process> EditProcessRequest = delegate { };
        public event Action<Script> OpenScriptBoxRequest = delegate { };
        public event Action AddActionRequest = delegate { };
        public event Action<string> AddEBMSRequest = delegate { };
        public event Action<string> EditEBMSRequest = delegate { };

        #endregion

        #region methods
        public void SetProcesses(Site site)
        {
            Processes = site.Processes;
            AllProcesses = site.Processes;
            Site = site;
            HasEBMS = CompareEBMS(Site.EBMS);
            Actions.X3Actions = site.X3Actions;
            Actions.OpenFolderActions = site.FolderActions;
            Actions.RDPActions = site.RDPActions;
            Actions.VNCActions = site.VNCActions;
            Actions.ExeActions = site.ExeActions;
            EBMSAction = site.EBMS;

            MenuName = site.Name;
            _contentHeader = Site.Name + " Processes";
        }

        private void OnAddEBMS()
        {
            //set context to site
            AddEBMSRequest(Constants.StrSite);
        }
        private void OnEditEBMS()
        {
            //set context to customer
            EditEBMSRequest(Constants.StrSite);
        }

        private void OnOpenScriptBox(Process process)
        {
            Script script = new Script { DbInfo = Site.X3Actions.FirstOrDefault(), Process = process };
            OpenScriptBoxRequest(script);
        }
        private void OnEditProcess(Process process)
        {
            EditProcessRequest(process);
        }

        private void OnAddProcess()
        {
            AddProcessRequest(new Process());
        }

        private void OnDelete(Process process)
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue(Constants.StrProcess);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteProcess(Site, process);
            }
        }

        private void OnDeleteEBMS()
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue("Delete EBMS Information");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteEBMS(Site);
                HasEBMS = false;
            }
        }
        private void OnOpenEBMS()
        {
            if (HasEBMS)
            {
                _actions.OnOpenEBMS(Site.EBMS);
            }
        }

        private void OnDeleteAction(BaseAction action)
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue("Action " + action.ActionName);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteAction(Site, action);
            }
        }

        private void OnAddAction()
        {
            AddActionRequest();
        }

        public override void Filtering(string _SearchInput)
        {
            if (string.IsNullOrWhiteSpace(_SearchInput))
            {
                Processes = new AsyncObservableCollection<Process>(_allProcesses);
                return;
            }
            else
            {
                Processes = new AsyncObservableCollection<Process>(_allProcesses.Where(p => p.Name.ToLower().Contains(_SearchInput.ToLower())));
            }
        }

        private void _startX3Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            EX3 x3Info = (EX3)e.Argument;
            EX3Service service = new EX3Service();

            service.Initialization(Site, x3Info, this);
        }

        private void _startX3Worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
            //reset progress parameters
            ProgressCount = 0;
            ProgressMessage = "";
        }

        /// <summary>
        /// Start X3 Action called from ActionListViewModel, routing to EX3Service methods
        /// </summary>
        protected void StartEX3(EX3 x3Info)
        {
            IsBusy = true;
            _startX3Worker.RunWorkerAsync(x3Info);
        }
        #endregion
    }
}
