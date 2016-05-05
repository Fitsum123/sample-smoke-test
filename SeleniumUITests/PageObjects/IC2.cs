using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gov.Baaqmd.BusinessObjects;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class IC2 : PageObject
    {
        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "ic2";
            }
        }

        /// <summary>
        /// Loads task screen from app page and loops until we have approval or 10 times.
        /// </summary>
        public IC2 ApproveApp(int approvalCount = 10)
        {
            int iApproval = approvalCount;  // if we exceed this, we're in a bad place.

            WaitForComplete();

            while (iApproval-- > 0)
            {
                try {

                    WaitFor(() => !string.IsNullOrEmpty(FindElement(By.CssSelector("#ApplicationStatusField")).Text));

                    //short circuit out of here
                    if (FindElement(By.CssSelector("#ApplicationStatusField")).Text.ToLowerInvariant() == "approved")
                        return this;


                    Click("#SubmitApproval", retrySeconds: 5);

                    //switch to task window
                    WaitForEnabled(By.CssSelector("#IfrDialog"));
                    wd.SwitchTo().Frame("IfrDialog");

                    if (wd.FindElement(By.CssSelector("#MainContent_taskHistoryList > tbody:nth-child(1) > tr:nth-child(2) > td:nth-child(3)")).Text == "Closed")
                        //task is closed, we're good
                        break;

                    if (FindElement(By.CssSelector("#MainContent_taskHistoryList > tbody:nth-child(1) > tr:nth-child(2) > td:nth-child(3)")).Text == "Revalidation Requested") 
                        throw new BaaqmdException("Task is in invalid state!");


                    Click("#MainContent_cbTaskStatus_cbTaskStatus_Button");
                    Click("#MainContent_cbTaskStatus_cbTaskStatus_OptionList li:nth-child(3)"); //Complete


                    FindElement(By.Id("MainContent_txtComments")).Click();
                    FindElement(By.Id("MainContent_txtComments")).Clear();
                    FindElement(By.Id("MainContent_txtComments")).SendKeys("Automation");
                    Click("#MainContent_btnUpdateTask");

                    WaitForComplete();
                    Refresh();
                    

                } catch(OpenQA.Selenium.NoSuchElementException)
                {
                    break;
                }
                catch (OpenQA.Selenium.ElementNotVisibleException)
                {
                    Refresh();
                    continue;
                }
            }

            //ensure we are on the main frame..
            Refresh();

            Retry(() => {

                //Need the status field box to be populated - it's ajax so let's wait until it is.
                //WaitForComplete();

                WaitFor(() => !string.IsNullOrEmpty(FindElement(By.CssSelector("#ApplicationStatusField")).Text));
                Assert.AreEqual("Approved", FindElement(By.CssSelector("#ApplicationStatusField")).Text);
            }, onFailure: () =>
            {
                Sleep(1000);
                Refresh();
            });

            return this;            
        }

        internal IC2 VerifyUpdateStatus(params string[] states)
        {
            WaitForComplete();

            var iRow = 2;
            foreach(var state in states)
            {
                var estring = "#updateStatusHistory_table > tbody:nth-child(1) > tr:nth-child({0}) > td:nth-child(1)".FormatString(iRow);

                var value = FindElement(By.CssSelector(estring)).Text;

                if (string.Compare(value, state, StringComparison.InvariantCultureIgnoreCase) != 0)
                    throw new BaaqmdException("Did not find expected state {0}/{1}".FormatString(state, value));

                iRow++;
            }


            //while here, grab the invoice number
            SeleniumUITests.SharedProperties["InvoiceNumber"] = FindBy("#InvoiceNumber").Text;


            return this;
        }

        internal IC2 VerifyRenewalStatus(params string[] states)
        {
            WaitForComplete();

            var iRow = 2;
            foreach (var state in states)
            {
                var estring = "#renewalStatusHistory_table > tbody:nth-child(1) > tr:nth-child({0}) > td:nth-child(1)".FormatString(iRow);

                var value = FindElement(By.CssSelector(estring)).Text;

                if (string.Compare(value, state, StringComparison.InvariantCultureIgnoreCase) != 0)
                    throw new BaaqmdException("Did not find expected state {0}/{1}".FormatString(state, value));

                iRow++;
            }
            
            //while here, grab the invoice number
            SeleniumUITests.SharedProperties["InvoiceNumber"] = FindBy("#InvoiceNumber").Text;

            return this;
        }


        public IC2 ConfirmPermitExpireDate(DateTime targetDate)
        {
            //normalize the target date to the first of the month.
            targetDate = new DateTime(targetDate.Year, targetDate.Month, 1);
            
            WaitForComplete();

            Retry( ()=>
            {
                var peDate = DateTime.Parse(FindElement(By.CssSelector("#PermitExpireDate")).GetAttribute("value"));

                if (targetDate < peDate)
                    throw new BaaqmdException("PermitDate not at least target");

            }, onFailure: () =>
            {
                //Could be waiting on workflow here, refresh, try again
                Sleep(500);

                wd.SwitchTo().DefaultContent();
                wd.Navigate().Refresh();
            });

            return this;
        }

        public ACPO NavigateToAppSummary()
        {
            var ctrl = FindElement(By.CssSelector("#lnkApplicationSummary"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<ACPO>(url);

        }

        public ACPO NavigateToRenewalFees()
        {
            var ctrl = FindElement(By.CssSelector("#lnkRenewalFees"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<ACPO>(url);
        }

        public ACPO NavigateToRenewalUpdate()
        {

            return NavigateTo<ACPO>(GetRenewalUpdateUrl());

        }
      
        public string GetRenewalUpdateUrl()
        {
            var ctrl = FindElement(By.CssSelector("#lnkUpdateInfo"));

            var url = ctrl.GetAttribute("href");

            return url;
        }

        public IC2 VerifyInvoiceDueDate(DateTime invoiceDueDate)
        {
            WaitForComplete();

            ExecuteScript("$('#InvoiceDuedate').css('display', 'block')");

            var dd = FindBy("#InvoiceDuedate").Text;

            var target = DateTime.Parse(dd);

            var ts = (invoiceDueDate.Date - target.Date).TotalDays;

            if (ts < -2 || ts > 2)
                throw new ArgumentOutOfRangeException("Invoice due date is not correct, should be renewal date. {0}".FormatString(ts));

            return this;
        }

        public string CurrentWindowHandle()
        {
            return wd.CurrentWindowHandle;

        }


       
        public EnforcementFroms AddTask()
        {
           
            WaitForComplete();
            wd.FindElement(By.Id("scbd_btnAddTask")).Click();
          
            WaitForEnabled(By.CssSelector("#IfrDialog"));
            wd.SwitchTo().Frame("IfrDialog");


            wd.FindElement(By.Id("MainContent_cbTaskCategory_cbTaskCategory_Button")).Click();
            Sleep(1000);
            wd.FindElement(By.XPath("//*[@id='MainContent_cbTaskCategory_cbTaskCategory_OptionList']/li[3]")).Click();
            Sleep(1000);
          

            wd.FindElement(By.Id("MainContent_cbTaskType_cbTaskType_Button")).Click();
            Sleep(2000); 
            wd.FindElement(By.XPath("//*[@id='MainContent_cbTaskType_cbTaskType_OptionList']/li[7]")).Click();

            Sleep(1000); 

            new SelectElement(wd.FindElement(By.Id("MainContent_ddlFacilityStatusType"))).SelectByText("Permitted");
            wd.FindElement(By.Id("MainContent_searchButton")).Click();

            Sleep(1000); 

            wd.FindElement(By.Id("MainContent_lvResults_btnResultsName_0")).Click();
            wd.FindElement(By.Id("MainContent_btnCreateAndViewTask")).Click();

            var viewAssociateFormButton = wd.FindElement(By.Id("MainContent_lnkForm"));
           var url= viewAssociateFormButton.GetAttribute("title");

            viewAssociateFormButton.Click();


            return NavigateTo<EnforcementFroms>(url);
            
        }
      

   

        public IC2 LogOut(string windowHandleName)
        {
            //wd.Close();
            wd.SwitchTo().Window(windowHandleName);
            
            wd.FindElement(By.LinkText("SIGN OUT")).Click();
            return this;
        }

        public IC2 AssignSupervisorForInspector(string inspectorEmail, string supervisorSelectionName)
        {
             
            var emailColumn= wd.FindElements(By.CssSelector("#readEmployee_table td:nth-child(5)"));
            for (int i = 0; i < emailColumn.Count; i++)
            {
                if (emailColumn[i].Text == inspectorEmail)
                {
                    var id = string.Format("{0}_ManagerId", i);
                    wd.FindElement(By.CssSelector(string.Format("#readEmployee_table tr:nth-child({0}) td:nth-child(7)", i+1))).Click();//
                    wd.FindElement(By.Id(id)).Click();
                   Sleep(3000);
                    new SelectElement(wd.FindElement(By.Id(id))).SelectByText(supervisorSelectionName); // supervisorSelectionName = "Test Engineer Supervisor 2"
                    wd.FindElement(By.CssSelector(string.Format("#readEmployee_table tr:nth-child({0}) td:nth-child(5)", i))).Click();//
                    Sleep(3000);
                    break;

                }
            }  
            return this;
        
       }

        public EnforcementFroms SelectTask(string taskNumber)
        {
            Sleep(5000);
            var taskSelector = string.Format("#tasks tbody td[title={0}] a", taskNumber);
            wd.FindElement(By.CssSelector(taskSelector)).Click();
            WaitForEnabled(By.CssSelector("#IfrDialog"));
            wd.SwitchTo().Frame("IfrDialog");

            var viewAssociateFormButton = wd.FindElement(By.Id("MainContent_lnkForm"));
            var url = viewAssociateFormButton.GetAttribute("title");

            viewAssociateFormButton.Click();

            return NavigateTo<EnforcementFroms>(url);
        }
        public IC2 VerifyCorrectFacilityPageisDisplayedAllMailingAddress()
        {
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityStatus']")).GetAttribute("class"))));
            FindElement(By.XPath(".//*[@id='top']/ul/li[4]/a")).Click();
            WaitForComplete();
            return this;
        }
       
        public IC2 Reassign()
        {
            FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            //switch to task window
            WaitForEnabled(By.CssSelector("#IfrDialog"));
            wd.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[4]")).Click();

            WaitForEnabled(By.Id("MainContent_txtName"));
            FindElement(By.Id("MainContent_txtName")).Click();
            FindElement(By.Id("MainContent_txtName")).Clear();
            FindElement(By.Id("MainContent_txtName")).SendKeys("Test engineer");
            FindElement(By.XPath(".//*[@id='MainContent_txtName_AutoCompleteExtender_completionListElem']/li[1]")).Click();
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            Sleep(25000);
            return this;
        }
        public IC2 MyDashboard()
        {
            var ctrl = FindElement(By.XPath(".//*[@id='myDashboard']"));
            var url = ctrl.GetAttribute("href");
            NavigateTo<IC2>(url);
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='1']/td[1]/a")).GetAttribute("class"))));
            FindElement(By.XPath("//div[2]/div/div/section/div[2]/div[3]/div[4]/div/table/tbody/tr[2]/td/a")).Click();
            Sleep(25000);
            FindElement(By.XPath(".//*[@id='1']/td[1]/a/img")).Click();
            WaitForEnabled(By.CssSelector("#IfrDialog"));
            wd.SwitchTo().Frame("IfrDialog");
            WaitForComplete();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityDeviceName']")).GetAttribute("name"))));
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Refresh();
            Sleep(20000);
            return this;
        }
        public IC2 clickApplicationDevice(string materialName3 = "Diesel fuel", string annualUsage1 = "123")
        {
            FindElement(By.XPath(".//*[@id='lnkAppDevices']")).Click();
            WaitForEnabled(By.CssSelector("#IfrDialog"));
            wd.SwitchTo().Frame("IfrDialog");
            Sleep(20000);
            FindElement(By.XPath(".//*[@id='modifiedDevices']/div[2]/div/div[5]/a/span")).Click();
            FindElement(By.XPath(".//*[@id='form_ContinueButton']")).Click();
            //cleck red x
            FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__RepeaterWrapper']/div[1]/div[3]/span")).Click();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel_deleteConfirmYesButton")).Click();
            Click("#Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton");
            FindBy("div[data-materialname=\"{0}\"]".FormatString(materialName3)).Click();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).Clear();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).SendKeys(annualUsage1);
            FindElement(By.XPath(".//*[@id='form_SaveAndCloseButton']/span")).Click();
            WaitForComplete();
            return this;
        }
        public IC2 DoneWithForm()
        {
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            return this;
        }
        public IC2 ManageRegulations(string addBAAQMDREG = "1-2", string ConditionNumber = "701", string OtherRegulations = "CARB", 
            string Title = "TITLE", string ConditionNumber1 = " 100013", string ConditionNumber2 = " 100016",
            ConditionDurationType.ConditionDurationTypeEnum conditionduration = ConditionDurationType.ConditionDurationTypeEnum.Construction, string phase2 = "test123",
            string carb = "test234", string addBAAQMDREG2 = "8-1")
        {
            //facility wide regulations
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[2]/a/span")).Click();
            FindElement(By.XPath(".//*[@id='addBaaqmdRegulationButton']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Click();
            FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).SendKeys(addBAAQMDREG);
            FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Sleep(1000);
            FindElement(By.XPath("//div[4]/a[2]/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Sleep(1000);
            ///facility wide condition
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[4]/a/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys(ConditionNumber);
            FindElement(By.XPath(".//*[@id='searchButton']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            WaitForComplete();
            // device regulation
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[6]/div[3]/a/span")).Click();
            FindElement(By.XPath(".//*[@id='addStateFedRegulationButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='stateFedRegSearch']")).Click();
            FindElement(By.XPath(".//*[@id='stateFedRegSearch']")).SendKeys(OtherRegulations);
            FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='stateFedRegEditSaveButton']/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Sleep(1000);
            // device CONDITIONS
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[8]/div[3]/a/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys(ConditionNumber1);
            FindElement(By.XPath(".//*[@id='searchButton']")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).Clear();
            FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys(ConditionNumber2);
            FindElement(By.XPath(".//*[@id='searchButton']")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='deviceConditionsList']/div[7]/a/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='ConditionDurationTypeId']/option[2]".FormatString((int)conditionduration))).Click();
            FindElement(By.XPath(".//*[@id='Parameters_0__Value_Value']")).Click();
            FindElement(By.XPath(".//*[@id='Parameters_0__Value_Value']")).SendKeys(phase2);
            FindElement(By.XPath(".//*[@id='Parameters_1__Value_Value']")).Click();
            FindElement(By.XPath(".//*[@id='Parameters_1__Value_Value']")).SendKeys(carb);
            FindElement(By.XPath(".//*[@id='editParameterDoneButton']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Sleep(1000);
            //add button
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[8]/div[3]/span")).Click();
            WaitForComplete();
            //managemnet exemptiopn
            FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[6]/div[6]/a/span")).Click();
            FindElement(By.XPath(".//*[@id='addBaaqmdRegulationButton']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Click();
            FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Clear();
            FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).SendKeys(addBAAQMDREG2);
            Sleep(1000);
            FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='baaqmdRegEditSaveButton']/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
            WaitForComplete();
            return this;

        }
        public IC2 AddFee(ScenarioFeeType.ScenarioFeeTypeEnum Feetype = ScenarioFeeType.ScenarioFeeTypeEnum.RiskScreenFeeAdditional, string amount = "500"
            , string comment = "Test Comment")
        {
            //FindElement(By.XPath(".//*[@id='addFeeButton']/span")).Click();
            //FindElement(By.XPath(".//*[@id='feeType']/option[7]".FormatString((int)Feetype))).Click();
            //FindElement(By.XPath(".//*[@id='amount']")).Click();
            //FindElement(By.XPath(".//*[@id='amount']")).SendKeys(amount);
            //FindElement(By.XPath(".//*[@id='comment']")).Click();
            //FindElement(By.XPath(".//*[@id='comment']")).SendKeys(comment);
            //FindElement(By.XPath(".//*[@id='feeChangeSave']/span")).Click();
            //Sleep(5000);
            //click edit button
            //FindElement(By.XPath(".//*[@id='applicationFeeSummary']/table/tbody/tr[8]/td[3]/div[1]")).Click();
            //FindElement(By.XPath(".//*[@id='feeChangeCancel']/span")).Click();
            //WaitForComplete();
            ////click red x
            //FindElement(By.XPath(".//*[@id='applicationFeeSummary']/table/tbody/tr[8]/td[3]/div[1]")).Click();
            //FindElement(By.XPath(".//*[@id='feeChangeCancel']/span")).Click();
            //WaitForComplete();
            FindElement(By.XPath(".//*[@id='applicationFooter']/a[2]")).Click();
            Sleep(20000);
            return this;
        }
        public IC2 SetInspectorAndsupervisorLogin()
        {
           // Sleep(5000);
            FindElement(By.XPath("//tr[15]/td[7]")).Click();
            FindElement(By.XPath(".//*[@id='14_ManagerId']/option[556]")).Click();
            WaitForComplete();
                   //inspector
            FindElement(By.XPath("//tr[15]/td[9]")).Click();
            FindElement(By.XPath(".//*[@id='14_EmployeeTypeId']/option[21]")).Click();
            WaitForComplete();
            //Sleep(15000);
            //enforcement
            FindElement(By.XPath("//tr[15]/td[11]")).Click();
            FindElement(By.XPath(".//*[@id='14_DivisionTypeId']/option[6]")).Click();
            WaitForComplete();
            //engi1
            FindElement(By.XPath("//tr[5]/td[7]")).Click();
            FindElement(By.XPath(".//*[@id='4_ManagerId']/option[391]")).Click();
            WaitForComplete();
            //enfo. supervisor
            FindElement(By.XPath("//tr[5]/td[9]")).Click();
            FindElement(By.XPath(".//*[@id='4_EmployeeTypeId']/option[12]")).Click();
            WaitForComplete();
            //enforcemnt
            FindElement(By.XPath("//tr[5]/td[11]")).Click();
            FindElement(By.XPath(".//*[@id='4_DivisionTypeId']/option[6]")).Click();
            WaitForComplete();
            FindElement(By.XPath("//tr[5]/td[4]")).Click();
            return this;

        }
       public IC2 CustomerFacilityPage()
        {
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='FacilityStatus']")).GetAttribute("class"))));
            FindElement(By.Id("lnkCustPortal")).Click();
            WaitForComplete();
            return this;
        }
        public IC2 StatusAndStatusHistory()
        {
            FindElement(By.XPath(".//*[@id='lnkAppStatusHistory']")).Click();
            WaitForComplete();
            return this;
        }
        public IC2 GenerateEvalReport()
        {
            FindElement(By.Id("GenerateEvalReport")).Click();
            Sleep(20000);
            return this;
        }
        public IC2 CustomerDocuments()
        {
            FindElement(By.Id("lnkDocuments")).Click();
            Sleep(10000);
            Refresh();
            return this;
        }
        public IC2 UPLOADDOCUMENT()
        {
            FindElement(By.Id("idUploadDocument")).Click();
            FindElement(By.Id("fileupload")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            FindElement(By.Id("FileSubmit")).Click();
            Sleep(5000);
            return this;
        }
        public IC2 NewOwningName()
        {
            Sleep(5000);
            var ctrl = FindElement(By.XPath("//td[3]/a"));

            var url = ctrl.GetAttribute("href");

            return NavigateTo<IC2>(url);
        }
        public IC2 CustomerDocumentsTransferOfOwnership()
        {
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='transferDetails']/div/div[1]/div/ul/li[8]/a/span/span")).Click();
            WaitForComplete();
            Refresh();
            Sleep(5000);
            return this;
        }
        public IC2 OpenApplication()
        {
            FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[1]/a")).Click();
            Sleep(10000);
            FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Click();
            Sleep(5000);
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.XPath(".//*[@id='InvoiceInfo']/div[1]/span[2]")).GetAttribute("class"))));
            string InvoiceNum = FindElement(By.XPath(".//*[@id='InvoiceInfo']/div[1]/span[2]")).Text;
            string BalaDue = FindElement(By.XPath(".//*[@id='InvoiceInfo']/div[2]/span[2]")).Text;
            SeleniumUITests.SharedProperties["InvoiceNumber"] = InvoiceNum;
            SeleniumUITests.SharedProperties["BalanceDue"] = BalaDue;
            return this;
        }
        public IC2 Permits()
        {
            FindElement(By.XPath(".//*[@id='lnkPermits']/span/span")).Click();
            WaitForComplete();
            Refresh();
            return this;

        }
    }

   
}
