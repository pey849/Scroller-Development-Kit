using SDK_Application.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using Microsoft.Xna.Framework;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for CastingTest and is intended
    ///to contain all CastingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CastingTest
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
        ///A test for FromTextToColor
        ///</summary>
        //[TestMethod()]
        public void FromTextToColorTest()
        {
            TextBox tx = new TextBox(); // TODO: Initialize to an appropriate value
            tx.Text = "{R:255 G:255 B:255 A:255}";
            Color expected = new Color(255,255,255,255); // TODO: Initialize to an appropriate value
            Color actual;
            actual = Casting.FromTextToColor(tx);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FromTextToVector
        ///</summary>
        //[TestMethod()]
        public void FromTextToVectorTest()
        {
            TextBox tx = new TextBox(); // TODO: Initialize to an appropriate value
            tx.Text = "{X:2 Y:2}";
            Vector2 expected = new Vector2(2,2); // TODO: Initialize to an appropriate value
            Vector2 actual;
            actual = Casting.FromTextToVector(tx);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
