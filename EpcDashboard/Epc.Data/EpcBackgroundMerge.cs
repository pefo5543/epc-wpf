using Epc.Data.Models;
using Epc.Data.Models.ActionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epc.Data
{
    public class EpcBackgroundMerge
    {

        #region Merge Brute-Force
        #region Merging Customers
        public static bool MergeCustomers(AsyncObservableCollection<Customer> oldCust, AsyncObservableCollection<Customer> newCust)
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
                    isChanged = MergeSites(old, newC);

                    if (!isChanged)
                    {
                        //Site list has not been changed
                        //check if something has changed in customer´s non-list properties
                        if (!EpcBackgroundSync.CustomerEquals(old, newC))
                        {
                            //changed - Merge in as new customer
                            oldCust.Add(newC);
                            isChanged = true;
                            break;
                        } //else - continue 
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

            return isChanged;
        }
        #endregion

        #region Merging Sites
        private static bool MergeSites(Customer old, Customer newC)
        {
            bool isChanged = false;
            foreach (Site newSite in newC.Sites)
            {
                Site oldSite = (from o in old.Sites
                                where o.Name == newSite.Name
                                select o).FirstOrDefault();
                if (oldSite != null) //Existing site - check if it has been edited
                {
                    //Check if something has changed in current Site´s processes list
                    isChanged = MergeProcesses(oldSite, newSite);

                    //Actions lists evaluation
                    if (!isChanged)
                    {
                        isChanged = MergeEX3Actions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = MergeFolderActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = MergeRDPActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = MergeVNCActions(oldSite, newSite);
                    }
                    if (!isChanged)
                    {
                        isChanged = MergeExeActions(oldSite, newSite);
                    }

                    if (!isChanged)
                    {
                        //Processes list has not been changed
                        //check if something has changed in site´s non-list properties
                        if (!EpcBackgroundSync.SitesEquals(oldSite, newSite))
                        {
                            old.Sites.Add(newSite); //Merge in as new site
                            isChanged = true;
                            break;
                        }
                        else if (EpcBackgroundSync.EBMSCheck(oldSite.EBMS, newSite.EBMS))
                        {
                            oldSite.EBMS = EpcBackgroundSync.UpdateEBMS(oldSite.EBMS, newSite.EBMS);
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
                    break;
                }
            }

            return isChanged;
        }
        #endregion

        #region Merging Processes
        private static bool MergeProcesses(Site oldSite, Site newSite)
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
                    if (!EpcBackgroundSync.ProcessEquals(oldProcess, newProcess))
                    {
                        //something has been edited in processes
                        oldSite.Processes.Add(newProcess); //Merge in as a new process
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

            return isChanged;

        }
        #endregion

        #region Merging actions
        private static bool MergeExeActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;

            foreach (Exe newExe in newSite.ExeActions)
            {
                Exe oldExe = (from ox in oldSite.ExeActions
                              where ox.ActionName == newExe.ActionName
                              select ox).FirstOrDefault();
                if (oldExe != null) //Existing action - check if it has been edited
                {
                    //Compare properties
                    if (EpcBackgroundSync.ExeCheck(oldExe, newExe))
                    {
                        //something has been changed, add as new action
                        oldSite.ExeActions.Add(newExe);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    oldSite.ExeActions.Add(newExe);
                    isChanged = true;
                    break;
                }
            }

            return isChanged;
        }

        private static bool MergeVNCActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;

            foreach (VNC newVNC in newSite.VNCActions)
            {
                VNC oldVNC = (from ox in oldSite.VNCActions
                              where ox.ActionName == newVNC.ActionName
                              select ox).FirstOrDefault();
                if (oldVNC != null) //Existing action - check if it has been edited
                {
                    //Compare properties
                    if (EpcBackgroundSync.VNCCheck(oldVNC, newVNC))
                    {
                        //something has been changed, add as new action
                        oldSite.VNCActions.Add(newVNC);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    oldSite.VNCActions.Add(newVNC);
                    isChanged = true;
                    break;
                }
            }

            return isChanged;
        }

        private static bool MergeRDPActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;

            foreach (RDP newRDP in newSite.RDPActions)
            {
                RDP oldRDP = (from ox in oldSite.RDPActions
                              where ox.ActionName == newRDP.ActionName
                              select ox).FirstOrDefault();
                if (oldRDP != null) //Existing action - check if it has been edited
                {
                    //Compare properties
                    if (EpcBackgroundSync.RDPCheck(oldRDP, newRDP))
                    {
                        //something has been changed, add as new action
                        oldSite.RDPActions.Add(newRDP);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    oldSite.RDPActions.Add(newRDP);
                    isChanged = true;
                    break;
                }
            }

            return isChanged;
        }

        private static bool MergeFolderActions(Site oldSite, Site newSite)
        {
            bool isChanged = false;

            foreach (OpenFolder newOpenFolder in newSite.FolderActions)
            {
                OpenFolder oldOpenFolder = (from ox in oldSite.FolderActions
                                            where ox.ActionName == newOpenFolder.ActionName
                                            select ox).FirstOrDefault();
                if (oldOpenFolder != null) //Existing action - check if it has been edited
                {
                    //Compare properties
                    if (EpcBackgroundSync.OpenFolderCheck(oldOpenFolder, newOpenFolder))
                    {
                        //something has been changed, add as new action
                        oldSite.FolderActions.Add(newOpenFolder);
                        isChanged = true;
                        break;
                    }
                }
                else
                {
                    oldSite.FolderActions.Add(newOpenFolder);
                    isChanged = true;
                    break;
                }
            }

            return isChanged;
        }

        private static bool MergeEX3Actions(Site oldSite, Site newSite)
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
                    if (EpcBackgroundSync.Ex3Check(oldX3, newX3))
                    {
                        //something has been changed, add new x3 action
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

            return isChanged;
        }
        #endregion

        #endregion
    }
}
