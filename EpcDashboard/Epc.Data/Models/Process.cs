using System.Xml.Serialization;
using System.ComponentModel;

namespace Epc.Data.Models
{
    [XmlRoot(ElementName = "Process")]
    public class Process : NameBaseModel, INotifyPropertyChanged
    {
        private string _processIcon;
        private string _ping;
        private string _program;
        private string _argument;

        [XmlElement(ElementName = "ProcessIcon")]
        public string ProcessIcon
        {
            get
            {
                return _processIcon;
            }
            set
            {
                SetField(ref _processIcon, value, "ProcessIcon");
            }
        }
        [XmlElement(ElementName = "Ping")]
        public string Ping
        {
            get
            {
                return _ping;
            }
            set
            {
                SetField(ref _ping, value, "Ping");
            }
        }
        [XmlElement(ElementName = "Program")]
        public string Program
        {
            get
            {
                return _program;
            }
            set
            {
                SetField(ref _program, value, "Program");
            }
        }
        [XmlElement(ElementName = "Argument")]
        public string Argument
        {
            get
            {
                return _argument;
            }
            set
            {
                SetField(ref _argument, value, "Argument");
            }
        }
    }
}
