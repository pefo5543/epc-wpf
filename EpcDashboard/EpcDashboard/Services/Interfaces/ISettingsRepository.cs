using EpcDashboard.Settings;

namespace EpcDashboard.Services.Interfaces
{
    public interface ISettingsRepository
    {
        Setting UpdateSetting(Setting settings);

        Setting GetSettings();
        void CleanupSourcePath(string sourcePath);
    }
}
