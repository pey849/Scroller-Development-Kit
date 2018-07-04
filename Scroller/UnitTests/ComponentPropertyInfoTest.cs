using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for ComponentPropertyInfoTest and is intended
    ///to contain all ComponentPropertyInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComponentPropertyInfoTest
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
        ///A test for ComponentPropertyInfo Constructor
        ///</summary>
        //[TestMethod()]
        public void ComponentPropertyInfoConstructorTest()
        {
            ComponentPropertyInfo target = new ComponentPropertyInfo();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        //[TestMethod()]
        public void ToStringTest()
        {
            ComponentPropertyInfo target = new ComponentPropertyInfo(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DefaultValue
        ///</summary>
        //[TestMethod()]
        public void DefaultValueTest()
        {
            ComponentPropertyInfo target = new ComponentPropertyInfo(); // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            target.DefaultValue = expected;
            actual = target.DefaultValue;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        //[TestMethod()]
        public void NameTest()
        {
            ComponentPropertyInfo target = new ComponentPropertyInfo(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadableName
        ///</summary>
        //[TestMethod()]
        public void ReadableNameTest()
        {
            ComponentPropertyInfo target = new ComponentPropertyInfo(); // TODO: Initialize to an appropriate value
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
            ComponentPropertyInfo target = new ComponentPropertyInfo(); // TODO: Initialize to an appropriate value
            Type expected = null; // TODO: Initialize to an appropriate value
            Type actual;
            target.Type = expected;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
