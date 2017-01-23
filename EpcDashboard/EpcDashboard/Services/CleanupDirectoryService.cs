using Epc.Data.Models;
using System;
using System.ComponentModel;
using System.Linq;
using Epc.Data;
using System.IO;

namespace EpcDashboard.Services
{
    /// <summary>
    /// Serviceclass for cleanup of obsolete EPC related files and directories
    /// </summary>
    public class CleanupDirectoryService
    {
        private string _targetPath;
        private readonly BackgroundWorker x3worker = new BackgroundWorker();
        private readonly BackgroundWorker rootWorker = new BackgroundWorker();

        public CleanupDirectoryService()
        {
            x3worker.DoWork += worker_DoWork;
            x3worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            rootWorker.DoWork += rootWorker_DoWork;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            EPC_Config_Data data = (EPC_Config_Data)e.Argument;
            CleanupX3FolderInBackground(data);
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
        }

        private void rootWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DeleteRootFolderInBackground();
        }

        internal void CleanupSourcePath(string path)
        {
            if (!String.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                _targetPath = path;
                //delete root in background
                rootWorker.RunWorkerAsync();
            }
        }

        internal void CleanupX3(string path, EPC_Config_Data data)
        {
            if (!String.IsNullOrEmpty(path))
            {
                _targetPath = Path.Combine(path, Constants.EX3Folder);
                if (Directory.Exists(_targetPath))
                {
                    x3worker.RunWorkerAsync(data);
                }
            }
        }

        private void CleanupX3FolderInBackground(EPC_Config_Data data)
        {
            DirectoryInfo d = new DirectoryInfo(_targetPath);
            //Get list of customerfolders
            DirectoryInfo[] directories = d.GetDirectories();
            CleanupX3CustomerFolder(directories, data.Customers);
        }

        private void CleanupX3CustomerFolder(DirectoryInfo[] directories, AsyncObservableCollection<Customer> customers)
        {
            foreach (DirectoryInfo d in directories)
            {
                Customer match =
                customers.FirstOrDefault(x => x.Name.Trim() == d.Name.Trim());
                if (match != null)
                {
                    //match - continue check site folders
                    //Get list of sitefolders
                    DirectoryInfo[] subDirectories = d.GetDirectories();
                    CleanupX3SiteFolder(subDirectories, match);
                }
                else
                {
                    //obsolete directory - delete
                    try
                    {
                        d.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void CleanupX3SiteFolder(DirectoryInfo[] directories, Customer customer)
        {
            foreach (DirectoryInfo d in directories)
            {
                Site match =
                customer.Sites.FirstOrDefault(x => x.Name.Trim() == d.Name.Trim());
                if (match == null)
                {
                    //obsolete directory - delete
                    try
                    {
                        d.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void DeleteRootFolderInBackground()
        {
            DirectoryInfo d = new DirectoryInfo(_targetPath);
            try
            {
                d.Delete(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
