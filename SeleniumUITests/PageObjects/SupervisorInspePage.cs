using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class SupervisorInspePage : PageObject {

        private IWebDriver driver
        {
            get
            {
                return webDriver;
            }
        }

        /// <summary>
        /// This is the URL of the supervisor page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "";
            }
        }



        public SupervisorInspePage ClickInspectionTask()
        {
            string GDFTaskNum = SeleniumUITests.SharedProperties["GDFTaskNumber"];
            Sleep(3000);
            FindElement(By.LinkText(GDFTaskNum)).Click();
            this.webDriver.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            FindElement(By.Id("MainContent_lnkForm")).Click();
           
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
          
            return this;
        }
        public SupervisorInspePage EditContact(string Name = "jake", string Title = "GDF", string Phone = "(345) 123-4567", string Email = "test73457@test.com")
        {
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Name']")).Clear();
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Name']")).SendKeys(Name);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Title']")).Clear();
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Title']")).SendKeys(Title);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Phone']")).Clear();
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Phone']")).SendKeys(Phone);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Email']")).Clear();
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Email']")).SendKeys(Email);
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage verifyContact()
        {
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
            //Sleep(10000);
            //var text = FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Name']")).Text;
            //if (!string.Equals("Smith", text))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            //var text1 = FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Title']")).Text;
            //if (!string.Equals("ApplicationGDF", text1))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            //var text2 = FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Phone']")).Text;
            //if (!string.Equals("(555) 123-4567", text2))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            //var text3 = FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Email']")).Text;
            //if (!string.Equals("test737@test.com", text3))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            return this;
        }
        public SupervisorInspePage InspectionTabButton()
        {
            //InspectionTabButton
            FindElement(By.Id("InspectionTabButton")).Click();
            //WaitForComplete();
            Sleep(5000);
            return this;
        }
        public SupervisorInspePage VerifyFacilityLevelandSourceLevelInspection()
        {
            //Sleep(10000);
            //var text = FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_1__PermittedValue']")).Text;
            //if (!string.Equals("11", text))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            //var text1 = FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_1__ActualValue']")).Text;
            //if (!string.Equals("11", text1))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage EnforcementActionForm()
        {
            //click on enforcement action
            Sleep(5000);
            FindElement(By.Id("EnforcementActionsTabButton")).Click();
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage VerifyEnforcementActionForm()
        {
            //Sleep(10000);
            //var text = FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionIdentifier']")).Text;
            //if (!string.Equals("343", text))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            //WaitForComplete();
            //var text1 = FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Rule_Section']")).Text;
            //if (!string.Equals("500", text1))
            //{
            //    throw new NotFoundException("Expected text!");
            //}
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage TechnicalServices()
        {
            //click on technical service
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='FollowUpsTabButton']")).Click();
            WaitForComplete();
            return this;

        }
        public SupervisorInspePage EditGDFInspections()
        {
            //InspectionTabButton
            FindElement(By.Id("InspectionTabButton")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_0__Notes")).GetAttribute("value"))));
            return this;
        }
        public SupervisorInspePage EditEnforcementActionForm()
        {
            //click on enforcement action
            FindElement(By.Id("EnforcementActionsTabButton")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).GetAttribute("value"))));
             return this;
        }
        public SupervisorInspePage EditAddTag(string TagID = "1112", string FueilPoint = "21", string NozzleSerialID = "122")
        {
            var driver = this;
           
            //click on tag button
            //  FindElement(By.XPath(".//*[@id='TagsSection']/div[2]/a")).Click();
            //   WaitForComplete();
            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).Click();
            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).Clear();
            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).SendKeys(TagID);
            
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__FuelingPoint']")).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__FuelingPoint']")).Clear();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__FuelingPoint']")).SendKeys(FueilPoint);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__NozzleSerialId']")).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__NozzleSerialId']")).Clear();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__NozzleSerialId']")).SendKeys(NozzleSerialID);
            
            return this;
        }
        public SupervisorInspePage EditAddMinor(string FueilPoint = "13", string NozzleSerialID = "123", string Description = "test123")
        {
            var driver = this;

            // click on Minors

            // FindElement(By.XPath(".//*[@id='MinorsSection']/div[2]/a")).Click();
            //   WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__FuelingPoint']")).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__FuelingPoint']")).Clear();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__FuelingPoint']")).SendKeys(FueilPoint);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__NozzleSerialId']")).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__NozzleSerialId']")).Clear();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__NozzleSerialId']")).SendKeys(NozzleSerialID);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__Description']")).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__Description']")).Clear();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__Description']")).SendKeys(Description);
            WaitForComplete();
           
            return this;
        }
        public SupervisorInspePage ReviewInspection()
        {
            // click on review inspection
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='ReviewInspectionTabButton']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ComplianceWithRegulationsSection")).GetAttribute("class"))));
            //Complience with regulation
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[1]/div[1]/h1")).Click();//complience
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[2]/div[1]/h1")).Click();//not applicable
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[3]/div[1]/h1")).Click();//Not Compliant
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[4]/div[1]/h1")).Click();//complience assistance
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[4]/div[1]/h1")).Click();                                                                                              // ........enforcement action
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ReviewInspectionTab']/div/div[4]/div[1]/h1")).Click();//tags
            FindElement(By.XPath(".//*[@id='ReviewInspectionTab']/div/div[5]/div[1]/h1")).Click();//minors
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage ReviewInspectionTabButton()
        {
            // click on review inspection
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='ReviewInspectionTabButton']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ComplianceWithRegulationsSection")).GetAttribute("class"))));
            return this;
        }

        public SupervisorInspePage ReviewInspectionAB()
        {
            // click on review inspection
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='ReviewInspectionTabButton']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ComplianceWithRegulationsSection")).GetAttribute("class"))));
            //Complience with regulation
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[1]/div[1]/h1")).Click();//complience
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[2]/div[1]/h1")).Click();//not applicable
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[3]/div[1]/h1")).Click();//Not Compliant
            FindElement(By.XPath(".//*[@id='ComplianceWithRegulationsSection']/div[2]/div[4]/div[1]/h1")).Click();//complience assistance
           // FindElement(By.XPath(".//*[@id='ReviewInspectionTab']/div/div[3]/div[1]/h11")).Click();  // .enforcement action
            WaitForComplete();
            return this;
        }

        public SupervisorInspePage SubmitInspectionForm()
        {
            //click on Submit Button
            FindElement(By.XPath("html/body/div[1]/div[1]/div[1]/div/a[5]")).Click();
            Sleep(68000);
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Refresh();
            Sleep(5000);
            
            return this;
        }
        public SupervisorInspePage FacilityLevelInspectionsTabButton()
        {
            //FacilityLevelInspectionsTabButton         
            FindElement(By.Id("FacilityLevelInspectionsTabButton")).Click();
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage SourceLevelInspectionTabButton()
        {
            Sleep(5000);         
            FindElement(By.Id("SourceLevelInspectionsTabButton")).Click();
            WaitForComplete();
            return this;
        }
        public SupervisorInspePage OpenTaskNumber()
        {
            Refresh();
            string taskNumber = SeleniumUITests.SharedProperties["TaskNumber"];
            Sleep(5000);
            FindElement(By.LinkText(taskNumber)).Click();
            this.webDriver.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
            return this;
        }
        public SupervisorInspePage FindFacility()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='top']/ul/li[3]/a"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<SupervisorInspePage>(url);
        }
        public SupervisorInspePage ClickReports()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='top']/ul/li[7]/a"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<SupervisorInspePage>(url);
        }
        public SupervisorInspePage SearchFacility()
        {
            var facilityNumberEnforcement = SeleniumUITests.SharedProperties["FacilityNumber"];
            Sleep(3000);
            FindElement(By.Id("FacilityNumber")).Click();
            FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumberEnforcement);
            FindElement(By.Id("searchSubmit")).Click();
            return this;
        }
        public SupervisorInspePage InspectionHistory()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[7]/a"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<SupervisorInspePage>(url);

        }
        public SupervisorInspePage VerifyTaskClosed()
        {            
            FindElement(By.XPath(".//*[@id='jqgh_inspectionHistory_table_TaskNumber']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='jqgh_inspectionHistory_table_TaskNumber']")).Click();
            Sleep(5000);
            var text = FindElement(By.XPath(".//*[@id='1']/td[5]")).Text;
            if (!string.Equals("Closed", text))
            {
                throw new NotFoundException("Expected text!");
            }

            WaitForComplete();

            return this;
        }
        public SupervisorInspePage AreaAsssignment()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='top']/ul/li[6]/a"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<SupervisorInspePage>(url);
        }
        public SupervisorInspePage InspectorGroupTypeIC2(InspectorGroupType.InspectorGroupTypeEnum GasStation = InspectorGroupType.InspectorGroupTypeEnum.GasStation)
        {
            FindElement(By.XPath(".//*[@id='s2id_autogen3']/a")).Click();
            FindElement(By.XPath(".//*[@id='s2id_autogen3']/a")).SendKeys("Gas" + Keys.Enter);
            WaitForComplete();
            selectFacilityParcel();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='s2id_autogen1']/a")).GetAttribute("class"))));
            FindElement(By.XPath(".//*[@id='s2id_autogen1']/a")).Click();
            FindElement(By.XPath(".//*[@id='s2id_autogen1']/a")).SendKeys("k" + Keys.Enter);
            return this;
        }
        public SupervisorInspePage AssignInspector()
        {
            WaitForEnabled(By.XPath(".//*[@id='assignment-form']/button"));
            FindElement(By.XPath(".//*[@id='assignment-form']/button")).Click();
            Sleep(1000);
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='confirm-assignment']")).GetAttribute("class"))));
            FindElement(By.XPath(".//*[@id='enforcement']/div[7]/div[3]/div/button[1]")).Click();
            WaitForComplete();
            selectFacilityParcel();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='s2id_autogen1']/a")).GetAttribute("class"))));
            return this;
        }
        private void selectFacilityParcel()
        {
            FindElement(By.XPath(".//*[@id='map-container']/div/canvas")).Click();
        }

    }
}
