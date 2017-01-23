using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Specialized;
using Epc.Data.Models.ActionModels;

namespace Epc.Data.Models
{
    [XmlRoot(ElementName = "Customer")]
    public class Customer : NameBaseModel, INotifyPropertyChanged
    {
        private string _customerIcon = "";
        private AsyncObservableCollection<Site> _sites;
        private EBMS _ebms;
        private string _color;

        public Customer()
        {
            _sites = new AsyncObservableCollection<Site>();
            _sites.CollectionChanged += CollectionChanged;
            _ebms = new EBMS();
        }
        [XmlElement(ElementName = "CustomerIcon")]
        public string CustomerIcon
        {
            get
            {
                return _customerIcon;
            }
            set
            {
                SetField(ref _customerIcon, value, "CustomerIcon");
            }
        }

        [XmlElement(ElementName = "Color")]
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                SetField(ref _color, value, "Color");
            }
        }
        [XmlElement(ElementName = "Site")]
        public AsyncObservableCollection<Site> Sites
        {
            get
            {
                return _sites;
            }
            set
            {
                // Detach the event handler from current instance, if any:
                if (_sites != null)
                {
                    _sites.CollectionChanged -= CollectionChanged;
                }

                _sites = value;

                // Attach the event handler to the new instance, if any:
                if (_sites != null)
                {
                    _sites.CollectionChanged += CollectionChanged;
                }

                //OnPropertyChanged("Sites");
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

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
            OnPropertyChanged("Sites");
        }
        //public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs("Sites"));
        //}
    }
}
