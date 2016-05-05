using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class ACPOGDF : PageObject
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

        public ACPOCreateNewFacilityGDF ClickCreateNewGDFFacility()
        {
            FindElement(By.Id("createNewFacility")).Click();
            return this.NavigateTo<ACPOCreateNewFacilityGDF>();
        }


    }
}
