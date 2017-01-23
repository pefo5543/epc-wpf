using Microsoft.VisualStudio.TestTools.UnitTesting;
using EpcDashboard.Services;
using Epc.Tests.Mocks;
using System.Collections.ObjectModel;
using EpcDashboard;
using Epc.Data.Models;
using EpcDashboard.Customers;
using Epc.Data;
using EpcDashboard.Services.Interfaces;

namespace Epc.Tests
{
    [TestClass]
    public class CustomerOverviewViewModelTests
    {
        private IMainRepository _repo;
        private AsyncObservableCollection<Customer> customers;
        private CustomerListViewModel _vm;

        [TestInitialize]
        public void Init()
        {
            _repo = new MockRepository();
            _vm = new CustomerListViewModel(_repo);
            customers = new AsyncObservableCollection<Customer>();
        }

        [TestMethod]
        public void SearchFilterCustomersOnNullAndWhitespaceTest()
        {
            //Arrange
            Customer c1 = new Customer() {Name = "test1" };
            Customer c2 = new Customer() { Name = "test2" };
            customers.Add(c1);
            customers.Add(c2);

            //Act
            _vm.SetCustomers(customers);
            _vm.Filtering(null);

            //Assure
            Assert.AreEqual(_vm.Customers[0], _vm.AllCustomers[0]);

            //Act
            _vm.Filtering("");
            //Assure
            Assert.AreEqual(_vm.Customers[0], _vm.AllCustomers[0]);

        }

        [TestMethod]
        public void OriginalCollectionAfterSearchIsCleared()
        {
            //Arrange
            Customer c1 = new Customer() { Name = "test1" };
            Customer c2 = new Customer() { Name = "test2" };
            customers.Add(c1);
            customers.Add(c2);

            //Act
            _vm.SetCustomers(customers);
            _vm.Filtering("test1"); //Filtering is achieved on Customers - only one customer remains

            //Assure
            Assert.AreNotEqual(_vm.Customers.Count, _vm.AllCustomers.Count);

            //Act
            _vm.Filtering(""); //Filtering is removed
            //Assure
            Assert.AreEqual(_vm.Customers.Count, _vm.AllCustomers.Count);

        }
    }
}
