using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Epc.Data.Models.ActionModels
{
    [XmlRoot(ElementName = "DbInfo")]
    public class DbInfo : BaseAction, INotifyPropertyChanged
    {
        private string _ipAdress;
        private string _dbUserName;
        private string _dbPassword;
        private string _dbName;

        public DbInfo()
        {
        }
        [XmlElement(ElementName = "DbUserName")]
        public string DbUserName
        {
            get
            {
                return _dbUserName;
            }
            set
            {
                SetField(ref _dbUserName, value, "DbUserName");
            }
        }
        [XmlElement(ElementName = "DbPassword")]
        public string DbPassword
        {
            get
            {
                return _dbPassword;
            }
            set
            {
                SetField(ref _dbPassword, value, "DbPassword");
            }
        }

        [XmlElement(ElementName = "DbName")]
        public string DbName
        {
            get
            {
                return _dbName;
            }
            set
            {
                SetField(ref _dbName, value, "DbName");
            }
        }

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
