/*
 * Application: Empiri Configuration Manager
 * Version: 1.0 
 * Date of origin: 2016-12-06
 * Author: Petter Fogelqvist, petter.fogelqvist@gmail.com
*/

using EpcDashboard.Customers;
using EpcDashboard.Sites;
using EpcDashboard.Processes;
using System.Collections.Generic;
using System.Linq;
using Epc.Data.Models;
using EpcDashboard.Services;
using Microsoft.Practices.Unity;
using System.Windows;
using EpcDashboard.Settings;
using System.Windows.Input;
using System;
using System.IO;
using System.Windows.Threading;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Actions.EBMSs;
using EpcDashboard.ViewModelBases;
using EpcDashboard.Helpers;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Actions;
using EpcDashboard.Actions.OpenFolders;
using EpcDashboard.Actions.RDPs;
using EpcDashboard.Actions.X3s;
using EpcDashboard.Actions.VNCs;
using EpcDashboard.Actions.EXEs;
using System.ComponentModel;
using System.Threading.Tasks;
using EpcDashboard.Services.ActionServices;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.SitesProcesses;

namespace EpcDashboard
{
    /// <summary>
    /// MainWindowViewModel is the central window viewmodel holding references to all other page viewmodels
    /// Navigation between pages is handled here,
    /// Event-Routing to services is mostly done here
    /// </summary>
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel, INotifyPropertyChanged
    {
        //MainWindowModel act as the parent ViewModel
        #region fields
        private List<BindableBase> _pageViewModels;
        private ICommand _changePageCommand;
        private ISettingsRepository _settingsRepo;
        private Setting _userSettings;
        private IMainRepository _mainRepo;
        private bool _serverIsAlive;
        private EPC_Config_Data _data;
        private CustomerListViewModel _customerListViewModel;
        private AddEditCustomerViewModel _addEditCustomerViewModel;
        private SiteListViewModel _siteListViewModel;
        private AddEditSiteViewModel _addEditSiteViewModel;
        private ProcessListViewModel _processListViewModel;
        private AddEditProcessViewModel _addEditProcessViewModel;
        private ScriptFormViewModel _scriptFormViewModel;
        private SettingsViewModel _settingsViewModel;
        private AddEditEBMSViewModel _addEditEBMSActionViewModel;
        private SelectActionViewModel _selectActionViewModel;
        private AddEditX3ActionViewModel _addEditX3ActionViewModel;
        private AddEditOpenFolderViewModel _addEditOpenFolderViewModel;
        private AddEditRDPActionViewModel _addEditRDPActionViewModel;
        private AddEditVNCActionViewModel _addEditVNCActionViewModel;
        private AddEditExeActionViewModel _addEditExeActionViewModel;
        private CopySiteProcessViewModel _copyViewModel;

        private BindableBase _CurrentViewModel;
        private DispatcherTimer _timer;
        private bool _isBusy = false;
        private bool _showNavBar = true;

        #endregion

        #region Constructors / Initializing methods

        /// <summary>
        /// Main Startup initializer 
        /// </summary>
        public MainWindowViewModel(IMainRepository repo)
        {
            _settingsRepo = new SettingsRepository();
            _mainRepo = repo;
            SettingsCommand = new RelayCommand(OnOpenSettings);
            ExitCommand = new RelayCommand(OnExit);
            UserSettings = _settingsRepo.GetSettings();

            ConstructViewModelInstances();
            //Navigation setup:
            InitializeNavigation();
            EventMethods();

            //Ping Server
            PingServer();
            //Load data from xml
            this.InitLoad();
            //Cleanup EX3 subdirectories in local directory in background thread
            Cleanup(UserSettings.SourcePath);
            //FileEventWatcher setup:
            InitializeFileEventWatcher();
            StartTimer(); //Timer checking server connection
        }

        private void SetupEpc()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes all viewmodels 
        /// </summary>
        private void ConstructViewModelInstances()
        {
            //Unity Dependency injection used - it constructs the repository instance in all relevant viewmodel constructors, 
            _customerListViewModel = ContainerHelper.Container.Resolve<CustomerListViewModel>();
            _addEditCustomerViewModel = ContainerHelper.Container.Resolve<AddEditCustomerViewModel>();
            _siteListViewModel = ContainerHelper.Container.Resolve<SiteListViewModel>();
            _addEditSiteViewModel = ContainerHelper.Container.Resolve<AddEditSiteViewModel>();
            _processListViewModel = ContainerHelper.Container.Resolve<ProcessListViewModel>();
            _addEditProcessViewModel = ContainerHelper.Container.Resolve<AddEditProcessViewModel>();
            _settingsViewModel = new SettingsViewModel(_settingsRepo);
            _addEditEBMSActionViewModel = ContainerHelper.Container.Resolve<AddEditEBMSViewModel>();
            _selectActionViewModel = ContainerHelper.Container.Resolve<SelectActionViewModel>();
            _addEditX3ActionViewModel = ContainerHelper.Container.Resolve<AddEditX3ActionViewModel>();
            _addEditOpenFolderViewModel = ContainerHelper.Container.Resolve<AddEditOpenFolderViewModel>();
            _scriptFormViewModel = ContainerHelper.Container.Resolve<ScriptFormViewModel>();
            _addEditRDPActionViewModel = ContainerHelper.Container.Resolve<AddEditRDPActionViewModel>();
            _addEditVNCActionViewModel = ContainerHelper.Container.Resolve<AddEditVNCActionViewModel>();
            _addEditExeActionViewModel = ContainerHelper.Container.Resolve<AddEditExeActionViewModel>();
            _copyViewModel = ContainerHelper.Container.Resolve<CopySiteProcessViewModel>();
        }

        /// <summary>
        /// Initialize pageviewmodels = pages that can be navigated to through navbar-links
        /// </summary>
        private void InitializeNavigation()
        {
            // Add linkable viewmodels here
            _customerListViewModel.ShowAsLink = true;
            _customerListViewModel.IsActive = true; //start page is set to active
            PageViewModels.Add(_customerListViewModel);
            _siteListViewModel.ShowAsLink = false;
            PageViewModels.Add(_siteListViewModel);
            _processListViewModel.ShowAsLink = false;
            PageViewModels.Add(_processListViewModel);

            // Set starting page
            CurrentViewModel = PageViewModels[0];
        }

        /// <summary>
        /// Events in viewmodels routed to mainviewmodel 
        /// </summary>
        private void EventMethods()
        {
            //left side = viewmodel events, right side : methods that changes currentviewmodel
            _customerListViewModel.AddCustomerRequest += NavToAddCustomer;
            _customerListViewModel.EditCustomerRequest += NavToEditCustomer;
            _customerListViewModel.OpenSitesRequest += NavToSiteList;

            _siteListViewModel.AddSiteRequest += NavToAddSite;
            _siteListViewModel.EditSiteRequest += NavToEditSite;
            _siteListViewModel.OpenProcessesRequest += NavToProcessList;
            _siteListViewModel.AddEBMSRequest += NavToAddEBMS;
            _siteListViewModel.EditEBMSRequest += NavToEditEBMS;
            _siteListViewModel.OpenEBMSRequest += OpenEBMS;
            _siteListViewModel.CopyRequest += NavToCopy;

            _processListViewModel.AddProcessRequest += NavToAddProcess;
            _processListViewModel.EditProcessRequest += NavToEditProcess;
            _processListViewModel.OpenScriptBoxRequest += NavToScriptView;
            _processListViewModel.CopyRequest += NavToCopy;

            //Actions
            _processListViewModel.AddActionRequest += NavToSelectAction;
            _processListViewModel.Actions.EditActionRequest += NavToEditAction;
            //EBMS
            _processListViewModel.AddEBMSRequest += NavToAddEBMS;
            _processListViewModel.EditEBMSRequest += NavToEditEBMS;
            _processListViewModel.Actions.OpenEBMSRequest += OpenEBMS;
            _addEditEBMSActionViewModel.Done += EBMSSaved;
            _addEditEBMSActionViewModel.Cancel += NavToActiveViewModel;
            //X3 Start
            _addEditX3ActionViewModel.Cancel += NavToActiveViewModel;
            _addEditX3ActionViewModel.Done += NavToActiveWithNotificationMessage;
            //Open folders
            _processListViewModel.Actions.OpenFolderRequest += MapFolder;
            _addEditOpenFolderViewModel.Cancel += NavToActiveViewModel;
            _addEditOpenFolderViewModel.Done += NavToActiveWithNotificationMessage;
            //RDP
            _processListViewModel.Actions.RunRDPRequest += RunRDP;
            _addEditRDPActionViewModel.Cancel += NavToActiveViewModel;
            _addEditRDPActionViewModel.Done += NavToActiveWithNotificationMessage;
            //VNC
            _processListViewModel.Actions.RunVNCRequest += RunVNC;
            _addEditVNCActionViewModel.Cancel += NavToActiveViewModel;
            _addEditVNCActionViewModel.Done += NavToActiveWithNotificationMessage;
            //Exe
            _processListViewModel.Actions.RunExeRequest += RunExe;
            _addEditExeActionViewModel.Cancel += NavToActiveViewModel;
            _addEditExeActionViewModel.Done += NavToActiveWithNotificationMessage;

            _addEditCustomerViewModel.Done += CustomerSaved;
            _addEditCustomerViewModel.Cancel += NavToCustomerList;

            _addEditSiteViewModel.Done += NavToActiveWithNotificationMessage;
            _addEditSiteViewModel.Cancel += NavToActiveViewModel;

            _addEditProcessViewModel.Done += NavToActiveWithNotificationMessage;
            _addEditProcessViewModel.Cancel += NavToActiveViewModel;

            _settingsViewModel.Done += SettingsSaved;
            _settingsViewModel.DoneCancel += NavFromSettingsToPreviousViewModel;

            _selectActionViewModel.ActionSelected += NavToAddAction;
            _selectActionViewModel.Cancel += NavToActiveViewModel;

            _scriptFormViewModel.Done += NavToActiveWithNotificationMessage;
            _scriptFormViewModel.Cancel += NavToActiveViewModel;

            _copyViewModel.Done += NavToActiveWithNotificationMessage;
            _copyViewModel.Cancel += NavToActiveViewModel;
        }

        /// <summary>
        /// Setup FileEventWatcher in another thread - listen for changes in centralized epc xml file
        /// </summary>
        private void InitializeFileEventWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            if (ServerIsAlive)
            {
                SetupWatcher(watcher);
            }

            var switchThreadForFsEvent = (Func<FileSystemEventHandler, FileSystemEventHandler>)(
            (FileSystemEventHandler handler) =>
                (object obj, FileSystemEventArgs e) =>
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                        handler(obj, e))));

            // Add event handlers.
            watcher.Changed += switchThreadForFsEvent(OnChangedCentralFile);
            if (ServerIsAlive)
            {
                StartWatcher(watcher);
            }

            _mainRepo.Watcher = watcher;
        }

        #endregion

        #region Properties

        public EPC_Config_Data Data
        {
            get
            {
                return _data;
            }
            set { SetProperty(ref _data, value); }
        }

        public bool ServerIsAlive
        {
            get
            {
                return _serverIsAlive;
            }
            set
            {
                SetProperty(ref _serverIsAlive, value);
            }
        }

        public Setting UserSettings
        {
            get
            {
                return _userSettings;
            }
            set
            {
                SetProperty(ref _userSettings, value);
            }
        }

        public BindableBase CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set
            {
                NotificationMessage = null;
                SetProperty(ref _CurrentViewModel, value);
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<BindableBase>(
                        p => ChangeViewModel((BindableBase)p),
                        p => p is BindableBase);
                }

                return _changePageCommand;
            }
        }

        public List<BindableBase> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<BindableBase>();

                return _pageViewModels;
            }
        }

        public bool ShowNavBar
        {
            get
            {
                return _showNavBar;
            }
            set { SetProperty(ref _showNavBar, value); }
        }

        public DispatcherTimer Timer
        {
            get
            {
                return _timer;
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

        #endregion

        #region Commands

        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// Called once at startup, fetch and unserialize xml data and provide start page with list of customers
        /// </summary>
        public void InitLoad()
        {
            Data = GetData();
            if (Data != null)
            {
                _customerListViewModel.SetCustomers(Data.Customers);
            }
            else
            {
                NotificationMessage = "Error loading data";
                //no data loaded, probably because local file problem
            }
        }

        /// <summary>
        /// Fetch and unserialize xml data and populate a EPC_Config_Data object with it 
        /// </summary>
        public EPC_Config_Data GetData()
        {

            Data = (EPC_Config_Data)_mainRepo.GetData();
            _mainRepo.Data = Data;

            return Data;
        }

        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        private void OnChangedCentralFile(object source, FileSystemEventArgs e)
        {
            UpdateCollection();
        }
        /// <summary>
        /// Starts the FileEventWatcher
        /// </summary>
        private void StartWatcher(FileSystemWatcher watcher)
        {
            if (Directory.Exists(@Properties.Settings.Default.CentralSourcePath))
            {
                try
                {
                    if (watcher.EnableRaisingEvents != true && !String.IsNullOrEmpty(watcher.Path))
                    {
                        watcher.EnableRaisingEvents = true;
                    }
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine("Catching argumentexception when enabling filesystemeventwatcher, msg: {0}", ae.Message);
                }
            }
        }
        /// <summary>
        /// Setup the FileEventWatcher, called at startup, and after server been offline
        /// </summary>
        private bool SetupWatcher(FileSystemWatcher watcher)
        {
            bool isSuccess = true;
            if (String.IsNullOrEmpty(watcher.Path))
            {
                if (Directory.Exists(@Properties.Settings.Default.CentralSourcePath))
                {
                    //if Path havent been set, setup watcher for the first time
                    try
                    {
                        watcher.Path = @Properties.Settings.Default.CentralSourcePath;
                        watcher.IncludeSubdirectories = false;
                        watcher.NotifyFilter = NotifyFilters.LastWrite;
                        watcher.Filter = Constants.FileName;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caught Exception creating watcher [{0}]", e.Message);
                        isSuccess = false;
                    }
                }
                else
                {
                    isSuccess = false;
                }
            }


            return isSuccess;
        }
        //private void StopFileEventWatcher()
        //{
        //    try
        //    {
        //        _mainRepo.Watcher.EnableRaisingEvents = false;
        //    }
        //    catch (Exception e)
        //    {

        //        Console.WriteLine("Error accessing source file..{0}", e.Message);
        //    }
        //}

        /// <summary>
        /// Called once at startup, starts and setup timer
        /// </summary>
        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(5000);
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Start();
        }
        /// <summary>
        /// Timer repeatedly ping central server, if server just has recovered after being offline, setup fileeventwatcher (restart) 
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            bool previousStatus = ServerIsAlive;
            PingServer();
            //Server just went online
            if (ServerIsAlive && !previousStatus)
            {
                TurnOnUIProgressElement();
                MergeOperation();
                TurnOffUIProgressElement();
            }
            //Server continous to be online 
            else if (ServerIsAlive && previousStatus)
            {
                //Check that fileEventwatcher is enabled
                if (!_mainRepo.Watcher.EnableRaisingEvents)
                {
                    //Try starting it again
                    SetupWatcher(this._mainRepo.Watcher);
                    StartWatcher(this._mainRepo.Watcher);
                }
            }
            else
            {
                //Server just went offline
            }
        }

        /// <summary>
        /// Setup Watcher and Call Merge service which updates Data
        /// </summary>
        private async void MergeOperation()
        {
            bool hasAccess = false;
            while (!hasAccess)
            {
                hasAccess = SetupWatcher(_mainRepo.Watcher);
            }

            MergeLocalSourceService s = await Task.Run(() => _mainRepo.MergeData(Data, new MergeLocalSourceService()));
            Data = _mainRepo.Data;

            if (s.LocalIsChanged)
            {
                NavToActiveWithNotificationMessage("Merge performed successfully");
            }

            StartWatcher(this._mainRepo.Watcher);
        }

        /// <summary>
        /// Called when file watcher identified a change in central config file
        /// </summary>
        private void UpdateCollection()
        {
            //Updates and saves data
            _mainRepo.UpdateData(Data);
            Data = _mainRepo.Data;
        }

        private void SetLinkablePageViewModel(ViewModelBase vm, bool linkable)
        {
            vm.ShowAsLink = linkable;
        }

        /// <summary>
        /// Pings centralized server
        /// </summary>
        private void PingServer()
        {
            ServerIsAlive = _mainRepo.PingHost();
        }
        /// <summary>
        /// Open EBMS and navigates to last active page
        /// </summary>
        private void OpenEBMS(EBMS ebmsInfo)
        {
            if (ebmsInfo.Homepage != null)
            {
                if (EBMSHelper.IsValidUrl(ebmsInfo.Homepage))
                {
                    EBMSHelper.OpenUriInBrowser(ebmsInfo.Homepage);
                }
                else
                {
                    NavToActiveWithNotificationMessage("Check EBMS homepage URI");
                }
            }
            else
            {
                NavToActiveWithNotificationMessage("Please add/edit your EBMS action configuration");
            }

        }

        /// <summary>
        /// Try to map and open a shared network folder in another thread 
        /// </summary>
        private async void MapFolder(OpenFolder serverInfo)
        {
            TurnOnUIProgressElement();

            NetworkFolderService nfs = new NetworkFolderService();
            await Task.Run(() => nfs.MapFolderAction(serverInfo));

            TurnOffUIProgressElement();
        }

        /// <summary>
        /// Try to start a remote desktop connection
        /// </summary>
        private async void RunRDP(RDP rdpInfo)
        {
            TurnOnUIProgressElement();
            RemoteDesktopService rdps = new RemoteDesktopService();
            await Task.Run(() => rdps.RunRemoteDesktop(rdpInfo));

            TurnOffUIProgressElement();
        }

        /// <summary>
        /// Try to start a virtual network connection
        /// </summary>
        private async void RunVNC(VNC vncInfo)
        {
            TurnOnUIProgressElement();
            VNCService service = new VNCService();
            await Task.Run(() => service.RunVNC(vncInfo));

            TurnOffUIProgressElement();
        }

        /// <summary>
        /// Try to start a executable file
        /// </summary>
        private async void RunExe(Exe exeInfo)
        {
            TurnOnUIProgressElement();
            StartProcessService service = new StartProcessService();
            await Task.Run(() => service.RunExe(exeInfo));

            TurnOffUIProgressElement();
        }

        /// <summary> Delete obsolete X3 folders and files 
        /// <para>string path -> path to local directory</para>
        /// <para></para>
        /// </summary>
        private void Cleanup(string path)
        {
            CleanupDirectoryService cds = new CleanupDirectoryService();
            cds.CleanupX3(path, _mainRepo.Data);
        }

        private void TurnOffUIProgressElement()
        {
            //UI Busy animation deactivator
            IsBusy = false;
        }

        private void TurnOnUIProgressElement()
        {
            //UI Busy animation activator
            IsBusy = true;
        }

        #region Navigation Methods

        /// <summary>
        /// Update navbar (pageviewmodels)
        /// </summary>
        public void ChangeViewModel(BindableBase viewModel)
        {
            if (PageViewModels.Contains(viewModel))
            {
                HideChildNavigationLinks(viewModel);
                CurrentViewModel = PageViewModels
                    .FirstOrDefault(vm => vm == viewModel);
                SetActivePageViewModel((ListViewModelBase)CurrentViewModel);
            }
        }

        /// <summary>
        /// Hides child navigation links if we are at a parent page
        /// </summary>
        private void HideChildNavigationLinks(BindableBase viewModel)
        {
            int index = PageViewModels.IndexOf(viewModel);
            int count = PageViewModels.Count();
            for (int i = index + 1; i < count; i++)
            {
                ListViewModelBase vm = (ListViewModelBase)PageViewModels.ElementAt(i);
                //Childlinks visibility set to false
                vm.ShowAsLink = false;
            }
        }
        /// <summary>
        /// Set the active flag indicating which pageviewmodel we most previously visited 
        /// </summary>
        private void SetActivePageViewModel(ListViewModelBase viewModel)
        {
            //Remove previous active vm:
            foreach (ListViewModelBase pvm in PageViewModels)
            {
                if (pvm.IsActive)
                {
                    pvm.IsActive = false;
                    break;
                }
            }
            //Set new active vm
            viewModel.IsActive = true;
        }

        /// <summary>
        /// Navigate to the most recently visited navbar pageviewmodel
        /// </summary>
        private void NavToActiveViewModel()
        {
            //Navigate to Active PageViewModel
            foreach (ListViewModelBase vm in PageViewModels)
            {
                if (vm.IsActive)
                {
                    CurrentViewModel = vm;
                    break;
                }
            }
        }

        /// <summary>
        /// Navigate to edit settings page
        /// </summary>
        private void OnOpenSettings()
        {
            this.ShowNavBar = false; //dont display Navigation bar/buttons
            _settingsViewModel.SetSetting(UserSettings);
            CurrentViewModel = _settingsViewModel;
            //PageViewModels.Add(_settingsViewModel);
        }

        private void NavToCustomerList()
        {
            ListViewModelBase vm = _customerListViewModel;
            SetActivePageViewModel(vm);
            CurrentViewModel = vm;
        }

        private void NavToSiteList(Customer cust)
        {
            SetActivePageViewModel(_siteListViewModel);
            _siteListViewModel.SetSites(cust);
            _siteListViewModel.ShowAsLink = true;
            //PageViewModels.Add(_siteListViewModel);
            CurrentViewModel = _siteListViewModel;
        }

        private void NavToProcessList(Site site)
        {
            SetActivePageViewModel(_processListViewModel);
            _processListViewModel.SetProcesses(site);
            _processListViewModel.ShowAsLink = true;
            //PageViewModels.Add(_processListViewModel);
            CurrentViewModel = _processListViewModel;
        }

        private void NavFromSettingsToPreviousViewModel()
        {
            this.ShowNavBar = true;
            NavToActiveViewModel();
        }

        private void SettingsSaved()
        {
            NavFromSettingsToPreviousViewModel();
            NotificationMessage = "Settings updated";
        }

        private void CustomerSaved()
        {
            NavToCustomerList();
            NotificationMessage = Constants.StrCustomer + "s updated";
        }

        /// <summary>
        /// Navigate to most recently visited pageviewmodel and display a notificationmessage
        /// </summary>
        private void NavToActiveWithNotificationMessage(string message)
        {
            NavToActiveViewModel();
            NotificationMessage = message;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EBMSSaved(string context)
        {
            //Depending on context - set the HasEBMS flag for Start EBMS button visibility
            switch (context)
            {
                case (Constants.StrCustomer):
                    _siteListViewModel.HasEBMS = true;
                    break;
                case (Constants.StrSite):
                    _processListViewModel.HasEBMS = true;
                    break;
                default:
                    break;
            }
            NavToActiveViewModel();
            NotificationMessage = "EBMS saved";
        }

        private void NavToEditCustomer(Customer cust)
        {
            _addEditCustomerViewModel.EditMode = true;
            _addEditCustomerViewModel.SetCustomer(cust);
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavToAddCustomer(Customer cust)
        {
            _addEditCustomerViewModel.EditMode = false;
            _addEditCustomerViewModel.SetCustomer(cust);
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavToEditSite(Site site)
        {
            _addEditSiteViewModel.EditMode = true;
            _addEditSiteViewModel.SetSite(_siteListViewModel.Customer, site);
            CurrentViewModel = _addEditSiteViewModel;
        }

        private void NavToAddSite(Site site)
        {
            _addEditSiteViewModel.EditMode = false;
            _addEditSiteViewModel.SetSite(_siteListViewModel.Customer, site);
            CurrentViewModel = _addEditSiteViewModel;
        }

        private void NavToEditProcess(Process process)
        {
            _addEditProcessViewModel.EditMode = true;
            _addEditProcessViewModel.SetProcess(_processListViewModel.Site, process);
            CurrentViewModel = _addEditProcessViewModel;
        }

        private void NavToAddProcess(Process process)
        {
            _addEditProcessViewModel.EditMode = false;
            _addEditProcessViewModel.SetProcess(_processListViewModel.Site, process);
            CurrentViewModel = _addEditProcessViewModel;
        }

        private void NavToAddEBMS(string context)
        {
            _addEditEBMSActionViewModel.EditMode = false;
            NavToAddEditEBMS(context);
        }
        private void NavToEditEBMS(string context)
        {
            _addEditEBMSActionViewModel.EditMode = true;
            NavToAddEditEBMS(context);
        }

        private void NavToAddEditEBMS(string context)
        {
            switch (context)
            {
                case Constants.StrCustomer:
                    _addEditEBMSActionViewModel.SetEBMS(_siteListViewModel.Customer, _siteListViewModel.Customer.EBMS, context);
                    break;
                case Constants.StrSite:
                    _addEditEBMSActionViewModel.SetEBMS(_processListViewModel.Site, _processListViewModel.Site.EBMS, context);
                    break;
                default:
                    break;
            }
            CurrentViewModel = _addEditEBMSActionViewModel;
        }

        private void NavToScriptView(Script scriptmodel)
        {
            _scriptFormViewModel.SetScriptModel(scriptmodel);
            CurrentViewModel = _scriptFormViewModel;
        }

        private void NavToSelectAction()
        {
            _selectActionViewModel.Site = _processListViewModel.Site;
            CurrentViewModel = _selectActionViewModel;
        }

        private void NavToAddAction(string selectedAction)
        {
            Site site = _selectActionViewModel.Site;
            if (selectedAction == Constants.TypeX3Txt)
            {
                _addEditX3ActionViewModel.SetAction(site, new EX3());
                _addEditX3ActionViewModel.EditMode = false;
                CurrentViewModel = _addEditX3ActionViewModel;
            }
            else if (selectedAction == Constants.TypeOpenFolderTxt)
            {
                _addEditOpenFolderViewModel.SetAction(site, new OpenFolder());
                _addEditOpenFolderViewModel.EditMode = false;
                CurrentViewModel = _addEditOpenFolderViewModel;
            }
            else if (selectedAction == Constants.TypeRDPTxt)
            {
                _addEditRDPActionViewModel.EditMode = false;
                _addEditRDPActionViewModel.SetAction(site, new RDP());
                CurrentViewModel = _addEditRDPActionViewModel;
            }
            else if (selectedAction == Constants.TypeVNCTxt)
            {
                _addEditVNCActionViewModel.EditMode = false;
                _addEditVNCActionViewModel.SetAction(site, new VNC());
                CurrentViewModel = _addEditVNCActionViewModel;
            }
            else if (selectedAction == Constants.TypeEXETxt)
            {
                _addEditExeActionViewModel.EditMode = false;
                _addEditExeActionViewModel.SetAction(site, new Exe());
                CurrentViewModel = _addEditExeActionViewModel;
            }
            else if (selectedAction == Constants.TypeEBMSTxt)
            {
                if (_processListViewModel.HasEBMS)
                {
                    NavToEditEBMS(Constants.StrSite);
                }
                else
                {
                    NavToAddEBMS(Constants.StrSite);
                }
            }
        }

        private void NavToEditAction(BaseAction action)
        {
            Site site = _processListViewModel.Site;

            if (action.ActionType == Constants.TypeX3Txt)
            {
                _addEditX3ActionViewModel.EditMode = true;
                _addEditX3ActionViewModel.SetAction(site, (EX3)action);
                CurrentViewModel = _addEditX3ActionViewModel;
            }
            else if (action.ActionType == Constants.TypeOpenFolderTxt)
            {
                _addEditOpenFolderViewModel.EditMode = true;
                _addEditOpenFolderViewModel.SetAction(site, (OpenFolder)action);
                CurrentViewModel = _addEditOpenFolderViewModel;
            }
            else if (action.ActionType == Constants.TypeRDPTxt)
            {
                _addEditRDPActionViewModel.EditMode = true;
                _addEditRDPActionViewModel.SetAction(site, (RDP)action);
                CurrentViewModel = _addEditRDPActionViewModel;
            }
            else if (action.ActionType == Constants.TypeVNCTxt)
            {
                _addEditVNCActionViewModel.EditMode = true;
                _addEditVNCActionViewModel.SetAction(site, (VNC)action);
                CurrentViewModel = _addEditVNCActionViewModel;
            }
            else if (action.ActionType == Constants.TypeEXETxt)
            {
                _addEditExeActionViewModel.EditMode = true;
                _addEditExeActionViewModel.SetAction(site, (Exe)action);
                CurrentViewModel = _addEditExeActionViewModel;
            }
        }

        private void NavToCopy(object entity)
        {
            if (entity is Site)
            {
                _copyViewModel.IsSite = true;
                _copyViewModel.SetContext(entity, _siteListViewModel.Customer);
            }
            else if (entity is Process)
            {
                _copyViewModel.IsSite = false;
                _copyViewModel.SetContext(entity, _processListViewModel.Site);
            }
            CurrentViewModel = _copyViewModel;
        }

        #endregion

        #endregion
    }
}
