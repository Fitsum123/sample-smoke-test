using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.WebUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;





namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    public partial class SeleniumUITests
    {

        [TestMethod] //<== This method Create A New GDF Facility
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
                                  // [DeploymentItem("TestData.xls")] //<== Use the Excel spreadsheet for data
                                  // [DataSource("CreateNewFacility")] //<== Use this tab in Excel

        public void SeleniumCreateNewGDFFacility()
        {
            //Create a new user - at end of test on acpo pages
            SeleniumCreateExternalUsers();

            //Create a facility.
            var page = new ACPOGDF();
            page.ClickCreateNewGDFFacility().testCreateFacilityGDF();


        }
        [TestMethod] //<== This method creates New Permit Application for GDF device
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateGDFApplicationPermit()
        {
            SeleniumCreateNewGDFFacility();

            var page = new ACPOCreateNewGDFApplicationPermit();
            page.CreateHeaderApplicationTitleGDF();
            page.CreateDeviceInformationForGDFApplication();
            page.PaymentForGDFApplication();

            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];

           page.NavigateTo<SPAGDF>()
         .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
           .TestIC2ReviewGDFApplicationRegulationCondition(facNumber);

        }
       
    }
}