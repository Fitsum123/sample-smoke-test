using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.WebUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;





namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    public partial class SeleniumUITests
    {
        [TestMethod] //<== This method creates ACPO login page and login as an external user. 
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateExternalUsers()
        {
            Console.WriteLine("Starting Sign Up Test");

            var rnd = new Random();
            var number = rnd.Next(1000, 9999);
            //Read Data from Excel, each column is a field. 
            string UserName = "test" + number.ToString() + "@test.com";
            string Password = "Possum100";

            //Root Page of this test
            var page = new ACPOSignin();

            //Run Create External User
            page.CreateExternalUser(UserName, Password);

            Console.WriteLine("Finished Sign Up Test");


            SeleniumUITests.SharedProperties["UserName"] = UserName;
            SeleniumUITests.SharedProperties["Password"] = Password;

            //Test for Console.Error messages -- Every test should end with this line.
            CheckForErrors();
        }


        [TestMethod] //<== This method Create A New DCR Facility
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
                                  // [DeploymentItem("TestData.xls")] //<== Use the Excel spreadsheet for data
                                  // [DataSource("CreateNewFacility")] //<== Use this tab in Excel

        public void SeleniumCreateNewDCRFacility()
        {
            //Create a new user - at end of test on acpo pages
            SeleniumCreateExternalUsers();

            //Create a facility.
            var page = new ACPODCR();
            page.ClickCreateNewDCRFacility().testCreateFacilityDCR();
          
        }
        [TestMethod] //<== This method creates New Permit Application for DCR device
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateDCRfacilityApplicationPermit()
        {
            SeleniumCreateNewDCRFacility();
            
            //Create dcr application - will not be paid

            var page = new ACPOCreateNewDCRApplicationPermit();
            page.CreateHeaderApplicationTitleDCR();
            page.CreateDeviceInformationForDCRApplication();
            page.PaymentForDCRApplication();

            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];
            // var vFacilityNum = SeleniumUITests.SharedProperties["FacilityNumber"];
            //log in as TestEngineer1, pay invoice <AQT apparently cannot>
            //this method login as Engineer - Approve application and issue permit            
            page.NavigateTo<SPA1>()
             .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .TestIC2ApproveDCRApplicationIssuePermit(facNumber);

        }

        [TestMethod] //<== This method login as External User - Save Start-Up information
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumTestSaveStartUpForDCR()
        {
            SeleniumCreateDCRfacilityApplicationPermit();

            string UserName = SeleniumUITests.SharedProperties["UserName"];
            string Password = SeleniumUITests.SharedProperties["Password"];

            var page = new ACPOSignin();
            page.SignAsExternalNewUse(UserName, Password);
            page.testACPOSaveStartUpInformationForDCR();
            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];

            // Engineer - Issue permit
            page.NavigateTo<SPA1>()
            .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .testIC2IssuePermitDCR(facNumber);

        }
        [TestMethod] //<== This method login as ENGINEER: startup device using Admin tool and login as Engineer: Admin tool (renewal at 149 days) 
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly

        public void SeleniumTestStartUpdeviceDCR()
        {
            SeleniumTestSaveStartUpForDCR();
            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];
            var page = new SPA1();
            page.NavigateToQAAdmin().TestIC2AdminToolStartupDCRdevice(facNumber);
           // login as Engineer: Admin tool (renewal at 149 days) 
            page.TestIC2AdminToolDCRRenewa149(facNumber);

        }
       

        [TestMethod] //<== This method login as Engineer - Check renewal invoice
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly

        public void SeleniumTestIC2CheckRenewalInvoiceDCR()
        {
            SeleniumTestStartUpdeviceDCR();
           // SeleniumtestIC2AdminToolDCRRenewal149();
            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];
            var page = new SPA1();
            page.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password).testIC2CheckRenewalInvoiceDCR(facNumber);

        }
        
     
        [TestMethod] //<== This method login as Engineer: Admin tool (renewal at 59 days)
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly

        public void SeleniumtestIC2AdminToolDCRRenewal59()
        {
            SeleniumTestIC2CheckRenewalInvoiceDCR();
           // SeleniumTestACPOCheckRenewalBarBoxDCR();
            var renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"];
            var facNumber = SeleniumUITests.SharedProperties["FacilityNumber"];
          
            var page = new SPA1();
            page.NavigateToQAAdmin().TestIC2AdminToolDCRRenewal59(renewalNumber, facNumber);
        }
        [TestMethod] //<== This indicates method login External User - Pay for renewal
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly

        public void SeleniumTestACPOPayRenewalDCR()
        {
            SeleniumtestIC2AdminToolDCRRenewal59();

            string UserName = SeleniumUITests.SharedProperties["UserName"];
            string Password = SeleniumUITests.SharedProperties["Password"];

            var page1 = new ACPOSignin();
            page1.SignAsExternalNewUse(UserName, Password);
            page1.testACPOPayRenewalDCR();


        }

    }


    }



