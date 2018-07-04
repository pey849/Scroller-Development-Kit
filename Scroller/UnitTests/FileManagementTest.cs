using SDK_Application.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for FileManagementTest and is intended
    ///to contain all FileManagementTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileManagementTest
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
        ///A test for open_File
        ///</summary>
        //[TestMethod()]
        public void open_FileTest()
        {
            string extension_type = ".png"; // TODO: Initialize to an appropriate value
            string expected = "C:\\Users\\Emmanuel\\Dropbox\\CMPT 370 Project\\Misc\\platformer_sprites_pixelized_0.png"; // TODO: Initialize to an appropriate value
            string actual;
            actual = FileManagement.open_File(extension_type);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for open_File
        ///</summary>
        //[TestMethod()]
        public void open_FileTest1()
        {
            TextBox txtbox = new TextBox(); // TODO: Initialize to an appropriate value
            txtbox.Text = "C:\\Users\\Emmanuel\\Dropbox\\CMPT 370 Project\\Misc\\platformer_sprites_pixelized_0.png";
            string extension = ".png"; // TODO: Initialize to an appropriate value
            string initialDirectory = "C:/Users/Emmanuel/Dropbox/CMPT 370 Project/Misc/"; // TODO: Initialize to an appropriate value
            FileManagement.open_File(txtbox, extension, initialDirectory);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for open_File
        ///</summary>
        [TestMethod()]
        public void open_FileTest2()
        {
            TextBox txtbox = new TextBox(); // TODO: Initialize to an appropriate value
            txtbox.Text = "C:\\Users\\Emmanuel\\Dropbox\\CMPT 370 Project\\Misc\\platformer_sprites_pixelized_0.png";
            FileManagement.open_File(txtbox);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
