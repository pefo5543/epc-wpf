using Epc.Data.Models;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EpcDashboard.Sites
{
    public class AddEditSiteViewModel : AddEditViewModelBase
    {
        #region fields
        private Site _editingSite = null;
        private IMainRepository _repo;
        private ImageSource _selectedIcon = null;
        private SimpleEditableSite _Site;
        private Customer _currentCustomer;

        #endregion

        #region constructors
        public AddEditSiteViewModel(IMainRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            BrowseCommand = new RelayCommand(OnBrowse);
        }
        #endregion

        #region properties/commands
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set { SetProperty(ref _currentCustomer, value); }
        }

        public ImageSource SelectedIcon
        {
            get
            {
                return _selectedIcon;
            }
            set { SetProperty(ref _selectedIcon, value); }
        }

        public SimpleEditableSite Site
        {
            get { return _Site; }
            set { SetProperty(ref _Site, value); }
        }
        public RelayCommand BrowseCommand { get; private set; }

        public event Action<string> Done = delegate { };

        #endregion

        #region methods
        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void SetSite(Customer cust, Site site)
        {
            CurrentCustomer = cust;
            _contentHeader = "Add new site to customer " + CurrentCustomer.Name;
            _editingSite = site;
            if (Site != null) Site.ErrorsChanged -= RaiseCanExecuteChanged;
            Site = new SimpleEditableSite();
            Site.ErrorsChanged += RaiseCanExecuteChanged;
            CopySite(site, Site);
        }

        private void CopySite(Site source, SimpleEditableSite target)
        {
            target.Name = source.Name;
            target.SiteIcon = source.SiteIcon;
            target.SetSiteIconPath();
            target.IpAdress = source.IpAdress;
        }

        private bool CanSave()
        {
            return !Site.HasErrors;
        }

        private void OnSave()
        {
            UpdateSite(Site, _editingSite);
            string fileName = Path.GetFileName(_editingSite.SiteIcon);
            if (SelectedIcon != null)
            {
                //Copy image to server and return icon filename - we dont want the whole path 
                _editingSite.SiteIcon = _repo.SaveImageOnServerAndLocal(SelectedIcon, _editingSite.SiteIcon, fileName);
                //reset selectedicon
                SelectedIcon = null;
            } else
            {
                //set the iconproperty to filename  not the whole path
                _editingSite.SiteIcon = fileName;
            }
            _repo.AddEditSite(CurrentCustomer, _editingSite, EditMode);
            Done(Constants.StrSite + " updated");
        }

        private void UpdateSite(SimpleEditableSite source, Site target)
        {
            target.Name = source.Name.Trim();
            target.SiteIcon = source.SiteIconPath;
            target.IpAdress  = source.IpAdress;
        }

        private void OnBrowse()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an icon";
            op.Filter = "\"Image files (*.png, *.ico)|*.png;*.ico\"";
            if (op.ShowDialog() == true)
            {
                SelectedIcon = new BitmapImage(new Uri(op.FileName));
                Site.SiteIcon = op.FileName;
                Site.SiteIconPath = op.FileName;
            }
        }
        #endregion
    }
}
