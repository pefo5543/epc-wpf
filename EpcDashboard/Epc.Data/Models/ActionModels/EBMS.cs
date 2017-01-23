using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    [XmlRoot(ElementName = "EBMS")]
    public class EBMS : DbInfo, INotifyPropertyChanged
    {
        private string _homepage;

        public EBMS()
        {
        }

        [XmlElement(ElementName = "Homepage")]
        public string Homepage
        {
            get
            {
                return _homepage;
            }
            set
            {
                SetField(ref _homepage, value, "Homepage");
            }
        }
    }
}
