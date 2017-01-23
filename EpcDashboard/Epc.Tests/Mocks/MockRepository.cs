using Epc.Data.Models;
using System;
using System.Windows.Media;
using System.IO;
using Epc.Data.Models.ActionModels;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.Services;

namespace Epc.Tests.Mocks
{
    class MockRepository : IMainRepository
    {
        public MockRepository()
        {
        }

        public EPC_Config_Data Data
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ServerIsAlive
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SourceFile
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string TargetFile
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public FileSystemWatcher Watcher
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        FileSystemWatcher IMainRepository.Watcher
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddEditCustomer(Customer cust, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditEmpiriAction(Site site, OpenFolder empiriAction, bool EditMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditExeAction(Site site, Exe _editingExeAction, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditOpenFolderAction(Site site, OpenFolder empiriAction, bool EditMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditProcess(Site currentSite, Process _editingProcess, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditRDPAction(Site site, RDP _editingRDPAction, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditSite(Customer cust, Site site, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditVNCAction(Site site, VNC _editingVNCAction, bool editMode)
        {
            throw new NotImplementedException();
        }

        public void AddEditX3Action(Site site, EX3 x3Action, bool EditMode)
        {
            throw new NotImplementedException();
        }

        public void DeleteAction(Site site, BaseAction action)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(Customer cust)
        {
            throw new NotImplementedException();
        }

        public void DeleteEBMS(object parent)
        {
            throw new NotImplementedException();
        }

        public void DeleteProcess(Site site, Process process)
        {
            throw new NotImplementedException();
        }

        public void DeleteSite(Customer cust, Site site)
        {
            throw new NotImplementedException();
        }

        public void GenericSave()
        {
            throw new NotImplementedException();
        }

        public object GetData()
        {
            throw new NotImplementedException();
        }

        public object GetData(string source, string target, string file)
        {
            EPC_Config_Data result = new EPC_Config_Data();
            result.Customers.Add(new Customer {
                Name = "test"
            });

            return result;
        }

        public EPC_Config_Data MergeData(EPC_Config_Data data)
        {
            throw new NotImplementedException();
        }

        public MergeLocalSourceService MergeData(EPC_Config_Data data, MergeLocalSourceService service)
        {
            throw new NotImplementedException();
        }

        public bool PingHost()
        {
            throw new NotImplementedException();
        }

        public bool PingIp(string ipAdress, int timeOut)
        {
            throw new NotImplementedException();
        }

        public void SaveData(EPC_Config_Data dataToSave, string filepath)
        {
            throw new NotImplementedException();
        }

        public string SaveExeOnServerAndLocal(FileInfo selectedFile)
        {
            throw new NotImplementedException();
        }

        public string SaveImageOnServerAndLocal(ImageSource icon, string sourcePath)
        {
            throw new NotImplementedException();
        }

        public string SaveImageOnServerAndLocal(ImageSource icon, string sourcePath, string fileName)
        {
            throw new NotImplementedException();
        }

        public void UpdateData(EPC_Config_Data data)
        {
            throw new NotImplementedException();
        }
    }
}
