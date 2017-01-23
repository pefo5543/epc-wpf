using Epc.Data.Models.ActionModels;
using System.ComponentModel;

namespace Epc.Data.Models
{
    public class Script : BaseModel, INotifyPropertyChanged
    {
        private DbInfo _dbInfo;
        private Process _process;

        public DbInfo DbInfo
        {
            get
            {
                return _dbInfo;
            }
            set
            {
                SetField(ref _dbInfo, value, "DbInfo");
            }
        }

        public Process Process
        {
            get
            {
                return _process;
            }
            set
            {
                SetField(ref _process, value, "Process");
            }
        }
    }
}
