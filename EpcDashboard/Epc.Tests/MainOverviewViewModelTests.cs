using EpcDashboard;
using EpcDashboard.MVVMHelpers;
using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using EpcDashboard.Sites;
using EpcDashboard.ViewModelBases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epc.Tests
{
    [TestClass]
    public class MainOverviewViewModelTests
    {
        private MainWindowViewModel mvm;
        private IMainRepository _repo;

        [TestInitialize]
        public void Init()
        {
            _repo = new MainRepository();
            mvm = new MainWindowViewModel(_repo);
        }

        [TestMethod]
        public void NavigationPageViewModelListTest()
        {
            //In current implementation the PageViewModel list shall always consist of 3 viewmodel objects, 
            //where one of them has isActive = true. We test the default startup state of it in this method.

            //Arrange
            List<BindableBase> pages = mvm.PageViewModels;
            int countActives = 0;

            //Act
            foreach (ListViewModelBase p in pages)
            {
                if (p.IsActive)
                {
                    countActives++;
                }
            }
            //Assert
            //check that all pageviewmodel objects are instances of ViewModelBase class
            CollectionAssert.AllItemsAreInstancesOfType(pages, typeof(ListViewModelBase));

            //check that there is 3 pageviewobjects
            Assert.AreEqual(pages.Count(), 3);

            //check that one object is active
            Assert.AreEqual(countActives, 1);
        }

        [TestMethod]
        public void NavigationListAfterNavigationTest()
        {
            //Arrange
            ListViewModelBase testvm = new SiteListViewModel(_repo);
            int countActives = 0;
            //Act
            mvm.ChangeViewModel(testvm);
            foreach (ListViewModelBase p in mvm.PageViewModels)
            {
                if (p.IsActive)
                {
                    countActives++;
                }
            }
            //Assert
            //check that there is 3 pageviewobjects
            Assert.AreEqual(mvm.PageViewModels.Count(), 3);

            //check that one object is active
            Assert.AreEqual(countActives, 1);
        }

        //Ping tests
        [TestMethod]
        public void CentralServerPing()
        {
            //Arrange
            _repo.ServerIsAlive = _repo.PingIp("WSSE01501.eur.gad.schneider-electric.com.wrongAdress", 100);
            //Act
            var result = _repo.ServerIsAlive;
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TimerAlwaysStartedIndependentOfServerStatus()
        {
            //Arrange
            //Act
            mvm.ServerIsAlive = true;
            //Assert
            Assert.IsTrue(mvm.Timer.IsEnabled);

            mvm.ServerIsAlive = false;
            Assert.IsTrue(mvm.Timer.IsEnabled);
        }
    }
}
