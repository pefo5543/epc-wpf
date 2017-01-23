namespace EpcDashboard.Services.Interfaces
{
    public interface IBaseRepository
    {
        string TargetFile { get; }
        string SourceFile { get; }
        bool PingIp(string ipAdress, int timeOut = 100);
    }
}
