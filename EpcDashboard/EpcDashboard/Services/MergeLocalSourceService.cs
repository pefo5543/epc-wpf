using Epc.Data;
using Epc.Data.Models;

namespace EpcDashboard.Services
{
    public class MergeLocalSourceService
    {
        public bool LocalIsChanged { get; set; }
        public bool SourceIsChanged { get; set; }

        public MergeLocalSourceService()
        {
            LocalIsChanged = false;
            SourceIsChanged = false;
        }
        /// <summary>
        /// Merges xml data, source version and sourcepath set to targets
        /// First Target is updated with changes from source
        /// Second Source is updated with changes from target
        /// <para></para>
        /// </summary>
        public EPC_Config_Data MergeSourceAndTarget(EPC_Config_Data xmlTarget, EPC_Config_Data xmlSource)
        {
            xmlTarget.Version = xmlSource.Version;
            xmlTarget.ServerPath = xmlSource.ServerPath;

            //Merge changes in source into local
            xmlTarget.Customers = Merge(xmlTarget.Customers, xmlSource.Customers, true);
            //Merge changes in local into source
            xmlSource.Customers = Merge(xmlSource.Customers, xmlTarget.Customers, false);

            return xmlSource;
        }

        public AsyncObservableCollection<Customer> Merge(AsyncObservableCollection<Customer> targetList, AsyncObservableCollection<Customer> sourceList, bool isLocal)
        {
            bool isChanged = true;

            while (isChanged)
            {
                isChanged = EpcBackgroundMerge.MergeCustomers(targetList, sourceList);
                if (isLocal)
                {
                    //Set localChanged flag to true if local has been updated
                    if (isChanged && !LocalIsChanged)
                    {
                        LocalIsChanged = true;
                    }
                }
                else
                {
                    // Set sourceChanged flag to true if source has been updated
                    if (isChanged && !SourceIsChanged)
                    {
                        SourceIsChanged = true;
                    }
                }
            }

            return targetList;
        }
    }
}
