using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class VNC : ServerBase, INotifyPropertyChanged
    {
        private string _vncPassword;

        [XmlElement(ElementName = "VNCPassword")]
        public string VNCPassword
        {
            get
            {
                return _vncPassword;
            }
            set
            {
                SetField(ref _vncPassword, value, "VNCPassword");
            }
        }
    }
}
