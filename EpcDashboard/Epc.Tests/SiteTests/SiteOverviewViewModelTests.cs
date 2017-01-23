using Epc.Data;
using Epc.Data.Models;
using Epc.Tests.Mocks;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.Sites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Epc.Tests
{
    [TestClass]
    public class SiteOverviewViewModelTests
    {
        private IMainRepository _repo;
        private AsyncObservableCollection<Site> sites;
        private SiteListViewModel _vm;
        private Customer _cust;

        [TestInitialize]
        public void Init()
        {
            _repo = new MockRepository();
            _vm = new SiteListViewModel(_repo);
            sites = new AsyncObservableCollection<Site>();
            Site s1 = new Site() { Name = "test1" };
            Site s2 = new Site() { Name = "test2" };
            _cust = new Customer() { Name = "customertest" };
            sites.Add(s1);
            sites.Add(s2);
            _cust.Sites = sites;
        }

        [TestMethod]
        public void SearchFilterSitesOnNullAndWhitespaceTest()
        {

            //Act
            _vm.SetSites(_cust);
            _vm.Filtering(null);

            //Assure
            Assert.AreEqual(_vm.Sites[0], _vm.AllSites[0]);

            //Act
            _vm.Filtering("");
            //Assure
            Assert.AreEqual(_vm.Sites[0], _vm.AllSites[0]);

        }

        [TestMethod]
        public void OriginalCollectionAfterSearchIsCleared()
        {
            //Arrange

            //Act
            _vm.SetSites(_cust);
            _vm.Filtering("test1"); //Filtering is achieved on Sites - only one site remains

            //Assure
            Assert.AreNotEqual(_vm.Sites.Count, _vm.AllSites.Count);

            //Act
            _vm.Filtering(""); //Filtering is removed
                               //Assure
            Assert.AreEqual(_vm.Sites.Count, _vm.AllSites.Count);

        }

        [TestMethod]
        public void PingSitesAcessibleTest()
        {
            //Arrange
            _cust.Sites[0].IpAdress = "12.12.12.12"; //wont-be-accessible ipadress
            _cust.Sites[1].IpAdress = "12.12.12.12"; //wont-be-accessible ipadress

            //Act
            _vm.SetSites(_cust);
            _vm.PingSites(_vm.Sites);

            //Assert
            foreach (Site s in _vm.Sites)
            {
                Assert.IsFalse(s.IsOnline);
            }
        }
    }
}
