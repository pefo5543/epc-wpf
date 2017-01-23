using System.ComponentModel;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    public class Exe : BaseAction, INotifyPropertyChanged
    {
        private string _fileName;

        private string _arguments;

        [XmlElement(ElementName = "FileName")]
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                SetField(ref _fileName, value, "FileName");
            }
        }

        [XmlElement(ElementName = "Arguments")]
        public string Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                SetField(ref _arguments, value, "Arguments");
            }
        }
    }
}
