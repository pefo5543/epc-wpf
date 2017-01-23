using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class BaseAction : BaseModel, INotifyPropertyChanged
    {
        private string _actionName;
        private string _actionType;

        [XmlElement(ElementName = "ActionName")]
        public string ActionName
        {
            get
            {
                return _actionName;
            }
            set
            {
                SetField(ref _actionName, value, "ActionName");
            }
        }

        [XmlElement(ElementName = "ActionType")]
        public string ActionType
        {
            get
            {
                return _actionType;
            }
            set
            {
                SetField(ref _actionType, value, "ActionType");
            }
        }
    }
}
