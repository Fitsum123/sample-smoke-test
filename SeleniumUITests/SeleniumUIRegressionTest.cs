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
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementGDFInspectionTaskRevisionsRequestedCreateandView()
        {
            var Y = new SPA();
            string Employees = "/IC2/Engineering/Employees";
            Y.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
                .NavigateTo<IC2>(Employees)
                .SetInspectorAndsupervisorLogin();

            var x = new SPA();
            string Inspector = "/IC2/Enforcement/Inspector/";
            var Inspection =

            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
            .NavigateTo<InspectionForm>(Inspector)
            .ClickAddTask()
            .ClickAssociateTask()
            .CreateTask()
            .OpenInspectionForm();

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
            //Supervisor - Revisions Requested
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            var s = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
            .ClickInspectionTask();
            i.RevisionRequest();
            //Inspector- Submit Inspection
            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
              .NavigateTo<InspectionForm>(Inspector)
             .ClickInspectionTask()
             .EditContact()
             .EditGDFInspections()
             .EditEnforcementActionForm()
             .TechnicalServices()
             .ReviewInspection()
             .SubmitInspectionForm();

            // supervisor submit and Close Inspection
            var ss = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
             .NavigateTo<SupervisorInspePage>(Supervisor)
             .ClickInspectionTask()
             .EditContact()
             .EditGDFInspections()
             .EditEnforcementActionForm()
             .EditAddTag()
             .EditAddMinor()
             .TechnicalServices()
             .ReviewInspection()
             .SubmitInspectionForm();

            //verify task is closed and verify Tags, Minors, NOVs, NTCs
            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
            .NavigateTo<InspectionForm>(Inspector)
            .VerifyTaskClosed()
            .VerifyCompliancehHistory();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementABInspectiontaskCreateandClose()

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
            .CreateAutoBodyTask()
            .ClickAssociateTask()
            .OpenInspectionForm();

            var i = new InspectionForm();
            var InspForm =
                i.EnterDate()
                .EnterContact()
                .UploadDocument()
                 .FacilityLevelInspectionsTabButton()
                .PODistrictRegistrationandStateRegistrationAvailibilityatFacility()
                 .AddRegulationsOnFacilityLevelInsopections()
                 .ClickFacilityLevelInspectionsConditions()
                 .SourceLevelInspectionTabButton()
                  .AddSources()
                 .AddRegulationsOnSourceLevelInsopections()
            //    .ClickSourceLevelConditions()
                  .EnforcementActionForm()
                  .EnforcementActionBAAQMD1500()
                  .EnforcementActionS2HalogenatedDryCleaningMachineBAAQMD10500()
                  .TechnicalServices()
                  .TecchnicalServiceFaccilityWideBAAQMD10100BAAQMD10300()
                  .ReviewInspection()
                  .SubmitInspectionForm();

            //Supervisor - Revisions Requested
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            var s = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
              .ClickInspectionTask()
             .FacilityLevelInspectionsTabButton()
             .SourceLevelInspectionTabButton()
             .EnforcementActionForm()
             .TechnicalServices()
             .ReviewInspectionAB()
             .SubmitInspectionForm();



        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementChecklinksMydashboardFindTasksFindFacilityFindSiteMapsEmployees()

        {
            var x = new SPA();
            string Inspector = "/IC2/Enforcement/Inspector/";
            var Inspection =

            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
            .NavigateTo<InspectionForm>(Inspector)
            .FindFacility()
            .SearchFacility();

        }
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementDCRInspectiontaskCreateandView()
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
            .CreateDCRtask()
            .ClickAssociateTask()
            .OpenInspectionForm();

            var i = new InspectionForm();
            var InspForm =
                i.EnterDate()
                .EnterContact()
                .UploadDocument()
                .FacilityLevelInspectionsTabButton()
                .PODistrictRegistrationandStateRegistrationAvailibilityatFacility()
                 .AddRegulationsOnFacilityLevelInsopections()
                 .ClickFacilityLevelInspectionsConditions()
                 .SourceLevelInspectionTabButton()
                  .AddSources()
                 .AddRegulationsOnSourceLevelInsopections()
            //    .ClickSourceLevelConditions()
                  .EnforcementActionForm()
                  .EnforcementActionBAAQMD1500()
                  .EnforcementActionS2HalogenatedDryCleaningMachineBAAQMD10500()
                  .TechnicalServices()
                  .TecchnicalServiceFaccilityWideBAAQMD10100BAAQMD10300()
                  .ReviewInspection()
                  .SubmitInspectionForm();
            //Supervisor - Revisions Requested
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            var s = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
              .ClickInspectionTask()
             .FacilityLevelInspectionsTabButton()
             .SourceLevelInspectionTabButton()
             .EnforcementActionForm()
             .TechnicalServices()
             .ReviewInspectionAB()
             .SubmitInspectionForm();
        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementSubmitInprogressInspectiontask()
        {
            //var Y = new SPA();
            //string Employees = "/IC2/Engineering/Employees";
            //Y.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //    .NavigateTo<IC2>(Employees)
            //    .SetInspectorAndsupervisorLogin();

            var x = new SPA();
            //Supervisor - Revisions Requested
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            string Inspector = "/IC2/Enforcement/Inspector/";
            string EnforcementTaskSearch = "/Reports/Pages/Report.aspx?ItemPath=%2fTest%2fEnforcementTaskSearch.rdl";
            var s = new SupervisorInspePage();
            var i = new InspectionForm();

            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
            .ClickReports()
            .NavigateTo<Reports>(EnforcementTaskSearch)
            .ClickTaskNumber()
           .Reassign();

            x.Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password);
            i.NavigateTo<InspectionForm>(Inspector)
               .OpenTaskNumber()
               .InspectionTabButton()
               .EnforcementActionForm()
               .TechnicalServices()
               .ReviewInspection()
               .SubmitInspectionForm();

            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password);
            s.NavigateTo<SupervisorInspePage>(Supervisor)
            .OpenTaskNumber()
            .InspectionTabButton()
            .EnforcementActionForm()
            .TechnicalServices()
            .ReviewInspectionTabButton()
            .SubmitInspectionForm()
            .FindFacility()
            .SearchFacility()
            .InspectionHistory()
            .VerifyTaskClosed();


        }

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")]
        public void SmokeTestCaseEnforcementAreaAssignmentasSupervisor_17054()
        {
            //var Y = new SPA();
            //string Employees = "/IC2/Engineering/Employees";
            //Y.Login(UITestHelper.TestEngineer1.UserName, UITestHelper.TestEngineer1.Password)
            //    .NavigateTo<IC2>(Employees)
            //    .SetInspectorAndsupervisorLogin();

            var x = new SPA();
            //Supervisor -
            string Supervisor = "/IC2/Enforcement/Supervisor/";
            var s = new SupervisorInspePage();
            x.Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.Testsupervisor2.Password)
            .NavigateTo<SupervisorInspePage>(Supervisor)
            .AreaAsssignment()
            .InspectorGroupTypeIC2()
            .AssignInspector();
        }  
}
}
