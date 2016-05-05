using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Gov.Baaqmd.Tests.Breeze;
using Gov.Baaqmd.BusinessObjects;
using Breeze.Sharp;
using System.Net;
using Gov.Baaqmd.Tests.WebUITests;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using OpenQA.Selenium.Firefox;

/*
    QA Automation Engineers, you can ignore this file.  Your tests should go in the other files.

    For example, ACPO tests should be located in SeleniumUITests.ACPO.cs.
    Supporting page objects are located in the PageObjects directory.

*/


namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    [TestClass]
    public partial class SeleniumUITests
    {

        /// <summary>
        /// Returns the root web site we are testing against
        /// (http://localhost/, http://ps-dev.baaqmd.gov, etc etc etc)
        /// </summary>
        static internal Uri TestEnvironmentUri
        {
            get { return new Uri("http://{0}".FormatString(Environment.TestEnvironment)); }
        }


        internal static Dictionary<string, string> SharedProperties = new Dictionary<string, string>();

        #region VS Framework boilerplate
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        /// This is part of the VS testing famework
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }
        private TestContext context;

        #endregion

        #region More Boilerplate - dev use only
        /// <summary>
        /// Reference to web driver
        /// </summary>
        static internal IWebDriver webDriver = null;

        /// <summary>
        /// Imported tests use the name wd so adding an alias to make thing smooth.
        /// </summary>
        static internal IWebDriver wd
        {
            get
            {
                return webDriver;
            }
        }

        #region Test Support
        /// <summary>
        /// Imported Selenium builder tests write logs to the Console Error stream.
        /// This class intercepts that and throws an exception.
        /// </summary>
        ConsoleTextWriter errorConsole = null;

        /// <summary>
        /// Imported Selenium builder tests write logs to the Console Error stream.
        /// This class intercepts that and throws an exception.
        /// </summary>
        class ConsoleTextWriter : TextWriter
        {
            internal bool WasTriggered = false;

            internal TextWriter InternalErrorConsole = null;

            internal StringWriter errorLog = new StringWriter();
            public ConsoleTextWriter(TextWriter source)
            {
                this.InternalErrorConsole = source;
            }
            public override void Write(char value)
            {
                InternalErrorConsole.Write(value);
                errorLog.Write(value);
                WasTriggered = true;
            }

            public void CheckForErrors()
            {
                if (WasTriggered)
                    throw new Gov.Baaqmd.BaaqmdException(errorLog.ToString());
            }

            public override Encoding Encoding
            {
                get { return Encoding.Default; }
            }
        }

        void CheckForErrors()
        {
            errorConsole.CheckForErrors();
        }
        #endregion
        

        [TestInitialize]
        public void TestInitialize()
        {
            var orig = Console.Error;
            errorConsole = new ConsoleTextWriter(Console.Error);
            Console.SetError(errorConsole);


            //Hit some URLs to start 'er up
            string[] relativeUrls = { "ACPO", "spa", "IC2" };

            foreach(var url in relativeUrls)
            using (var wc = new WebClient())
            {
                var tc = new Uri("http://{0}/{1}".FormatString(Environment.TestEnvironment, url));
                var x = wc.DownloadString(tc);
            }


            InitializeWebDriver();
        }

        private IWebDriver InitializeWebDriver()
        {

            if (webDriver != null)
            {
                webDriver.Quit();
                webDriver = null;
            }
           
            var commandTimeout = TimeSpan.FromSeconds(240);

            if (Environment.IsInSauce) {
                var desiredCapabilities = DesiredCapabilities.Firefox();
                desiredCapabilities.SetCapability("username", Environment.UserName);
                desiredCapabilities.SetCapability("access-key", Environment.AccessKey);

                desiredCapabilities.SetCapability("name", TestContext.TestName);

                //For saucelabs, linux appears more reliable than windows.
                //switched back to windows so resolution could be set
                desiredCapabilities.SetCapability("platform", "windows");
                desiredCapabilities.SetCapability("screenResolution", "1920x1200");

                Uri commandExecutorUri = new Uri("http://ondemand.saucelabs.com/wd/hub");

                webDriver = new RemoteWebDriver(commandExecutorUri, desiredCapabilities, commandTimeout);
                (webDriver as RemoteWebDriver).FileDetector = new LocalFileDetector();
            } else
            {
                FirefoxBinary fb = null;

                var file = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";

                if (File.Exists(file))
                    fb = new FirefoxBinary(file);
                else
                    fb = new FirefoxBinary();

                webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver(fb, new FirefoxProfile(), commandTimeout);
            }

            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            webDriver.Manage().Window.Maximize();           
            return webDriver;
        }

        /// <summary>
        /// Cleanup, close the web driver
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            bool passed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;

            if (passed && errorConsole.WasTriggered)
                //if we passed and we have a message in the error stream it means we did not check for errors in our test.
                throw new Gov.Baaqmd.BaaqmdException("You did not check for errors at the end of your test.  Please add CheckForErrors(); at the end of your test. ");


            // log the result to sauce labs
            if (Environment.IsInSauce)
                ((IJavaScriptExecutor)webDriver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));

            webDriver.Quit();

            //reset the error stream
            Console.SetError(errorConsole.InternalErrorConsole);
        }

        #endregion
    }
}
