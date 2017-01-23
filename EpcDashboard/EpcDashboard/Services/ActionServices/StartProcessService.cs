

using Epc.Data.Models.ActionModels;
using System.IO;

namespace EpcDashboard.Services.ActionServices
{
    internal class StartProcessService : RunProcessBaseService
    {
        internal void RunExe (Exe ExeInfo)
        {
            string launchExePath = Path.Combine(UserSettings.Default.SourcePath, Constants.ExecutablesFolder, ExeInfo.FileName);
            StartProcess(launchExePath, ExeInfo.Arguments);
        }
    }
}