using Gov.Baaqmd.Tests.WebUITests;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public enum SelectorType
    {
        CSS,
        XPath,
        ID
    };

    public abstract class PageObject
    {
        const int RetryCount = 300;

        /// <summary>
        /// Reference to web driver
        /// </summary>
        protected IWebDriver webDriver = null;

        /// <summary>
        /// Used by FindElement, some tests (Asbestos) appear to need a delay to function appropriately
        /// This shouldn't be the case and if needed, should be reported as a bug.
        /// </summary>
        protected int ActionDelay
        {
            get; private set;
        }

        private Stack<int> delayQueue = new Stack<int>();
        protected void PushDelay(int newDelay)
        {
            delayQueue.Push(ActionDelay);
            ActionDelay = newDelay;
        }

        protected void PopDelay()
        {
            if (delayQueue.Count > 0)
                ActionDelay = delayQueue.Pop();
            else ActionDelay = 0;

        }

        /// <summary>
        /// Imported tests use the name wd so adding an alias to make thing smooth.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected IWebDriver wd => webDriver;

        protected Uri TestEnvironmentUri => SeleniumUITests.TestEnvironmentUri;

        protected PageObject() : this(SeleniumUITests.webDriver) { }

        protected PageObject(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        /// <summary>
        /// Navigates to a new page object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T NavigateTo<T>(string url = null) where T : PageObject, new()
        {
            var newObject = new T {webDriver = this.webDriver};

            if (url == null)
                newObject.Navigate();
            else
            {
                Console.WriteLine("Manual navigation to:" + url);
                newObject.Navigate(url, true);
            }
            return newObject;
        }

        public void ClearAllCookies()
        {
            webDriver.Manage().Cookies.DeleteAllCookies();
        }

        public string Cookie => (string) ExecuteScript("return document.cookie");

        /// <summary>
        /// The Relative Url for this page
        /// </summary>
        public virtual string PageRelativeUrl => "ACPO/Account/Login?ReturnUrl=%2fACPO%2f";

        /// <summary>
        /// Navigates us to the page we're supposed to be on (PageRelativeUrl)
        /// </summary>
        /// <returns></returns>
        public virtual PageObject Navigate(string url = null, bool force = false)
        {
            url = url ?? PageRelativeUrl;

            if (!string.IsNullOrEmpty(url) 
                && (!wd.Url.Contains(url) || force))
                wd.Navigate().GoToUrl(new Uri(TestEnvironmentUri, url));

            return this;
        }

        protected void Refresh()
        {
            wd.SwitchTo().DefaultContent();
            wd.Navigate().Refresh();
        }

        public void PauseForDemo([CallerMemberName] string caller = "")
        {
            if (Environment.DemoMode)
            {
                System.Windows.Forms.MessageBox.Show(text: $"Waiting ({caller})");
            }
        }

        protected void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        protected void Sleep(TimeSpan ts)
        {
            System.Threading.Thread.Sleep(ts);
        }

        protected void Retry(Action action, Action onFailure = null, int retrySeconds = RetryCount)
        {
            Retry<bool>(() => { action(); return true; }, onFailure, retrySeconds);
        }

        protected T Retry<T>(Func<T> action, Action onFailure = null, int retrySeconds = RetryCount)
        {
            onFailure = onFailure ?? (() => Sleep(250));

            DateTime waitUntil = DateTime.Now.AddSeconds(retrySeconds);
            Exception lastException = null;

            while (DateTime.Now < waitUntil)
            {
                try
                {
                    Sleep(ActionDelay);
                    return action();
                }
                catch (BaaqmdException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    lastException = e;
                    if (DateTime.Now >= waitUntil)
                        throw;
                    else
                        onFailure();
                }
            }

            lastException = lastException ?? new TimeoutException();
            throw lastException;
        }

        protected object ExecuteScript(string script)
        {
            var js = webDriver as IJavaScriptExecutor;
            Debug.Assert(js != null, "js != null");
            return js.ExecuteScript(script);
        }

        protected void WaitForComplete(int retryCount = RetryCount)
        {
            Console.WriteLine("Waiting for complete");
            //first check doc complete
            //then check jquery busy
            //finally check angular
            var isBusy = @"if (!(document.readyState == 'complete')) return false;
                    if (!(window.jQuery === undefined ? true : jQuery.active == 0)) return false;
                    if (!(window.busyCount === undefined ? true : window.busyCount == 0)) return false;
                    return true;";

            Retry(() =>
            {
                if ((bool)ExecuteScript(isBusy))
                    return;

                throw new Exception("WaitForComplete failed!");
            }, retrySeconds: retryCount);
        }

        protected void WaitFor(Func<bool> eval, int retrySeconds = RetryCount)
        {
           Retry(() => {
               if (!eval())
                   throw new Exception("Eval failed");
           }, retrySeconds: retrySeconds);
        }

        protected IWebElement WaitForEnabled(By by)
        {
           return Retry(() =>
           {
               var returnValue = wd.FindElement(by);
               if (returnValue.Enabled)
                   return returnValue;

               throw new Exception("Not Enabled");
           });            
        }


        /// <summary>
        /// Looks for an element, checks every second for 30 seconds.
        /// </summary>
        /// <param name="by"></param>
        /// <param name="retrySeconds"></param>
        /// <returns></returns>
        protected IWebElement FindElement(By by, int retrySeconds = RetryCount)
        {
            return Retry(() => wd.FindElement(by), retrySeconds: retrySeconds);
        }

        protected IWebElement FindBy(string selector, SelectorType type = SelectorType.CSS, int retrySeconds = RetryCount)
        {
            By b = null;

            switch (type)
            {
                case SelectorType.CSS:
                    b = By.CssSelector(selector);
                    break;
                case SelectorType.XPath:
                    b = By.XPath(selector);
                    break;
                case SelectorType.ID:
                    b = By.Id(selector);
                    break;
                default: throw new NotImplementedException();
            }

            return FindElement(b);
        }

        protected void UploadFile(string elementName, 
            string fileName="TEST.TXT", 
            string body = "This is a Test File",
            bool waitForComplete = true)
        {
            //need to ensure underlying upload control is visible, otherwise selenium error
            ExecuteScript(string.Format("document.getElementsByName('{0}')[0].style.display = 'block'; ", elementName));

            var upload = FindElement(By.Name(elementName));

            string tmpfile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);
            System.IO.File.WriteAllText(tmpfile, body);
            upload.SendKeys(tmpfile);

            if (waitForComplete)
                WaitForComplete();
        }

        protected IWebElement SendKeys(string selector, string value, SelectorType type = SelectorType.CSS)
        {
            IWebElement ret;
            (ret = FindBy(selector, type)).Clear();
            ret.SendKeys(value);
            return ret;
        }

        protected IWebElement Click(string selector, SelectorType type = SelectorType.CSS, int retrySeconds = 60)
        {
           return Retry(() =>
          {
              IWebElement ret = FindBy(selector, type);
              ret.Click();
              return ret;
          }, retrySeconds: retrySeconds);
        }

        protected IWebElement Click(By by, int retrySeconds = 60)
        {
            return Retry(() =>
            {
                IWebElement ret = FindElement(by);

                Actions actions = new Actions(wd);
                actions.MoveToElement(ret);
                actions.Perform();
                

                ret.Click();
                return ret;
            }, retrySeconds: retrySeconds);
        }




    }
}
