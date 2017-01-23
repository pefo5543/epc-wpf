using System;
using System.ComponentModel;
using Epc.Data.Models;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.ViewModelBases;
using System.IO;
using EpcDashboard.Services.Interfaces;

namespace EpcDashboard.Customers
{
    public class AddEditCustomerViewModel : AddEditViewModelBase
    {
        #region fields
        private string _name;
        private Customer _editingCustomer = null;
        private IMainRepository _repo;
        private ImageSource _selectedIcon = null;

        #endregion

        #region constructors
        public AddEditCustomerViewModel(IMainRepository repo)
        {
            _repo = repo;
            _contentHeader = "Add new customer";
            SaveCommand = new RelayCommand(OnSave, CanSave);
            BrowseCommand = new RelayCommand(OnBrowse);
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

        public ImageSource SelectedIcon
        {
            get
            {
                return _selectedIcon;
            }
            set { SetProperty(ref _selectedIcon, value); }
        }

        private SimpleEditableCustomer _Customer;
        public SimpleEditableCustomer Customer
        {
            get { return _Customer; }
            set { SetProperty(ref _Customer, value); }
        }

        public RelayCommand BrowseCommand { get; private set; }

        public event Action Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetCustomer(Customer cust)
        {
            _editingCustomer = cust;
            if (Customer != null) Customer.ErrorsChanged -= RaiseCanExecuteChanged;
            Customer = new SimpleEditableCustomer();
            Customer.ErrorsChanged += RaiseCanExecuteChanged;
            CopyCustomer(cust, Customer);
        }

        private void CopyCustomer(Customer source, SimpleEditableCustomer target)
        {
            target.Name = source.Name;
            target.CustomerIcon = source.CustomerIcon;
            target.SetCustomerIconPath();
            target.Color = source.Color;
        }

        private bool CanSave()
        {
            return !Customer.HasErrors;
        }

        private void OnSave()
        {
            UpdateCustomer(Customer, _editingCustomer);
            string fileName = Path.GetFileName(_editingCustomer.CustomerIcon);
            if(SelectedIcon != null)
            {
                //Copy image to server and return icon filename - we dont want the whole path 
                _editingCustomer.CustomerIcon = _repo.SaveImageOnServerAndLocal(SelectedIcon, _editingCustomer.CustomerIcon, fileName);
                //reset selectedicon
                SelectedIcon = null;
            } else
            {
                //set the iconproperty to filename  not the whole path
                _editingCustomer.CustomerIcon = fileName;
            }
            _repo.AddEditCustomer(_editingCustomer, EditMode);
            Done();
        }

        private void UpdateCustomer(SimpleEditableCustomer source, Customer target)
        {
            target.Name = source.Name.Trim();
            target.CustomerIcon = source.CustomerIconPath;
            target.Color = source.Color;
        }

        private void OnBrowse()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an icon";
            op.Filter = "\"Image files (*.png, *.ico)|*.png;*.ico\"";
            if (op.ShowDialog() == true)
            {
                SelectedIcon = new BitmapImage(new Uri(op.FileName));
                Customer.CustomerIcon = op.FileName;
                Customer.CustomerIconPath = op.FileName;
            }
        }
        #endregion
    }
}