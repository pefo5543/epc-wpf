namespace EpcDashboard.Services.Interfaces
{
    //Interface forcing Navbar linkable viewmodels to implement MenuName
    public interface IPageViewModel : IPageBaseViewModel
    {
        string MenuName { get; }
    }
}
