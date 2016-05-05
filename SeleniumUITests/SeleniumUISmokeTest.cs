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
        public void SmokeTestComplaintWizardPageGDF()
        {
            var page = new PublicForms();
            page.ComplaintOverview()
                .AllegedSourceLocation()
                .ObservationLocation()
                .FollowUpOptions()
                .ReviewSubmitComplaint()
                .EditAllegedSourceLocation()
                .EditAdditionalInformation()
                .EditFollowUpOption()
                .EditContactInformation()
                .EditMailingAddress()
                .SubmitComplaint()
                .CloseComplientForm()
                .CloseComplaintWizardButton();
            //CheckForErrors();
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumEnforcementGDFInspectionTaskInspectorSubmitsAndSupervisorApproves_18653()
        {
            //var Y = new SPA();
            //string Employees = "/IC2/Engineering/Employees";
            //Y.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //    .NavigateTo<IC2>(Employees)
            //    .SetInspectorAndsupervisorLogin();
           
            var x = new SPA();
            string Inspector = "/IC2/Enforcement/Inspector/";
            var Inspection =

            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
            .NavigateTo<InspectionForm>(Inspector)
            .ClickAddTask()
            .CreateTask()
            .ClickAssociateTask()
            .OpenInspectionForm();
            //Inspector - Create inspection task
            var i = new InspectionForm();
            var InspForm =
                i.EnterDate()
                .EnterContact()
                .UploadDocument()
                .InspectionTabButton()
                .AddRegulations()
                .AddGDFConditions()
                .AddSourceTestIssues()
                .EnforcementActionForm()
                .EnforcementActionBAAQMD87500()
                .AddTag()
                .AddMinor()
                .TechnicalServices()
                .ReviewInspection()
                .SubmitInspectionForm();
            //Supervisor - Submit and Close Inspection
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            var s = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
            .ClickInspectionTask()
            // .verifyContact()
            .InspectionTabButton()
            // .VerifyFacilityLevelandSourceLevelInspection()
            .EnforcementActionForm()
            //  .VerifyEnforcementActionForm()
            .TechnicalServices()
            .ReviewInspection()
            .SubmitInspectionForm();

            //verify task is closed and verify Tags, Minors, NOVs, NTCs
            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
            .NavigateTo<InspectionForm>(Inspector)
            .VerifyTaskClosed();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumDCRCreatFacilityAndAplicationAsEngineer()

        {
            string facilityNumber, applicationNumber;

            var x = new SPA();
            string SearchFacility = "/IC2/Facilities/Search/";

            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .NavigateTo<SPA>(SearchFacility)
              .ClickCreateNewFacilityIC2();

            //   Create a facility.
            var page = new ACPO();
            SharedProperties["FacilityNumber"] = page.ClickCreateNewFacility().CreateNewFacility();

            //  page
            // .ClickNewApplication()
            //.PopulateApplicationHeaderIC2()
            //.GeneralInformationIC2()
            //  .AddNonHalogenatedDryCleaningMachineIC2()
            //  .SubmitAndPayIC2()
            // .WaitForEvaluatingPermitApplicationCompleteness()
            //      ;
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

            approveAndIssuePermit(x,
            (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));
            //   // (facilityNumber = "200281") , (applicationNumber = "415417"));

        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumEngineerLegacyDCRTaketoPermitVoidedduetNSF()
        {
            string facilityNumber, renewalNumber;

            var x = new SPA();
            string SearchFacility = "/IC2/Facilities/Search/";
            string fs = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fRenewalStatus.rdl";

            //Find legacy DCR facility with existing renewal in Eval. Complete status
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
              .NavigateTo<SPA>(SearchFacility)
              .SearchReports()
              .NavigateTo<SPA>(fs)
             .RenewalStatusReport()
             .ViewReport();
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
             .NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]));

         ////   Run renewal-Engineer: Advance renewal to Invoice Issued
         //   .NavigateTo<SPA>()
         //    .NavigateToQAAdmin()
         //       .UpdateRenewalDates(DateTime.Today.AddDays(57))
         //       .ChangeInvoiceDueDate(DateTime.Today.AddDays(57), renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"])
         //       .RunRenewalForFacility(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]);
         //   //Verify renewal
         //   x.NavigateTo<SPA>()
         //    .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"], renewalNumber = SeleniumUITests.SharedProperties["RenewalNumber"]))
         //    .VerifyUpdateStatus("Not Required")
         //    .VerifyRenewalStatus("Invoice Issued");

         //   //Create a new user - Engineer: Get Facility Access Code- External User: Create new account and link legacy  facility
         //   SeleniumLinkNewFacilityToExternalUser();

         //   //Run renewal- Engineer - Update renewal at -2 days 
         //   x.NavigateTo<SPA>()
         //    .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
         //    .NavigateToQAAdmin()
         //   .UpdateRenewalDates(DateTime.Today.AddDays(-2), renewalNumber)
         //   .ChangeInvoiceDueDate(DateTime.Today.AddDays(-2), renewalNumber)
         //   .RunRenewalForFacility(facilityNumber)
         //  //Engineer: Check Renewal at Late and verify Late Fee 
         //  .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
         //   .VerifyRenewalStatus("Late Notice Sent");

         //   //Run renewal- Engineer - Update renewal at -32 days 
         //   x.NavigateToQAAdmin()
         //  .UpdateRenewalDates(DateTime.Today.AddDays(-32), renewalNumber)
         //  .ChangeInvoiceDueDate(DateTime.Today.AddDays(-32), renewalNumber)
         //  .RunRenewalForFacility(facilityNumber)
         //    //Engineer: Check Renewal at delinquent and verify Late Fee 
         //    .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
         //   .VerifyRenewalStatus("Delinquent Notice Sent");

         //   //Run renewal- Engineer - Update renewal at -62 days 
         //   x.NavigateToQAAdmin()
         //  .UpdateRenewalDates(DateTime.Today.AddDays(-62), renewalNumber)
         //  .ChangeInvoiceDueDate(DateTime.Today.AddDays(-62), renewalNumber)
         //  .RunRenewalForFacility(facilityNumber)
         //    //Engineer: Check Renewal at Permit Voided Due to NSF
         //    .NavigateTo<IC2>("IC2/Facilities/RenewalStatusHistory/{0}/{1}".FormatString(facilityNumber, renewalNumber))
         //   .VerifyRenewalStatus("Permit Voided Due to NSF");

         //   string UserName = SeleniumUITests.SharedProperties["UserName"];
         //   string Password = SeleniumUITests.SharedProperties["Password"];

         //   //External User: Online Payment For Renewal
         //   var acpo = new ACPOLogin()
         //      .Login(UserName, Password)
         //      .RewFacilityPermit()
         //      .PayRenewalFees();
         // //  Engineer: check renewed Registration Certificate
         //      x.NavigateTo<SPA>()
         //      .NavigateTo<IC2>("IC2/Facilities/FacilityApplicationOverview/{0}/".FormatString(facilityNumber))
         //       .Permits();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestEngineeringReportsVerifyReports()
        {
            string facilityNumber, applicationNumber;

            var x = new SPA();
            string SearchFacility = "/IC2/Facilities/Search/";
            string AllMailingAddresses = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fAll+Mailing+Addresses.rdl";
            string ApplicationHistoryByFacilityDBSPP = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fApplication+History+by+Facility+DB-SP-P.rdl";
            string ApplicationHistoryByFacilityDBSPA = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fApplication+History+DB-SP-A.rdl";
            string ConditionByNumber = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fConditionByConditionNumber.rdl";
            string CurrentConditions = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fCurrentConditions.rdl";
            string DeviceDetails = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fDeviceDetails.rdl";
            string EnforcementActions = "Reports/Pages/Report.aspx?ItemPath=%2fTest%2fEnforcement+Actions.rdl";
            string EnforcementTaskSearch = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fEnforcementTaskSearch.rdl";
            string FacilityRenewalStatusDBSTP = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fFacility+Renewal+Status+DB-ST-P.rdl";
            string FacilityInvoiceStatusReport = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fFacilityInvoiceStatusReport.rdl";
            string FacilitySearch = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fFacilitySearch.rdl";
            string FeeSchedule = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fFeeSchedule.rdl";
            string GenerateAuthorityToConstruct = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fGenerateAuthorityToConstruct.rdl";
            string GeneratePermit = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fGeneratePermit.rdl";
            string GenerateRegistrationCertificate = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fGenerateRegistrationCertificate.rdl";
            string InspectionHistoryReport = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fInspection+History+Report.rdl";
            string InvoicePaymentReport = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fInvoicePaymentReport.rdl";
            string OpenApplicationsByEmployeeAPSTAT = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fOpen+Applications+by+Employee+-+APSTAT.rdl";
            string OwnerHistory = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fOwnerHistory.rdl";
            string WorkLoadReport = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fWorkloadReport.rdl";

            ////  All Mailing Addresses
            //x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //  .NavigateTo<SPA>(SearchFacility)
            //  .SearchReports()
            //  .NavigateTo<Reports>(AllMailingAddresses)
            // .FacilityStatus()
            //.ViewReportAllMailingAddress()

            // // Verify renewal
            // .NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]))
            // .VerifyCorrectFacilityPageisDisplayedAllMailingAddress();

            //    Application History by Facility DB - SP - P
            //x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //   .NavigateTo<SPA>(SearchFacility)
            //    .SearchReports()
            //    .NavigateTo<Reports>(ApplicationHistoryByFacilityDBSPP)
            //    .EnterFacilityNumber()
            //    .ViewReportApplicationHistorybyFacilityDBSPandPDBSPA()
            // //Verify renewal
            // .NavigateTo<IC2>("IC2/Facilities/Application/{0}/{1}".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"], applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));
            ////   Application History DB - SP - A-- - log a bug
            //x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //.NavigateTo<SPA>(SearchFacility)
            //.SearchReports()
            //.NavigateTo<Reports>(ApplicationHistoryByFacilityDBSPA)
            //.EnterapplicationNumber()
            //.ViewReport()
            ////Verify renewal
            //.NavigateTo<IC2>("IC2/Facilities/Application/{0}/{1}".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"], applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            //  Application Status
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .NavigateTo<SPA>(SearchFacility)
             .SearchReports()
             .NavigateTo<Reports>(ConditionByNumber)
             .EnterConditionNumber()
             .ViewReportConditionbyConditionNumber()
             .VerifyConditionbyConditionNumber();
            //  Current Conditions
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
              .NavigateTo<SPA>(SearchFacility)
               .SearchReports()
               .NavigateTo<Reports>(CurrentConditions)
               .SelectCurrentorFuture()
               .EnterFacilityNumber()
               .ViewReportConditionbyConditionNumber()
               .VerifyCurrentConditionReport();
            //     Device Details
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                 .NavigateTo<SPA>(SearchFacility)
                 .SearchReports()
                 .NavigateTo<Reports>(DeviceDetails)
                 .EnterFacilityNumber()
                 .ViewReportConditionbyConditionNumber()
                 .SelectDevice()
                 .ViewReportConditionbyConditionNumber()
                 .VerifyDeviceDetailReport();
            //  Enforcement Action
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                 .NavigateTo<SPA>(SearchFacility)
                 .SearchReports()
                 .NavigateTo<Reports>(EnforcementActions)
                 .VerifyEnforcementActionReport();
            //    Enforcement Task Search GDF
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .NavigateTo<SPA>(SearchFacility)
            .SearchReports()
            .NavigateTo<Reports>(EnforcementTaskSearch)
            .VerifyEnforcementTasks()
            .VerifyInspectionForm();
            //    Enforcement Task Search FCV
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            .NavigateTo<SPA>(SearchFacility)
            .SearchReports()
            .NavigateTo<Reports>(EnforcementTaskSearch)
            .VerifyEnforcementTasks()
            .VerifyFCVInspectionForm();
            //  FacilityRenewalStatusDBSTP
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                  .NavigateTo<SPA>(SearchFacility)
                  .SearchReports()
                  .NavigateTo<Reports>(FacilityRenewalStatusDBSTP)
                  .EnterRenewalYearandFacilityNumber()
                  .ViewReportConditionbyConditionNumber()
                  .VerifyPlantRenewalStatus();
            //  Facility Invoice Status Report
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
              .NavigateTo<SPA>(SearchFacility)
              .SearchReports()
              .NavigateTo<Reports>(FacilityInvoiceStatusReport)
              .EnterFacilityNumber()
              .ViewReportConditionbyConditionNumber()
              .ClickInvoiceNumber()
              .VerifyInvoicePaymentReportisDisplayed()
              .VerifyAnnualRegistrationRenewalInvoice();
            //   FacilitySearch
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
           .NavigateTo<SPA>(SearchFacility)
           .SearchReports()
           .NavigateTo<Reports>(FacilitySearch)
           .EnterFacilityName();
            //   FeeSchedule
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
               .NavigateTo<SPA>(SearchFacility)
              .SearchReports()
             .NavigateTo<Reports>(FeeSchedule)
             .VerifyFeeSchedulePageisDisplayed();
            //    GenerateAuthorityToConstruct
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
              .NavigateTo<SPA>(SearchFacility)
              .SearchReports()
             .NavigateTo<Reports>(GenerateAuthorityToConstruct)
             .EnterApplicationNumber()
             .VerifySearchResultIsDisplayed();
            //  GeneratePermit
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                 .NavigateTo<SPA>(SearchFacility)
                 .SearchReports()
                 .NavigateTo<Reports>(GeneratePermit)
                 .EnterFacilityNumber()
                 .ViewReportConditionbyConditionNumber()
                 .VerifyPermitToOperateReportisDisplayed();
            //    GenerateRegistrationCertificate
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateTo<SPA>(SearchFacility)
                .SearchReports()
                .NavigateTo<Reports>(GenerateRegistrationCertificate)
                .EnterFacilityNumber()
                .ViewReportConditionbyConditionNumber()
                .VerifyCertificateOfRegistrationReportisDisplayed();
            //  InspectionHistoryReport
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateTo<SPA>(SearchFacility)
                .SearchReports()
                .NavigateTo<Reports>(InspectionHistoryReport)
                .VerifyInspectionHistoryReport()
                .ViewReportConditionbyConditionNumber()
                .VerifyInspectionHistoryReportisDisplayed();
            //  InvoicePaymentReport
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
             .NavigateTo<SPA>(SearchFacility)
             .SearchReports()
             .NavigateTo<Reports>(InvoicePaymentReport)
             .EnterInvoiceNumber()
             .ViewReportConditionbyConditionNumber()
             .VerifyInvoicePaymentReport();
            //   OpenApplicationsByEmployeeAPSTAT
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
             .NavigateTo<SPA>(SearchFacility)
             .SearchReports()
             .NavigateTo<Reports>(OpenApplicationsByEmployeeAPSTAT)
             .SelectAsssinedEngineerAndApplicationStatusType()
             .ViewReportConditionbyConditionNumber()
             .VerifyApplicationLatestStatus();
            // OwnerHistory
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                 .NavigateTo<SPA>(SearchFacility)
                .SearchReports()
                 .NavigateTo<Reports>(OwnerHistory)
                .EnterFacilityNumber()
                .ViewReportConditionbyConditionNumber()
                .VerifyFacilityOwnerHistoryReportisDisplayed();
            //WorkLoadReport
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
             .NavigateTo<SPA>(SearchFacility)
             .SearchReports()
             .NavigateTo<Reports>(WorkLoadReport)
             .IncludeONLYCutoverFacilities()
             .ViewReportConditionbyConditionNumber()
             .VerifyPermitApplicationWorkloadPageisDisplayed();

        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void RegressionTestEngineeringTransferofOwnershipEffectiveDateYesterdayFacilitywithOpenApplication()
        {
            string facilityNumber;
            var x = new SPA();
            string SearchFacility = "/IC2/Facilities/Search/";
            string ApplicationStatus = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fApplication+Status.rdl";

            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
           .NavigateTo<SPA>(SearchFacility)
           .SearchReports()
           .NavigateTo<Reports>(ApplicationStatus)
           .ApplicationStatus()
           .ViewReportConditionbyConditionNumber()
           .ShowPermitApplicationDetail();
            x.NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]))
            .CustomerFacilityPage()
            .NavigateTo<ACPO>()
            .TransferOwnershp()
           .TransferProcessOverview()
           .CurrentOperatorContact()
           .PayAndSubmitTransfer();
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
             .NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]))
             .NewOwningName()
             //verify 3 docu
             .CustomerDocumentsTransferOfOwnership()
             .NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]))
             .OpenApplication();

             string invNumber, BalaDue;
            x.NavigateTo<SPA>()
             .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateToQAAdmin()
                .PayInvoiceByInvoiceNumber(invNumber = SeleniumUITests.SharedProperties["InvoiceNumber"], BalaDue = SeleniumUITests.SharedProperties["BalanceDue"]);
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
              .NavigateTo<IC2>("IC2/Facilities/Portal/{0}/".FormatString(SeleniumUITests.SharedProperties["FacilityNumber"]))
              .OpenApplication()
              .ApproveApp()
              .NavigateToAppSummary()
              .StartUpDevices();
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
           .NavigateToQAAdmin()
           .StartUpDevices(facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]);
            x.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
         .NavigateTo<IC2>("IC2/Facilities/FacilityApplicationOverview/{0}/".FormatString(facilityNumber))
          .Permits();

        }


        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SeleniumCreateGDFapplication()
        {
            string facilityNumber;
            string applicationNumber;
            SeleniumCreateNewFacility();

            string UserName = SeleniumUITests.SharedProperties["UserName"];
            string Password = SeleniumUITests.SharedProperties["Password"];
            var page = new ACPO();
            page.ClickCreateNewApplication()
                .CreateHeaderApplicationTitleGDF()
              .UploadRELATEDPROJECTS()
             .CreateDeviceInformationForGDFApplication("PUMPS", startupDate: DateTime.Today.AddDays(-1),
             RefuelingMotorVehicles: GasolineDispensingActivityType.GasolineDispensingActivityTypeEnum.RefuelingMotorVehiclesRetail,
             Gasolineunleaded: GDFTankMaterialType.GDFTankMaterialTypeEnum._551,
            TankType: TankType.TankTypeEnum.UnderGround,
            Phase1VaporRecoveryType: Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum.PhilTiteVR101,
            Phase2VaporRecoveryType: Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum.HirtVCS200,
            Diesel: GDFTankMaterialType.GDFTankMaterialTypeEnum._98,
            TankType2: TankType.TankTypeEnum.AboveGround,
            Phase1VaporRecoveryType2: Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum.OPWVR102,
            Phase2VaporRecoveryType2: Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum.Healy400ORVR,
            ProductType: NozzleProductType.NozzleProductTypeEnum.GasolineTripleProductNozzle,
            ProductType2: NozzleProductType.NozzleProductTypeEnum.Diesel,
            Location: BuildingLocationType.BuildingLocationTypeEnum.Outside)
             .materialUsage()
               .SubmitAndPay()
                .WaitForEvaluatingPermitApplicationCompleteness();

            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            ApplicationDevice(spa,
            (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));

            spa.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            approveAndStartDevices(spa,
              (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));
          
            //Relogin as new user
            var p = new ACPOLogin()
               .Login(SeleniumUITests.SharedProperties["UserName"], SeleniumUITests.SharedProperties["Password"])

                 .SaveStartUpInformation();

            spa.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password);
            StartUpDevice(spa,
          (facilityNumber = SeleniumUITests.SharedProperties["FacilityNumber"]), (applicationNumber = SeleniumUITests.SharedProperties["ApplicationNumber"]));
       
            //  Run renewal
            spa.NavigateTo<SPA>()
                .Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateToQAAdmin()
                .ChangeFacilityAnniversaryDate(facilityNumber, DateTime.Today.AddDays(148)) //first state
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(118))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(88))
                .RunRenewalForFacility(facilityNumber)
                .UpdateRenewalDates(DateTime.Today.AddDays(58)) //invoice issued
                .RunRenewalForFacility(facilityNumber);

            spa.NavigateTo<SPA>()
           .NavigateToPrintQueue()
              .CheckPrintQueueForDocuments(facilityNumber);

            //Relogin as new user
            var p1 = new ACPOLogin()
                .Login(SeleniumUITests.SharedProperties["UserName"], SeleniumUITests.SharedProperties["Password"])
                  .ClickRenewal()
                 .RenewalClickContinueToNextSection()
                 .RenewalClickContinueToNextSection()
                 .PayRenewalFees();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestEnforcementAsbestosRenovationEmergencyOnlinepaymentVerifyApprovedJobStatusandLetters()
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



            ////Create asbestos application - will not be paid
            //var page = new ACPO().CreateNewAsbestosJob();

            //page
            //   .CreateAsbestosJobHeaderRenovation()
            //    .AddContractorDetails()
            //   //.AddLocationDetailsRenovation()
            //   .AddLocationDetails()
            //   .AddRenovationJobDetails()
            //   .AddStartEndDates()
            //   .AddRenoCertification()
            //   .AddPayOnline()
            //   .WaitForApproved()
            //   .ChangeAsbestosJobDetails()
            //   .NavigateTo<ACPO>();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void SmokeTestAsbestosDemolitionFutureJobMultifamilyDwellingOnlinepaymentVerifyApprovedJobStatusandletters()
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



            // //Create asbestos application - will not be paid
            // var page = new ACPO().CreateNewAsbestosJob();

            // page
            //    .CreateAsbestosJobHeader()
            //     .AddContractorDetails()
            //    .AddLocationDetailsMultifamilyDwelling()
            //    .AddFireTrainingDetails(isFireTraining: true)
            //    .GovernmerOrderedDemolition()
            //    .AddLabAnalysis()
            //    .AddStartEndDates()
            //    .AddDemoCertification()
            //     .AddPayOnline()
            //    .WaitForApproved()
            //    .ChangeAsbestosJobDetails();

            // var x = new SPA();
            // string TestEnforcementAQT = "spa/#/enforcement/aqt";

            // x.Login(UITestHelper.EnforcementAQT.UserName, UITestHelper.EnforcementAQT.Password)
            // .NavigateTo<SPA>(TestEnforcementAQT);

            // var y = new SPA();
            // string TestAsbestosSupervisor = "/spa/#/enforcement/aqtsupervisor";
            // y.Login(UITestHelper.AsbestosSupervisor.UserName, UITestHelper.AsbestosSupervisor.Password)
            //.NavigateTo<SPA>(TestAsbestosSupervisor);


        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        public void RegressionTestEngineeringExternalCustomerCreateExternalCustomerAccount()
        {
            string facilityNumber = "200296";
            // var rnd2 = new Random();
            // var anyrndnumber = rnd2.Next(18, 99);
            // string facilityNumber = "2001" + anyrndnumber.ToString();

            var spa = new SPA().Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateToQAAdmin()
                .GenerateFacilityAccessCodes(facilityNumber);
            // string AccessCode ;

            var rnd = new Random();
            var number = rnd.Next(1000, 9999);

            //Read Data from Excel, each column is a field. 
            string UserName = "test" + number.ToString() + "@test.com";
            string Password = "Possum100";
            //Root Page of this test
            var page = new ACPOLogin();
            page.VerifyCreateExternalUserAccount(UserName, Password)
                .logout()
                .VerifyLoginPage(SeleniumUITests.SharedProperties["EmailAddress"]);

        }
        private static void approveAndIssuePermit(PageObject page, string facilityNumber, string applicationNumber)
        {
            var applicationDashboardPage = string.Format("IC2/Facilities/Application/{0}/{1}/",
                    facilityNumber, applicationNumber);



            var ic2 = page.NavigateTo<IC2>(applicationDashboardPage);


            ic2.ApproveApp()
               //  .CustomerDocuments()
               .NavigateToAppSummary()
            //   .IssuePermit()
               .NavigateTo<SPA>("/spa/#/admin/qa")
               .StartUpDevices(facilityNumber.ToString())
               .NavigateTo<IC2>("/IC2/Facilities/Portal/{0}".FormatString(facilityNumber));
            //  .Permits();

        }
        private static void ApplicationDevice(PageObject page, string facilityNumber, string applicationNumber)
        {
            var applicationDashboardPage = string.Format("/IC2/Facilities/Application/{0}/{1}/",
                    facilityNumber, applicationNumber);

            var ic2 = page.NavigateTo<IC2>(applicationDashboardPage);
            ic2
                .Reassign()
               .MyDashboard()
                .clickApplicationDevice()
                .DoneWithForm()
                .ManageRegulations()
                .AddFee();
        }
        private static void StartUpDevice(PageObject page, string facilityNumber, string applicationNumber)
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
                   .NavigateToAppSummary()
                  .StartUpDevices()
                  .NavigateTo<SPA>("/spa/#/admin/qa")
                  .StartUpDevices(facilityNumber.ToString())
                  .NavigateTo<IC2>("/IC2/Facilities/Portal/{0}".FormatString(facilityNumber))
                  .ConfirmPermitExpireDate(targetPermitExpireDate);
        }
    }

}