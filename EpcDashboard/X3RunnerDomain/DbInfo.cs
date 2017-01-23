using System;

namespace Epc.ExternalRunnerDomain
{
    [Serializable]
    public class DbInfo
    {
        private string _userName;
        private string _password;
        private string _name;
        private string _ipAdress;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string IpAdress
        {
            get { return _ipAdress; }
            set { _ipAdress = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password =value; }
        }
    }
}
