using Epc.Data;
using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.SitesProcesses;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace EpcDashboard.Sites
{
    public class SiteListViewModel : SiteProcessListsViewModelBase, IPageViewModel
    {
        #region fields
        private IMainRepository _repo;
        private AsyncObservableCollection<Site> _sites;
        private Customer _customer;
        private AsyncObservableCollection<Site> _allSites;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private EBMS _ebmsAction;
        #endregion

        #region constructors
        public SiteListViewModel(IMainRepository repo)
        {
            _repo = repo;
            _contentHeader = Constants.StrSite + "s";
            OpenProcessesCommand = new RelayCommand<Site>(OnOpenProcesses);
            AddCommand = new RelayCommand(OnAddSite);
            EditCommand = new RelayCommand<Site>(OnEditSite);
            DeleteCommand = new RelayCommand<Site>(OnDelete);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            PingSitesCommand = new RelayCommand(OnPingSites);
            //EBMS
            AddEBMSCommand = new RelayCommand(OnAddEBMS);
            EditEBMSCommand = new RelayCommand(OnEditEBMS);
            OpenEBMSCommand = new RelayCommand(OnOpenEBMS);
            DeleteEBMSCommand = new RelayCommand(OnDeleteEBMS);

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

        }
        #endregion

        #region properties/commands
        public AsyncObservableCollection<Site> Sites
        {
            get
            { return _sites; }
            set { SetProperty(ref _sites, value); }
        }
        public AsyncObservableCollection<Site> AllSites
        {
            get
            { return _allSites; }
            set { SetProperty(ref _allSites, value); }
        }

        public RelayCommand<Site> OpenProcessesCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<Site> EditCommand { get; private set; }
        public RelayCommand ClearSearchCommand { get; private set; }
        public RelayCommand<Site> DeleteCommand { get; private set; }
        public RelayCommand PingSitesCommand { get; private set; }

        public event Action<Site> AddSiteRequest = delegate { };
        public event Action<Site> EditSiteRequest = delegate { };
        public event Action<Site> OpenProcessesRequest = delegate { };
        public event Action<string> AddEBMSRequest = delegate { };
        public event Action<string> EditEBMSRequest = delegate { };
        public event Action<EBMS> OpenEBMSRequest = delegate { };

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            PingSites(_allSites);
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
        }

        public string MenuName
        {
            get
            {
                return _menuName;
            }
            set { SetProperty(ref _menuName, value); }
        }
        public Customer Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
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
        #endregion

        #region methods
        public void SetSites(Customer customer)
        {
            Sites = customer.Sites;
            AllSites = customer.Sites;
            Customer = customer;
            HasEBMS = CompareEBMS(Customer.EBMS);
            EBMSAction = Customer.EBMS;
            MenuName = customer.Name;
        }

        private void OnOpenProcesses(Site site)
        {
            site.CustomerName = Customer.Name;
            OpenProcessesRequest(site);
        }
        private void OnEditSite(Site site)
        {
            EditSiteRequest(site);
        }

        private void OnAddSite()
        {
            AddSiteRequest(new Site());
        }
        private void OnAddEBMS()
        {
            //set context to customer
            AddEBMSRequest(Constants.StrCustomer);
        }
        private void OnEditEBMS()
        {
            //set context to customer
            EditEBMSRequest(Constants.StrCustomer);
        }
        private void OnDeleteEBMS()
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue("EBMS information");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteEBMS(Customer);
                HasEBMS = false;
            }
        }
        private void OnOpenEBMS()
        {
            if (HasEBMS)
            {
                OpenEBMSRequest(Customer.EBMS);
            }
        }

        private void OnPingSites()
        {
            worker.RunWorkerAsync();
        }

        private void OnDelete(Site site)
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue(Constants.StrSite);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteSite(Customer, site);
            }
        }

        public override void Filtering(string _SearchInput)
        {
            if (string.IsNullOrWhiteSpace(_SearchInput))
            {
                Sites = new AsyncObservableCollection<Site>(_allSites);
                return;
            }
            else
            {
                Sites = new AsyncObservableCollection<Site>(_allSites.Where(s => s.Name.ToLower().Contains(_SearchInput.ToLower())));
            }
        }
        public void PingSites(AsyncObservableCollection<Site> collection)
        {
            foreach(Site site in collection)
            {
                bool pingResult = false;
                if (!String.IsNullOrEmpty(site.IpAdress))
                {
                    try
                    {
                        pingResult = _repo.PingIp(site.IpAdress, 100);
                    }
                    catch (Exception)
                    {
                        pingResult = false;
                    }
                }
                site.IsOnline = pingResult;
            }
        }
        #endregion
    }
}
