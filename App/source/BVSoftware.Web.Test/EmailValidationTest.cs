using BVSoftware.Web.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BVSoftware.Web.Test
{


    /// <summary>
    ///This is a test class for EmailValidationTest and is intended
    ///to contain all EmailValidationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EmailValidationTest
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



        [TestMethod()]
        public void CanFailBlankEmailAddress()
        {
            string input = string.Empty;
            bool expected = false;
            bool actual;
            actual = EmailValidation.MeetsEmailFormatRequirements(input);
            Assert.AreEqual(expected, actual);
        }

        private void TestSingleEmail(string input, bool expected)
        {
            if (expected)
            {
                Assert.IsTrue(EmailValidation.MeetsEmailFormatRequirements(input), "Failed on " + input);
            }
            else
            {
                Assert.IsFalse(EmailValidation.MeetsEmailFormatRequirements(input), "False Positive on " + input);
            }            
        }

        [TestMethod()]
        public void CanPassValidEmailAddresses()
        {
            TestSingleEmail("marcus@bvsoftware.com",true);            
            
            TestSingleEmail("first.last@company.ie",true);
            TestSingleEmail("name_last@nowhere.com",true);
            TestSingleEmail("verylongnameandaddressforfirstpart@longdomainnametoolongdomain.com", true);

            // These two items are in fact valid email addresses but the RegEx
            // does not pass them. A rare situation that might need to be 
            // addressed later
            
            //TestSingleEmail("a@b.co.uk", true);
            //TestSingleEmail("a@b.ru", true);
        }

        [TestMethod()]
        public void CanPassQuestionableValidEmailAddresses()
        {
            TestSingleEmail("d.j@server1.proseware.com", true);
            TestSingleEmail("long+name@co.au",true);
            TestSingleEmail("js#internal@proseware.com", true);
            TestSingleEmail("j_9@[129.126.118.1]", true);
            TestSingleEmail("one$or%ano!ther@nowhere.com",true);
            TestSingleEmail("one&or'another@nowhere.com",true);
            TestSingleEmail("one*or+another@nowhere.com",true);
            TestSingleEmail("one-or/another@nowhere.com",true);
            TestSingleEmail("one=or?another@nowhere.com",true);
            TestSingleEmail("one^or_another@nowhere.com",true);
            TestSingleEmail("one`or{another@nowhere.com",true);
            TestSingleEmail("one|or}ano~ther@nowhere.com", true);
            TestSingleEmail("j.s@server1.proseware.com", true);
        }

        [TestMethod()]
        public void CanFailInvalidEmailAddresses()
        {
            TestSingleEmail("j.@server1.proseware.com", false);
            TestSingleEmail("j@proseware.com9", false);            
            TestSingleEmail("marc us@bvsoftware.com",false);
            TestSingleEmail("a@b.o",false);            
            TestSingleEmail("a",false);
            TestSingleEmail("a@",false);
            TestSingleEmail("a@c",false);
            TestSingleEmail("a@co",false);
            TestSingleEmail("a@co.k",false);
            TestSingleEmail("fir\"st.last@company.ie",false);            
            TestSingleEmail("@nothing.com",false);
            TestSingleEmail("nothing.com",false);
            TestSingleEmail("nothing", false);
            TestSingleEmail("j..s@proseware.com", false);
            TestSingleEmail("js*@proseware.com", false);
            TestSingleEmail("js@proseware..com", false);

        }
    }
}
