using System;
using System.Collections.Generic;
using System.Linq;
using FluentAutomation;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using OpenQA.Selenium.Remote;
using Gov.Baaqmd.Tests.WebUITests;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class SPAGDF : PageObject
    {
        /// <summary>
        /// For imported tests
        /// </summary>
        private SPAGDF driver
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "spa";
            }
        }



        protected bool WaitForAccessToken()
        {
            int iCounter = 30;
            while (iCounter > 0)
            {
                var status = (string)ExecuteScript("return document.cookie;");
                if (status.Contains("access_token"))
                    return true;
                else
                {
                    iCounter--;
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }

        protected bool WaitForIdle()
        {
            int iCounter = 30;
            while (iCounter-- > 0)
            {
                try
                {
                    System.Threading.Thread.Sleep(250);
                    var isBusy = (bool)ExecuteScript("return document.isBusy;"); //set by our angular controller
                    if (!isBusy) //we're not busy
                        return true;
                    else
                        //Still busy...
                        System.Threading.Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }


        public SPAGDF Login(string userName, string password)
        {
            this.ClearAllCookies();
            this.Navigate("/spa/#/signin?returnUrl=/IC2/", true);

            WaitForComplete();

            FindElement(By.Id("email")).Clear();
            FindElement(By.Id("email")).SendKeys(userName);
            FindElement(By.Id("password")).Clear();
            FindElement(By.Id("password")).SendKeys(password);
            FindElement(By.Id("btn_submit")).Click();

            WaitForAccessToken();

            return this;
        }

        public SPAGDF NavigateToQAAdmin()
        {
            this.Navigate("spa/#/admin/qa", true);

            WaitForComplete();
            return this;
        }

        public SPAGDF PayInvoiceByAppNumber(string applicationNumber)
        {
            FindElement(By.CssSelector(".applicationNumber"));

            var outstandingBalance = getinvoiceBalance(applicationNumber);

            FindElement(By.Name("amount")).SendKeys(outstandingBalance);

            FindElement(By.Name("payInvoiceButton")).Click();

            WaitForIdle();

            //Balance will refresh
            outstandingBalance = getinvoiceBalance(applicationNumber);
            Assert.AreEqual("0.00", outstandingBalance);

            return this;
        }

        private string getinvoiceBalance(string applicationNumber)
        {
            FindElement(By.Name("applicationNumber")).Clear();
            FindElement(By.Name("applicationNumber")).SendKeys(applicationNumber);
            FindElement(By.Name("getInvoiceByApplicationNumber")).Click();

            WaitForIdle();

            string outstandingBalance = "";
            var iCounter = 30;
            while (outstandingBalance.Length == 0
                && iCounter-- > 0)
            {
                outstandingBalance = FindElement(By.CssSelector("span.invoiceOutstandingBalance")).Text;
                System.Threading.Thread.Sleep(1000);
            }
            //strip off $
            outstandingBalance = outstandingBalance.Substring(1);

            return outstandingBalance;
        }
        /// Switch to the IFrame and get the new driver.
        /// Call SwitchOutofIFrame after done working with the element.
        /// </summary>
        /// <param name="driver">the current driver</param>
        /// <returns>the IFrame driver.</returns>
        /// 



        public void TestIC2ReviewGDFApplicationRegulationCondition(string facilityNumber)
        {
            var driver = this;

            driver.FindElement(By.XPath(".//*[@id='content-area']/div[2]/div/ul/li[2]/a")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Click();
            driver.FindElement(By.Id("FacilityNumber")).Clear();

            driver.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);
            driver.FindElement(By.Id("searchSubmit")).Click();
            driver.FindElement(By.XPath(".//*[@id='1']/td[2]/a")).Click();

            //Click application XPath
            driver.FindElement(By.XPath(".//*[@id='leftpanel']/ul/li[2]/ul/li[1]/a")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='1']/td[1]/a")).Click();
            Thread.Sleep(8000);
            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[4]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtName']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtName']")).Clear();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtName']")).SendKeys("Test Engineer");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtName_AutoCompleteExtender_completionListElem']/li[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(8000);
            //REFRESH
            driver.FindElement(By.Id("UpdateApplicationStatus")).SendKeys(OpenQA.Selenium.Keys.F5);
            Thread.Sleep(10000);
            driver.FindElement(By.Id("myDashboard")).Click();
            Thread.Sleep(10000);
            //application number         
            driver.FindElement(By.XPath("//div[2]/div/div/section/div[2]/div[3]/div[4]/div/table/tbody/tr[2]/td/a")).Click();
            Thread.Sleep(10000);
            //device icon

            driver.FindElement(By.XPath(".//*[@id='1']/td[1]/a/img")).Click();
            this.webDriver.SwitchTo().Frame("IfrDialog");
            Thread.Sleep(8000);
            //close the box              
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(8000);
            driver.FindElement(By.Id("UpdateApplicationStatus")).SendKeys(OpenQA.Selenium.Keys.F5);
            Thread.Sleep(5000);
            //application deveice link
            driver.FindElement(By.Id("lnkAppDevices")).Click();
            Thread.Sleep(8000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            //Click open form button
            driver.FindElement(By.XPath(".//*[@id='modifiedDevices']/div[2]/div/div[5]/a/span")).Click();
            Thread.Sleep(8000);
            //Go to Step 2 Material Usage 
            driver.FindElement(By.XPath("//div[1]/div/div/div/div/div/div[2]/form/div/div[2]/div[2]/div/p")).Click();
            Thread.Sleep(5000);
            //Click on the red X to remove Diesel
            driver.FindElement(By.XPath("//div[1]/div/div/div/div/div/div[2]/form/div/div[7]/div[1]/div/div/div/div[2]/div[1]/div[3]/span")).Click();
            Thread.Sleep(3000);
            //Are you sure you want to delete this material?
            driver.FindElement(By.XPath("//div[1]/div/div/div/div/div/div[2]/form/div/div[7]/div[1]/div/div/div/div[2]/div[3]/a[1]/span")).Click();
            Thread.Sleep(8000);
            //Click Add Another Material button 
            driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton']/span")).Click();
            Thread.Sleep(5000);
            //Select Diesel
            driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__materialList']/div[3]")).Click();
            Thread.Sleep(8000);
            //Enter 1234 for Maximum Annual Usage 
            driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage']")).Click();
            driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage']")).SendKeys("1234");
            Thread.Sleep(5000);
            // Click Save and Close button
            driver.FindElement(By.XPath(".//*[@id='form_SaveAndCloseButton']/span")).Click();
            Thread.Sleep(8000);
            //Done with form
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(25000);
            ///continue to next page- ESTIMATED EMISSIONS
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);
            //continue to next page - Evaluation Information
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(8000);
            //continue to next page - HRSA FORM
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(8000);
            //continue to next page-TRIGGER EVALUATIONS
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(25000);
            //FACILITY WIDE REGULATIONS 
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[2]/a/span")).Click();
            Thread.Sleep(10000);
            //ADD BAAQMD REGULATIONS 
            driver.FindElement(By.XPath(".//*[@id='addBaaqmdRegulationButton']/span")).Click();
            // this.driver.SwitchTo().Frame("IfrDialog");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Click();
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).SendKeys("1-2");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegEditSaveButton']/span")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);

            //FACILITY WIDE CONDITIONS 
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[4]/a/span")).Click();
            Thread.Sleep(5000);
            //SEARCH PERMIT CONDITIONS 
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys("701");
            Thread.Sleep(3000);
            //search permit condition
            driver.FindElement(By.XPath(".//*[@id='searchButton']/span")).Click();
            Thread.Sleep(5000);
            //ADD
            driver.FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a/span")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);


            //DEVICE REGULATIONS
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[6]/div[3]/a/span")).Click();
            Thread.Sleep(5000);
            //ADD OTHER REGULATIONS 
            driver.FindElement(By.XPath(".//*[@id='addStateFedRegulationButton']/span")).Click();
            //  this.driver.SwitchTo().Frame("IfrDialog");
            Thread.Sleep(5000);
            //carb-REGULATION
            driver.FindElement(By.XPath(".//*[@id='stateFedRegSearch']")).Click();
            driver.FindElement(By.XPath(".//*[@id='stateFedRegSearch']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='stateFedRegSearch']")).SendKeys("CARB");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Thread.Sleep(3000);
            //TEST-TITLE
            driver.FindElement(By.XPath(".//*[@id='stateFedRuleTitle']")).SendKeys("TEST");
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='stateFedRegEditSaveButton']/span")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);



            //DEVICE CONDITIONS 
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[8]/div[3]/a/span")).Click();
            Thread.Sleep(5000);
            //SEARCH PERMIT CONDITIONS -100013
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys("100013");
            Thread.Sleep(3000);
            //search permit condition
            driver.FindElement(By.XPath(".//*[@id='searchButton']/span")).Click();
            Thread.Sleep(5000);
            //ADD
            driver.FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a/span")).Click();
            Thread.Sleep(5000);
            //SEARCH PERMIT CONDITIONS -100016
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Click();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='ConditionNumber']")).SendKeys("100016");
            Thread.Sleep(3000);
            //search permit condition
            driver.FindElement(By.XPath(".//*[@id='searchButton']/span")).Click();
            Thread.Sleep(5000);
            //ADD
            driver.FindElement(By.XPath(".//*[@id='searchResults']/div/div[2]/div[1]/a/span")).Click();
            Thread.Sleep(5000);

            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(8000);
            //CLICK + next to #100013 AND #100016
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[8]/div[3]/a/span")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='deviceConditionsList']/div[4]/span")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='deviceConditionsList']/div[8]/span")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='deviceConditionsList']/div[7]/a/span")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='Parameters_0__Value_Value']")).Click();
            driver.FindElement(By.XPath(".//*[@id='Parameters_0__Value_Value']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='Parameters_0__Value_Value']")).SendKeys("PUMPS");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='Parameters_1__Value_Value']")).Click();
            driver.FindElement(By.XPath(".//*[@id='Parameters_1__Value_Value']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='Parameters_1__Value_Value']")).SendKeys("555");
            Thread.Sleep(3000);
            //DONE
            driver.FindElement(By.XPath(".//*[@id='editParameterDoneButton']/span")).Click();
            Thread.Sleep(5000);
            //Click the + next to #100016
            driver.FindElement(By.XPath(".//*[@id='deviceConditionsList']/div[8]/span")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);

            // DEVICE S1 EXEMPTIONS 
            driver.FindElement(By.XPath(".//*[@id='conditionsRegulations']/div[6]/div[6]/a/span")).Click();
            Thread.Sleep(5000);
            //ADD BAAQMD REGULATIONS 
            driver.FindElement(By.XPath(".//*[@id='addBaaqmdRegulationButton']/span")).Click();
            //  this.driver.SwitchTo().Frame("IfrDialog");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Click();
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegSearch']")).SendKeys("8-1");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("html/body/div[6]/ul/li[1]")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='baaqmdRegEditSaveButton']/span")).Click();
            Thread.Sleep(5000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(5000);
            //CONTINUE TO NEXT PAGE
            driver.FindElement(By.XPath(".//*[@id='wizardNextButton']/span")).Click();
            Thread.Sleep(10000);
            //ADD A FEE
            driver.FindElement(By.XPath(".//*[@id='addFeeButton']/span")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='feeType']/option[7]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='amount']")).Click();
            driver.FindElement(By.XPath(".//*[@id='amount']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='amount']")).SendKeys("500");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='comment']")).Click();
            driver.FindElement(By.XPath(".//*[@id='comment']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='comment']")).SendKeys("TEST COMMENT");
            Thread.Sleep(3000);
            //SAVE AND CLOSE
            driver.FindElement(By.XPath(".//*[@id='feeChangeSave']/span")).Click();
            //EDIT
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='applicationFeeSummary']/table/tbody/tr[8]/td[3]/div[1]")).Click();
            //CANCLE
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(".//*[@id='feeChangeCancel']/span")).Click();
            //CLICK RED X
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='applicationFeeSummary']/table/tbody/tr[8]/td[3]/div[2]")).Click();
            Thread.Sleep(5000);
            //are you sure?
            driver.FindElement(By.XPath(".//*[@id='feeDeleteConfirmYesButton']")).Click();
            Thread.Sleep(5000);
            //CLICK SUBMIT APPLICATION
            driver.FindElement(By.XPath(".//*[@id='submitApplicationFeesInTabs']/a/span")).Click();
            Thread.Sleep(20000);
            //close the box              
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(8000);
            driver.FindElement(By.Id("UpdateApplicationStatus")).SendKeys(OpenQA.Selenium.Keys.F5);
            Thread.Sleep(5000);

            //Click on Submit for approval button 3 times
            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);

            this.webDriver.SwitchTo().Frame("IfrDialog");

            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("1stTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(5000);

            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("2ndTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            Thread.Sleep(5000);

            driver.FindElement(By.XPath(".//*[@id='SubmitApproval']")).Click();
            Thread.Sleep(5000);
            this.webDriver.SwitchTo().Frame("IfrDialog");
            driver.FindElement(By.XPath(".//*[@id='MainContent_txtComments']")).SendKeys("3rdTask Update");
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            driver.FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[3]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            // driver.FindElement(By.XPath(".//*[@id='RefreshApplicationScenario']")).Click();
            Thread.Sleep(10000);

            //REFRESH
            driver.FindElement(By.Id("UpdateApplicationStatus")).SendKeys(OpenQA.Selenium.Keys.F5);
            Thread.Sleep(5000);
            //Click on Status & Status History in left Nav bar 
            driver.FindElement(By.XPath(".//*[@id='lnkAppStatusHistory']")).Click();
            Thread.Sleep(8000);
            //Click Generate Eval Report button 
            driver.FindElement(By.XPath(".//*[@id='GenerateEvalReport']")).Click();
            Thread.Sleep(8000);
            //UPLOAD DOC
            driver.FindElement(By.XPath(".//*[@id='idUploadDocument']")).Click();
            Thread.Sleep(8000);

            driver.FindElement(By.XPath(".//*[@id='fileupload']")).Click();
            System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
            Thread.Sleep(3000);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");

            Thread.Sleep(8000);
            //CLICK UPLOAD
            driver.FindElement(By.XPath(".//*[@id='FileSubmit']")).Click();
            Thread.Sleep(10000);




        }

    }






}






















