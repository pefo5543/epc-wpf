using EpcDashboard.ViewModelBases;
using System;
using EpcDashboard.Services.Interfaces;
using Epc.Data.Models;
using System.Collections.Generic;
using EpcDashboard.MVVMHelpers;

namespace EpcDashboard.SitesProcesses
{
    public class CopySiteProcessViewModel : ViewModelBase
    {
        private IMainRepository _repo;
        private ICopyService _service;
        private bool _isSite;
        private string _customerName;
        private Site _site;
        private Process _process;
        private List<NameBaseModel> _tableData;

        public CopySiteProcessViewModel(IMainRepository repo, ICopyService service)
        {
            _repo = repo;
            _service = service;
            CopyCommand = new RelayCommand<List<NameBaseModel>>(OnCopy);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public RelayCommand<List<NameBaseModel>> CopyCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public event Action<string> Done = delegate { };
        public event Action Cancel = delegate { };

        public bool IsSite
        {
            get
            {
                return _isSite;
            }
            set { SetProperty(ref _isSite, value); }
        }

        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set { SetProperty(ref _customerName, value); }
        }

        public Site Site
        {
            get
            {
                return _site;
            }
            set { SetProperty(ref _site, value); }
        }

        public Process Process
        {
            get
            {
                return _process;
            }
            set { SetProperty(ref _process, value); }
        }

        public List<NameBaseModel> TableData
        {
            get
            {
                return _tableData;
            }
            set { SetProperty(ref _tableData, value); }
        }

        public string ContentHeader
        {
            get
            {
                return _contentHeader;
            }
        }

        internal void SetContext(object entity, object parent)
        {
            if (IsSite)
            {
                Site = (Site)entity;
                CustomerName = ((Customer)parent).Name;
                _contentHeader = "Select customers to copy site " + Site.Name + " into.";
            }
            else
            {
                Process = (Process)entity;
                Site = (Site)parent;
                CustomerName = Site.CustomerName;
                _contentHeader = "Select sites to copy process " + Process.Name + " into.";
            }

            LoadTableData();
        }

        internal void LoadTableData()
        {
            TableData = _service.PrepareSiteProcessTable(_repo.Data.Customers, CustomerName, Site, IsSite);
        }

        private void OnCopy(List<NameBaseModel> list)
        {
            _repo.Data = _service.ExecuteCopy(_repo.Data, list, (IsSite ? (NameBaseModel)Site : Process), IsSite);
            _repo.GenericSave();
            Done((IsSite ? Constants.StrSite + " " + Site.Name : Constants.StrProcess + " " + Process.Name) + " successfully copied.");
        }

        private void OnCancel()
        {
            Cancel();
        }
    }
}
