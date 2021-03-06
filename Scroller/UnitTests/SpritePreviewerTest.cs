﻿using SDK_Application.Image_Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for SpritePreviewerTest and is intended
    ///to contain all SpritePreviewerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpritePreviewerTest
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
        ///A test for ReturnBitmapFile
        ///</summary>
        //[TestMethod()]
        public void ReturnBitmapFileTest()
        {
            TextBox fileBox = new TextBox(); // TODO: Initialize to an appropriate value
            
            BitmapImage expected = null; // TODO: Initialize to an appropriate value
            BitmapImage actual;
            actual = SpritePreviewer.ReturnBitmapFile(fileBox.Text);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

       
    }
}
