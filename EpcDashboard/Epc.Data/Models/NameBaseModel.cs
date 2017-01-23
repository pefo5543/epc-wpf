using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models
{
    public class NameBaseModel : BaseModel, INotifyPropertyChanged
    {
        private string _name = "";
        private bool _isSelected;

        [XmlElement(ElementName = "Name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetField(ref _name, value, "Name");
            }
        }

        [XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                SetField(ref _isSelected, value, "IsSelected");
            }
        }
    }
}
