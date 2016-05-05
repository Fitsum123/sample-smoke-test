using Breeze.Sharp;
using Gov.Baaqmd.BusinessObjects;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Gov.Baaqmd.Tests.Breeze;
using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.WebUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    public partial class SeleniumUITests
    {
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        //[TestCategory("Nightly")] //<== This is to be run nightly
        [DeploymentItem("TestData.xls")] //<== Use the Excel spreadsheet for data
        [DataSource("ACPOSignIn")] //<== Use this tab in Excel
        public void SeleniumACPOLogin()
        {
            //Read Data from Excel, each column is a field. 
            string UserName = context.DataRow["UserName"].ToString();
            string Password = context.DataRow["Password"].ToString();

            //Root Page of this test
            var page = new ACPOLogin();

            //Run login
            page.Login(UserName, Password);


            //Test for Console.Error messages -- Every test should end with this line.
            CheckForErrors();
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Dev")]
        public void SeleniumCreateTwoApplications()
        {
            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddNonHalogenatedDryCleaningMachine( deviceName: "D One")
                .SubmitAndPay(populateHrsa: false)
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            ////Let's approve app....
            //var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            //approveAndStartDevices(spa,
            //    (SeleniumUITests.SharedProperties["FacilityNumber"]), (SeleniumUITests.SharedProperties["ApplicationNumber"]));


            ////lets do it again..
            //var acpoLogin = new ACPOLogin();

            ////login as external user, create new app.
            //acpoLogin.Login(SeleniumUITests.SharedProperties["UserName"], SeleniumUITests.SharedProperties["Password"])
            //    .NavigateTo<ACPO>()
            //    .ClickCreateNewApplication()
            //    .PopulateApplicationHeader()
            //    .AddNonHalogenatedDryCleaningMachine(deviceName: "D Two")
            //    .SubmitAndPay(populateHrsa: false)
            //    .WaitForEvaluatingPermitApplicationCompleteness();



        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumCreateNewDCP_17100_19050()
        {

            string facilityNumber, renewalNumber, applicationNumber;

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddHalogenatedDryCleaningMachine(startupDate: DateTime.Today.AddDays(1), 
                    dryCleaningFacilityType: DryCleaningFacilityType.DryCleaningFacilityTypeEnum.CoCommercial, 
                    manufacturer: DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum.Escape, 
                    machineType: MachineType.MachineTypeEnum.DipTank, drumCapacity: "40",
                    materialName: "Perchloroethylene", annualUsage: "25")
                .SubmitAndPay()
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa,
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            //Start of TC 19050

            //Run renewal
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(149)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(119))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(89))
                .RunRenewalForFacility(facilityNumber);

            //Verify renewal
            spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .VerifyUpdateStatus("Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Not Submitted", "Not Issued");

            //Check print queue for docs
            spa.NavigateTo<SPA>()
            .NavigateToPrintQueue()
                .CheckPrintQueueForDocuments(renewalNumber, null,
                    "UpdateReminder2_",
                    "UpdateReminder1_",
                    "UpdateRenewal_"
                );

            //Submit changes as an engineer
            spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))

                .NavigateToRenewalUpdate()
                .RenewalSupplyUpdatePeriodEndDate()
                .RenewalUpdateAllAnnualUsages()
                .RenewalClickContinueToNextSection()
                .WaitForEmissions()
                .SetNoAB2588()
                .RenewalClickContinueToNextSection()

                .RenewalClickContinueToNextSection() //On EvalPage->Conditions page
                .RenewalClickContinueToNextSection()  //On Conditions Page -> Fees Page

                .SubmitAsEngineer()
                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyUpdateStatus("Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Evaluation Complete", "Not Submitted", "Not Issued");

            //Go to invoice Issued
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .UpdateRenewalDates(DateTime.Today.AddDays(59)) //Invoice Issued
                .RunRenewalForFacility(facilityNumber)
                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyRenewalStatus("Invoice Issued", "Evaluation Complete", "Not Submitted", "Not Issued");

            //Check print queue for docs
            spa.NavigateTo<SPA>()
                .NavigateToPrintQueue()
                    .CheckPrintQueueForDocuments(renewalNumber, null,
                        "-Renewal.pdf",
                        "UpdateClosed_",
                        "UpdateReminder2_",
                        "UpdateReminder1_",
                        "UpdateRenewal_"
                    )
                ;
            /*
            cannot pay as customer - we have JDE Failure
            end test.
            var page = new ACPOLogin();

            //Run login
            page.Login(userName, password)
                .ClickRenewal()
                .ClickRenewalFeesTab()
                .PayRenewalFees()
                ;
            */
        }



        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        public void NOTCOMPLETE_SeleniumCreateNewGDF()
        {

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                
                //.AddNonHalogenatedDryCleaningMachine()

                //.SubmitAndPay(populateHrsa: false)
                //.WaitForEvaluatingPermitApplicationCompleteness()
                ;

            System.Threading.Thread.Sleep(TimeSpan.FromHours(1));

        }


        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Not Complete")]
        public void NOTCOMPLETE_SeleniumCreateNewBackupDiesel()
        {
            string facilityNumber, renewalNumber, applicationNumber;

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            var devices = page.ClickCreateNewApplication()
                .PopulateApplicationHeader(hasCEQA: true);

            devices
                .AddBackupDiesel(serialNumber: "");

            //devices
            //    .AddEmissionPoint();

            devices
                .SubmitAndPay(populateHrsa: true);

            devices
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa,
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            return; 

            //Run renewal
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(149)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(119))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(89))
                .RunRenewalForFacility(facilityNumber);

            //Verify renewal
            spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .VerifyUpdateStatus("Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Not Submitted", "Not Issued");

            //Submit changes as the user
            var renewalUrl = spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .GetRenewalUpdateUrl();


            spa.ClearAllCookies();
            //Relogin as new user
            var p = new ACPOLogin();
            p.Login(SeleniumUITests.SharedProperties["UserName"], SeleniumUITests.SharedProperties["Password"])
                .NavigateTo<ACPO>(renewalUrl)
                .RenewalSupplyUpdatePeriodEndDate()
                .RenewalUpdateAllAnnualUsages()
                .RenewalClickContinueToNextSection()
                .WaitForEmissions()
                .SetNoAB2588()
                .RenewalClickContinueToNextSection()

                .RenewalClickContinueToNextSection() //On EvalPage->Conditions page
                .RenewalClickContinueToNextSection()  //On Conditions Page -> Fees Page

                .SubmitAsExternalUser();

            new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyUpdateStatus("Update Accepted", "Update Submitted", "Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Evaluation Complete", "Not Submitted", "Not Issued");

        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Not Complete")]
        public void NOTCOMPLETE_SeleniumCreateNewAutobody()
        {

            string facilityNumber, renewalNumber, applicationNumber;

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddAutobody()
                .SubmitAndPay(populateHrsa: false)
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa,
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            //Run renewal
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(149)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(119))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(89))
                .RunRenewalForFacility(facilityNumber);

            //Verify renewal
            spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .VerifyUpdateStatus("Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Not Submitted", "Not Issued");

            //Submit changes as the user
            var renewalUrl = spa.NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .GetRenewalUpdateUrl();


            spa.ClearAllCookies();
            //Relogin as new user
            var p = new ACPOLogin();
            p.Login(SeleniumUITests.SharedProperties["UserName"], SeleniumUITests.SharedProperties["Password"])
                .NavigateTo<ACPO>(renewalUrl)
                .RenewalSupplyUpdatePeriodEndDate()
                .RenewalUpdateAllAnnualUsages()
                .RenewalClickContinueToNextSection()
                .WaitForEmissions()
                .SetNoAB2588()
                .RenewalClickContinueToNextSection()

                .RenewalClickContinueToNextSection() //On EvalPage->Conditions page
                .RenewalClickContinueToNextSection()  //On Conditions Page -> Fees Page

                .SubmitAsExternalUser();

            new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyUpdateStatus("Update Accepted", "Update Submitted", "Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Evaluation Complete", "Not Submitted", "Not Issued");

        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumCreateNewDCR_FastTrack()
        {

            string facilityNumber, renewalNumber, applicationNumber;

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddNonHalogenatedDryCleaningMachine()
                .SubmitAndPay(populateHrsa: false)
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa,
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            //Run renewal
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(149)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(119))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(89))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(59)) //invoice issued
                .RunRenewalForFacility(facilityNumber)


            //Verify renewal
            .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .VerifyUpdateStatus("Not Required")
                .VerifyRenewalStatus("Invoice Issued", "Evaluation Complete", "Not Issued")
                .VerifyInvoiceDueDate(DateTime.Today.AddDays(59))  //due date should be the invoice issued date.

            //Check print queue for docs
            .NavigateTo<SPA>()
            .NavigateToPrintQueue()
                .CheckPrintQueueForDocuments(renewalNumber, null,
                    "-Renewal.pdf"
                )

            //Make the renewal go late
            .NavigateTo<IC2>("IC2/Facilities/Renewal/{0}/{1}/".FormatString(facilityNumber, renewalNumber))
                .NavigateToRenewalFees()
                .ChangeInvoiceDueDate(DateTime.Today.AddDays(-1))

                .NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .RunRenewalForFacility(facilityNumber)

                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyRenewalStatus("Late Notice Sent");


            //Only grab documents past now - to pick up late document
            DateTime searchTime = DateTime.Now;

            //let's change the invoice late print date.
            var cookie = spa.Cookie;
            var breezy = new Breezy();
            var invoice = breezy.ExecuteQuery(breezy.PrepQuery<Invoice>().Where(n => n.AccountingSystemInvoiceNum == SeleniumUITests.SharedProperties["InvoiceNumber"]), cookie).Last();
            Assert.AreEqual(DateTime.Today.AddDays(10).Date, invoice.WarningLetterDate.Value.Date);

            //to use the dde, we need the invoice ID.  only way to get that is throguh breeze
            spa.NavigateTo<SPA>("/spa/#/dde/edit/Invoice/{0}".FormatString(invoice.InvoiceId))
                .DDE_UpdateWarningLetterDate(DateTime.Today.AddDays(-2));

            //Run renewal again
            spa.NavigateToQAAdmin()
                .RunRenewalForFacility(SeleniumUITests.SharedProperties["FacilityNumber"]);


            spa.NavigateToPrintQueue()
                .CheckPrintQueueForDocuments(SeleniumUITests.SharedProperties["RenewalNumber"], searchTime,
                    "-Renewal.pdf");

        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumCreateNewDCR_18635()
        {

            string facilityNumber, renewalNumber, applicationNumber;

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddNonHalogenatedDryCleaningMachine()
                .AddHalogenatedDryCleaningMachine()
                .SubmitAndPay()
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;
            
            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa, 
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            //Run renewal
            spa.NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(149)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(119))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(89))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(59)) //invoice issued
                .RunRenewalForFacility(facilityNumber)


            //Verify renewal
            .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, (renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])))
                .VerifyUpdateStatus("Update Window Closed", "Warning Sent", "Reminder Sent", "Update Requested", "Not Required")
                .VerifyRenewalStatus("Invoice Issued", "Not Submitted", "Not Issued")
                .VerifyInvoiceDueDate(DateTime.Today.AddDays(59))  //due date should be the invoice issued date.

            //Check print queue for docs
            .NavigateTo<SPA>()
            .NavigateToPrintQueue()
                .CheckPrintQueueForDocuments(renewalNumber, null,
                    "-Renewal.pdf",
                    "UpdateClosed_",
                    "UpdateReminder2_",
                    "UpdateReminder1_",
                    "UpdateRenewal_"
                )

            //Make the renewal go late
            .NavigateTo<IC2>("IC2/Facilities/Renewal/{0}/{1}/".FormatString(facilityNumber, renewalNumber))
                .NavigateToRenewalFees()
                .ChangeInvoiceDueDate(DateTime.Today.AddDays(-1))

                .NavigateTo<SPA>()
                .NavigateToQAAdmin()
                .RunRenewalForFacility(facilityNumber)

                .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
                .VerifyRenewalStatus("Late Notice Sent");


            //Only grab documents past now - to pick up late document
            DateTime searchTime = DateTime.Now;

            //let's change the invoice late print date.
            var cookie = spa.Cookie;
            var breezy = new Breezy();
            var invoice = breezy.ExecuteQuery(breezy.PrepQuery<Invoice>().Where(n => n.AccountingSystemInvoiceNum == SeleniumUITests.SharedProperties["InvoiceNumber"]), cookie).Last();
            Assert.AreEqual(DateTime.Today.AddDays(10).Date, invoice.WarningLetterDate.Value.Date);

            //to use the dde, we need the invoice ID.  only way to get that is throguh breeze
            spa.NavigateTo<SPA>("/spa/#/dde/edit/Invoice/{0}".FormatString(invoice.InvoiceId))
                .DDE_UpdateWarningLetterDate(DateTime.Today.AddDays(-2));

            //Run renewal again
            spa.NavigateToQAAdmin()
                .RunRenewalForFacility(SeleniumUITests.SharedProperties["FacilityNumber"]);


            spa.NavigateToPrintQueue()
                .CheckPrintQueueForDocuments(SeleniumUITests.SharedProperties["RenewalNumber"], searchTime,
                    "-Renewal.pdf");

        }

        [TestMethod]
        [TestCategory("UI")]
        public void SeleniumTestRenewalMessaging()
        {
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            spa.NavigateToRenewalPortal("112273","409303")
                .CreateNewMessage();
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        public void TEST__JGR()
        {
            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddHalogenatedDryCleaningMachine(startupDate: DateTime.Today.AddDays(1),
                    dryCleaningFacilityType: DryCleaningFacilityType.DryCleaningFacilityTypeEnum.CoCommercial,
                    manufacturer: DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum.Escape,
                    machineType: MachineType.MachineTypeEnum.DipTank, drumCapacity: "40",
                    materialName: "Perchloroethylene", annualUsage: "25")
                .SubmitAndPay()
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;
            return;
        }

        [TestMethod]
        [TestCategory("UI")]
        public void SeleniumApproveLatestUnapprovedApplication()
        {
            var page = new SPA();

            page
                .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);


            int facilityNumber = 0, applicationNumber = 0;

            //lookup IDs
            var breezy = new Breezy();

            var apps = (EntityQuery<BusinessEvent>)
                breezy.PrepQuery<BusinessEvent>()
                .Expand("Application")
                .Expand("ApplicationStatusHistory")
                .Expand("Facility")
                .OrderByDescending(n => n.BusinessEventNumber )
                .Take(100);

            var allApps = breezy.ExecuteQuery<BusinessEvent>(apps, page.Cookie).ToArray()
                .OrderByDescending(n => n.BusinessEventNumber)
                .ToArray()
                .Where(n =>
                    n.ApplicationStatusHistory.OrderByDescending(n1 => n1.StatusDate)
                    .FirstOrDefault().ApplicationStatusTypeId == (int)ApplicationStatusType.ApplicationStatusTypeEnum.EvaluatingPermitApplicationCompleteness)
                    .ToArray();

            var x = allApps.First();

            facilityNumber = x.Facility.FacilityNumber;
            applicationNumber = x.BusinessEventNumber.Value;

            approveAndStartDevices(page, facilityNumber.ToString(), applicationNumber.ToString());

        }

        [TestMethod]
        [TestCategory("UI")]
        public void SelemiunStartupConditionUploadRetainsDocumentName_19724()
        {

            string facilityNumber, applicationNumber;
            const string fileNameToUpload = "sampleFile.txt";

            //Call the create new facility test
            SeleniumCreateNewFacility();

            var page = new ACPO();
            page.ClickCreateNewApplication()
                .PopulateApplicationHeader()
                .AddHalogenatedDryCleaningMachine(startupDate: DateTime.Today.AddDays(1),
                    dryCleaningFacilityType: DryCleaningFacilityType.DryCleaningFacilityTypeEnum.CoCommercial,
                    manufacturer: DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum.Escape,
                    machineType: MachineType.MachineTypeEnum.DipTank, drumCapacity: "40",
                    materialName: "Perchloroethylene", annualUsage: "25")
                .SubmitAndPay()
                .WaitForEvaluatingPermitApplicationCompleteness()
                ;

            //Let's approve app....
            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAppAndAndConfirmUploadRetainsDocumentName(spa,
                (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]),
                (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]),
                fileNameToUpload);
        }



        private static void approveAndStartDevices(PageObject page, string facilityNumber, string applicationNumber)
        {
            var applicationDashboardPage = string.Format("/IC2/Facilities/ApplicationStatusHistory/{0}/{1}/",
                    facilityNumber, applicationNumber);



            var ic2 = page.NavigateTo<IC2>(applicationDashboardPage);

            var targetDay = 1;
            var targetMonth = DateTime.Today.Day < 15
                ? DateTime.Today.Month
                : DateTime.Today.Month + 1;
            var targetYear = DateTime.Today.Year + 1;

            var targetPermitExpireDate = new DateTime(targetYear, targetMonth, targetDay);


            ic2
                .ApproveApp()
                .StatusAndStatusHistory()
                .GenerateEvalReport()
                .CustomerDocuments()
                .UPLOADDOCUMENT();
               // .NavigateToAppSummary()
               //.StartUpDevices()
               //.NavigateTo<SPA>("/spa/#/admin/qa")
               //.StartUpDevices(facilityNumber.ToString())
               //.NavigateTo<IC2>("/IC2/Facilities/Portal/{0}".FormatString(facilityNumber))
               //.ConfirmPermitExpireDate(targetPermitExpireDate);
        }

        private static void approveAppAndAndConfirmUploadRetainsDocumentName(PageObject page, string facilityNumber, string applicationNumber, string filenameToUpload)
        {
            var applicationDashboardPage = string.Format("/IC2/Facilities/ApplicationStatusHistory/{0}/{1}/",
                    facilityNumber, applicationNumber);


            var ic2 = page.NavigateTo<IC2>(applicationDashboardPage);
         

            ic2.ApproveApp()
                .NavigateToAppSummary()
                .StartUpDevices()
                .ConfirmUploadRetainsDocumentName(filenameToUpload);
        }
    }
}
