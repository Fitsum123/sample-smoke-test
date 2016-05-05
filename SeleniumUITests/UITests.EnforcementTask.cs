using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.WebUITests;
using Gov.Baaqmd.Tests.WebUITests.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    public partial class SeleniumUITests
    {
      

        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("EnforcementTask")]
        public void SeleniumEnforcement_20080()
        {
            var supervisorSelectionName = "Test Engineer Supervisor 2";
            var inspectorUrl = "IC2/Enforcement/Inspector";
            string supervisorUrl = "ic2/Enforcement/Supervisor";
            string faclilitySearchUrl = "/ic2/Facilities/Search";
            var comment = "general test\ncomment accepted";
            //Login 
            var ic2 = new SPA().Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password);

            //Navigate to Employee
            var employeePage = ic2.NavigateTo<IC2>("IC2/Engineering/Employees");

            //Assign TestEngineerSupervisor2 for TestGDFInspector1
            employeePage.AssignSupervisorForInspector(UITestHelper.TestInspector1.UserName, supervisorSelectionName);

            //Navigate to IC2
            var inspector=ic2.NavigateTo<IC2>(inspectorUrl);

            var windowHandle = inspector.CurrentWindowHandle();
            
            var enforcementForm=inspector.AddTask().PopulateAndSubmitEnforcementForm();

            //Ensure that the dialogs disappear before the next action is made on the parent window
            while (wd.WindowHandles.Count > 1)
            {
              
            }
            
            inspector.LogOut(windowHandle);

            var supervisor= new SPA().Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.TestInspector1.Password)
                              .NavigateTo<IC2>(supervisorUrl);

            windowHandle = supervisor.CurrentWindowHandle();

           supervisor
                .SelectTask(enforcementForm.TaskNumber)
                .MakeRevisionRequest();
         

            supervisor.LogOut(windowHandle);

            inspector = new SPA().Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
                .NavigateTo<IC2>(inspectorUrl);

            windowHandle = inspector.CurrentWindowHandle();


            inspector
                .SelectTask(enforcementForm.TaskNumber)
                .MakeRevisionResponse(comment);

            //Ensure that the dialogs disappear before the next action is made on the parent window
            while (wd.WindowHandles.Count > 1)
            {

            }
            inspector.LogOut(windowHandle);

            supervisor = new SPA().Login(UITestHelper.Testsupervisor2.UserName, UITestHelper.TestInspector1.Password)
                             .NavigateTo<IC2>(supervisorUrl);

            windowHandle = supervisor.CurrentWindowHandle();

            supervisor
                .SelectTask(enforcementForm.TaskNumber)
                .ApproveRevisionResponse();

            //Ensure that the dialogs disappear before the next action is made on the parent window
            while (wd.WindowHandles.Count > 1)
            {

            }
            supervisor.LogOut(windowHandle);

            var facilityPortal = new SPA().Login(UITestHelper.TestInspector1.UserName, UITestHelper.TestInspector1.Password)
                .NavigateTo<FacilityPortal>(faclilitySearchUrl);

            facilityPortal 
                .FindFacilityByFacilityNumber(enforcementForm.FacilityNumber)
                .ShowInspectionHistory(enforcementForm.TaskNumber)
                .ShowComplianceHistory(enforcementForm.TaskNumber);



        }
    }
}
