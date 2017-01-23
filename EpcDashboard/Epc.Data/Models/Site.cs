using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Specialized;
using Epc.Data.Models.ActionModels;

namespace Epc.Data.Models
{
    [XmlRoot(ElementName = "Site")]
    public class Site : NameBaseModel, INotifyPropertyChanged
    {
        private string _siteIcon;
        private EBMS _ebms;
        private AsyncObservableCollection<Process> _processes;
        private bool _isOnline;
        private string _customerName;
        private string _ipAdress;
        private AsyncObservableCollection<EX3> _x3Actions;
        private AsyncObservableCollection<OpenFolder> _folderActions;
        private AsyncObservableCollection<RDP> _rdpActions;
        private AsyncObservableCollection<VNC> _vncActions;
        private AsyncObservableCollection<Exe> _ExeActions;

        public Site()
        {
            _processes = new AsyncObservableCollection<Process>();
            _processes.CollectionChanged += CollectionChanged;

            _x3Actions = new AsyncObservableCollection<EX3>();
            _x3Actions.CollectionChanged += CollectionChanged;

            _folderActions = new AsyncObservableCollection<OpenFolder>();
            _folderActions.CollectionChanged += CollectionChanged;

            _rdpActions = new AsyncObservableCollection<RDP>();
            _rdpActions.CollectionChanged += CollectionChanged;

            _vncActions = new AsyncObservableCollection<VNC>();
            _vncActions.CollectionChanged += CollectionChanged;

            _ExeActions = new AsyncObservableCollection<Exe>();
            _ExeActions.CollectionChanged += CollectionChanged;

            _ebms = new EBMS();
        }

        [XmlElement(ElementName = "SiteIcon")]
        public string SiteIcon
        {
            get
            {
                return _siteIcon;
            }
            set
            {
                SetField(ref _siteIcon, value, "SiteIcon");
            }
        }
        [XmlElement(ElementName = "Process")]
        public AsyncObservableCollection<Process> Processes
        {
            get
            {
                return _processes;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_processes != null)
                {
                    _processes.CollectionChanged -= CollectionChanged;
                }

                _processes = value;

                // Attach the event handler to the new instance, if any:
                if (_processes != null)
                {
                    _processes.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("Processes");
            }
        }
        [XmlElement(ElementName = "IpAdress")]
        public string IpAdress
        {
            get
            {
                return _ipAdress;
            }
            set
            {
                SetField(ref _ipAdress, value, "IpAdress");
            }
        }
        [XmlElement(ElementName = "EBMS")]
        public EBMS EBMS
        {
            get
            {
                return _ebms;
            }
            set
            {
                SetField(ref _ebms, value, "EBMS");
            }
        }

        [XmlIgnore]
        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
            set
            {
                SetField(ref _isOnline, value, "IsOnline");
            }
        }
        [XmlIgnore]
        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                SetField(ref _customerName, value, "CustomerName");
            }
        }

        [XmlElement(ElementName = "X3Actions")]
        public AsyncObservableCollection<EX3> X3Actions
        {
            get
            {
                return _x3Actions;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_x3Actions != null)
                {
                    _x3Actions.CollectionChanged -= CollectionChanged;
                }

                _x3Actions = value;

                // Attach the event handler to the new instance, if any:
                if (_x3Actions != null)
                {
                    _x3Actions.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("X3Actions");
            }
        }

        [XmlElement(ElementName = "FolderActions")]
        public AsyncObservableCollection<OpenFolder> FolderActions
        {
            get
            {
                return _folderActions;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_folderActions != null)
                {
                    _folderActions.CollectionChanged -= CollectionChanged;
                }

                _folderActions = value;

                // Attach the event handler to the new instance, if any:
                if (_folderActions != null)
                {
                    _folderActions.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("FolderActions");
            }
        }

        [XmlElement(ElementName = "RDPActions")]
        public AsyncObservableCollection<RDP> RDPActions
        {
            get
            {
                return _rdpActions;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_rdpActions != null)
                {
                    _rdpActions.CollectionChanged -= CollectionChanged;
                }

                _rdpActions = value;

                // Attach the event handler to the new instance, if any:
                if (_rdpActions != null)
                {
                    _rdpActions.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("RDPActions");
            }
        }

        [XmlElement(ElementName = "VNCActions")]
        public AsyncObservableCollection<VNC> VNCActions
        {
            get
            {
                return _vncActions;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_vncActions != null)
                {
                    _vncActions.CollectionChanged -= CollectionChanged;
                }

                _vncActions = value;

                // Attach the event handler to the new instance, if any:
                if (_vncActions != null)
                {
                    _vncActions.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("VNCActions");
            }
        }

        [XmlElement(ElementName = "ExeActions")]
        public AsyncObservableCollection<Exe> ExeActions
        {
            get
            {
                return _ExeActions;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_ExeActions != null)
                {
                    _ExeActions.CollectionChanged -= CollectionChanged;
                }

                _ExeActions = value;

                // Attach the event handler to the new instance, if any:
                if (_ExeActions != null)
                {
                    _ExeActions.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged("ExeActions");
            }
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Processes");
        }
    }
}
