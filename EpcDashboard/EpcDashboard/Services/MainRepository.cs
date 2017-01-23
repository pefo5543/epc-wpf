/*
 * Application: Empiri Configuration Manager
 * Version: 1.0 
 * Date of origin: 2016-12-06
 * Author: Petter Fogelqvist, petter.fogelqvist@gmail.com
*/

using Epc.Data;
using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Services.Interfaces;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EpcDashboard.Services
{
    /// <summary>
    /// Core Service class, holds the xml data model instance
    /// The same instance of MainRepository is shared across almost all viewmodels as an intermediate, common service for updating and saving the data model
    /// </summary>
    public class MainRepository : BaseRepository, IMainRepository
    {
        #region Fields
        private EPC_Config_Data _data;
        private bool _serverIsAlive;
        private FileSystemWatcher _watcher;
        private Random _random = new Random();
        #endregion

        #region Properties
        public EPC_Config_Data Data
        {
            get
            {
                return _data;
            }
            set { SetProperty(ref _data, value); }
        }

        public bool ServerIsAlive
        {
            get
            {
                return _serverIsAlive;
            }
            set
            {
                if (_serverIsAlive != value)
                {
                    _serverIsAlive = value;
                }
            }
        }
        public FileSystemWatcher Watcher
        {
            get
            {
                return _watcher;
            }
            set
            {
                if (_watcher != value)
                {
                    _watcher = value;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called once during startup from mainwindowviewmodel
        /// Sync local file, and then deserialize xmldata and populate an EPC_Config_Data object
        /// </summary>
        public object GetData()
        {
            LocalSourceSync();
            EPC_Config_Data result = new EPC_Config_Data();
            //Read xml from local configfile
            result = LoadData(TargetFile, result);

            return result;
        }
        /// <summary>Update local directory with content of source directory
        /// <para>If fromBackground = true - Local images are overwritten too</para>
        /// <para>If it is outdated</para>
        /// </summary>
        private void LocalSourceSync(bool fromBackground = false)
        {
            bool sourceAccess = Directory.Exists(SourcePath);
            bool localExists = CheckIfLocalExists(TargetFile);
            if (ServerIsAlive && sourceAccess)
            {
                //Dont sync images when another client made an update 
                if (!fromBackground)
                {
                    SyncImages();
                    SyncExecutables();
                }
                SyncConfig(localExists, sourceAccess);
            }
            else
            {
                if (!localExists)
                {
                    //create new local file here with basic xml-node-structure 
                }
            }
        }
        /// <summary> Overwrites target file with sourcefile if targetfile is outdated
        /// <para></para>
        /// <para></para>
        /// </summary>
        private void SyncConfig(bool localExists, bool sourceAccess)
        {
            bool? isUpdated = false;
            if (localExists)
            {
                //check version
                isUpdated = CompareVersion(SourceFile, TargetFile, sourceAccess);
            }
            if (isUpdated == false)
            {
                //Update to newer version
                try
                {
                    CopyFile(TargetPath, TargetFile, SourceFile, localExists);
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("OnChanged: Caught Exception copying file [{0}]", ioe.ToString());
                }
            }
        }
        /// <summary>
        /// Called once at startup and updates local images from central images folder
        /// </summary>
        private void SyncImages()
        {
            string sourceDirectory = Path.Combine(SourcePath, Constants.ImageFolder);
            string targetDirectory = Path.Combine(TargetPath, Constants.ImageFolder);
            CopySourceFilesToTarget(sourceDirectory, targetDirectory);
        }

        /// <summary>
        /// Called once at startup and updates local executables from central executables folder
        /// </summary>
        private void SyncExecutables()
        {
            string sourceDirectory = Path.Combine(SourcePath, Constants.ExecutablesFolder);
            string targetDirectory = Path.Combine(TargetPath, Constants.ExecutablesFolder);
            CopySourceFilesToTarget(sourceDirectory, targetDirectory);
        }

        private void CopySourceFilesToTarget(string sourceDirectory, string targetDirectory)
        {
            bool sourceAccess = Directory.Exists(sourceDirectory);
            if (!sourceAccess)
            {
                Directory.CreateDirectory(sourceDirectory);
                //Cant copy files this time
            }
            else
            {
                //Copy files from source to target
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirectory);
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string tempPath = Path.Combine(targetDirectory, file.Name);
                    FileInfo targetFile = new FileInfo(tempPath);
                    //local file already exists - check that its not locked by another process
                    if (File.Exists(tempPath))
                    {
                        try
                        {
                            file.CopyTo(tempPath, true);
                        }
                        catch (Exception)
                        {
                            //throw; ignore
                        }
                    }
                    else
                    {
                        //local file doesnt exist - no need to overwrite
                        file.CopyTo(tempPath, false);
                    }
                }
            }

        }

        /// <summary>
        /// <para>FileEventWatcher has noticed an update in central config file - local file is updated here - if necessary</para>
        /// </summary>
        public async void UpdateData(EPC_Config_Data data)
        {
            bool sourceAccess = Directory.Exists(SourcePath);
            if (ServerIsAlive && sourceAccess)
            {
                //Check if versions differ
                bool? versionControl = CompareVersion(SourceFile, TargetFile, true);
                if (versionControl == false)
                {
                    //Update local xml config
                    EPC_Config_Data xmlSource = new EPC_Config_Data();
                    xmlSource = LoadData(SourceFile, xmlSource);
                    Data = UpdateChangedProperties(data, xmlSource);
                } else if(versionControl == null)
                {
                    //Local xml newer than source - merge target and source
                    MergeLocalSourceService s = await Task.Run(() => MergeData(Data, new MergeLocalSourceService()));
                }
            }
            LocalSourceSync(true);
        }

        /// <summary>Call merge service and save if any updates performed
        /// Thread.Sleep is to avoid that multiple clients try to merge at the same time  
        /// <para>data is the databound object reflecting target xml file</para>
        /// </summary>
        public MergeLocalSourceService MergeData(EPC_Config_Data data, MergeLocalSourceService service)
        {
            bool sourceAccess = Directory.Exists(SourcePath);
            if (ServerIsAlive && sourceAccess)
            {
                int delay = _random.Next(1, 50) * 100; //interval 100ms - 5000ms
                Thread.Sleep(delay);

                //Check if source file is locked
                
                //Load source config file
                EPC_Config_Data xmlSource = new EPC_Config_Data();
                xmlSource = LoadData(SourceFile, xmlSource);

                //Merge source and target - return the merged source
                Data = service.MergeSourceAndTarget(data, xmlSource);

                if (service.SourceIsChanged)
                {
                    GenericSave();
                }
            }

            return service;
        }

        /// <summary>
        /// Gets here when Centralized xml config has been updated by another client
        ///
        /// </summary>
        private EPC_Config_Data UpdateChangedProperties(EPC_Config_Data data, EPC_Config_Data xmlData)
        {
            bool isChanged = true;
            data.Version = xmlData.Version;
            data.ServerPath = xmlData.ServerPath;

            //TODO: Run Update from background bruteforce solution in background thread
            while (isChanged)
            {
                //BruteForce solution to get ObservableCollections to update properly
                isChanged = EpcBackgroundSync.CompareCustomers(data.Customers, xmlData.Customers);
                //NOTICE - if new properties are added to data model - Epc_ConfigDataChangeSync.cs must be updated as well if update from background should be able for these properties 
            }

            return data;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// <para>Save data to both local and central configfile</para>
        /// </summary>
        public void GenericSave()
        {
            IncrementVersion();

            //SaveLocally
            SaveData(Data, TargetFile);
            //Save at central
            if (ServerIsAlive)
            {
                //Local is already updated so turn off FileEventWatcher during write operation
                TurnOnOffFileSystemEventWatcher(false);
                SaveData(Data, SourceFile);
                TurnOnOffFileSystemEventWatcher(true);
            }
        }

        /// <summary>
        /// <para>Add or edit a customer to collection</para>
        /// </summary>
        public void AddEditCustomer(Customer customer, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new
                Data.Customers.Add(customer);
            }
            GenericSave();
        }

        private string GetEx3CustomerDirectoryPath(string customerName)
        {
            return Path.Combine(TargetPath, Constants.EX3Folder, customerName);
        }

        private string GetEx3SiteDirectoryPath(string customerName, string name)
        {
            return Path.Combine(TargetPath, Constants.EX3Folder, customerName, name);
        }

        private void DeleteEx3Directories(string directory)
        {
            if (Directory.Exists(directory))
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete ex3 directory..." + e.Message);
                }
            }
        }

        /// <summary>
        /// <para>Add or edit a site to collection</para>
        /// </summary>
        public void AddEditSite(Customer customer, Site site, bool editMode)
        {
            //Make generic method
            if (!editMode)
            {
                //Add new site to customer               
                customer.Sites.Add(site);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a process to a site in collection</para>
        /// </summary>
        public void AddEditProcess(Site site, Process process, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new site to customer               
                site.Processes.Add(process);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a x3Action to a site in collection</para>
        /// </summary>
        public void AddEditX3Action(Site site, EX3 x3Action, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new x3Action to site               
                site.X3Actions.Add(x3Action);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a empiriAction to a site in collection</para>
        /// </summary>
        public void AddEditOpenFolderAction(Site site, OpenFolder empiriAction, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new Action to site               
                site.FolderActions.Add(empiriAction);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a rdpAction to a site in collection</para>
        /// </summary>
        public void AddEditRDPAction(Site site, RDP rdpAction, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new Action to site               
                site.RDPActions.Add(rdpAction);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a vncAction to a site in collection</para>
        /// </summary>
        public void AddEditVNCAction(Site site, VNC vncAction, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new Action to site               
                site.VNCActions.Add(vncAction);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Add or edit a exeAction to a site in collection</para>
        /// </summary>
        public void AddEditExeAction(Site site, Exe exeAction, bool EditMode)
        {
            //Make generic method
            if (!EditMode)
            {
                //Add new Action to site               
                site.ExeActions.Add(exeAction);
            }
            GenericSave();
        }

        /// <summary>
        /// <para>Delete a process from collection</para>
        /// </summary>
        public void DeleteProcess(Site site, Process process)
        {
            site.Processes.Remove(process);
            GenericSave();
        }

        /// <summary>
        /// <para>Delete a site from collection</para>
        /// </summary>
        public void DeleteSite(Customer customer, Site site)
        {
            customer.Sites.Remove(site);
            GenericSave();
        }

        /// <summary>
        /// <para>Delete a customer from collection</para>
        /// </summary>
        public void DeleteCustomer(Customer cust)
        {
            Data.Customers.Remove(cust);
            GenericSave();
        }
        /// <summary>
        /// <para>Delete/reset a EBMS info object from parent</para>
        /// </summary>
        public void DeleteEBMS(object parent)
        {
            bool canSave = false;
            if (parent is Customer)
            {
                //overwrite EBMS object with fresh instance
                (parent as Customer).EBMS = new EBMS();
                canSave = true;
            }
            else if (parent is Site)
            {
                //overwrite EBMS object with fresh instance
                (parent as Site).EBMS = new EBMS();
                canSave = true;
            }
            if (canSave)
            {
                GenericSave();
            }
        }

        public void DeleteAction(Site site, BaseAction action)
        {
            if (action.ActionType == Constants.TypeX3Txt)
            {
                site.X3Actions.Remove((EX3)action);
            }
            else if (action.ActionType == Constants.TypeOpenFolderTxt)
            {
                site.FolderActions.Remove((OpenFolder)action);
            }
            else if (action.ActionType == Constants.TypeRDPTxt)
            {
                site.RDPActions.Remove((RDP)action);
            }
            else if (action.ActionType == Constants.TypeVNCTxt)
            {
                site.VNCActions.Remove((VNC)action);
            }
            else if (action.ActionType == Constants.TypeEXETxt)
            {
                site.ExeActions.Remove((Exe)action);
            }

            GenericSave();
        }

        /// <summary>
        /// Turn on or off the Filesystemevent listener
        /// </summary>
        private void TurnOnOffFileSystemEventWatcher(bool onOff)
        {
            try
            {
                if (_watcher.EnableRaisingEvents = !onOff && !String.IsNullOrEmpty(_watcher.Path))
                {
                    _watcher.EnableRaisingEvents = onOff;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Error turning FileEventWatcher OnOff {0}", e.Message);
            }
        }

        /// <summary>
        /// <para>Copy a local image to Central server, and local epc directory, if it doesnt already exists</para>
        /// </summary>
        public string SaveImageOnServerAndLocal(ImageSource icon, string filePath, string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                //store to server
                if (ServerIsAlive)
                {
                    string serverDirectory = Path.Combine(Properties.Settings.Default.CentralSourcePath, Constants.ImageFolder);
                    string serverPath = Path.Combine(serverDirectory, fileName);
                    CopyFile(serverDirectory, serverPath, filePath);
                }
                // store locally
                string localDirectory = Path.Combine(UserSettings.Default.SourcePath, Constants.ImageFolder);
                string localPath = Path.Combine(localDirectory, fileName);
                CopyFile(localDirectory, localPath, filePath);
            }

            return fileName;
        }

        /// <summary>
        /// <para>Copy a local file to Central server epc directory, and local epc directory, if it doesnt already exists</para>
        /// </summary>
        public string SaveExeOnServerAndLocal(FileInfo file)
        {
            string fileName = Path.GetFileName(file.FullName);
            //store to server
            if (ServerIsAlive)
            {
                string serverDirectory = Path.Combine(Properties.Settings.Default.CentralSourcePath, Constants.ExecutablesFolder);
                string serverPath = Path.Combine(serverDirectory, fileName);
                CopyFile(serverDirectory, serverPath, file.FullName, true);
            }
            // store locally
            string localDirectory = Path.Combine(UserSettings.Default.SourcePath, Constants.ExecutablesFolder);
            string localPath = Path.Combine(localDirectory, fileName);
            CopyFile(localDirectory, localPath, file.FullName, true);

            return fileName;
        }

        /// <summary>
        /// <para>Read and deserialize xmldata from filePath and populate the EPC_ConfigData model object with its content</para>
        /// </summary>
        public EPC_Config_Data LoadData(string filePath, EPC_Config_Data data)
        {
            while (true)
            {
                try
                {
                    try
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(EPC_Config_Data));
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            Data = (EPC_Config_Data)
                            mySerializer.Deserialize(fs);
                        }
                    }
                    catch (IOException ioe)
                    {
                        Console.WriteLine("OnChanged: Caught Exception reading file [{0}]", ioe.ToString());
                    }
                    break;
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", filePath, ex.Message));
                }
                Thread.Sleep(500);
            }

            return Data;
        }
        /// <summary>
        /// Serialize EPC_Config_Data object and write to file
        /// </summary>
        public void SaveData(EPC_Config_Data obj, string filePath)
        {
            while (true)
            {
                try
                {
                    try
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(EPC_Config_Data));
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            mySerializer.Serialize(fs, obj);
                        }
                    }
                    catch (IOException ioe)
                    {
                        Console.WriteLine("OnChanged: Caught Exception writing to file [{0}]", ioe.ToString());
                    }
                    break;
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                Thread.Sleep(500);
            }
        }
        /// <summary> Ping central server where EPC source files is stored
        /// </summary>
        public bool PingHost()
        {
            ServerIsAlive = PingIp(Properties.Settings.Default.ServerIP, 50);

            return ServerIsAlive;
        }
        #endregion

        #region Helper Methods
        private void IncrementVersion()
        {
            int version = int.Parse(Data.Version);
            int increment = version + 1;
            Data.Version = increment.ToString();
        }
        /// <summary>
        /// Compare the versions of local xml and central xml files, return true if local xml is up-to-date
        /// </summary>
        public bool? CompareVersion(string sourcePath, string targetPath, bool accessible)
        {
            if (accessible)
            {
                int sourceVersion = GetVersion(sourcePath);
                int targetVersion = GetVersion(targetPath);
                if (targetVersion == -1) return false; // try to update target - target file corrupt or not accessible
                else if (sourceVersion == -1) return true; // dont update target - source file corrupt or not accessible
                else if (sourceVersion == targetVersion) return true; //no update needed
                else if (sourceVersion > targetVersion)
                {
                    //local file shall be updated
                    return false;
                }
                else
                {
                    //local is newer version than source, 
                    return null;
                }
            }
            else
            {
                // source not accessible, dont update
                return true;
            }
        }
        /// <summary>
        /// Get the version value from an epc config xml file
        /// </summary>
        private int GetVersion(string filePath)
        {
            string version = "";
            int count = 0;
            while (true && count <= 30) //stop trying after 3 sec - return -1
            {
                try
                {
                    try
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            try
                            {
                                using (XmlReader xmlRdr = new XmlTextReader(fs))
                                    version = XDocument.Load(xmlRdr).Element("EPC").Element("Version").Value;
                            }
                            catch (XmlException xe)
                            {
                                Console.WriteLine("OnChanged: Caught Exception loading xml [{0}]", xe.ToString());
                            }
                        }
                    }
                    catch (IOException ioe)
                    {
                        Console.WriteLine("OnChanged: Caught Exception reading file [{0}]", ioe.ToString());
                    }
                    //To deal with issue where app tries to get version node when another client rewrite file
                    if (!String.IsNullOrEmpty(version))
                    {
                        break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", filePath, ex.Message));
                }
                Thread.Sleep(100);
                count++;
            }

            if (String.IsNullOrEmpty(version))
            {
                version = "-1"; 
            }

            return int.Parse(version);
        }

        public bool CheckIfLocalExists(string localPath)
        {
            return File.Exists(localPath);
        }

        #endregion
    }
}
