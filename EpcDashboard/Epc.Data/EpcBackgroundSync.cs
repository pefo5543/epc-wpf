using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Epc.Data
{
    /// <summary> This class is used to recognize changes from background, i.e. updates from centralized config file
    /// <para>Static class which only purpose is to identify differences between Epc_ConfigDataOld & Epc_ConfigDataNew</para>
    /// <para>IF EPC_CONFIGDATA IMPLEMENTATION CHANGES - METHODS IN THIS CLASS MUST BE UPDATED AS WELL, in order to recognize all possible updates</para>
    /// </summary>
    public static class EpcBackgroundSync
    {
        #region Standard Brute-Force Update

        #region Customers 
        public static bool CompareCustomers(AsyncObservableCollection<Customer> oldCust, AsyncObservableCollection<Customer> newCust)
        {
            bool isChanged = false;
            //bool removeOldCheck = false;

            foreach (Customer newC in newCust)
            {
                Customer old = (from o in oldCust
                                where o.Name == newC.Name
                                select o).FirstOrDefault();

                if (old != null) //Existing customer - check if it has been edited
                {
                    //Check if something has changed in current Customer´s Sites list
                    isChanged = CompareSites(old, newC);

                    if (!isChanged)
                    {
                        //Site list has not been changed
                        //check if something has changed in customer´s non-list properties
                        if (!CustomerEquals(old, newC))
                        {
                            //changed
                            oldCust.Remove(old);
                            oldCust.Add(newC);
                            isChanged = true;
                            break;
                        }
                        else if (EBMSCheck(old.EBMS, newC.EBMS))
                        {
                            old.EBMS = UpdateEBMS(old.EBMS, newC.EBMS);
                            //old.EBMS = newC.EBMS;
                            isChanged = true;
                            break;
                        }
                    }
                    else
                    {
                        //something was changed in sites
                        break;
                    }
                }
                else //New customer has been added
                {
                    oldCust.Add(newC);
                    isChanged = true;
                    //removeOldCheck = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<Customer> removeList = new List<Customer>();
                //Check for outdated customers
                foreach (Customer oc in oldCust)
                {
                    Customer n = (from nc in newCust
                                  where nc.Name == oc.Name
                                  select nc).FirstOrDefault();
                    if (n == null)
                    {
                        removeList.Add(oc);
                        isChanged = true;
                        break;
                    }
                }
                foreach (Customer c in removeList)
                {

                    oldCust.Remove(c);
                    break;
                }

            }

            return isChanged;
        }

        internal static bool CustomerEquals(Customer oldCustomer, Customer newCustomer)
        {
            bool result = true;
            if (oldCustomer.Name != newCustomer.Name)
            {
                result = false;
            }
            else if (oldCustomer.CustomerIcon != newCustomer.CustomerIcon)
            {
                result = false;
            }
            else if (oldCustomer.Color != newCustomer.Color)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region Sites
        private static bool CompareSites(Customer old, Customer newC)
        {
            bool isChanged = false;
            //bool removeOldCheck = false;
            foreach (Site newSite in newC.Sites)
            {
                Site oldSite = (from o in old.Sites
                                where o.Name == newSite.Name
                                select o).FirstOrDefault();
                if (oldSite != null) //Existing site - check if it has been edited
                {
                    //Check if something has changed in current Site´s processes list
                    isChanged = CompareProcesses(oldSite, newSite);

                    //Actions lists evaluation
                    if (!isChanged)
                    {
                        isChanged = CompareEX3Actions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = CompareFolderActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = CompareRDPActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = CompareVNCActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = CompareExeActions(oldSite, newSite);
                    }

                    if (!isChanged)
                    {
                        //Processes list adn actions lists has not been changed
                        //check if something has changed in site´s non-list properties
                        if (!SitesEquals(oldSite, newSite))
                        {
                            old.Sites.Remove(oldSite);
                            old.Sites.Add(newSite);
                            isChanged = true;
                            break;
                        }
                        else if (EBMSCheck(oldSite.EBMS, newSite.EBMS))
                        {
                            oldSite.EBMS = UpdateEBMS(oldSite.EBMS, newSite.EBMS);
                            isChanged = true;
                            break;
                        }
                    }
                    else
                    {
                        //something was changed in processes
                        break;
                    }
                }
                else
                {
                    old.Sites.Add(newSite);
                    isChanged = true;
                    //removeOldCheck = true;
                    break;
                }
            }
            if (!isChanged) // check for deleted items
            {
                List<Site> removeList = new List<Site>();
                foreach (Site oldSite in old.Sites)
                {
                    Site newSite = (from n in newC.Sites
                                    where n.Name == oldSite.Name
                                    select n).FirstOrDefault();
                    if (newSite == null)
                    {
                        removeList.Add(oldSite);
                        isChanged = true;
                        break;
                    }
                }
                foreach (Site s in removeList)
                {

                    old.Sites.Remove(s);
                    break;
                }
            }
            return isChanged;
        }

       internal static EBMS UpdateEBMS(EBMS target, EBMS source)
        {
            target.ActionName = source.ActionName;
            target.ActionType = source.ActionType;
            target.DbName = source.DbName;
            target.DbPassword = source.DbPassword;
            target.DbUserName = source.DbUserName;
            target.Homepage = source.Homepage;
            target.IpAdress = source.IpAdress;

            return target;
        }

        internal static bool SitesEquals(Site oldSite, Site newSite)
        {
            bool result = true;
            if (oldSite.Name != newSite.Name)
            {
                result = false;
            }
            else if (oldSite.SiteIcon != newSite.SiteIcon)
            {
                result = false;
            }
            else if (oldSite.IpAdress != newSite.IpAdress)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region Processes 
        private static bool CompareProcesses(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            //bool removeOldCheck = false;
            foreach (Process newProcess in newSite.Processes)
            {
                Process oldProcess = (from op in oldSite.Processes
                                      where op.Name == newProcess.Name
                                      select op).FirstOrDefault();
                if (oldProcess != null) //Existing process - check if it has been edited
                {
                    //Compare properties
                    if (!ProcessEquals(oldProcess, newProcess))
                    {
                        //something has been edited in processes
                        oldSite.Processes.Remove(oldProcess);
                        oldSite.Processes.Add(newProcess);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New process - add
                    oldSite.Processes.Add(newProcess);
                    isChanged = true;
                    //removeOldCheck = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<Process> removeList = new List<Process>();
                foreach (Process oldProcess in oldSite.Processes)
                {
                    Process newProcess = (from n in newSite.Processes
                                          where n.Name == oldProcess.Name
                                          select n).FirstOrDefault();
                    if (newProcess == null)
                    {
                        removeList.Add(oldProcess);
                        isChanged = true;
                        break;
                    }
                }
                foreach (Process p in removeList)
                {

                    oldSite.Processes.Remove(p);
                    break;
                }
            }

            return isChanged;

        }

        internal static bool ProcessEquals(Process oldProcess, Process newProcess)
        {
            bool result = true;
            if (oldProcess.Argument != newProcess.Argument)
            {
                result = false;
            }
            else if (oldProcess.Ping != newProcess.Ping)
            {
                result = false;
            }
            else if (oldProcess.Name != newProcess.Name)
            {
                result = false;
            }
            else if (oldProcess.Program != newProcess.Program)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region EX3Actions 
        private static bool CompareEX3Actions(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            //bool removeOldCheck = false;
            foreach (EX3 newX3 in newSite.X3Actions)
            {
                EX3 oldX3 = (from ox in oldSite.X3Actions
                             where ox.ActionName == newX3.ActionName
                             select ox).FirstOrDefault();
                if (oldX3 != null) //Existing x3 - check if it has been edited
                {
                    //Compare properties
                    if (Ex3Check(oldX3, newX3))
                    {
                        //something has been edited in x3 action
                        oldSite.X3Actions.Remove(oldX3);
                        oldSite.X3Actions.Add(newX3);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New x3action - add
                    oldSite.X3Actions.Add(newX3);
                    isChanged = true;
                    //removeOldCheck = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<EX3> removeList = new List<EX3>();
                foreach (EX3 oldX3 in oldSite.X3Actions)
                {
                    EX3 newX3 = (from n in newSite.X3Actions
                                 where n.ActionName == oldX3.ActionName
                                 select n).FirstOrDefault();
                    if (newX3 == null)
                    {
                        removeList.Add(oldX3);
                        isChanged = true;
                        break;
                    }
                }
                foreach (EX3 x in removeList)
                {

                    oldSite.X3Actions.Remove(x);
                    break;
                }
            }

            return isChanged;
        }

        public static bool Ex3Check(EX3 ex3Old, EX3 ex3New)
        {
            bool isChanged = BaseActionCheck(ex3Old, ex3New);

            if (!isChanged)
            {
                isChanged = DbInfoCheck((DbInfo)ex3Old, (DbInfo)ex3New);
            }

            return isChanged;
        }

        #endregion

        #region FolderActions 
        private static bool CompareFolderActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            //bool removeOldCheck = false;
            foreach (OpenFolder newFolderAction in newSite.FolderActions)
            {
                OpenFolder oldFolderAction = (from oe in oldSite.FolderActions
                                              where oe.ActionName == newFolderAction.ActionName
                                              select oe).FirstOrDefault();
                if (oldFolderAction != null) //Existing empiri - check if it has been edited
                {
                    //Compare properties
                    if (OpenFolderCheck(oldFolderAction, newFolderAction))
                    {
                        //something has been edited in open folder action
                        oldSite.FolderActions.Remove(oldFolderAction);
                        oldSite.FolderActions.Add(newFolderAction);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New x3action - add
                    oldSite.FolderActions.Add(newFolderAction);
                    isChanged = true;
                    //removeOldCheck = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<OpenFolder> removeList = new List<OpenFolder>();
                foreach (OpenFolder oldFolderAction in oldSite.FolderActions)
                {
                    OpenFolder newFolderAction = (from n in newSite.FolderActions
                                                  where n.ActionName == oldFolderAction.ActionName
                                                  select n).FirstOrDefault();
                    if (newFolderAction == null)
                    {
                        removeList.Add(oldFolderAction);
                        isChanged = true;
                        break;
                    }
                }
                foreach (OpenFolder x in removeList)
                {

                    oldSite.FolderActions.Remove(x);
                    break;
                }
            }

            return isChanged;
        }

        public static bool OpenFolderCheck(OpenFolder oldS, OpenFolder newS)
        {
            bool isChanged = CredentialBaseActionCheck(oldS, newS);
            if (!isChanged)
            {
                //Openfolder specific property checks here
            }

            return isChanged;
        }

        #endregion

        #region RDPActions 
        private static bool CompareRDPActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            foreach (RDP newRDPAction in newSite.RDPActions)
            {
                RDP oldRDPAction = (from oe in oldSite.RDPActions
                                    where oe.ActionName == newRDPAction.ActionName
                                    select oe).FirstOrDefault();
                if (oldRDPAction != null) //Existing - check if it has been edited
                {
                    //Compare properties
                    if (RDPCheck(oldRDPAction, newRDPAction))
                    {
                        //something has been edited in open folder action
                        oldSite.RDPActions.Remove(oldRDPAction);
                        oldSite.RDPActions.Add(newRDPAction);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New - add
                    oldSite.RDPActions.Add(newRDPAction);
                    isChanged = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<RDP> removeList = new List<RDP>();
                foreach (RDP oldRDPAction in oldSite.RDPActions)
                {
                    RDP newRDPAction = (from n in newSite.RDPActions
                                        where n.ActionName == oldRDPAction.ActionName
                                        select n).FirstOrDefault();
                    if (newRDPAction == null)
                    {
                        removeList.Add(oldRDPAction);
                        isChanged = true;
                        break;
                    }
                }
                foreach (RDP x in removeList)
                {

                    oldSite.RDPActions.Remove(x);
                    break;
                }
            }

            return isChanged;
        }

        public static bool RDPCheck(RDP oldR, RDP newR)
        {
            bool isChanged = CredentialBaseActionCheck(oldR, newR);
            if (!isChanged)
            {
                if (oldR.Port != newR.Port)
                {
                    isChanged = true;
                }
                else if (oldR.Domain != newR.Domain)
                {
                    isChanged = true;
                }
            }

            return isChanged;
        }

        #endregion

        #region VNC actions
        private static bool CompareVNCActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            foreach (VNC newVNCAction in newSite.VNCActions)
            {
                VNC oldVNCAction = (from oe in oldSite.VNCActions
                                    where oe.ActionName == newVNCAction.ActionName
                                    select oe).FirstOrDefault();
                if (oldVNCAction != null) //Existing - check if it has been edited
                {
                    //Compare properties
                    if (VNCCheck(oldVNCAction, newVNCAction))
                    {
                        //something has been edited in open folder action
                        oldSite.VNCActions.Remove(oldVNCAction);
                        oldSite.VNCActions.Add(newVNCAction);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New - add
                    oldSite.VNCActions.Add(newVNCAction);
                    isChanged = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<VNC> removeList = new List<VNC>();
                foreach (VNC oldVNCAction in oldSite.VNCActions)
                {
                    VNC newVNCAction = (from n in newSite.VNCActions
                                        where n.ActionName == oldVNCAction.ActionName
                                        select n).FirstOrDefault();
                    if (newVNCAction == null)
                    {
                        removeList.Add(oldVNCAction);
                        isChanged = true;
                        break;
                    }
                }
                foreach (VNC x in removeList)
                {

                    oldSite.VNCActions.Remove(x);
                    break;
                }
            }

            return isChanged;
        }

        public static bool VNCCheck(VNC oldV, VNC newV)
        {
            bool isChanged = BaseActionCheck(oldV, newV);
            if (!isChanged)
            {
                if (oldV.IpAdress != newV.IpAdress)
                {
                    isChanged = true;
                }
                else if (oldV.VNCPassword != newV.VNCPassword)
                {
                    isChanged = true;
                }
            }


            return isChanged;
        }
        #endregion

        #region Exe actions
        private static bool CompareExeActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;
            foreach (Exe newExeAction in newSite.ExeActions)
            {
                Exe oldExeAction = (from oe in oldSite.ExeActions
                                    where oe.ActionName == newExeAction.ActionName
                                    select oe).FirstOrDefault();
                if (oldExeAction != null) //Existing - check if it has been edited
                {
                    //Compare properties
                    if (ExeCheck(oldExeAction, newExeAction))
                    {
                        //something has been edited in open folder action
                        oldSite.ExeActions.Remove(oldExeAction);
                        oldSite.ExeActions.Add(newExeAction);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    //New - add
                    oldSite.ExeActions.Add(newExeAction);
                    isChanged = true;
                    break;
                }
            }
            if (!isChanged)
            {
                List<Exe> removeList = new List<Exe>();
                foreach (Exe oldExeAction in oldSite.ExeActions)
                {
                    Exe newExeAction = (from n in newSite.ExeActions
                                        where n.ActionName == oldExeAction.ActionName
                                        select n).FirstOrDefault();
                    if (newExeAction == null)
                    {
                        removeList.Add(oldExeAction);
                        isChanged = true;
                        break;
                    }
                }
                foreach (Exe x in removeList)
                {

                    oldSite.ExeActions.Remove(x);
                    break;
                }
            }

            return isChanged;
        }

        public static bool ExeCheck(Exe oldE, Exe newE)
        {
            bool isChanged = BaseActionCheck(oldE, newE);
            if (!isChanged)
            {
                if (oldE.FileName != newE.FileName)
                {
                    isChanged = true;
                }
                else if (oldE.Arguments != newE.Arguments)
                {
                    isChanged = true;
                }
            }


            return isChanged;
        }
        #endregion
        #region Generic

        private static bool BaseActionCheck(BaseAction oldObj, BaseAction newObj)
        {
            bool isChanged = false;
            if (oldObj.ActionName != newObj.ActionName)
            {
                isChanged = true;
            }
            else if (oldObj.ActionName != newObj.ActionName)
            {
                isChanged = true;
            }
            return isChanged;
        }

        private static bool CredentialBaseActionCheck(CredentialsBase oldObj, CredentialsBase newObj)
        {
            bool isChanged = BaseActionCheck(oldObj, newObj);
            if (!isChanged)
            {
                if (oldObj.UserName != newObj.UserName)
                {
                    isChanged = true;
                }
                else if (oldObj.Password != newObj.Password)
                {
                    isChanged = true;
                }
                else if (oldObj.IpAdress != newObj.IpAdress)
                {
                    isChanged = true;
                }
            }

            return isChanged;
        }



        private static bool DbInfoCheck(DbInfo dbOldInfo, DbInfo dbNewInfo)
        {
            bool isChanged = false;
            if (dbOldInfo.IpAdress != dbNewInfo.IpAdress)
            {
                isChanged = true;
            }
            else if (dbOldInfo.DbName != dbNewInfo.DbName)
            {
                isChanged = true;
            }
            else if (dbOldInfo.DbUserName != dbNewInfo.DbUserName)
            {
                isChanged = true;
            }
            else if (dbOldInfo.DbPassword != dbNewInfo.DbPassword)
            {
                isChanged = true;
            }

            return isChanged;
        }

        public static bool EBMSCheck(EBMS oldE, EBMS newE)
        {
            bool isChanged = BaseActionCheck(oldE, newE);
            if (!isChanged)
            {
                if (oldE.Homepage != newE.Homepage)
                {
                    isChanged = true;
                }
                else
                {
                    isChanged = DbInfoCheck((DbInfo)oldE, (DbInfo)newE);
                }
            }

            return isChanged;
        }

        #endregion

        #endregion
    }
}
