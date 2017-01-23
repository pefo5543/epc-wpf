using Epc.Data.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Epc.Data.Models.ActionModels;
using System.Threading.Tasks;
using EpcDashboard.Processes;

namespace EpcDashboard.Services.ActionServices
{
    public class EX3Service : BaseRepository
    {
        private string _ex3ExeFileName;
        private string _srcDirPath;
        private string _srcMapFolder = Path.Combine("Schneider", "Empiri X3");
        private string _mapdrive;
        private ProcessListViewModel _vm;
        private int _totalFileCount;

        public bool IsAlive { get; set; }
        public string Ex3SourceDirectory { get; private set; }
        public string Ex3TargetDirectory { get; private set; }

        public EX3Service()
        {
            _ex3ExeFileName = "AppLauncher.exe";
            _srcDirPath = "Updates";
        }

        internal void Initialization(Site site, EX3 x3Info, ProcessListViewModel vm)
        {
            _vm = vm;
            //Ping server
            bool IsAlive = PingIp(x3Info.IpAdress);
            //Create local directory
            CreateTargetDirectory(site);

            if (IsAlive)
            {
                UpdateLocalX3(x3Info.IpAdress, site);
                InitEx3Start(x3Info);
            }
            else
            {
                MessageBox.Show("Server is not accessible");
            }
        }

        private void UpdateLocalX3(string ipAdress, Site site)
        {
            //Check directory access
            bool hasAccess = CheckDirectoryAccess(ipAdress);
            if (hasAccess)
            {
                //use the Dispatcher to delegate progress message back to the UI
                Application.Current.Dispatcher.Invoke((Action)(() => _vm.ProgressMessage = "Prepares for copying"));

                //Check directories and count files to-be copied
                _totalFileCount = PrepareEX3Update(site);

                //Copy files
                InitCopyEx3Progress();
            }
        }

        /// <summary>prepares the paths for EX3 update
        /// <para>Site object containing EX3 relevant data</para>
        /// <para>returns number of files in Updates folder on server</para>
        /// </summary>
        internal int PrepareEX3Update(Site site)
        {
            //Path to Updates content
            Ex3SourceDirectory = Path.Combine(_mapdrive, _srcDirPath);
            if (!String.IsNullOrEmpty(Ex3SourceDirectory) && Directory.Exists(Ex3SourceDirectory))
            {
                int count = CountFiles(Ex3SourceDirectory);

                return count;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>Prepares Copying server EX3 updates content to local path 
        /// <para></para>
        /// <para></para>
        /// </summary>
        internal void InitCopyEx3Progress()
        {
            if (_totalFileCount > 0)
            {
                try
                {
                    CopyEX3Directory(Ex3SourceDirectory, Ex3TargetDirectory, 1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("EX3 Update failed" + e.Message);
                }
            }
            else if (_totalFileCount == 0)
            {
                MessageBox.Show("No updates available");
            }
            else
            {
                MessageBox.Show("EX3 updates on server not available!");
            }
        }

        private int CountFiles(string directory)
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            int result = -1;
            try
            {
                result = info.EnumerateFiles().Count();
            }
            catch
            {
            }
            //Count files in subdirectories
            DirectoryInfo[] dirs = info.GetDirectories();
            foreach (DirectoryInfo d in dirs)
            {
                result += CountFiles(d.FullName);
            }
            return result;
        }

        /// <summary> Does the actual recursive copying of EX3 updates to local directory
        /// and write progress-info to user in ui.
        /// <para></para>
        /// <para></para>
        /// </summary>
        internal int CopyEX3Directory(string sourceDirName, string destDirName, int fileCount = 1, bool overwrite = true, bool copySubDirs = true)
        {

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                //use the Dispatcher to delegate back progress message to the UI
                Application.Current.Dispatcher.Invoke((Action)(() => _vm.ProgressMessage = "Copying " + file.Name));

                //For percentage progress display
                double ratio = (double)fileCount / _totalFileCount;
                double percentage = Math.Round((ratio * 100), 0);

                //use the Dispatcher to delegate back to the UI
                Application.Current.Dispatcher.Invoke((Action)(() => _vm.ProgressCount = Convert.ToInt32(percentage)));


                string tempPath = Path.Combine(destDirName, file.Name);
                FileInfo oldFile = null;
                if (File.Exists(tempPath))
                {
                    oldFile = new FileInfo(tempPath);
                }
                //if file is outdated or doesnt exist we copy/overwrite
                if (oldFile == null || oldFile.LastWriteTime < file.LastWriteTime)
                {
                    file.CopyTo(tempPath, overwrite);
                }
                fileCount++;
            }

            //If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    //count files of subdirectories
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    fileCount = CopyEX3Directory(subdir.FullName, temppath, fileCount, overwrite, copySubDirs);
                }
            }

            return fileCount;
        }

        /// <summary> Creates Local Ex3 directory if it doesnt already exist
        /// <para></para>
        /// <para></para>
        /// </summary>
        internal void CreateTargetDirectory(Site site)
        {
            if (!String.IsNullOrEmpty(site.CustomerName))
            {
                Ex3TargetDirectory = Path.Combine(TargetPath, Constants.EX3Folder, site.CustomerName, site.Name, _srcDirPath);
                Directory.CreateDirectory(Ex3TargetDirectory);
            }
        }

        internal bool CheckDirectoryAccess(string ipAdress)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine("\\\\", ipAdress, _srcMapFolder));
            _mapdrive = di.FullName;

            return di.Exists;
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        internal async void InitEx3Start(EX3 x3Info)
        {
            if (File.Exists(Path.Combine(Ex3TargetDirectory, _ex3ExeFileName)))
            {
                try
                {
                    await Task.Run(() => StartEX3FromLocalSource(x3Info));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Starting EX3 exception, msg: {0}", e.Message);
                }
            }
            else
            {
                MessageBox.Show("Local EX3 instance missing!");
            }
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        internal void StartEX3FromLocalSource(EX3 x3Info)
        {
            try
            {
                Epc.ExternalRunnerDomain.X3Runner.RunInOtherDomain(Ex3TargetDirectory, "EX3.Client.exe", x3Info.IpAdress, x3Info.DbName, x3Info.DbUserName,
                x3Info.DbPassword, UserSettings.Default.UserId);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("X3AppDomain Exception, msg: {0}", e.Message);
            }
        }
    }
}