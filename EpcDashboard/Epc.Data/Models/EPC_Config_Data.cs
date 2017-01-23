/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Epc.Data.Models
{
    [XmlRoot(ElementName = "EPC")]
    public class EPC_Config_Data : BaseModel, INotifyPropertyChanged
    {
        public EPC_Config_Data()
        {
            _customers = new AsyncObservableCollection<Customer>();
            _customers.CollectionChanged += CollectionChanged;
        }
        private string _serverPath;
        private string _version;
        private AsyncObservableCollection<Customer> _customers;

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
            OnPropertyChanged("Customers");
        }

        [XmlElement(ElementName = "ServerPath")]
        public string ServerPath
        {
            get
            {
                return _serverPath;
            }
            set
            {
                SetField(ref _serverPath, value, "ServerPath");
                //if (_serverPath != value)
                //{
                //    _serverPath = value;
                //    PropertyChanged(this, new PropertyChangedEventArgs("ServerPath"));
                //}
            }
        }
        [XmlElement(ElementName = "Version")]
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                SetField(ref _version, value, "Version");
            }
        }
        [XmlElement(ElementName = "Customer")]
        public AsyncObservableCollection<Customer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                if (_customers != value)
                {
                    // Detach the event handler from current instance, if any:
                    if (_customers != null)
                    {
                        _customers.CollectionChanged -= CollectionChanged;
                    }

                    _customers = value;

                    // Attach the event handler to the new instance, if any:
                    if (_customers != null)
                    {
                        _customers.CollectionChanged += CollectionChanged;
                    }
                    // Notify that the 'Customers' list has changed:
                    //PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
                    //OnPropertyChanged("Customers");
                }
            }
        }
    }
}