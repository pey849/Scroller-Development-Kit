using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using ScrollerEngine.Components.Graphics;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for HitboxAnalyzerTest and is intended
    ///to contain all HitboxAnalyzerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HitboxAnalyzerTest
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
        ///A test for IntializeAutomaticHurtBoxes
        ///</summary>
        //[TestMethod()]
        public void IntializeAutomaticHurtBoxesTest()
        {
            Bitmap spriteMap = null; // TODO: Initialize to an appropriate value
            AnimationFrame aFrame = null; // TODO: Initialize to an appropriate value
            HitboxAnalyzer.IntializeAutomaticHurtBoxes(spriteMap, aFrame);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
