using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class InspectionForm : PageObject
    {
        private IWebDriver driver
        {
            get
            {
                return webDriver;
            }
        }

        /// <summary>
        /// This is the URL of the InspectionForm page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Loads task screen from app page and loops until we have approval or 10 times.
        /// </summary>

        public InspectionForm ClickAddTask()
        {
                  //CLICK ADD TANK
            FindElement(By.XPath(".//*[@id='scbd_btnAddTask']")).Click();
            this.webDriver.SwitchTo().Frame("IfrDialog");
            return this;
        }
        public InspectionForm CreateTask(TaskCategoryType.TaskCategoryTypeEnum enforcement = TaskCategoryType.TaskCategoryTypeEnum.Enforcement,
           TaskType.TaskTypeEnum GDFinspection = TaskType.TaskTypeEnum.GasolineDispensingFacilityInspection,
           FacilityStatusType.FacilityStatusTypeEnum permitted = FacilityStatusType.FacilityStatusTypeEnum.Permitted)
        {
            //SELECT TASK CATAGORY-Enforcement
            WaitForComplete();
            FindElement(By.Id("MainContent_cbTaskCategory_cbTaskCategory_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskCategory_cbTaskCategory_OptionList']/li[3]".FormatString((int)enforcement))).Click();
            //WaitForComplete();
            Sleep(3000);
            //TASK TYPE-GasolineDispensingFacilityInspection
            FindElement(By.Id("MainContent_cbTaskType_cbTaskType_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskType_cbTaskType_OptionList']/li[7]".FormatString((int)GDFinspection))).Click();
            //Need to wait until box is populated
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("MainContent_ddlSearchType")).GetAttribute("name"))));

            //FACILITY STATUS TYPE   
            FindElement(By.XPath(".//*[@id='MainContent_ddlFacilityStatusType']/option[8]".FormatString((int)permitted))).Click();
            WaitForComplete();
            //CLICK SEARCH
            FindElement(By.XPath(".//*[@id='MainContent_searchButton']")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm CreateAutoBodyTask(TaskCategoryType.TaskCategoryTypeEnum enforcement = TaskCategoryType.TaskCategoryTypeEnum.Enforcement,
           TaskType.TaskTypeEnum FacilityInspection = TaskType.TaskTypeEnum.FacilityInspection,
           SiteType.SiteTypeEnum Facility = SiteType.SiteTypeEnum.Facility,
           FacilityStatusType.FacilityStatusTypeEnum permitted = FacilityStatusType.FacilityStatusTypeEnum.Permitted)
        {
            //SELECT TASK CATAGORY-Enforcement
            WaitForComplete();
            FindElement(By.Id("MainContent_cbTaskCategory_cbTaskCategory_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskCategory_cbTaskCategory_OptionList']/li[3]".FormatString((int)enforcement))).Click();
            //WaitForComplete();
            Sleep(3000);
            //TASK TYPE-GasolineDispensingFacilityInspection
            FindElement(By.Id("MainContent_cbTaskType_cbTaskType_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskType_cbTaskType_OptionList']/li[6]".FormatString((int)FacilityInspection))).Click();
            //Need to wait until box is populated
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("MainContent_ddlSearchType")).GetAttribute("name"))));
            //FACILITY Site TYPE   
            FindElement(By.XPath(".//*[@id='MainContent_ddlSearchType']/option[2]".FormatString((int)Facility))).Click();
            //FACILITY STATUS TYPE   
            FindElement(By.XPath(".//*[@id='MainContent_ddlFacilityStatusType']/option[8]".FormatString((int)permitted))).Click();
            WaitForComplete();
            //CLICK SEARCH
            FindElement(By.XPath(".//*[@id='MainContent_searchButton']")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm CreateDCRtask(TaskCategoryType.TaskCategoryTypeEnum enforcement = TaskCategoryType.TaskCategoryTypeEnum.Enforcement,
           TaskType.TaskTypeEnum FacilityInspection = TaskType.TaskTypeEnum.FacilityInspection,
           SiteType.SiteTypeEnum Facility = SiteType.SiteTypeEnum.Facility,
           FacilityStatusType.FacilityStatusTypeEnum Registered = FacilityStatusType.FacilityStatusTypeEnum.Registered, string SiteName = "Dry Cleaner")
        {
            //SELECT TASK CATAGORY-Enforcement
            WaitForComplete();
            FindElement(By.Id("MainContent_cbTaskCategory_cbTaskCategory_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskCategory_cbTaskCategory_OptionList']/li[3]".FormatString((int)enforcement))).Click();
            //WaitForComplete();
            Sleep(3000);
            //TASK TYPE-GasolineDispensingFacilityInspection
            FindElement(By.Id("MainContent_cbTaskType_cbTaskType_Button")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskType_cbTaskType_OptionList']/li[6]".FormatString((int)FacilityInspection))).Click();
            //Need to wait until box is populated
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("MainContent_ddlSearchType")).GetAttribute("name"))));
            //FACILITY Site TYPE   
            FindElement(By.XPath(".//*[@id='MainContent_ddlSearchType']/option[2]".FormatString((int)Facility))).Click();
            //FACILITY STATUS TYPE   
            FindElement(By.XPath(".//*[@id='MainContent_ddlFacilityStatusType']/option[10]".FormatString((int)Registered))).Click();
            WaitForComplete();
            //Facility or SiteName
            FindElement(By.Id("MainContent_tbSearchName")).Click();
            FindElement(By.Id("MainContent_tbSearchName")).Clear();
            FindElement(By.Id("MainContent_tbSearchName")).SendKeys(SiteName);
            WaitForComplete();
            //CLICK SEARCH
            FindElement(By.XPath(".//*[@id='MainContent_searchButton']")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm ClickAssociateTask() { 
            var GDFFacilityNum = FindElement(By.Id("MainContent_lvResults_lblResultsNumber_3")).Text;
            SeleniumUITests.SharedProperties["GDFFacilityNumber"] = GDFFacilityNum;
            SeleniumUITests.SharedProperties["GDFInspectionPage"] = new Uri(wd.Url).PathAndQuery;

            //select permitted facility
            FindElement(By.Id("MainContent_lvResults_btnResultsName_3")).Click();
            WaitForComplete();
            //click on create and view form
            FindElement(By.Id("MainContent_btnCreateAndViewTask")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm OpenInspectionForm()
        {
            var GDFTaskNum = FindElement(By.Id("MainContent_lblTaskIdentifier")).Text;
            SeleniumUITests.SharedProperties["GDFTaskNumber"] = GDFTaskNum;
            //click on View Associated form button
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitForComplete();
            return this;
        }
        public InspectionForm OpenTaskNumber()
        {
            Refresh();
            string taskNumber = SeleniumUITests.SharedProperties["TaskNumber"];
            Sleep(3000);
            FindElement(By.LinkText(taskNumber)).Click();
            this.webDriver.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
            return this;
        }
         public InspectionForm EnterDate(DateTime? startupDate = null)
        {

            this.PushDelay(100);

            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(-1);
          //  WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));
            FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).SendKeys(startupDate.Value.ToShortDateString());
            WaitForComplete();

            return this;
        }

        public InspectionForm EnterContact(string Name = "Smith", string Title = "ApplicationGDF", string Phone = "(555) 123-4567",
            string Email = "test737@test.com")
        {
           // WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Name']")).GetAttribute("value"))));
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Name']")).SendKeys(Name);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Title']")).SendKeys(Title);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Phone']")).SendKeys(Phone);
            FindElement(By.XPath(".//*[@id='GeneralInformationTab_InspectionContacts_Items_0__Email']")).SendKeys(Email);
            WaitForComplete();
            return this;
        }
        public InspectionForm UploadDocument() {
            FindElement(By.XPath(".//*[@id='AttachmentsSection']/div[2]/div/a")).Click();
            WaitForComplete();
            FindElement(By.XPath("//a/input")).Click();
            WaitForComplete();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            WaitForComplete();
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            FindElement(By.XPath("html/body/div[5]/div/div[3]/div/div/a[1]")).Click();
            Sleep(10000);
            return this;
        }
        public InspectionForm EditContact(string Name = "jake", string Title = "GDF", string Phone = "(345) 123-4567", string Email = "test73457@test.com")
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

        public InspectionForm InspectionTabButton()
        {
            //InspectionTabButton         
            FindElement(By.Id("InspectionTabButton")).Click();
            Sleep(5000);
            // WaitForComplete();
            return this;
        }
        public InspectionForm FacilityLevelInspectionsTabButton()
        {
            //FacilityLevelInspectionsTabButton         
            FindElement(By.Id("FacilityLevelInspectionsTabButton")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm PODistrictRegistrationandStateRegistrationAvailibilityatFacility()
        {
            //click on Yes for Current PO Available at Facility?
            FindElement(By.Id("FacilityLevelInspectionsTab_IsCurrentPermitAvailable_True")).Click();
            WaitForComplete();
           // Current District Registration Available at Facility?
            FindElement(By.Id("FacilityLevelInspectionsTab_IsDistrictRegistrationAvailable_-9999952")).Click();
            WaitForComplete();
            //Current State Registration Available at Facility?
            FindElement(By.Id("FacilityLevelInspectionsTab_IsStateRegistrationAvailable")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm AddRegulationsOnFacilityLevelInsopections(ComplianceStatusType.ComplianceStatusTypeEnum complient = ComplianceStatusType.ComplianceStatusTypeEnum.Compliant,
           ComplianceStatusType.ComplianceStatusTypeEnum complianceAssistance = ComplianceStatusType.ComplianceStatusTypeEnum.ComplianceAssistance,
           ComplianceStatusType.ComplianceStatusTypeEnum notApplicable = ComplianceStatusType.ComplianceStatusTypeEnum.NotApplicable,
           ComplianceStatusType.ComplianceStatusTypeEnum NotCompliant = ComplianceStatusType.ComplianceStatusTypeEnum.NotCompliant, string technicalNote = "Technical Note", string regulation = "1")
        {
            //Click Regulstions
            FindElement(By.XPath(".//*[@id='RegulationsSection']/div[2]/a")).Click();
            WaitForComplete();
            //  ADD REGULATIONS
            FindElement(By.Id("AddRegulationDialog_RegulationText")).SendKeys(regulation);
            Sleep(5000); 
            FindElement(By.Id("AddRegulationDialog_RegulationText")).SendKeys(Keys.Down + Keys.Enter);
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_0__Status']/option[3]".FormatString((int)complient))).Click();
            FindElement(By.Id("FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_0__FollowUp")).Click();
            WaitForComplete();
            FindElement(By.Id("FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_0__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_1__Status']/option[2]".FormatString((int)complianceAssistance))).Click();
            FindElement(By.Id("FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_1__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_2__Status']/option[4]".FormatString((int)notApplicable))).Click();
            FindElement(By.Id("FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_2__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_3__Status']/option[5]".FormatString((int)NotCompliant))).Click();
            FindElement(By.Id("FacilityLevelInspectionsTab_DistrictRules_Items_0__Rules_3__Notes")).SendKeys(technicalNote);
            WaitForComplete();


            return this;
        }
        public InspectionForm ClickFacilityLevelInspectionsConditions()
        {
            // click condition buttton
            FindElement(By.XPath(".//*[@id='ConditionsSection']/div[2]/a")).Click();
            WaitForComplete();
            FindElement(By.Id("AddConditionDialog_SelectedCondition")).Click();
            WaitForComplete();
            FindElement(By.Id("AddConditionDialog_SelectedCondition")).Click();
            WaitForComplete();
            FindElement(By.XPath("html/body/div[6]/div/div[3]/div/div/a[2]")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm AddRegulationsOnSourceLevelInsopections(ComplianceStatusType.ComplianceStatusTypeEnum complient = ComplianceStatusType.ComplianceStatusTypeEnum.Compliant,
           ComplianceStatusType.ComplianceStatusTypeEnum complianceAssistance = ComplianceStatusType.ComplianceStatusTypeEnum.ComplianceAssistance,
           ComplianceStatusType.ComplianceStatusTypeEnum notApplicable = ComplianceStatusType.ComplianceStatusTypeEnum.NotApplicable,
           ComplianceStatusType.ComplianceStatusTypeEnum NotCompliant = ComplianceStatusType.ComplianceStatusTypeEnum.NotCompliant, string technicalNote = "Technical Note", string regulation = "10")
        {
            //Click Regulstions
            FindElement(By.XPath(".//*[@id='RegulationsSection_0']/div[2]/a")).Click();
            WaitForComplete();
            //  ADD REGULATIONS
            FindElement(By.Id("AddRegulationDialog_RegulationText")).SendKeys(regulation);
            Sleep(5000);
            FindElement(By.Id("AddRegulationDialog_RegulationText")).SendKeys(Keys.Down + Keys.Enter);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_0__Status']/option[3]".FormatString((int)complient))).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_0__FollowUp")).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_0__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_1__Status']/option[2]".FormatString((int)complianceAssistance))).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_1__FollowUp")).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_1__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_2__Status']/option[4]".FormatString((int)notApplicable))).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_2__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_3__Status']/option[5]".FormatString((int)NotCompliant))).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__DistrictRules_0__Rules_3__Notes")).SendKeys(technicalNote);
            WaitForComplete();


            return this;
        }
        public InspectionForm ClickSourceLevelConditions(string PermittedValue = "11", string ActualValue = "11", string Unit = "1", string Notes = "test",
            string addParameter = "Du")
        {
            // click condition buttton
            FindElement(By.XPath(".//*[@id='ConditionsSection_0']/div[2]/a")).Click();
            WaitForComplete();
            FindElement(By.Id("AddConditionDialog_SelectedCondition")).Click();
            WaitForComplete();
            FindElement(By.Id("AddConditionDialog_SelectedCondition")).SendKeys(Keys.Down + Keys.Enter);
            Sleep(5000);

            // click PARAMETER button
            FindElement(By.XPath(".//*[@id='ConditionsSection_0']/div[2]/div/div[2]/a")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='AddConditionParameterDialog_ParameterName']")).SendKeys(addParameter);
            // WaitForComplete();
            Sleep(5000);
            FindElement(By.Id("AddConditionParameterDialog_ParameterName")).SendKeys(Keys.Enter);
            WaitForComplete();
            // click on parameter button
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__PermittedValue']")).Click();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__PermittedValue']")).Clear();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__PermittedValue']")).SendKeys(PermittedValue);
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__ActualValue']")).Click();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__ActualValue']")).Clear();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__ActualValue']")).SendKeys(ActualValue);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__Units']")).SendKeys(Unit);
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__Units']")).SendKeys(Keys.Enter);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab_InspectionDevices_Items_0__Conditions_0__Parameters_0__Notes']")).SendKeys(Notes);
            WaitForComplete();
            return this;
        }
        public InspectionForm SourceLevelInspectionTabButton()
        {
            //FacilityLevelInspectionsTabButton         
            FindElement(By.Id("SourceLevelInspectionsTabButton")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm AddSources(string Description = "Source Level Inspections Tab Inspection Devices")
        {
            //add sources         
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='DevicesSection']/div/a")).Click();
            WaitForComplete(); 
            // click check box       
            FindElement(By.Id("AddDevicesDialog_Devices_Items_0__Value")).Click();
            WaitForComplete();
            //click Okay
            FindElement(By.Id("AddDevicesDialog_Devices_Items_0__Value")).SendKeys(Keys.Enter);
            WaitForComplete();
            //click on + sign         
            FindElement(By.XPath(".//*[@id='SourceLevelInspectionsTab']/div/div[2]/div[1]/table/tbody/tr/td[1]/h1")).Click();
            WaitForComplete();
            //click okay         
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__Description")).Click();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__Description")).Clear();
            FindElement(By.Id("SourceLevelInspectionsTab_InspectionDevices_Items_0__Description")).SendKeys(Description);
            WaitForComplete();          
            return this;
        }
        public InspectionForm EditGDFInspections()
        {
            //GDF - InspectionTabButton

            FindElement(By.Id("InspectionTabButton")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_0__Notes")).GetAttribute("value"))));
            return this;
        }
        public InspectionForm AddRegulations(ComplianceStatusType.ComplianceStatusTypeEnum complient = ComplianceStatusType.ComplianceStatusTypeEnum.Compliant,
            ComplianceStatusType.ComplianceStatusTypeEnum complianceAssistance = ComplianceStatusType.ComplianceStatusTypeEnum.ComplianceAssistance,
            ComplianceStatusType.ComplianceStatusTypeEnum notApplicable = ComplianceStatusType.ComplianceStatusTypeEnum.NotApplicable,
            ComplianceStatusType.ComplianceStatusTypeEnum NotCompliant = ComplianceStatusType.ComplianceStatusTypeEnum.NotCompliant, string technicalNote = "Technical Note")
        {
            //click on Yes for Current PO Available at Facility?
            FindElement(By.Id("InspectionTab_IsCurrentPermitAvailable_True")).Click();
            WaitForComplete();

            FindElement(By.XPath(".//*[@id='InspectionTab_DistrictRules_Items_0__Rules_0__Status']/option[3]".FormatString((int)complient))).Click();
            FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_0__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='InspectionTab_DistrictRules_Items_0__Rules_1__Status']/option[2]".FormatString((int)complianceAssistance))).Click();
            FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_1__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='InspectionTab_DistrictRules_Items_0__Rules_2__Status']/option[4]".FormatString((int)notApplicable))).Click();
            FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_2__Notes")).SendKeys(technicalNote);
            FindElement(By.XPath(".//*[@id='InspectionTab_DistrictRules_Items_0__Rules_3__Status']/option[5]".FormatString((int)NotCompliant))).Click();
            FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_3__Notes")).SendKeys(technicalNote);
            WaitForComplete();

            return this;
        }

        public InspectionForm AddGDFConditions(string PermittedValue = "11", string ActualValue = "11", string Notes = "test",
            string addParameter = "s")
        {
            // click condition buttton
            FindElement(By.XPath(".//*[@id='ConditionsSection']/div[2]/a")).Click();
            WaitForComplete();

            FindElement(By.Id("AddConditionDialog_SelectedCondition")).SendKeys(Keys.Down + Keys.Enter);
            Sleep(5000);

            // click PARAMETER button
            FindElement(By.XPath(".//*[@id='ConditionsSection']/div[2]/div/div[2]/a")).Click();
            WaitForComplete();
            FindElement(By.Id("AddConditionParameterDialog_ParameterName")).SendKeys(addParameter);
            // WaitForComplete();
            Sleep(2000);
            FindElement(By.Id("AddConditionParameterDialog_ParameterName")).SendKeys(Keys.Enter);
            WaitForComplete();
            // click on parameter button
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__PermittedValue']")).Click();
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__PermittedValue']")).Clear();
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__PermittedValue']")).SendKeys(PermittedValue);
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__ActualValue']")).Click();
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__ActualValue']")).Clear();
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__ActualValue']")).SendKeys(ActualValue);
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__Units']")).SendKeys("1");
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__Units']")).SendKeys(Keys.Down + Keys.Enter);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='InspectionTab_Conditions_Items_0__Parameters_0__Notes']")).SendKeys(Notes);
            WaitForComplete();

            return this;
        }

        public InspectionForm AddSourceTestIssues(SourceTestType.SourceTestTypeEnum SourceTestType = SourceTestType.SourceTestTypeEnum.Phase1,
            SourceTestSubType.SourceTestSubTypeEnum SourceTestSubType = SourceTestSubType.SourceTestSubTypeEnum.All,
            SourceTestProblemType.SourceTestProblemTypeEnum problem = SourceTestProblemType.SourceTestProblemTypeEnum.Failed, string Notes = "test")
        {

            //click source test isse button
            FindElement(By.XPath(".//*[@id='SourceTestIssuesSection']/div[2]/a")).Click();

            WaitForComplete();
            FindElement(By.XPath(".//*[@id='InspectionTab_SourceTestIssues_Items_0__SourceTestType']/option[3]".FormatString((int)SourceTestType))).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='InspectionTab_SourceTestIssues_Items_0__SourceTestSubType']/option[3]".FormatString((int)SourceTestSubType))).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='InspectionTab_SourceTestIssues_Items_0__Problem']/option[2]".FormatString((int)problem))).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='InspectionTab_SourceTestIssues_Items_0__Contractor']")).SendKeys(Notes);
            FindElement(By.XPath(".//*[@id='InspectionTab_SourceTestIssues_Items_0__Notes']")).SendKeys(Notes);

            return this;
        }
    

        public InspectionForm EnforcementActionForm()
        {
            //click on enforcement action
            FindElement(By.Id("EnforcementActionsTabButton")).Click();
            WaitForComplete();
             return this;
        }
        public InspectionForm EditEnforcementActionForm()
        {
            //click on enforcement action
            FindElement(By.Id("EnforcementActionsTabButton")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).GetAttribute("value"))));
            return this;
        }
        public InspectionForm EnforcementActionBAAQMD1500(string NOV = "343", string Section = "500", DateTime? PastDate = null, string Discription = "12",
           string Description = "test123")
        {

            if (!PastDate.HasValue)
                PastDate = PastDate = DateTime.Today.AddDays(-11);
            // choose recipient name
            FindElement(By.Id("EnforcementActionsTab_EnforcementActionsRecipient_Name")).Click();
            Sleep(1000);
            FindElement(By.Id("EnforcementActionsTab_EnforcementActionsRecipient_Name")).SendKeys(Keys.Down + Keys.Enter);
            WaitForComplete();
            //click NTC/NOV
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__EnforcementActionIdentifier")).SendKeys(NOV);
            WaitForComplete();
            //FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Rule_Section']")).SendKeys(Section);
            //WaitForComplete();
            //DATE/TIME
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__Occurrence")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__Issue")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__Discovery")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            //decription
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__Description")).SendKeys(Description);
            WaitForComplete();
            return this;
        }
         public InspectionForm EnforcementActionS2HalogenatedDryCleaningMachineBAAQMD10500(string NOV = "234", string Section = "331", DateTime? PastDate = null, string Discription = "test12345",
           string Description = "test123")  {

            if (!PastDate.HasValue)
                PastDate = DateTime.Today;
            //ntc or nov
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_0__DistrictEnforcementActions_0__Items_0__EnforcementActionType_NTC")).Click();
            WaitForComplete();
            //alogenated Dry Cleaning Machine click NTC/NOV
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__EnforcementActionIdentifier']")).SendKeys(NOV);
            WaitForComplete();
            //FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_1__Items_0__Rule_Section']")).SendKeys(Section);
            //WaitForComplete();
            ////DATE/TIME
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__Occurrence")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__Issue")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__Discovery")).SendKeys(PastDate.Value.ToShortDateString());
            //decription
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__Description']")).SendKeys(Description);
            WaitForComplete();
            //ntc or nov
            FindElement(By.Id("EnforcementActionsTab_EnforcementActions_Items_1__DistrictEnforcementActions_0__Items_0__EnforcementActionType_NOV")).Click();
            WaitForComplete();
            return this;
        }
        public InspectionForm EnforcementActionBAAQMD87500(string NOV = "343", string Section = "500", DateTime? PastDate = null, string Discription = "12", 
            string Description = "100 Milagra DrPacifica CA 94044-2327")
        {
            var driver = this;
            if (!PastDate.HasValue)
                PastDate = DateTime.Today.AddDays(-11);
            //click NTC/NOV
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionIdentifier']")).SendKeys(NOV);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Rule_Section']")).SendKeys(Section);
            WaitForComplete();
            //DATE/TIME
            FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Occurrence")).SendKeys(PastDate.Value.ToShortDateString());
            FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Issue")).SendKeys(PastDate.Value.ToShortDateString());
            FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Discovery")).SendKeys(PastDate.Value.ToShortDateString());
            //decription
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__Description']")).SendKeys(Description);
            //ntc or nov
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionType_NOV']")).Click();
            WaitForComplete();
            return this;
        }




        public InspectionForm AddTag(string TagID = "111", EnforcementActionType.EnforcementActionTypeEnum TagType = EnforcementActionType.EnforcementActionTypeEnum.Hose,
           EnforcementActionSubType.EnforcementActionSubTypeEnum SubType = EnforcementActionSubType.EnforcementActionSubTypeEnum.HoseCondition,
            DateTime? PastDate = null, string FueilPoint = "1", string NozzleSerialID = "12", string Description = "test", string GasGrade = "87",
            EnforcementActionType.EnforcementActionTypeEnum MinorType = EnforcementActionType.EnforcementActionTypeEnum.PhaseI,
            EnforcementActionSubType.EnforcementActionSubTypeEnum MinorSubType = EnforcementActionSubType.EnforcementActionSubTypeEnum.LooseAdapter
            )
        {
            var driver = this;
            if (!PastDate.HasValue)
                PastDate = DateTime.Today.AddDays(-11);

        

            //click on tag button
            FindElement(By.XPath(".//*[@id='TagsSection']/div[2]/a")).Click();
            WaitForComplete();

            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).SendKeys(TagID);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__TagType']/option[2]".FormatString((int)TagType))).Click();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__TagSubType']/option[3]".FormatString((int)SubType))).Click();

            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__OccurrenceDate")).SendKeys(PastDate.Value.ToShortDateString());
            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__IssuanceDate")).SendKeys(PastDate.Value.ToShortDateString());
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__FuelingPoint']")).SendKeys(FueilPoint);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__NozzleSerialId']")).SendKeys(NozzleSerialID);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Tags_Items_0__Description']")).SendKeys(Description);

            FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__GasGrade")).SendKeys(Keys.Down + Keys.Enter);
            WaitForComplete();
          
            return this;
        }
        
        public InspectionForm AddMinor(DateTime? PastDate = null, string FueilPoint = "1", string NozzleSerialID = "12", string Description = "test", string GasGrade = "89",
            EnforcementActionType.EnforcementActionTypeEnum MinorType = EnforcementActionType.EnforcementActionTypeEnum.PhaseI,
            EnforcementActionSubType.EnforcementActionSubTypeEnum MinorSubType = EnforcementActionSubType.EnforcementActionSubTypeEnum.LooseAdapter
            )
        {
            var driver = this;
            if (!PastDate.HasValue)
                PastDate = DateTime.Today.AddDays(-11);

            // click on Minors

            FindElement(By.XPath(".//*[@id='MinorsSection']/div[2]/a")).Click();
            WaitForComplete();

            //Minor type
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__MinorType']/option[2]".FormatString((int)MinorType))).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__MinorSubType']/option[3]".FormatString((int)MinorSubType))).Click();
            WaitForComplete();
            //past date
            FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__OccurrenceDate")).SendKeys(PastDate.Value.ToShortDateString());
            FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__IssuanceDate")).SendKeys(PastDate.Value.ToShortDateString());

            WaitForComplete();
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__FuelingPoint']")).SendKeys(FueilPoint);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__NozzleSerialId']")).SendKeys(NozzleSerialID);
            FindElement(By.XPath(".//*[@id='EnforcementActionsTab_Minors_Items_0__Description']")).SendKeys(Description);
            WaitForComplete();
            //gas Grade
            FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__GasGrade")).SendKeys(Keys.Down);
            FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__GasGrade")).SendKeys(Keys.Down + Keys.Enter);
            WaitForComplete();

            return this;
        }
        
        public InspectionForm TechnicalServices()
        {

            Sleep(5000);
            //click on technical service
            FindElement(By.XPath(".//*[@id='FollowUpsTabButton']")).Click();
            Sleep(5000);
            return this;
        }
        public InspectionForm TecchnicalServiceFaccilityWideBAAQMD10100BAAQMD10300(FollowUpType.FollowUpTypeEnum LabAnalysisRequest = FollowUpType.FollowUpTypeEnum.LabAnalysisRequest,
            FollowUpType.FollowUpTypeEnum SourceTestRequest = FollowUpType.FollowUpTypeEnum.SourceTestRequest,
            string RequestID = "34", string Note = "TEST123")
        {
            //Facility Wide- Techinical type
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_0__Items_0__Type']/option[2]".FormatString((int)LabAnalysisRequest))).Click();
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_0__Items_0__RequestId']")).SendKeys(RequestID);
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_0__Items_0__Notes']")).SendKeys(Note);
            //s2/S3 / Non-Halogenated Dry Cleaning Machine / BAAQMD 10-100
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_1__Items_0__Type']/option[3]".FormatString((int)SourceTestRequest))).Click();
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_1__Items_0__RequestId']")).SendKeys(RequestID);
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_1__Items_0__Notes']")).SendKeys(Note);
            //s2/S3 / Non-Halogenated Dry Cleaning Machine / BAAQMD 10-300
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_2__Items_0__Type']/option[2]".FormatString((int)LabAnalysisRequest))).Click();
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_2__Items_0__RequestId']")).SendKeys(RequestID);
            FindElement(By.XPath(".//*[@id='FollowUpsTab_FollowUps_Items_2__Items_0__Notes']")).SendKeys(Note);
             WaitForComplete();
            return this;
        }
        public InspectionForm ReviewInspection()
        {
            // click on review inspection
            FindElement(By.XPath(".//*[@id='ReviewInspectionTabButton']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ComplianceWithRegulationsSection")).GetAttribute("class"))));

            return this;
        }
        public InspectionForm SubmitInspectionForm()
        {
            //click on Submit Button
            FindElement(By.XPath("html/body/div[1]/div[1]/div[1]/div/a[5]")).Click();
            Sleep(68000);
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Refresh();
            Sleep(5000);
            NavigateTo<SPA>("spa/#/signin");
           
            return this;
        }
        public InspectionForm RevisionRequest(string RevisionsNotes = "Revisions Requested Dialog Requested Notes")
        {
            //click Revision Requested button
            FindElement(By.XPath("html/body/div[1]/div[1]/div[1]/div/a[1]")).Click();
            WaitForComplete();
            FindElement(By.Id("RevisionsRequestedDialog_RequestedNotes")).SendKeys(RevisionsNotes);
            WaitForComplete();
            FindElement(By.Id("RevisionsRequestedDialog_RequestedNotes")).SendKeys(Keys.Enter);
            Sleep(5000);
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Refresh();
            WaitForComplete();
          
            NavigateTo<SPA>("spa/#/signin");
//            WaitForComplete();
            return this;
        }
        private DateTime RandomPastDay()
        {
            DateTime start = new DateTime(2010, 1, 1);
            Random gen = new Random();

            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));

        }

        public InspectionForm ComplianceHistory()
        {
            FindElement(By.XPath(".//*[@id='top']/ul/li[3]/areturn ")).Click();
            WaitForComplete();
            return this;
        }

        public InspectionForm ClickInspectionTask()
        {
            string GDFTaskNum = SeleniumUITests.SharedProperties["GDFTaskNumber"];
            
            //CLICK GDF Task

            WaitForComplete();
            FindElement(By.LinkText(GDFTaskNum)).Click();
            //SeleniumUITests.SharedProperties["GDFTaskNumber"] = GDFTaskNum;
            this.webDriver.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("GeneralInformationTab_Inspection_StartDate")).GetAttribute("value"))));

            return this;
        }
        public InspectionForm VerifyTaskClosed()
        {

            // var GDFFacilityNum = FindElement(By.XPath(".//*[@id='1']/td[4]")).Text;
            var GDFFacilityNum = SeleniumUITests.SharedProperties["GDFFacilityNumber"];
            SeleniumUITests.SharedProperties["GDFInspectionPage"] = new Uri(wd.Url).PathAndQuery;
            //WaitForComplete();
            FindElement(By.XPath(".//*[@id='top']/ul/li[3]/a")).Click();
            WaitForComplete();
            FindElement(By.Id("FacilityNumber")).Click();
            FindElement(By.Id("FacilityNumber")).SendKeys(GDFFacilityNum);
            FindElement(By.Id("searchSubmit")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Click();
            WaitForComplete();
                      
            //click inspection history
            InspectionHistory();
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

        public InspectionForm InspectionHistory()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[7]/a"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<InspectionForm>(url);
            

        }
        public InspectionForm VerifyCompliancehHistory()
        {
            //click complience history
            FindElement(By.XPath("//a[contains(text(),'Compliance History')]")).Click();

            var text1 = FindElement(By.XPath(".//*[@id='gbox_enforcementActions_table']")).Text;
            if (!string.Equals("Minor", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            if (!string.Equals("Tag", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            if (!string.Equals("Notice of Violation", text1))
            {
                throw new NotFoundException("Expected text!");
            }

            return this;
           
        }
        public InspectionForm FindFacility()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='top']/ul/li[3]/a"));
            var url = ctrl.GetAttribute("href");
            return NavigateTo<InspectionForm>(url);
        }
        public InspectionForm SearchFacility(string FacilityNo = "289")
        {           
            FindElement(By.Id("FacilityNumber")).Click();
            FindElement(By.Id("FacilityNumber")).SendKeys(FacilityNo);
            FindElement(By.Id("searchSubmit")).Click();
            return this;
        }
    }
}
