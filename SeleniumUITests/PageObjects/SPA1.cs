using System;
using System.Collections.Generic;
using System.Linq;
using FluentAutomation;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using OpenQA.Selenium.Remote;
using Gov.Baaqmd.Tests.WebUITests;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class SPA1 : PageObject
    {
        /// <summary>
        /// For imported tests
        /// </summary>
        private SPA1 driver
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "spa";
            }
        }



        protected bool WaitForAccessToken()
        {
            int iCounter = 30;
            while (iCounter > 0)
            {
                var status = (string)ExecuteScript("return document.cookie;");
                if (status.Contains("access_token"))
                    return true;
                else
                {
                    iCounter--;
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }

        protected bool WaitForIdle()
        {
            int iCounter = 30;
            while (iCounter-- > 0)
            {
                try
                {
                    System.Threading.Thread.Sleep(250);
                    var isBusy = (bool)ExecuteScript("return document.isBusy;"); //set by our angular controller
                    if (!isBusy) //we're not busy
                        return true;
                    else
                        //Still busy...
                        System.Threading.Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }


        public SPA1 Login(string userName, string password)
        {
            this.ClearAllCookies();
            this.Navigate("/spa/#/signin?returnUrl=/IC2/", true);

            WaitForComplete();

            FindElement(By.Id("email")).Clear();
            FindElement(By.Id("email")).SendKeys(userName);
            FindElement(By.Id("password")).Clear();
            FindElement(By.Id("password")).SendKeys(password);
            FindElement(By.Id("btn_submit")).Click();

            WaitForAccessToken();

            return this;
        }

        public SPA1 NavigateToQAAdmin()
        {
            this.Navigate("spa/#/admin/qa", true);

            WaitForComplete();
            return this;
        }

        public SPA1 PayInvoiceByAppNumber(string applicationNumber)
        {
            FindElement(By.CssSelector(".applicationNumber"));

            var outstandingBalance = getinvoiceBalance(applicationNumber);

            FindElement(By.Name("amount")).SendKeys(outstandingBalance);

            FindElement(By.Name("payInvoiceButton")).Click();

            WaitForIdle();

            //Balance will refresh
            outstandingBalance = getinvoiceBalance(applicationNumber);
            Assert.AreEqual("0.00", outstandingBalance);

            return this;
        }

        private string getinvoiceBalance(string applicationNumber)
        {
            FindElement(By.Name("applicationNumber")).Clear();
            FindElement(By.Name("applicationNumber")).SendKeys(applicationNumber);
            FindElement(By.Name("getInvoiceByApplicationNumber")).Click();

            WaitForIdle();

            string outstandingBalance = "";
            var iCounter = 30;
            while (outstandingBalance.Length == 0
                && iCounter-- > 0)
            {
                outstandingBalance = FindElement(By.CssSelector("span.invoiceOutstandingBalance")).Text;
                System.Threading.Thread.Sleep(1000);
            }
            //strip off $
            outstandingBalance = outstandingBalance.Substring(1);

            return outstandingBalance;
        }
        /// Switch to the IFrame and get the new driver.
        /// Call SwitchOutofIFrame after done working with the element.
        /// </summary>
        /// <param name="driver">the current driver</param>
        /// <returns>the IFrame driver.</returns>
        /// 



        public void TestIC2ApproveDCRApplicationIssuePermit(string facilityNumber)
        {
            var driver = this;


            driver.FindElement(By.XPath(".//*[@id='content-area']/div[2]/div/ul/li[2]/a")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Clear();

            driver.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);
            driver.FindElement(By.Id("searchSubmit")).Click();
            driver.FindElement(By.XPath(".//*[@id='1']/td[2]/a")).Click();

            //click application xpath
            driver.FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[1]/a")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Click();
            Thread.Sleep(5000);

            //Click on Submit for approval button 3 times
            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);

            this.webDriver.SwitchTo().Frame("IfrDialog");

            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("1stTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(5000);

            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("2ndTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(5000);

            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("3rdTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            // driver.FindElement(By.XPath(".//*[@id='RefreshApplicationScenario']")).Click();
            Thread.Sleep(10000);
        }

        public void testIC2IssuePermitDCR(string facilityNumber)
        {
            var driver = this;
            driver.FindElement(By.XPath(".//*[@id='content-area']/div[2]/div/ul/li[2]/a")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Clear();

            driver.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);
            driver.FindElement(By.Id("searchSubmit")).Click();
            driver.FindElement(By.XPath(".//*[@id='1']/td[2]/a")).Click();

            //click application xpath
            driver.FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[1]/a")).Click();
            Thread.Sleep(5000);
            //click on application number
            driver.FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("lnkApplicationSummary")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[1]/span[1]")).Click();
            Thread.Sleep(5000);
            //upload a document
            this.driver.FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).SendKeys("C:/Users/fitsum/Desktop/QA Info/qa.txt");
            Thread.Sleep(7000);
            //driver.FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).Click();
            //System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
            //System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            //Thread.Sleep(10000);

            //yes radio button
            driver.FindElement(By.XPath("//div[1]/div/div/div/div/div/form/div/div[11]/div[2]/div/div[8]/div[2]/div[11]/div[2]/div[2]/input[1]")).Click();
            Thread.Sleep(5000);
            //issue permit button
            driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[2]/div[11]/div[2]/a/span")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(5000);

        }
        public void TestIC2AdminToolStartupDCRdevice(string facilityNumber)
        {
            var driver = this;
            // Navigate();
         //   driver.FindElement(By.XPath("html/body/div[5]/div/div/div[3]/button[2]")).Click();
         //   Navigate();
            driver.FindElement(By.CssSelector("form[name=\"form\"] > fieldset > div.form-group > div.input-group > input[name=\"facilityNumber\"]")).Click();

            driver.FindElement(By.CssSelector("form[name=\"form\"] > fieldset > div.form-group > div.input-group > input[name=\"facilityNumber\"]")).SendKeys(facilityNumber);
            driver.FindElement(By.XPath("html/body/div[3]/div[2]/div[2]/div[1]/div/div/div[2]/section/div/div/div[2]/div[3]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            Thread.Sleep(3000);
            //STARTUP DEVICE BUTTON
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[1]/div/div[2]/div/div/form/div/button")).Click();
            Thread.Sleep(3000);

        }
        public void testIC2VerifyRegistrationCertificationDCR(string facilityNumber)
        {
            var driver = this;
            driver.FindElement(By.XPath(".//*[@id='content-area']/div[2]/div/ul/li[2]/a")).Click();

            driver.FindElement(By.Id("FacilityNumber")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Clear();

           
            driver.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);
            driver.FindElement(By.Id("searchSubmit")).Click();

            driver.FindElement(By.LinkText("Test123")).Click();
            Thread.Sleep(5000);
            //driver.FindElement(By.XPath(".//*[@id='lnkPermits']/span/span")).Click();
            //Thread.Sleep(5000);
            //this.webDriver.SwitchTo().Frame("IfrDialog");

            //driver.FindElement(By.XPath(".//*[@id='msomenuid2']/div[2]")).Click();
            //Thread.Sleep(5000);
            //driver.FindElement(By.XPath(".//*[@id='ID_ViewProperties']/span[1]")).Click();
        }
       
        public void TestIC2AdminToolDCRRenewa149(string facilityNumber)
        {
            var driver = this;
            //Navigate();
            //driver.FindElement(By.XPath("html/body/div[5]/div/div/div[3]/button[2]")).Click();
            //Navigate();
            //Change Facility Anniversary Date
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Clear();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).SendKeys(facilityNumber);
            Thread.Sleep(5000);
            //get facility               
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            Thread.Sleep(5000);
            // New Anniversary Date
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).Clear();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).SendKeys(DateTime.Today.AddDays(149).ToShortDateString());
            //update facility
            Thread.Sleep(5000);         
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div/form/div/button")).Click();
            //Run Renewal
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).SendKeys(facilityNumber);
            Thread.Sleep(5000);
            //get facility
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            //run renewal
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/div/button")).Click();
            Thread.Sleep(20000);


        }


        public void testIC2CheckRenewalInvoiceDCR(string facilityNumber)
        {
            var driver = this;

            driver.FindElement(By.XPath(".//*[@id='content-area']/div[2]/div/ul/li[2]/a")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Clear();
            driver.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);

            driver.FindElement(By.Id("searchSubmit")).Click();

            driver.FindElement(By.LinkText("Test123")).Click();
            //Emmision updates and renewal
            driver.FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[9]/a")).Click();
            Thread.Sleep(5000);
            ///click application number
         //   driver.FindElement(By.XPath(".//*[@id='gview_renewals_table']/div[3]")).Click();

            var renewalNumber = FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Text;

            SeleniumUITests.SharedProperties["RenewalNumber"] = renewalNumber;
          //  SeleniumUITests.SharedProperties["DCRJobPage"] = new Uri(wd.Url).PathAndQuery;
            return;


            //Click on Documents link from left nav.
            //driver.FindElement(By.Id(".//*[@id='lnkDocuments']")).Click();

            ////Open renewal invoice PDF
            //driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[1]/span[1]")).Click();
            //driver.FindElement(By.XPath(".//*[@id='Deviceb2948332e9b74869b3ac32d309b474d0']")).Click();
            //driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[2]/div[11]/div[2]/a/span")).Click();
        }
        public void TestIC2AdminToolDCRRenewal59(string RenewalNumber, string facilityNumber)
        {
            var driver = this;
            //Navigate();

            //driver.FindElement(By.XPath("html/body/div[5]/div/div/div[3]/button[2]")).Click();
            //Navigate();
            //Enter Renewal Number in admin tool of update renewal tab
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).SendKeys(RenewalNumber);
            //hit get renewal number button
            Thread.Sleep(5000);          
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            Thread.Sleep(5000);
            //type New Renewal Permit Expire Date in update renewal
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).Clear();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/fieldset/div[3]/div/input")).SendKeys(DateTime.Today.AddDays(59).ToShortDateString());
            Thread.Sleep(5000);
            //click update renewal in update renewal 
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[2]/div/div[2]/div/div/form/div/button")).Click();
            Thread.Sleep(5000);

            //Click on Run Renewal
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Click();
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).SendKeys(facilityNumber);
            Thread.Sleep(5000);
            ///click get facility in run renewal 
            driver.FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[3]/div[3]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            Thread.Sleep(10000);
                
            // click Run renewal for facility in Run Renewal
            driver.FindElement(By.CssSelector("form[name=\"frmRunRenewal\"] > div.control-group > button.btn.btn-submit")).Click();
            Thread.Sleep(100000);


            ////Run Renwal
            //Thread.Sleep(5000);
            //driver.FindElement(By.CssSelector("form[name=\"frmRunRenewal\"] > div.control-group > button.btn.btn-submit")).Click();
            //Thread.Sleep(5000);


        }

    }






}






















