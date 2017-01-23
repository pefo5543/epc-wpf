using System;
using Epc.Data.Models.ActionModels;
using System.IO;

namespace EpcDashboard.Services.ActionServices
{
    /// <summary>
    /// Serviceclass for setup VNC connections
    /// </summary>
    internal class VNCService : RunProcessBaseService
    {

        internal void RunVNC(VNC vncInfo)
        {
            string launchVNCPath = Path.Combine(Environment.CurrentDirectory, "Resources", "vnc.exe");
            //vnc.exe arguments: {Server} /password {password}
            string arguments = String.Format(@"{0} /password {1}",
                                        vncInfo.IpAdress,
                                        vncInfo.VNCPassword);
            StartProcess(launchVNCPath, arguments);
        }
    }
}