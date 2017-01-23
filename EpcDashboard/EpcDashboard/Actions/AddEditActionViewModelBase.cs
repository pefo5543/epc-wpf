using Epc.Data.Models;
using EpcDashboard.Actions.ShorthandActions;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.ViewModelBases;
using System;

namespace EpcDashboard.Actions
{
    public class AddEditActionViewModelBase : AddEditViewModelBase, IPageBaseViewModel
    {
        protected Site _site;

        #region constructors
        public AddEditActionViewModelBase()
        {
        }
        #endregion

        #region properties/commands

        public Site Site
        {
            get { return _site; }
            set { SetProperty(ref _site, value); }
        }
        #endregion

        #region methods
        internal void SetDefaultValues(SimpleEditableServerBase serverInfo, string actionName = null, string ipAdress = null)
        {
            if(!String.IsNullOrEmpty(actionName))
            {
                serverInfo.ActionName = actionName;
            }
            if (!String.IsNullOrEmpty(ipAdress))
            {
                serverInfo.IpAdress = ipAdress;
            }
        }
        #endregion
    }
}
