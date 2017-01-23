using EpcDashboard.ViewModelBases;
using System.Net.NetworkInformation;

namespace EpcDashboard.Services
{
    public class BaseService : BindableBase
    {

        /// <summary>
        /// <para></para>
        /// </summary>
        public bool PingIp(string ip, int timeOut = 100)
        {
            bool pingSuccess = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(ip, timeOut);
                pingSuccess = (reply.Status == IPStatus.Success);
            }
            catch
            {
                //Here when for example internet connection is down - ignoring exception
                //Console.WriteLine("Caught Ping Exception [{0}]", e.Message);
                pingSuccess = false;
            }

            return pingSuccess;
        }
    }
}
