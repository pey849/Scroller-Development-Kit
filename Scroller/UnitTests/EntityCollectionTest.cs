using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.Generic;
using ScrollerEngine.Components;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for EntityCollectionTest and is intended
    ///to contain all EntityCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EntityCollectionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for EntityCollection Constructor
        ///</summary>
        //[TestMethod()]
        public void EntityCollectionConstructorTest()
        {
            EntityCollection target = new EntityCollection();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetAssemblies
        ///</summary>
        //[TestMethod()]
        [DeploymentItem("SDK Application.exe")]
        public void GetAssembliesTest()
        {
            EntityCollection_Accessor target = new EntityCollection_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<Assembly> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Assembly> actual;
            actual = target.GetAssemblies();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for delete
        ///</summary>
        //[TestMethod()]
        public void deleteTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.delete(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getComponentValues
        ///</summary>
        //[TestMethod()]
        public void getComponentValuesTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            ComponentCollection cc = null; // TODO: Initialize to an appropriate value
            target.getComponentValues(cc);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for getEntity
        ///</summary>
        //[TestMethod()]
        public void getEntityTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Entity expected = null; // TODO: Initialize to an appropriate value
            Entity actual;
            actual = target.getEntity(fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getFirstFileName
        ///</summary>
        //[TestMethod()]
        public void getFirstFileNameTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getFirstFileName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getPropertyValue
        ///</summary>
        //[TestMethod()]
        public void getPropertyValueTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            target.getPropertyValue(filename);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for insert
        ///</summary>
        //[TestMethod()]
        public void insertTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            string fileNameExpected = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.insert(ref fileName);
            Assert.AreEqual(fileNameExpected, fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for isEmpty
        ///</summary>
        //[TestMethod()]
        public void isEmptyTest()
        {
            EntityCollection target = new EntityCollection(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.isEmpty();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
