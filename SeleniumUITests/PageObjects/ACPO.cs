using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class ACPO : PageObject
    {
        /// <summary>
        /// For imported tests
        /// </summary>
        private IWebDriver driver
        {
            get
            {
                return webDriver;
            }
        }

        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "ACPO";
            }
        }

        public ACPOCreateNewFacility ClickCreateNewFacility()
        {
            FindElement(By.Id("createNewFacility")).Click();
            return this.NavigateTo<ACPOCreateNewFacility>();
        }

        public ACPOAsbestos CreateNewAsbestosJob()
        {
            FindElement(By.CssSelector("div.section:nth-child(4) > a:nth-child(4) > span:nth-child(1)")).Click();

            return this.NavigateTo<ACPOAsbestos>();
        }

        public ACPOCreateNewApplication ClickCreateNewApplication()
        {
            driver.FindElement(By.CssSelector("#NewPermitApplicationButton > span")).Click();
            WaitForComplete();

            return this.NavigateTo<ACPOCreateNewApplication>();
        }

        public ACPO StartUpDevices()
        {
            foreach (var e in wd.FindElements(By.CssSelector("span.expandCollapse")))
            {
                e.Click();
            }


            //Save Dates
            foreach (var e in wd.FindElements(By.CssSelector("a.button.saveInformation")))
            {
                e.Click();
            }

            //Ready to issue PO
            foreach(var e in wd.FindElements(By.CssSelector("input[value='True'][type='Radio\']")))
            {
                e.Click();
            }

            foreach (var e in wd.FindElements(By.CssSelector("a.greenButton")))
            {
                if (e.Text.Contains("ISSUE PERMIT"))
                {
                    e.Click();
                }
            }


            return this;
        }

      
        public void ConfirmUploadRetainsDocumentName( string fileNameValue)
        {

           UploadFile("Device_0__upload_uploadInput", fileName: fileNameValue, waitForComplete:false);

            var isFileNameRetained = driver
                 .FindElement(By.CssSelector("#Device_0__upload_files > div:nth-child(1) > span > a"))
                 .Text.Contains(fileNameValue);
           
            Assert.IsTrue(isFileNameRetained,"Upload doesn't retain the original document name somehow.");
        }


        public ACPO ChangeInvoiceDueDate(DateTime newInvoiceDate)
        {
            WaitForComplete();

            FindBy("#InvoiceDueDate").Clear();
            FindBy("#InvoiceDueDate").SendKeys(newInvoiceDate.ToShortDateString());

            FindBy("#title > h1:nth-child(1)").Click(); // click the title to close the calendar.  cheap trick

            Click("a.engineer_submit_button:nth-child(2)");

            WaitForComplete();

            return this;
        }

        public ACPO RenewalSupplyUpdatePeriodEndDate(DateTime? updatePeriodEndDate = null)
        {
            if (!updatePeriodEndDate.HasValue)
                updatePeriodEndDate = DateTime.Today.AddDays(-1); 

            WaitForComplete();

            ExecuteScript("$('#UpdatePeriodEndDate').get(0).scrollIntoView()");
            ExecuteScript("$('#UpdatePeriodEndDate').val('{0}')".FormatString(updatePeriodEndDate.Value.ToShortDateString()));


            ExecuteScript("$('#UpdatePeriodEndDate').focus()");

            //close the calendar
            ExecuteScript("$('#ui-datepicker-div  td.ui-datepicker-current-day a').click()");


            return this;
        }

        public ACPO RenewalUpdateAllAnnualUsages()
        {
            ExecuteScript("$('#DeviceMaterialUpdateRepeaterTarget > div.sectionExpanderGridList > div.sectionExpanderGridRow > span').click()");

            Sleep(250);

            foreach (var x in wd.FindElements(By.CssSelector("input[name$='_LastReportedUsageDisplay']")))
            {
                var ctrl = x;

                var lastValue = ctrl.GetAttribute("value");
                var name = ctrl.GetAttribute("name");

                name = name.Replace("_LastReportedUsageDisplay", "ReportedAnnualUsage");

                var targetCtrl = string.Format("input[name=\"{0}\"]", name);

                string setValTemplate = " $('{0}').get(0).scrollIntoView(); $('{0}').focus(); $('{0}').val('{1}'); $('{0}').blur();";
                var z = setValTemplate.FormatString(targetCtrl, lastValue);
                ExecuteScript(z);
            }

            Sleep(250);

            foreach (var x in wd.FindElements(By.CssSelector(".responseHolder input[value='True']")))
            {

                var ctrl = x;

                var id = ctrl.GetAttribute("id");
                var exactSelector = "input#{0}[value=\"True\"]".FormatString(id);
                Click(exactSelector);

                Sleep(500);
                var b = ctrl.GetAttribute("checked");

                //if (string.IsNullOrEmpty(b))
                 //   keepTrying = true;
            }

            return this;
        }

        public ACPO RenewalClickContinueToNextSection()
        {

            Click("#wizardNextButton");

            return this;
        }

        public ACPO WaitForEmissions()
        {
            WaitForComplete();
            FindBy("#emissionsTabs > span.angledbuttonTab.angledTabFirst.selected");

            return this;
        }

        public ACPO SetNoAB2588()
        {
            Click("input#HasAB2588Emissions[value='False']");
            return this;
        }

        public ACPO SubmitAsEngineer()
        {
            Click("#applicationFooter > a.engineer_submit_button");

            WaitForComplete();

            return this;
        }

        public ACPO SubmitAsExternalUser()
        {
            
            Click("#applicationFooter > a#footerSubmitEmissions");
            Sleep(500);

            Click("#submitConfirmYesButton");

            WaitForComplete();

            return this;
        }

        public ACPO ClickRenewal()
        {
            FindBy("a.facilityRenewalButtonIcon");

            ExecuteScript("$('a.facilityRenewalButtonIcon:visible')[0].click()");

            WaitForComplete();

            return this;

        }

        public ACPO ClickRenewalFeesTab()
        {
            Click("div[rel = 'RenewalFees']");

            WaitForComplete();

            return this;
        }

        public ACPO PayRenewalFees()
        {

            Click("a.renewal_pay_fees:nth-child(2)");

            WaitForComplete();

            FindElement(By.Id("IsSubmittalCertification")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();
            FindElement(By.Id("isOnlinePaymentYes")).Click();
            FindElement(By.Id("CardOwnerName")).Clear();
            FindElement(By.Id("CardOwnerName")).SendKeys("Jack Spade");
            FindElement(By.Id("cardNumber")).Clear();
            FindElement(By.Id("cardNumber")).SendKeys("4111111111111111");
            FindElement(By.Id("expirationDate")).Clear();
            FindElement(By.Id("expirationDate")).SendKeys("1219");
            FindElement(By.Id("cardCode")).Clear();
            FindElement(By.Id("cardCode")).SendKeys("123");
            FindElement(By.Id("UseContactAddress")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();

            WaitForComplete(120);

            return this;
        }
        public ACPO TransferOwnershp()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='startTransferOfOwnership']"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<ACPO>(url);
        }
        public ACPO logout()
        {
            var ctrl =  FindElement(By.XPath(".//*[@id='AccountManagement']/a[2]"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<ACPO>(url);
        }
        public ACPO VerifyLoginPage(string emailaddress)
        {

            emailaddress = SeleniumUITests.SharedProperties["EmailAddress"];

                     
            FindElement(By.XPath(".//*[@id='LogInSubmit']")).Click();
            FindElement(By.XPath(".//*[@id='UserName']")).SendKeys(emailaddress);
            FindElement(By.XPath(".//*[@id='LogInSubmit']")).Click();
            FindElement(By.XPath(".//*[@id='Password']")).SendKeys("TEST123");
            FindElement(By.XPath(".//*[@id='LogInSubmit']")).Click();

            FindElement(By.XPath(".//*[@id='ShowCantAccessDialog']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='Button1']")).Click();

            FindElement(By.XPath(".//*[@id='ShowForgotPasswordDialog']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ForgotCancel']")).Click();
            FindElement(By.XPath(".//*[@id='ShowForgotPasswordDialog']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ForgotPasswordEmail']")).SendKeys(emailaddress);
            FindElement(By.XPath(".//*[@id='ForgotSubmit']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ForgotContainer']/div[3]/p[2]/input")).Click();
            WaitForComplete();
            return NavigateTo<ACPO>();
        }
        public ACPO TransferProcessOverview(string NewFacilityName = "New Facility Name", string NewOwningName = "New Owning Name",
            string typeOfBusiness = "Local Government", DateTime? PastDate = null, string firstname = "joe", string lastName = "OWNER", string facilityAddressLine1 = "424 32nd Ave",
            string ZIPCODE = "94121", string city = "San Francisco", string email = "joe@testing.com", string phone = "4155555555")
        {
            if (!PastDate.HasValue)
                PastDate = DateTime.Today.AddDays(-1);

            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='NewFacilityDetails_FacilityName']")).SendKeys(NewFacilityName);
            FindElement(By.XPath(".//*[@id='NewBusinessEntity_Entity_CompanyName']")).SendKeys(NewOwningName);
            new SelectElement(FindElement(By.XPath(".//*[@id='NewBusinessEntity_Entity_BusinessEntityTypeId']"))).SelectByText(typeOfBusiness);
            //past date
            FindElement(By.XPath(".//*[@id='OwnershipStartDate']")).Click();
            FindElement(By.XPath(".//*[@id='OwnershipStartDate']")).Clear();
            FindElement(By.XPath(".//*[@id='OwnershipStartDate']")).SendKeys(PastDate.Value.ToShortDateString());
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_LastName']")).Click();

            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_FirstName']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_FirstName']")).SendKeys(firstname);
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_LastName']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_LastName']")).SendKeys(lastName);

            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_AddressLine1']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_AddressLine1']")).SendKeys(facilityAddressLine1);
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_ZipCode']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_ZipCode']")).SendKeys(ZIPCODE);
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_City']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_Address_City']")).SendKeys(city);

            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_EmailAddress']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_EmailAddress']")).SendKeys(email);
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_PhoneNumber']")).Clear();
            FindElement(By.XPath(".//*[@id='ContactInfo_Entity_PhoneNumber']")).SendKeys(phone);
            // upload a file
            FindElement(By.XPath(".//*[@id='Documents_Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            // upload a file
            FindElement(By.XPath(".//*[@id='Documents_Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);

            // upload a file
            FindElement(By.XPath(".//*[@id='Documents_Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            WaitForComplete();
            return this;
        }
        public ACPO CurrentOperatorContact()
        {
            FindElement(By.XPath(".//*[@id='contactPickerSelector']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='materialList']/div[2]")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='contactPickerSelector']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='materialList']/div[2]")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath("//td[6]/input")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            WaitForComplete();
            return this;

        }
        public ACPO PayAndSubmitTransfer()
        {

            FindElement(By.XPath(".//*[@id='applicationFooter']/a[2]/span")).Click();
            FindElement(By.XPath(".//*[@id='contactPickerSelector']")).Click();
            FindElement(By.XPath(".//*[@id='materialList']/div[2]")).Click();

            WaitForComplete();

            FindElement(By.Id("IsSubmittalCertification")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();
            FindElement(By.Id("isOnlinePaymentYes")).Click();
            FindElement(By.Id("CardOwnerName")).Clear();
            FindElement(By.Id("CardOwnerName")).SendKeys("Jack Spade");
            FindElement(By.Id("cardNumber")).Clear();
            FindElement(By.Id("cardNumber")).SendKeys("4111111111111111");
            FindElement(By.Id("expirationDate")).Clear();
            FindElement(By.Id("expirationDate")).SendKeys("1219");
            FindElement(By.Id("cardCode")).Clear();
            FindElement(By.Id("cardCode")).SendKeys("123");
            FindElement(By.Id("UseContactAddress")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();

            WaitForComplete(120);

            return this;
        }
        public ACPO SaveStartUpInformation()
        {
            FindElement(By.XPath("//td[8]/span")).Click();
            WaitForComplete();
            foreach (var e in wd.FindElements(By.CssSelector("span.expandCollapse")))
            {
                e.Click();
            }
            FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            //Save Dates
            foreach (var e in wd.FindElements(By.CssSelector("a.button.saveInformation")))
            {
                e.Click();
            }
            
            return NavigateTo<ACPO>();
        }
    }
}
