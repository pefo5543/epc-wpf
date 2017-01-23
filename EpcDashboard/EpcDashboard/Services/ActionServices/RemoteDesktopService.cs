using Epc.Data.Models.ActionModels;
using System.Windows;
using System;
using System.IO;

namespace EpcDashboard.Services.ActionServices
{
    /// <summary>
    /// Serviceclass for setup RDP connections
    /// </summary>
    internal class RemoteDesktopService : RunProcessBaseService
    {

        internal void RunRemoteDesktop(RDP rdpInfo)
        {
            //Ping server
            bool IsAlive = PingIp(rdpInfo.IpAdress);
            if (IsAlive)
            {
                string launchRDPPath = Path.Combine(Environment.CurrentDirectory, "Resources", "LaunchRDP.exe");
                //LaunchRDP arguments: Server Port Username Domain Password Console(0=False, 1 =True) ReDirectDrives(0=Flase, 1=true) Redirectprinters(0=false, 1=true)
                string arguments = String.Format(@"{0} {1} {2} {3} {4} 0 0 0",
                                                rdpInfo.IpAdress,
                                                rdpInfo.Port,
                                                rdpInfo.UserName,
                                                rdpInfo.Domain,
                                                rdpInfo.Password);
                StartProcess(launchRDPPath, arguments);
            }
            else
            {
                MessageBox.Show("Server is not accessible");
            }
        }
    }
}
