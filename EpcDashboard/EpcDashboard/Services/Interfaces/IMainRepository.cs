using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using System.IO;
using System.Windows.Media;

namespace EpcDashboard.Services.Interfaces
{
    public interface IMainRepository : IBaseRepository
    {
        EPC_Config_Data Data { get; set; }

        bool ServerIsAlive { get; set; }

        FileSystemWatcher Watcher { get;  set; }

        object GetData();

        void UpdateData(EPC_Config_Data data);

        MergeLocalSourceService MergeData(EPC_Config_Data data, MergeLocalSourceService service);

        bool PingHost();

        void SaveData(EPC_Config_Data dataToSave, string filepath);

        void AddEditCustomer(Customer cust, bool editMode);

        void AddEditSite(Customer cust, Site site, bool editMode);

        string SaveImageOnServerAndLocal(ImageSource icon, string sourcePath, string fileName);

        void AddEditProcess(Site currentSite, Process _editingProcess, bool editMode);

        void DeleteProcess(Site site, Process process);

        void DeleteSite(Customer cust, Site site);

        void DeleteCustomer(Customer cust);

        void GenericSave();

        void DeleteEBMS(object parent);

        void AddEditX3Action(Site site, EX3 x3Action, bool EditMode);

        void AddEditOpenFolderAction(Site site, OpenFolder empiriAction, bool EditMode);

        void DeleteAction(Site site, BaseAction action);

        void AddEditRDPAction(Site site, RDP _editingRDPAction, bool editMode);
        void AddEditVNCAction(Site site, VNC _editingVNCAction, bool editMode);
        void AddEditExeAction(Site site, Exe _editingExeAction, bool editMode);
        string SaveExeOnServerAndLocal(FileInfo selectedFile);
    }
}
