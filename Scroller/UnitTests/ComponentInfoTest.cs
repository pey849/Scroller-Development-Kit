using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for ComponentInfoTest and is intended
    ///to contain all ComponentInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComponentInfoTest
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
        ///A test for ComponentInfo Constructor
        ///</summary>
        //[TestMethod()]
        public void ComponentInfoConstructorTest()
        
       {
            ComponentInfo target = new ComponentInfo();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        //[TestMethod()]
        public void ToStringTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        //[TestMethod()]
        public void FullNameTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.FullName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        //[TestMethod()]
        public void NameTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for NameSpace
        ///</summary>
        //[TestMethod()]
        public void NameSpaceTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.NameSpace = expected;
            actual = target.NameSpace;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Properties
        ///</summary>
        //[TestMethod()]
        public void PropertiesTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            List<ComponentPropertyInfo> expected = null; // TODO: Initialize to an appropriate value
            List<ComponentPropertyInfo> actual;
            target.Properties = expected;
            actual = target.Properties;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadableName
        ///</summary>
        //[TestMethod()]
        public void ReadableNameTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ReadableName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        //[TestMethod()]
        public void TypeTest()
        {
            ComponentInfo target = new ComponentInfo(); // TODO: Initialize to an appropriate value
            Type expected = null; // TODO: Initialize to an appropriate value
            Type actual;
            target.Type = expected;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
