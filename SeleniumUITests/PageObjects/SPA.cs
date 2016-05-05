using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class SPA : PageObject
    {

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
                    Sleep(1000);
                }
            }
            return false;
        }

        public SPA DisableToast()
        {
            WaitFor(() => (bool)ExecuteScript("return !(window.jQuery === undefined);"));

            ExecuteScript("window.showToast = false; jQuery('#toast-container').css('display', 'none');");
            Sleep(250);
            return this;
        }

        public SPA Login(string userName, string password)
        {
            this.ClearAllCookies();
            this.Navigate("spa/#/signin", true);

            WaitForComplete();
            DisableToast();

            FindElement(By.Id("email")).Clear();
            FindElement(By.Id("email")).SendKeys(userName);
            FindElement(By.Id("password")).Clear();
            FindElement(By.Id("password")).SendKeys(password);
            FindElement(By.Id("btn_submit")).Click();

            WaitForAccessToken();

            return this;
        }

        public SPA NavigateToQAAdmin()
        {
            this.Navigate("spa/#/admin/qa", true);

            WaitForComplete();
            DisableToast();

            return this;
        }

        public SPA NavigateToPrintQueue()
        {
            this.Navigate("spa/#/admin/printqueue", true);

            WaitForComplete();
            DisableToast();

            return this;
        }

        public SPA NavigateToRenewalPortal(string facilityNumber, string applicationNumber)
        {
            this.Navigate("spa/#/renewal/portal/" + facilityNumber + "/" + applicationNumber, true);

            WaitForComplete();
            DisableToast();

            return this;
        }

        public SPA CreateNewMessage() {
            string title = "Parent Post Title";
            string body = "Parent Post Body. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras leo augue, interdum non est non, sagittis fringilla dolor. Praesent volutpat lacus at ornare convallis.";
            
            string replyTitle = "Reply to Original Post";

            FindElement(By.Id("gridNav_addMessage")).Click();

            FindElement(By.Id("msgTitle")).SendKeys(title);
            FindElement(By.Id("msgBody")).SendKeys(body);

            FindElement(By.Id("msgSubmit")).Click();

            Sleep(10000);

            WaitFor(() => FindElement(By.CssSelector(".messageLink.ng-binding")).Text.Contains(title));
            
            // Reply to Message
            FindElement(By.CssSelector(".messageLink.ng-binding")).Click();

            FindElement(By.Id("msgDetailReply")).Click();
            FindElement(By.Id("msgDetailReply")).Click();

            FindElement(By.Id("msgRTitle")).SendKeys(replyTitle);
            FindElement(By.Id("msgRBody")).SendKeys(body);

            FindElement(By.Id("msgRSubmit")).Click();

            return this;
        }

       

        public SPA PayInvoiceByAppNumber(string applicationNumber)
        {
            WaitForComplete();
            DisableToast();

            FindElement(By.CssSelector(".applicationNumber"));
            
            var outstandingBalance = getinvoiceBalance(applicationNumber);

            FindElement(By.Name("amount")).SendKeys(outstandingBalance);
            
            FindElement(By.Name("payInvoiceButton")).Click();

            WaitForComplete();

            //Balance will refresh
            outstandingBalance = getinvoiceBalance(applicationNumber);
            Assert.AreEqual("0.00", outstandingBalance);

            return this;
        }

        public SPA StartUpDevices(string facilityNumber)
        {
            WaitForComplete();
            DisableToast();

            FindElement(By.CssSelector(".startUpDevicesFacilityNumber")).Clear();
            FindElement(By.CssSelector(".startUpDevicesFacilityNumber")).SendKeys(facilityNumber);
            WaitForComplete();

            FindElement(By.CssSelector(".startUpDevicesGetFacility")).Click();
            WaitForComplete();


            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.CssSelector(".startupDevicesFacilityName")).Text));

            //Wait for the button to be nabled before we click it...
            WaitForEnabled(By.CssSelector(".startUpDevicesGo"));
            Click(By.CssSelector(".startUpDevicesGo"));

            WaitForComplete();

            WaitFor(() => FindElement(By.CssSelector(".startupDevicesFacilityPrompt")).Text.ToLower().Contains("started"));

            return this;
        }

        public SPA ChangeFacilityAnniversaryDate(string facilityNumber, DateTime newAnniversaryDate)
        {
            WaitForComplete();
            DisableToast();

            FindElement(By.CssSelector(".updateFacilityFacilityNumber")).Clear();
            FindElement(By.CssSelector(".updateFacilityFacilityNumber")).SendKeys(facilityNumber);

            FindElement(By.CssSelector(".updateFacilityLookupFacility")).Click();

            WaitForComplete();

            FindElement(By.CssSelector(".updateFacilityNewPermitExpireDate")).Clear();
            FindElement(By.CssSelector(".updateFacilityNewPermitExpireDate")).SendKeys(newAnniversaryDate.ToShortDateString());

            WaitForComplete();
            FindElement(By.CssSelector(".updateFacilityGo")).Click();

            WaitForComplete();

            
            return this;
        }

        internal SPA DDE_UpdateWarningLetterDate(DateTime dateTime)
        {
            WaitForComplete();
            DisableToast();


            //A cheat is required b/c the dde doesnt allow direct entry..
            var ctrl = FindBy("div.panel-body:nth-child(2) > div:nth-child(6) > div:nth-child(1) > span:nth-child(2) > span:nth-child(1) > div:nth-child(1) > input:nth-child(1)");
            ctrl.Clear();
            ExecuteScript("$('div.panel-body:nth-child(2) > div:nth-child(6) > div:nth-child(1) > span:nth-child(2) > span:nth-child(1) > div:nth-child(1) > input:nth-child(1)').val('{0}')".FormatString(dateTime.ToShortDateString()));
            ctrl.SendKeys(" ");

            //adjust last printed date too          
            ctrl = FindBy("div.panel-body:nth-child(2) > div:nth-child(8) > div:nth-child(1) > span:nth-child(2) > span:nth-child(1) > div:nth-child(1) > input:nth-child(1)");
            ctrl.Clear();
            ExecuteScript("$('div.panel-body:nth-child(2) > div:nth-child(8) > div:nth-child(1) > span:nth-child(2) > span:nth-child(1) > div:nth-child(1) > input:nth-child(1)').val('{0}')".FormatString(dateTime.AddDays(-1).ToShortDateString()));
            ctrl.SendKeys(" ");

            Sleep(500);
            Click(".control-group > button:nth-child(1)");
            Sleep(2000);

            return this;
        }

        public SPA CheckPrintQueueForDocuments(string searchText, DateTime? searchTime = null, params string[] docsToCheck)
        {
            Retry(() =>
           {
               WaitForComplete();
               DisableToast();


               FindElement(By.CssSelector(".printQueueSearchText")).Clear();
               FindElement(By.CssSelector(".printQueueSearchText")).SendKeys(searchText);

               if (searchTime.HasValue)
               {
                   FindElement(By.CssSelector(".printQueueBeginDate")).Clear();
                   FindElement(By.CssSelector(".printQueueBeginDate")).SendKeys(searchTime.Value.ToString("yyyy-MM-ddTHH:mm"));

                   //search out to the future..
                   FindElement(By.CssSelector(".printQueueEndDate")).Clear();
                   FindElement(By.CssSelector(".printQueueEndDate")).SendKeys(DateTime.Today.AddDays(2).ToString("yyyy-MM-ddTHH:mm"));

               }

               FindElement(By.CssSelector(".printQueueGo")).Click();

               WaitForComplete();

                //Need a slight pause to allow Angular to sort the results.
                Sleep(250);

               var iRow = 1;
               foreach (var doc in docsToCheck)
               {
                   var estring = "div.list-group-item:nth-child({0}) > h4:nth-child(1) > a:nth-child(1)".FormatString(iRow);

                   var value = wd.FindElement(By.CssSelector(estring)).Text;

                   if (!value.Trim().ToLowerInvariant().Contains(doc.ToLowerInvariant()))
                       throw new Exception("Did not find expected document {0}/{1}".FormatString(doc, value));

                   iRow++;
               }
           }, onFailure: () =>
           {
               Sleep(1500);
               wd.Navigate().Refresh();
           }, retrySeconds: 120);

            return this;
        }

        public SPA RunRenewalForFacility(string facilityNumber, bool allowFail = true)
        {
            Retry(() => {

                DisableToast();

                FindElement(By.CssSelector(".runRenewalFacilityNumber")).Clear();
                FindElement(By.CssSelector(".runRenewalFacilityNumber")).SendKeys(facilityNumber);

                WaitForComplete();

                FindElement(By.CssSelector(".runRenewalLookupFacility")).Click();

                WaitForComplete();

                FindElement(By.CssSelector(".runRenewalGo")).Click();

                WaitFor(() => {
                    var text = FindElement(By.CssSelector(".runRenewalPrompt p.text-success")).Text;
                    if (string.IsNullOrEmpty(text))
                        throw new NotFoundException("Expected text!");

                    if ((text = text.Trim()).IndexOf('/') < 0)
                        throw new NotFoundException("Cannot find renewal number");

                    SeleniumUITests.SharedProperties["RenewalNumber"] = text.Substring(text.IndexOf('/') + 1);

                    return true;
                });



            }, onFailure: () => { Refresh(); Sleep(1000); WaitForComplete(); }, retrySeconds: 300);


            return this;
        }

        public SPA UpdateRenewalDates(DateTime newPermitExpireDate, string renewalNumber = null)
        {
            WaitForComplete();
            DisableToast();

            ExecuteScript("$('.navbar').css('display', 'none')");

            if (string.IsNullOrEmpty(renewalNumber))
                renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"];

            FindElement(By.CssSelector(".updateRenewalRenewalNumber")).Clear();
            FindElement(By.CssSelector(".updateRenewalRenewalNumber")).SendKeys(renewalNumber);

            Click(By.CssSelector(".updateRenewalLookup"));
            WaitForComplete();

            FindElement(By.CssSelector(".updateRenewalNewPermitExpireDate")).Clear();
            FindElement(By.CssSelector(".updateRenewalNewPermitExpireDate")).SendKeys(newPermitExpireDate.ToShortDateString());

            Click(By.CssSelector(".updateRenewalGo"));
            WaitForComplete();
            
            return this;
        }


        private string getinvoiceBalance(string applicationNumber)
        {
            WaitForComplete();
            DisableToast();

            FindElement(By.Name("applicationNumber")).Clear();
            FindElement(By.Name("applicationNumber")).SendKeys(applicationNumber);
            FindElement(By.Name("getInvoiceByApplicationNumber")).Click();

            WaitForComplete();

            string outstandingBalance = "";
            var iCounter = 30;
            while (outstandingBalance.Length == 0 
                && iCounter-- > 0)
            {
                outstandingBalance = FindElement(By.CssSelector("span.invoiceOutstandingBalance")).Text;
                Sleep(1000);
            }
            //strip off $
            outstandingBalance = outstandingBalance.Substring(1);

            return outstandingBalance;
        }
        public SPA SearchReports()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='top']/ul/li[6]/a"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<SPA>(url);
        }
        public SPA ClickCreateNewFacilityIC2()
        {
            FindElement(By.XPath(".//*[@id='facilitySearchForm']/table/tbody/tr[1]/td[3]/div/p/a/img")).Click();
            return this;
        }
        public SPA GenerateFacilityAccessCodes(string facilityNumber)
        {
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).Click();
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div/input")).SendKeys(facilityNumber);
            //get facility
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div/span/button")).Click();
            //Wait until we have the facility name.
            // WaitFor(() => !string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[2]/span[1]")).Text));
            WaitForComplete();
            //generate access code
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/div/button")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[2]/span[1]")).Text));
           // facility Number =                  
            string accessCode = FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[2]/div[1]/div/div[2]/div/div/form/fieldset/div[2]/div[2]/label")).Text;
            SeleniumUITests.SharedProperties["AccessCode"] = accessCode;
            PopDelay();
            return this;
        }
        public SPA PayInvoiceByInvoiceNumber(string InvoiceNum, string BalaDue)
        {
          
            InvoiceNum = SeleniumUITests.SharedProperties["InvoiceNumber"] ;
            BalaDue = SeleniumUITests.SharedProperties["BalanceDue"];
            BalaDue = BalaDue.Substring(1);

            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div[2]/input")).Clear();
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div[2]/input")).SendKeys(InvoiceNum);
            //get invoice
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[1]/div[2]/span/button")).Click();
            //Wait until we have the customer number
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[2]/span[1]")).Text));
            Sleep(5000);
            //write the amount
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[3]/input")).Clear();
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[3]/input")).SendKeys(BalaDue);
            //pay amount
            FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/div/button")).Click();
            //Wait until we have the customer number
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityCompliance-view']/div/div/div[2]/div[1]/div[1]/div/div[2]/div/div/form/fieldset/div[2]/span[1]")).Text));
            Sleep(5000);
            return this;
        }
        public SPA RenewalStatusReport()
        {

            //Application Status 
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl07_ddDropDownButton")).Click();
            WaitForComplete();
            //complete application under evaluation
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl07_divDropDown']/span/div[1]")).FindElement(By.XPath("//input[@id='ctl32_ctl04_ctl07_divDropDown_ctl20']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ctl32_ctl04_ctl07_txtValue")).GetAttribute("name"))));
            //Contains Component Category-Gasoline
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl09_ddDropDownButton")).Click();
            WaitForComplete();
            //dry cleaning
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl09_divDropDown']/span/div[1]")).FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl09_divDropDown_ctl13']")).Click();
            WaitForComplete();
            return this;
        }
        public SPA ViewReport()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));

           return this;
        }
    }
}
