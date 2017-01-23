using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class ServerBase : BaseAction, INotifyPropertyChanged
    {
        private string _ipAdress;

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
    }
}
