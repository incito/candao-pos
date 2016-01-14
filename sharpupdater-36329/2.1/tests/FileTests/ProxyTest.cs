using System.Net;
using System.Text;
using System.Web;
using CnSharp.Updater.SharpPack.Plugin.PublishProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CnSharp.Updater.SharpPack.API;
using System.Collections.Generic;
using CnSharp.Windows.Updater.Util;

namespace FileTests
{
    
    
    /// <summary>
    ///This is a test class for ProxyTest and is intended
    ///to contain all ProxyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProxyTest
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

        [TestMethod]
        public void TestSerializer()
        {
            var url = "http://10.10.11.54:8010/wms/releaselist.xml";
            Console.Write(FileUtil.ReadReleaseList(url).UpdateDescription);
        }

        /// <summary>
        ///A test for Post
        ///</summary>
        [TestMethod()]
        public void PostTest()
        {
            string url = "http://localhost:8010/software/publish.aspx?m=publish&cid=wms&time=20121031113605&token=06B174933E5417A738C790852C663072";
            var v =  HttpUtility.UrlEncode("2.18.2");
            var desc = HttpUtility.UrlEncode("ffffffffffffffffffff");
            var data = string.Format("v={0}&desc={1}", v, desc);
            //url += "&" + data;
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            string expected = "OK";
            var wc = new WebClient();

            //string actual = wc.DownloadString(url);
            string actual = Proxy.Post(url, sendData);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}
