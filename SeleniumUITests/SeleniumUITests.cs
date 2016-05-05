using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.WebUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentDate;
using FluentDateTime;

namespace Gov.Baaqmd.Tests.SeleniumUITests
{

    [DeploymentItem("Microsoft.Practices.ServiceLocation.dll")]
    public partial class SeleniumUITests
    {
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        //[TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateExternalUser()
        {
            Console.WriteLine("Starting Sign Up Test");

            var rnd = new Random();
            var number = rnd.Next(1000, 9999);
            //Read Data from Excel, each column is a field. 
            string UserName = "test" + number.ToString() + "@test.com";
            string Password = "Possum100";

            //Root Page of this test
            var page = new ACPOLogin();

            //Run Create External User
            page.CreateExternalUser(UserName, Password);

            Console.WriteLine("Finished Sign Up Test");

            Console.WriteLine("Created {0} / {1}", UserName, Password);


            //Test for Console.Error messages -- Every test should end with this line.
            CheckForErrors();
        }

        
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        //[TestCategory("Nightly")] //<== This is to be run nightly
       // [DeploymentItem("TestData.xls")] //<== Use the Excel spreadsheet for data
       // [DataSource("CreateNewFacility")] //<== Use this tab in Excel

        public void SeleniumCreateNewFacility()
        {
            //Create a new user - at end of test on acpo pages
            SeleniumCreateExternalUser();

            //var facilityName = context.DataRow["FacilityName"].ToString();
            //var OwningEntityName = context.DataRow["OwningEntityName"].ToString();
            //var TypeOfOrganization = context.DataRow["TypeOfOrganization"].ToString();
            //var FacilityStreetAddress1 = context.DataRow["FacilityStreetAddress1"].ToString();
            //var FacilityCity = context.DataRow["FacilityCity"].ToString();
            //var FacilityZipCode = context.DataRow["FacilityZipCode"].ToString();
            //var OwnerFirstName = context.DataRow["OwnerFirstName"].ToString();
            //var OwnerLastName = context.DataRow["OwnerLastName"].ToString();
            //var OwnerBusinessName = context.DataRow["OwnerBusinessName"].ToString();
            //var OwnerContactTitle = context.DataRow["OwnerContactTitle"].ToString();
            //var OwnerStreetAddress1 = context.DataRow["OwnerStreetAddress1"].ToString();
            //var OwnerCity = context.DataRow["OwnerCity"].ToString();
            //var OwnerZipCode = context.DataRow["OwnerZipCode"].ToString();
            //var OwnerEmail = context.DataRow["OwnerEmail"].ToString();
            //var OwnerPrimaryPhone = context.DataRow["OwnerPrimaryPhone"].ToString();
            

            //Create a facility.
            var page = new ACPO();
            SharedProperties["FacilityNumber"] = page.ClickCreateNewFacility().CreateNewFacility();
            //.CreateNewFacility(facilityName, OwningEntityName, TypeOfOrganization, FacilityStreetAddress1, FacilityCity, FacilityZipCode,OwnerFirstName,OwnerLastName,OwnerBusinessName,OwnerContactTitle, OwnerStreetAddress1,OwnerCity,OwnerZipCode,OwnerEmail, OwnerPrimaryPhone);
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateAsbestosDemo_18804()
        {
            SeleniumCreateExternalUser();

            //Create asbestos application - will not be paid
            var page = new ACPO().CreateNewAsbestosJob();

            page
                .CreateAsbestosJobHeader()
                .AddContractorDetails()
                .AddLocationDetails()
                .AddFireTrainingDetails()
                .AddLabAnalysis()
                .AddStartEndDates()
                .AddDemoCertification()
                .AddManualPayment()
                .VerifyApplicationStatus();

            var appNumber = SeleniumUITests.SharedProperties["ApplicationNumber"];

            //log in as TestEngineer1, pay invoice <AQT apparently cannot>            
            page.NavigateTo<SPA>()
            .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateToQAAdmin()
                .PayInvoiceByAppNumber(appNumber)
                //return to the asbestos portal
                .NavigateTo<ACPOAsbestos>(SeleniumUITests.SharedProperties["AsbestosJobPage"])
                .WaitForApproved();             
        }


        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateAsbestosDemo_19102_S1_7()
        {
            SeleniumCreateExternalUser();

            //Create asbestos application - will not be paid
            var page = new ACPO().CreateNewAsbestosJob();

            page
                .CreateAsbestosJobHeader()
                .AddContractorDetails()
                .AddLocationDetails()
                .AddFireTrainingDetails(isFireTraining: false)
                .AddLabAnalysis()
                .AddStartEndDates(isNightWork: true)
                .AddDemoCertification()
                .VerifyApplicationStatus(targetStatus: "Under Evaluation");
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SeleniumCreateAsbestosReno_19140()
        {
            SeleniumCreateExternalUser();

            //Create asbestos application - will not be paid
            var page = new ACPO().CreateNewAsbestosJob();

            page
                .CreateAsbestosJobHeader(asbestosJobTitle: "New Asbestos Renovation Unit Test 19140", asbestosType: BusinessObjects.GenericLookups.BusinessEventSubType.BusinessEventSubTypeEnum.AsbestosRenovation, isNotificationForPastOrCurrentJob: false)
                .AddContractorDetails()
                .AddLocationDetails()
                .AddRenovationDetails()
                .AddStartEndDates(startDate: DateTime.Today.AddDays(1), endDate: DateTime.Today.AddDays(1))
                .AddRenoCertification()
                .AddPayOnline()
                .VerifyApplicationStatus(targetStatus: "Approved");
        ;
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Not Complete")]
        public void NOTCOMPLETE_SeleniumCreateAsbestosDemo_19323()
        {
            SeleniumCreateExternalUser();

            //Create asbestos application - will not be paid
            var page = new ACPO().CreateNewAsbestosJob();

            page
                .CreateAsbestosJobHeader(asbestosJobTitle: "Fee Test Case 19323", asbestosType: BusinessObjects.GenericLookups.BusinessEventSubType.BusinessEventSubTypeEnum.AsbestosDemolition, isNotificationForPastOrCurrentJob: false
                )
                .AddContractorDetails()
                .AddLocationDetails(locationBuildingType: "Single Family Dwelling or 4 units or less", address1: "939 ellis st", city: "San Francisco", postalCode: "94131")
                .AddFireTrainingDetails(isFireTraining: false, removalMethod: BusinessObjects.GenericLookups.ActivityMethodType.ActivityMethodTypeEnum.ByHand)
                .AddLabAnalysis()
                .AddStartEndDates(startDate: DateTime.Today.AddBusinessDays(3), endDate: DateTime.Today.AddBusinessDays(5));

                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(5));

            return;
                //.AddDemoCertification()
                //.AddManualPayment()
                //.VerifyApplicationStatus();

            var appNumber = SeleniumUITests.SharedProperties["ApplicationNumber"];

            //log in as TestEngineer1, pay invoice <AQT apparently cannot>            
            page.NavigateTo<SPA>()
            .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateToQAAdmin()
                .PayInvoiceByAppNumber(appNumber)
                //return to the asbestos portal
                .NavigateTo<ACPOAsbestos>(SeleniumUITests.SharedProperties["AsbestosJobPage"])
                .WaitForApproved();

            
        }

    }
}
