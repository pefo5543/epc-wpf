using Epc.Data;
using System;
using System.ComponentModel;
using System.Linq;

namespace EpcDashboard.Settings
{
    public class Setting : INotifyPropertyChanged
    {
        private string _userId;
        private string _lastName;
        private string _firstName;
        private bool _winStart;
        private string _sourcePath;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string UserId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("UserId"));
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                }
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
            }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (_sourcePath != value)
                {
                    _sourcePath = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SourcePath"));
                }
            }
        }
        public bool WinStart
        {
            get { return _winStart; }
            set
            {
                if (_winStart != value)
                {
                    _winStart = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("WinStart"));
                }
            }
        }

        public Setting()
        {

        }

        public Setting(string userid, string firstName, string lastName, bool winStart, string sourcePath)
        {
                UserId = userid;
                FirstName = firstName;
                LastName = lastName;
                WinStart = winStart;
                SourcePath = sourcePath;
        }
    }
}

