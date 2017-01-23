using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    [XmlRoot(ElementName = "Server")]
    public class OpenFolder : CredentialsBase, INotifyPropertyChanged
    {
        //TODO:Implement Path propery in OpenFolder model, right now open folder action only opens root. 

        public OpenFolder()
        {
        }
    }
}
