using Epc.Data;
using Epc.Data.Models;
using EpcDashboard;
using EpcDashboard.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epc.Tests
{
    [TestClass]
    public class MainRepositoryTests
    {
        string _fileName;
        string _falseSource;
        string _falseTarget;
        string _sourceFile;
        string _destFile;
        MainRepository _repo;
        [TestInitialize]
        public void Init()
        {
            //Test xml files is in same folder as originals

            //correct ones defaults
            _fileName = "UnitTestEPC.xml";
            _falseSource = @"Ö:\\False Empiri Product Configurator\\";
            _falseTarget = @"Ä:\\False Empiri Product Configurator\\";
            _sourceFile = Path.Combine(_falseSource, _fileName);
            _destFile = Path.Combine(_falseTarget, _fileName);
            _repo = new MainRepository();
        }
        [TestMethod]
        public void AssertThatVersionCompareIsCorrect()
        { 
            //Prepare
            //Act
            bool? result = _repo.CompareVersion(_repo.SourceFile,_repo.TargetFile,true);
            //Assert
            Assert.IsTrue(result == true);
        }
        //CompareVersion method should return true -signifies that local file shall not be updated from source 
        //- if source is not accessible
        [TestMethod]
        public void TrueWhenNotAccessibleSource() 
        {
            //Prepare
            //Act
            bool? result = _repo.CompareVersion(_sourceFile, _destFile, false);
            //Assert
            Assert.IsTrue(result == true);
        }
        [TestMethod]
        public void FalseWhen2ObjectsAreIdentical()
        {
            //Prepare
            EPC_Config_Data objOld = new EPC_Config_Data();
            EPC_Config_Data objNew = new EPC_Config_Data();
            objOld.Customers.Add(new Customer() { Name = "Test" });
            objNew.Customers.Add(new Customer() { Name = "Test" });
            //Act
            bool isChanged = EpcBackgroundSync.CompareCustomers(objOld.Customers, objNew.Customers);
            //Assert
            Assert.IsFalse(isChanged);
        }
        [TestMethod]
        public void FalseWhen2ListsAreIdenticalAndNull()
        {
            //Prepare
            EPC_Config_Data objOld = new EPC_Config_Data();
            EPC_Config_Data objNew = new EPC_Config_Data();
            //Act
            bool isChanged = EpcBackgroundSync.CompareCustomers(objOld.Customers, objNew.Customers);
            //Assert
            Assert.IsFalse(isChanged);
        }

        [TestMethod]
        public void PropertiesChangedWhen2ObjectsAreNotIdentical()
        {
            //Prepare
            EPC_Config_Data objOldNull = new EPC_Config_Data();
            EPC_Config_Data objNew = new EPC_Config_Data();
            objOldNull.Customers.Add(new Customer() {Name = "Test2"});
            objNew.Customers.Add(new Customer() { Name = "Test" });
            bool isChanged = true;
            //Act
            while(isChanged)
            {
                isChanged = EpcBackgroundSync.CompareCustomers(objOldNull.Customers, objNew.Customers);
            }
            //Assert that the name property really is changed
            Assert.IsTrue(objOldNull.Customers[0].Name == "Test");
        }
        [TestMethod]
        public void TrueWhen2ObjectsAreNotIdenticalAndOneIsNull()
        {
            //Prepare
            EPC_Config_Data objOldNull = new EPC_Config_Data();
            EPC_Config_Data objNew = new EPC_Config_Data();

            objNew.Customers.Add(new Customer() { Name = "Test" });
            //Act
            bool isChanged = EpcBackgroundSync.CompareCustomers(objOldNull.Customers, objNew.Customers);
            //Assert
            Assert.IsTrue(isChanged);
        }
    }
}
