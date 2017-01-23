using System;
using EpcDashboard.Settings;
using EpcDashboard.Services.Interfaces;

namespace EpcDashboard.Services
{
    /// <summary>
    ///Settingsrepository should only be instantiated once - holds a single setting object which is accessed by multiple viewmodels
    /// </summary>
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        private void CopyToSettings(Setting settings)
        {
            UserSettings.Default.UserId = settings.UserId;
            UserSettings.Default.FirstName = settings.FirstName;
            UserSettings.Default.Lastname = settings.LastName;
            UserSettings.Default.WinStart = settings.WinStart;
            UserSettings.Default.SourcePath = settings.SourcePath;
        }

        public Setting UpdateSetting(Setting settings)
        {
            if (TargetPath != settings.SourcePath)
            {
                TargetPath = settings.SourcePath;
                //Copy central config file to new local directory
                CopyFile(TargetPath, TargetFile, SourceFile, true);
            }
            CopyToSettings(settings);
            UserSettings.Default.Save();

            return settings;
        }

        private void CopyServerFileToLocal()
        {
            throw new NotImplementedException();
        }

        private Setting PopulateSetting()
        {
            Setting usersettings = new Setting(
                UserSettings.Default.UserId,
                UserSettings.Default.FirstName,
                UserSettings.Default.Lastname,
                UserSettings.Default.WinStart,
                UserSettings.Default.SourcePath
                );

            return usersettings;
        }

        public Setting GetSettings()
        {
            Setting usersettings = new Setting(
                UserSettings.Default.UserId,
                UserSettings.Default.FirstName,
                UserSettings.Default.Lastname,
                UserSettings.Default.WinStart,
                UserSettings.Default.SourcePath
                );

            return usersettings;
        }

        public void CleanupSourcePath(string sourcePath)
        {
            CleanupDirectoryService cds = new CleanupDirectoryService();
            cds.CleanupSourcePath(sourcePath);
        }
    }
}
