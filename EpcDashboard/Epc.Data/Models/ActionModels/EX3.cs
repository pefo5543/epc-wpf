using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    [XmlRoot(ElementName = "EX3")]
    public class EX3 : DbInfo, INotifyPropertyChanged
    {
        public EX3()
        {
        }
    }
}
