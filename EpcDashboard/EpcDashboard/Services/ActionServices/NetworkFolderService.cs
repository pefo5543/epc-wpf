using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Epc.Data.Models.ActionModels;
using System.Threading;

namespace EpcDashboard.Services.ActionServices
{
    /// <summary>
    /// Serviceclass for mapping shared network resources
    /// </summary>
    public class NetworkFolderService : BaseService
    {
        internal void MapFolderAction(OpenFolder serverInfo)
        {
            //Ping server
            bool IsAlive = PingIp(serverInfo.IpAdress);
            if (IsAlive)
            {
                //Map server folder
                string path = MapDirectory(serverInfo.IpAdress, serverInfo.UserName, serverInfo.Password);
                //Open folder
                OpenDirectory(path);
            }
            else
            {
                MessageBox.Show("Server is not accessible");
            }
        }

        internal string MapDirectory(string ipAdress, string serverUserName, string serverPassword)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine("\\\\", ipAdress, "share"));
            if (!di.Exists)
            {
                NetworkDrive nd = new NetworkDrive();
                string driveLetter = GetUnoccupiedDriveLetter();
                nd.LocalDrive = driveLetter;
                nd.ShareName = Path.Combine("\\\\", ipAdress);
                nd.Persistent = false;

                try
                {
                    nd.MapDrive(serverUserName, serverPassword);
                }
                catch
                {
                    //MessageBox.Show("Failed to map server, msg: " + e.Message + ", inner exception: " + e.InnerException);
                }
                //check that drive is now mapped
                //nd.IsMapped = Directory.Exists(_mapdrive);
                //if (!nd.IsMapped)
                //{
                //    MessageBox.Show("Mapping failed. Check your server credentials");
                //}

                return nd.ShareName;
            }
            else
            {
                return di.FullName;
            }
        }

        internal void OpenDirectory(string directory)
        {
            try
            {
                System.Diagnostics.Process.Start(directory);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the directory specified...
                Console.WriteLine(win32Exception.Message);
            }

        }

        private string GetUnoccupiedDriveLetter()
        {
            string drive = "";
            char[] reversed = NetworkDrive.Alphabet.Reverse().ToArray();
            foreach (char driveLetter in reversed)
            {
                drive = Path.GetPathRoot(driveLetter + @":\");
                if (!Directory.Exists(drive))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            return drive;
        }
    }
}
