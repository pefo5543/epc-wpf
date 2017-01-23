using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class RDP : CredentialsBase, INotifyPropertyChanged
    {
        private string _port = "3389";
        private string _domain;

        [XmlElement(ElementName = "Port")]
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                SetField(ref _port, value, "Port");
            }
        }

        [XmlElement(ElementName = "Domain")]
        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                SetField(ref _domain, value, "Domain");
            }
        }
    }
}
