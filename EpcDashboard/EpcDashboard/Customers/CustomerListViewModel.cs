using Epc.Data;
using Epc.Data.Models;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using System;
using System.Linq;
using System.Windows;

namespace EpcDashboard.Customers
{
    public class CustomerListViewModel : ListViewModelBase, IPageViewModel
    {
        #region fields
        private IMainRepository _repo;
        private Customer _selectedCustomer;
        private AsyncObservableCollection<Customer> _customers;
        private AsyncObservableCollection<Customer> _allCustomers;
        #endregion

        #region constructors
        public CustomerListViewModel(IMainRepository repo)
        {
            _repo = repo;
            MenuName = "Home";
            _contentHeader = Constants.StrCustomer + "s";
            AddCommand = new RelayCommand(OnAddCustomer);
            EditCommand = new RelayCommand<Customer>(OnEditCustomer);
            DeleteCommand = new RelayCommand<Customer>(OnDelete);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            OpenSitesCommand = new RelayCommand<Customer>(OnOpenSite);
        }
        #endregion

        #region properties/commands
        public string MenuName
        {
            get
            {
                return _menuName;
            }
            set { SetProperty(ref _menuName, value); }
        }

        public AsyncObservableCollection<Customer> AllCustomers
        {
            get
            { return _allCustomers; }
            set { SetProperty(ref _allCustomers, value); }
        }

        public AsyncObservableCollection<Customer> Customers
        {
            get
            { return _customers; }
            set { SetProperty(ref _customers, value); }
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { SetProperty(ref _selectedCustomer, value); }
        }

        public string ContentHeader
        {
            get
            {
                return _contentHeader;
            }
        }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<Customer> EditCommand { get; private set; }
        public RelayCommand<Customer> DeleteCommand { get; private set; }
        public RelayCommand ClearSearchCommand { get; private set; }
        public RelayCommand<Customer> OpenSitesCommand { get; private set; }
        public event Action<Customer> AddCustomerRequest = delegate { };
        public event Action<Customer> EditCustomerRequest = delegate { };
        public event Action<Customer> OpenSitesRequest = delegate { };

        private void OnEditCustomer(Customer customer)
        {
            EditCustomerRequest(customer);
        }

        private void OnAddCustomer()
        {
            AddCustomerRequest(new Customer());
        }
        private void OnOpenSite(Customer cust)
        {
            OpenSitesRequest(cust);
        }

        private void OnDelete(Customer cust)
        {
            MessageBoxResult messageBoxResult = DeleteConfirmationDialogue(Constants.StrCustomer);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _repo.DeleteCustomer(cust);
            }
        }
        #endregion

        #region methods
        public void SetCustomers(AsyncObservableCollection<Customer> customers)
        {
            Customers = customers;
            AllCustomers = customers;
            _contentHeader = Constants.StrCustomer + "s";
        }

        public override void Filtering(string _SearchInput)
        {
            if (string.IsNullOrWhiteSpace(_SearchInput))
            {
                Customers = new AsyncObservableCollection<Customer>(_allCustomers);
                return;
            }
            else
            {
                Customers = new AsyncObservableCollection<Customer>(_allCustomers.Where(c => c.Name.ToLower().Contains(_SearchInput.ToLower())));
            }
        }
        #endregion
    }
}
