using EpcDashboard.Services.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace EpcDashboard.Services
{
    public class BaseRepository : BaseService, IBaseRepository
    {
        private string _fileName;
        private string _sourcePath;
        private string _targetPath;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
            }
        }
        public string TargetPath
        {
            get
            {
                return _targetPath;
            }
            set
            {
                _targetPath = value;
            }
        }
        //Centralized config file
        public string SourceFile
        {
            get
            {
                return Path.Combine(SourcePath, FileName);
            }
        }
        //Local config file
        public string TargetFile
        {
            get
            {
                return Path.Combine(TargetPath, FileName);
            }
        }

        public BaseRepository()
        {
            FileName = Constants.FileName;
            SourcePath = (string)@Properties.Settings.Default.CentralSourcePath;
            TargetPath = @UserSettings.Default.SourcePath;
        }

        public void CopyFile(string targetPath, string targetFile, string sourceFile, bool overwrite = false)
        {
            // Create a new target folder, if necessary.
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            //Copy if overwrite is allowed or file doesnt exist at target
            if ((!overwrite && !File.Exists(targetFile)) || overwrite)
            {
                while (true)
                {
                    try
                    {
                        // Copy sourcefile to new local file.
                        File.Copy(sourceFile, targetFile, overwrite);
                        break;
                    }
                    catch (FileNotFoundException ex)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", targetFile, ex.Message));
                    }
                    catch (IOException ex)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", targetFile, ex.Message));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", targetFile, ex.Message));
                    }
                    Thread.Sleep(500);
                }
            }
        }
    }
}
