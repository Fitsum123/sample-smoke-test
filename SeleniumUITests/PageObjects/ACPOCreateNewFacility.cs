using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class ACPOCreateNewFacility : PageObject
    {
        public override string PageRelativeUrl
        {
            get
            {
                return "ACPO";
            }
        }

        public string CreateNewFacility(string facilityName = "Test123",
            string owningEntityName = "TestING-Wnning",
            string typeOfBusiness = "Local Government",
            string facilityAddressLine1 = "401 jacklin road",
            string facilityCity = "Milpitas",
            string facilityZipCode = "95035",
            string contactFirstName = "First",
            string contactLastName = "Last",
            string contactEmail = "testEmail@nospam.nope",
            string contactPhone = "(415)555-1212")
        {
            var driver = this;

            enterFacilityNameAndLocation(facilityName, owningEntityName, typeOfBusiness, facilityAddressLine1, facilityCity, facilityZipCode);

            driver.FindElement(By.Id("wizardNextButton")).Click();

            selectFacilityParcel();

            moveToWizardPage("FACILITY CONTACTS: FACILITY OWNER");
            fillOutContactPicker("FacilityOwnerContact", contactFirstName, contactLastName, contactEmail, contactPhone, facilityAddressLine1, facilityCity, facilityZipCode);

            moveToWizardPage("FACILITY CONTACTS: FACILITY OPERATOR");
            fillOutContactPicker("FacilityOperatorContact", contactFirstName, contactLastName, contactEmail, contactPhone, facilityAddressLine1, facilityCity, facilityZipCode);

            moveToWizardPage("FACILITY CONTACTS: BILLING CONTACT");
            fillOutContactPicker("BillingContact", contactFirstName, contactLastName, contactEmail, contactPhone, facilityAddressLine1, facilityCity, facilityZipCode);

            moveToWizardPage("PRIMARY OPERATION OF FACILITY: NAICS IDENTIFICATION");
            driver.FindElement(By.Id("PrimaryOperationCategory2")).Click();

            moveToWizardPage("NEW FACILITY SUMMARY");
            driver.FindElement(By.Id("wizardNextButton")).Click();

            WaitForComplete();
            var ctrl = FindBy(@"//*[@id=""facilityDetails""]/div[1]/div[2]/span[2]", SelectorType.XPath);
            String vFacilityNum = ctrl.Text;
            Console.WriteLine(vFacilityNum);
            return vFacilityNum;
        }

        /// <summary>
        /// Fills out the current contact picker
        /// </summary>
        /// <param name="cssPrefix"></param>
        /// <param name="contactFirstName"></param>
        /// <param name="contactLastName"></param>
        /// <param name="contactEmail"></param>
        /// <param name="contactPhone"></param>
        /// <param name="facilityAddressLine1"></param>
        /// <param name="facilityCity"></param>
        /// <param name="facilityZipCode"></param>
        private void fillOutContactPicker(string cssPrefix, string contactFirstName, string contactLastName, string contactEmail, string contactPhone, string facilityAddressLine1, string facilityCity, string facilityZipCode)
        {
            Click(@"//*[@id=""contactPickerSelector""]/div[3]/img", SelectorType.XPath);
            //Select New Contact
            Click(@"//*[@id=""materialList""]/div[1]/div/div[1]", SelectorType.XPath);
            SendKeys("#" + cssPrefix + "_FirstName", contactFirstName);
            SendKeys("#" + cssPrefix + "_LastName", contactLastName);
            SendKeys("#" + cssPrefix + "_Address_AddressLine1", facilityAddressLine1);
            SendKeys("#" + cssPrefix + "_Address_City", facilityCity);
            SendKeys("#" + cssPrefix + "_Address_ZipCode", facilityZipCode);
            SendKeys("#" + cssPrefix + "_EmailAddress", contactEmail);
            SendKeys("#" + cssPrefix + "_PhoneNumber", contactPhone);
        }

        /// <summary>
        /// Keeps clicking next until Wizard page title matches target page
        /// </summary>
        /// <param name="targetPage"></param>
        private void moveToWizardPage(string targetPage)
        {
            WaitForComplete(); //wait until document is complete.

            //Keep clicking next until we are on the the desired page
            //why? certain steps may have intermediate pages, for example if the facility already exists
            //a page comes up that we can just bypass..

            IWebElement target = FindBy(@"//*[@id=""wizardPageTitle""]", SelectorType.XPath);
            var iRepeatCount = 10;
            while (!target.Text.Contains(targetPage) && --iRepeatCount > 0)
            {
                FindElement(By.Id("wizardNextButton")).Click();
                WaitForComplete(); //wait unti document is complete.
                target = FindBy(@"//*[@id=""wizardPageTitle""]", SelectorType.XPath);
            }

            if (iRepeatCount == 0) //We've tried 10 times, not gonna happen
                //TODO - maybe look for error validator instead...
                throw new Gov.Baaqmd.BaaqmdException("Cannot find page");
        }

        /// <summary>
        /// Selects the parcel on the map
        /// </summary>
        private void selectFacilityParcel()
        {
            FindElement(By.XPath(".//*[@id='map']/div/canvas")).Click();
        }

        /// <summary>
        /// Fills out the facility location
        /// </summary>
        /// <param name="facilityName"></param>
        /// <param name="owningEntityName"></param>
        /// <param name="typeOfBusiness"></param>
        /// <param name="facilityAddressLine1"></param>
        /// <param name="facilityCity"></param>
        /// <param name="facilityZipCode"></param>
        private void enterFacilityNameAndLocation( string facilityName, string owningEntityName, string typeOfBusiness, string facilityAddressLine1, string facilityCity, string facilityZipCode)
        {
            var driver = this;

            driver.FindElement(By.Id("FacilityInfo_FacilityName")).Clear();
            driver.FindElement(By.Id("FacilityInfo_FacilityName")).SendKeys(facilityName);
            driver.FindElement(By.Id("FacilityInfo_OwningEntityName")).Clear();
            driver.FindElement(By.Id("FacilityInfo_OwningEntityName")).SendKeys(owningEntityName);
            new SelectElement(driver.FindElement(By.Id("FacilityInfo_TypeOfBusiness"))).SelectByText(typeOfBusiness);
            driver.FindElement(By.Id("FacilityInfo_Address_AddressLine1")).Clear();
            driver.FindElement(By.Id("FacilityInfo_Address_AddressLine1")).SendKeys(facilityAddressLine1);
            driver.FindElement(By.Id("FacilityInfo_Address_City")).Clear();
            driver.FindElement(By.Id("FacilityInfo_Address_City")).SendKeys(facilityCity);
            driver.FindElement(By.Id("FacilityInfo_Address_ZipCode")).Clear();
            driver.FindElement(By.Id("FacilityInfo_Address_ZipCode")).SendKeys(facilityZipCode);
        }
    }
}
