using System.Collections.Generic;
using Epc.Data.Models;

namespace EpcDashboard.Services.Interfaces
{
    public interface ICopyService
    {
        List<NameBaseModel> PrepareSiteProcessTable(IList<Customer> customers, string customerName, Site site, bool copySite);
        EPC_Config_Data ExecuteCopy(EPC_Config_Data data, List<NameBaseModel> list, NameBaseModel copy, bool isSite);
    }
}
