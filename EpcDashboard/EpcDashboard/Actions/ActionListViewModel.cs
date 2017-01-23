using Epc.Data;
using Epc.Data.Models.ActionModels;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.SitesProcesses;
using EpcDashboard.ViewModelBases;
using System;

namespace EpcDashboard.Actions
{
    /// <summary>
    /// ViewModel encapsulating actions, property of ProcessListViewModel
    /// </summary>
    public class ActionListViewModel : SiteProcessListsViewModelBase
    {
        private AsyncObservableCollection<EX3> _x3Actions;
        private AsyncObservableCollection<OpenFolder> _folderActions;
        private AsyncObservableCollection<RDP> _rdpActions;
        private AsyncObservableCollection<VNC> _vncActions;
        private AsyncObservableCollection<Exe> _exeActions;

        public ActionListViewModel()
        {
            //Actions
            InstantiateActionRelays();
        }

        private void InstantiateActionRelays()
        {
            //Action related
            EditActionCommand = new RelayCommand<BaseAction>(OnEditAction);
            //X3
            StartEX3Command = new RelayCommand<EX3>(OnStartEX3);
            //Open Folder
            MapFolderCommand = new RelayCommand<OpenFolder>(OnMapFolder);
            //RDP
            RunRDPCommand = new RelayCommand<RDP>(OnRunRDP);
            //VNC
            RunVNCCommand = new RelayCommand<VNC>(OnRunVNC);
            //Exe
            RunExeCommand = new RelayCommand<Exe>(OnRunExe);
        }

        public AsyncObservableCollection<EX3> X3Actions
        {
            get
            { return _x3Actions; }
            set { SetProperty(ref _x3Actions, value); }
        }

        public AsyncObservableCollection<OpenFolder> OpenFolderActions
        {
            get
            { return _folderActions; }
            set { SetProperty(ref _folderActions, value); }
        }

        public AsyncObservableCollection<RDP> RDPActions
        {
            get
            { return _rdpActions; }
            set { SetProperty(ref _rdpActions, value); }
        }

        public AsyncObservableCollection<VNC> VNCActions
        {
            get
            { return _vncActions; }
            set { SetProperty(ref _vncActions, value); }
        }

        public AsyncObservableCollection<Exe> ExeActions
        {
            get
            { return _exeActions; }
            set { SetProperty(ref _exeActions, value); }
        }

        public RelayCommand<BaseAction> EditActionCommand { get; private set; }
        public RelayCommand<EX3> StartEX3Command { get; private set; }
        public RelayCommand<OpenFolder> MapFolderCommand { get; private set; }
        public RelayCommand<RDP> RunRDPCommand { get; private set; }
        public RelayCommand<VNC> RunVNCCommand { get; private set; }
        public RelayCommand<Exe> RunExeCommand { get; private set; }

        //Action events
        public event Action<EBMS> OpenEBMSRequest = delegate { };
        public event Action<EX3> StartEX3Request = delegate { };
        public event Action<OpenFolder> OpenFolderRequest = delegate { };
        public event Action<RDP> RunRDPRequest = delegate { };
        public event Action<VNC> RunVNCRequest = delegate { };
        public event Action<Exe> RunExeRequest = delegate { };
        public event Action<BaseAction> EditActionRequest = delegate { };

        private void OnStartEX3(EX3 x3)
        {
            StartEX3Request(x3);
        }

        private void OnMapFolder(OpenFolder fold)
        {
            OpenFolderRequest(fold);
        }

        private void OnRunRDP(RDP rdp)
        {
            RunRDPRequest(rdp);
        }

        private void OnRunVNC(VNC vnc)
        {
            RunVNCRequest(vnc);
        }

        private void OnRunExe(Exe exe)
        {
            RunExeRequest(exe);
        }

        private void OnEditAction(BaseAction action)
        {
            EditActionRequest(action);
        }

        internal void OnOpenEBMS(EBMS eBMS)
        {
            OpenEBMSRequest(eBMS);
        }
    }
}
