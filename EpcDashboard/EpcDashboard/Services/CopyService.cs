using Epc.Data.Models;
using EpcDashboard.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EpcDashboard.Services
{
    public class CopyService : ICopyService
    {
        public EPC_Config_Data ExecuteCopy(EPC_Config_Data data, List<NameBaseModel> selectList, NameBaseModel copy, bool isSite)
        {
            if (isSite)
            {
                List<NameBaseModel> selected = (from cust in selectList
                                                where cust.IsSelected == true
                                                select (NameBaseModel)cust).ToList();
                foreach (NameBaseModel n in selected)
                {
                    Customer customer = (from cust in data.Customers
                                         where cust.Name == n.Name
                                         select cust).FirstOrDefault();

                    customer.Sites.Add((Site)copy);
                }
            }
            else
            {
                //Copy a process mode
                //selectList consists of Sites in this context
                foreach (Customer c in data.Customers)
                {
                    List<NameBaseModel> selected = (from si in selectList
                                                    where si.IsSelected == true && ((Site)si).CustomerName == c.Name
                                                    select (NameBaseModel)si).ToList();

                    foreach (NameBaseModel n in selected)
                    {
                        Site site = (from s in c.Sites
                                     where s.Name == n.Name
                                     select s).FirstOrDefault();

                        site.Processes.Add((Process)copy);
                    }
                }
            }

            return data;
        }

        public List<NameBaseModel> PrepareSiteProcessTable(IList<Customer> customers, string customerName, Site site, bool CopySite)
        {
            List<NameBaseModel> nameList = new List<NameBaseModel>();

            if (CopySite)
            {
                //Get a list of all customernames, except the chosen sites parent customer
                nameList = (from cust in customers
                            where cust.Name != customerName
                            select (NameBaseModel)cust).ToList();
            }
            else
            {
                //Get a list of all customernames
                List<Customer> customerList = (from cust in customers
                                               select cust).ToList();
                //Copy process mode, Get a list of all sites, except copied process parent site
                foreach (Customer c in customerList)
                {
                    List<Site> sites = new List<Site>();
                    if (c.Name != customerName)
                    {
                        sites = (from s in c.Sites
                                 select s).ToList();
                    }
                    else
                    {
                        sites = (from s in c.Sites
                                 where s.Name != site.Name
                                 select s).ToList();
                    }
                    foreach (Site si in sites)
                    {
                        si.CustomerName = c.Name;
                    }

                    nameList = nameList.Concat(sites).ToList();
                }
            }

            return nameList;
        }
    }
}
