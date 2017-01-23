using System;
using System.Diagnostics;
using System.IO;

namespace EpcDashboard.Services.ActionServices
{
    internal class RunProcessBaseService : BaseService
    {
        internal void StartProcess(string filePath, string arguments = null)
        {
            if (File.Exists(filePath))
            {
                Process launchProcess = new Process();
                launchProcess.StartInfo.FileName = filePath;
                if(!String.IsNullOrEmpty(arguments))
                {
                    launchProcess.StartInfo.Arguments = arguments;
                }
                
                launchProcess.Start();
            }
        }
    }
}
