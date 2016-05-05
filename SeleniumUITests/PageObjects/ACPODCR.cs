using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class ACPODCR : PageObject
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

        public ACPOCreateNewFacilityDCR ClickCreateNewDCRFacility()
        {
            FindElement(By.Id("createNewFacility")).Click();
            return this.NavigateTo<ACPOCreateNewFacilityDCR>();
        }

        public ACPOCreateNewDCRApplicationPermit CreateNewDCRapplication()
        {
            FindElement(By.Id("startNewApplication2")).Click();
            return this.NavigateTo<ACPOCreateNewDCRApplicationPermit>();
        }
        public ACPOSignin ClickSignin()
        {
           return this.NavigateTo<ACPOSignin>();
        }

       


    }
}
