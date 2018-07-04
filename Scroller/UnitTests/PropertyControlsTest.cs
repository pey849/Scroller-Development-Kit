using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using ScrollerEngine.Components;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for PropertyControlsTest and is intended
    ///to contain all PropertyControlsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PropertyControlsTest
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
        ///A test for PropertyControls Constructor
        ///</summary>
        //[TestMethod()]
        public void PropertyControlsConstructorTest()
        {
            PropertyInfo p = null; // TODO: Initialize to an appropriate value
            object pValue = null; // TODO: Initialize to an appropriate value
            Component compo = null; // TODO: Initialize to an appropriate value
            PropertyControls target = new PropertyControls(p, pValue, compo);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for setPropValueToBeControl
        ///</summary>
        //[TestMethod()]
        public void setPropValueToBeControlTest()
        {
            PropertyInfo p = null; // TODO: Initialize to an appropriate value
            object pValue = null; // TODO: Initialize to an appropriate value
            Component compo = null; // TODO: Initialize to an appropriate value
            PropertyControls target = new PropertyControls(p, pValue, compo); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.setPropValueToBeControl(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
