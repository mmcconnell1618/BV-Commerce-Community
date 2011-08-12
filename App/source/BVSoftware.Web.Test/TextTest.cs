using BVSoftware.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BVSoftware.Web.Test
{
    
    
    /// <summary>
    ///This is a test class for TextTest and is intended
    ///to contain all TextTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TextTest
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
        ///A test for RemoveHtmlTags
        ///</summary>
        [TestMethod()]
        public void RemoveHtmlTagsTest()
        {
            string input = "This<br/>Is<br />a test of the <b>html</b> stripper. <a href=\"something\" title\"test\">Link here</a>.";
            string expected = "This*Is*a test of the *html* stripper. *Link here*.";
            string actual;
            actual = Text.RemoveHtmlTags(input,"*");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveHtmlTagsTest2()
        {
            string input = "This</DIV>Is";
            string expected = "This*Is";
            string actual;
            actual = Text.RemoveHtmlTags(input,"*");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CanSlugifyCorrectlyWithSlashes()
        {
            string input =    @"shoes/nikes/awesome stuff/Shared Options.Tester_1";
            string expected = @"shoes/nikes/awesome-stuff/Shared-Options.Tester_1";
            string actual = Text.Slugify(input, true, true);
            Assert.AreEqual(actual, expected);
        }
    }
}
