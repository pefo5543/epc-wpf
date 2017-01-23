using Epc.Data;
using Epc.Data.Models;
using Epc.Tests.Mocks;
using EpcDashboard.Processes;
using EpcDashboard.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epc.Tests
{
    [TestClass]
    public class ProcessOverviewViewModeltests
    {
        private IMainRepository _repo;
        private AsyncObservableCollection<Process> processes;
        private ProcessListViewModel _vm;
        private Customer _cust;

        [TestInitialize]
        public void Init()
        {
            _repo = new MockRepository();
            _vm = new ProcessListViewModel(_repo);
            processes = new AsyncObservableCollection<Process>();
            Site s1 = new Site() { Name = "test1" };
            Process p1 = new Process() { Name = "testp1" };
            Process p2 = new Process() { Name = "testp2" };
            _cust = new Customer() { Name = "customertest" };
            _cust.Sites.Add(s1);
            _cust.Sites[0].Processes.Add(p1);
            _cust.Sites[0].Processes.Add(p2);
        }

        [TestMethod]
        public void SearchFilterSitesOnNullAndWhitespaceTest()
        {

            //Act
            _vm.SetProcesses(_cust.Sites[0]);
            _vm.Filtering(null);

            //Assure
            Assert.AreEqual(_vm.Processes[0], _vm.AllProcesses[0]);

            //Act
            _vm.Filtering("");
            //Assure
            Assert.AreEqual(_vm.Processes[0], _vm.AllProcesses[0]);

        }

        [TestMethod]
        public void OriginalCollectionAfterSearchIsCleared()
        {
            //Arrange

            //Act
            _vm.SetProcesses(_cust.Sites[0]);
            _vm.Filtering("testp1"); //Filtering is achieved on Processes - only one process remains

            //Assure
            Assert.AreNotEqual(_vm.Processes.Count, _vm.AllProcesses.Count);

            //Act
            _vm.Filtering(""); //Filtering is removed
                               //Assure
            Assert.AreEqual(_vm.Processes.Count, _vm.AllProcesses.Count);
        }

        [TestMethod]
        public void HasEBMSFlagTest()
        {
            //Arrange
            //Act
            _vm.SetProcesses(_cust.Sites[0]);

            //Assure
            //EBMS null check
            Assert.IsFalse(_vm.HasEBMS);

            //Act
            _cust.Sites[0].EBMS.ActionName = "test";
            _vm.SetProcesses(_cust.Sites[0]);

            //Assure
            //HasEBMS = true -> has an EBMS object which is not null
            Assert.IsTrue(_vm.HasEBMS);

        }
    }
}
