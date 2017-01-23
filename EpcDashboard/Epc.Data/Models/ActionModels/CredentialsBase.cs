using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class CredentialsBase : ServerBase, INotifyPropertyChanged
    {
        private string _userName;
        private string _password;

        [XmlElement(ElementName = "UserName")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                SetField(ref _userName, value, "Username");
            }
        }

        [XmlElement(ElementName = "Password")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetField(ref _password, value, "Password");
            }
        }
    }
}
