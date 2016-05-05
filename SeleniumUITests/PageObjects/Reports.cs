using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class Reports : PageObject
    {

        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "";
            }
        }
        public Reports FacilityStatus()
        {
            //click drop down
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_txtValue']")).Click();
            //select Demolished
            WaitForComplete();
            if (!FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_divDropDown']/span/div[1]/span/table/tbody/tr[4]/td/span/label")).Selected)
            {
                FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_divDropDown']/span/div[1]/span/table/tbody/tr[4]/td/span/label")).Click();
            }
            //click drop down
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_txtValue']")).Click();
            //click Mailing Contact Type
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl05_txtValue']")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl05_divDropDown']/span/table/tbody/tr[2]/td/span/label")).Click();
            //click drop down
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_txtValue']")).Click();
            WaitForComplete();
            return this;
        }
        public Reports ViewReportAllMailingAddress()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));

            Sleep(5000);
            // facility Number = 100024
            var facilityNumber = FindElement(By.XPath("//tr[8]/td/div/a")).Text;
            SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;

            return this;

        }
        public Reports EnterFacilityNumber(string FacNumber = "200103")
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(FacNumber);
            return this;
        }
        public Reports ViewReportApplicationHistorybyFacilityDBSPandPDBSPA()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));

            WaitForComplete();
            ////cleck application number
            FindElement(By.XPath("//tr[3]/td/div/a")).Click();
            WaitForComplete();

            var applicationNumber = FindElement(By.XPath("//td[4]/table/tbody/tr/td/div/a")).Text;
            var facilityNumber = FindElement(By.XPath("//tr[11]/td[4]/table/tbody/tr/td/div/a")).Text;

            SeleniumUITests.SharedProperties["ApplicationNumber"] = applicationNumber;
            SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;

            return this;
        }
        public Reports ViewReportApplicationHistorybyFacilityDBSPA()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));

            WaitForComplete();

            var text = FindElement(By.XPath("//tr[5]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("1/26/2016", text))
            {
                throw new NotFoundException("Expected text!");
            }
            var text1 = FindElement(By.XPath("//tr[27]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Alexia Inigues", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//tr[18]/td[4]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Costco Gasoline", text2))
            {
                throw new NotFoundException("Expected text!");
            }


            var applicationNumber = FindElement(By.XPath("//td[4]/table/tbody/tr/td/div/a")).Text;
            var facilityNumber = FindElement(By.XPath("//tr[11]/td[4]/table/tbody/tr/td/div/a")).Text;

            SeleniumUITests.SharedProperties["ApplicationNumber"] = applicationNumber;
            SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;
            ////cleck application number
         //   FindElement(By.XPath("//td[4]/table/tbody/tr/td/div/a")).Click();
            WaitForComplete();

            return this;
        }
        public Reports ViewReport()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));

            WaitForComplete();
            
            var applicationNumber = FindElement(By.XPath("//td[4]/table/tbody/tr/td/div/a")).Text;
            var facilityNumber = FindElement(By.XPath("//tr[11]/td[4]/table/tbody/tr/td/div/a")).Text;

            SeleniumUITests.SharedProperties["ApplicationNumber"] = applicationNumber;
            SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;
            ////cleck application number
            FindElement(By.XPath("//td[4]/table/tbody/tr/td/div/a")).Click();
            WaitForComplete();

            return this;
        }
        public Reports EnterApplicationNumber(string AppNumber = "414989")
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(AppNumber);
            return this;
        }
        public Reports EnterapplicationNumber(string AppNumber = "289")
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(AppNumber);
            return this;
        }
        public Reports VerifyConditionbyConditionNumber()
        {
            WaitForComplete();
            
            var text1 = FindElement(By.XPath("//li/div/span")).Text;
            if (!string.Equals("The Owner/Operator shall comply with the requirements set forth in BAAQMD Regulation 8, Rule 45 and the terms and conditions of this Registration.", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//div[2]/li/div/span")).Text;
            if (!string.Equals("The Owner/Operator shall maintain all records required under BAAQMD Regulation 8, Rule 45 for no less than three (3) years and make the records available to the District upon request.", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//div[3]/li/div/span")).Text;
            if (!string.Equals("The Owner/Operator shall keep a copy of this certificate in each vehicle performing mobile refinishing under this regulation.", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//span[2]")).Text;
            if (!string.Equals("100064", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports IncludeONLYCutoverFacilities(bool CutoverFacility = true)
        {
            if (CutoverFacility) { 
                    FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_rbTrue']")).Click();
                }
                else
                {
                    FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_rbFalse']")).Click();
                    
                }
          return this;
        }
        public Reports ViewReportConditionbyConditionNumber()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));
            WaitForComplete();
            return this;
        }
        public Reports EnterConditionNumber(string ConditiNumber = "100064")
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(ConditiNumber);
            return this;
        }
        public Reports SelectCurrentorFuture()
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl05_ddValue")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl05_ddValue']/option[2]")).Click();
            FindElement(By.Id("ctl32_ctl04_ctl05_ddValue")).Click();
            return this;
        }
         public Reports VerifyCurrentConditionReport()
        {
            Sleep(3000);
            var text = FindElement(By.XPath("//td/table/tbody/tr/td[2]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("200103", text))
            {
                throw new NotFoundException("Expected text!");
            }
            var text1 = FindElement(By.XPath("//div/div/div/span[2]")).Text;
            if (!string.Equals("100052", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//li/div/span")).Text;
            if (!string.Equals("The owner/operator affirms its understanding of, and agrees to comply with, the requirements set forth in BAAQMD Regulation 8, Rule 17, BAAQMD Regulation 1-410, and the terms and conditions of this Registration.", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//div[2]/li/div/span")).Text;
            if (!string.Equals("The owner/operator shall not exceed the gross usage of 200 gallons of all non-halogenated solvents used at all dry cleaning machines at the facility in any consecutive 12-month period.", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//div[3]/li/div/span")).Text;
            if (!string.Equals("The owner/operator shall maintain records to demonstrate compliance with Part 2 for each consecutive 12-month period.", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            var text5 = FindElement(By.XPath("//div[4]/li/div/span")).Text;
            if (!string.Equals("The owner/operator shall notify the District within 30 days if the facility changes ownership (the owning entity).", text5))
            {
                throw new NotFoundException("Expected text!");
            }
            var text6= FindElement(By.XPath("//div[5]/li/div/span")).Text;
            if (!string.Equals("The owner/operator must renew this registration on or before the expiration of the current registration period for as long as the registered equipment remains in operation.", text6))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports SelectDevice()
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl05_ddValue")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl05_ddValue']/option[2]")).Click();
            FindElement(By.Id("ctl32_ctl04_ctl05_ddValue")).Click();
            return this;
        }
        public Reports VerifyDeviceDetailReport()
        {

            var text1 = FindElement(By.XPath("//td/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Device Detail Report", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//span[2]")).Text;
            if (!string.Equals("200103 - Hi-Hat Dry Cleaners", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//tr[3]/td[6]/div")).Text;
            if (!string.Equals("Registered", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyEnforcementActionReport()
        {

            var text = FindElement(By.XPath("//td/div/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Enforcement Actions Report", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyEnforcementTasks()
        {

            var text = FindElement(By.XPath("//td/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Enforcement Tasks", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyInspectionForm()
        {
            //click GDF TASK NUMBER
            FindElement(By.XPath("//a/div/div/span")).Click();
            //click View associated form
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitForComplete();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            //NavigateTo<SPA>();
            NavigateTo<SPA>("spa/#/signin");
            return this;
        }
        public Reports VerifyFCVInspectionForm()
        {
            FindElement(By.XPath("//tr[12]/td/div/a/div/div/span")).Click();
            //click View associated form
            FindElement(By.Id("MainContent_lnkForm")).Click();
            this.webDriver.SwitchTo().Window("_blankWindow");
            WaitForComplete();
            this.webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            //NavigateTo<SPA>();
            NavigateTo<SPA>("spa/#/signin");
            return this;
        }
        public Reports EnterRenewalYearandFacilityNumber(string Year = "2016", string FacilNumber = "13153")
        {
            //enter year date
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).Click();
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).Clear();
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(Year);
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).Click();
            FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).Clear();
            FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).SendKeys(FacilNumber);
            WaitForComplete();
            return this;
        }
        public Reports VerifyPlantRenewalStatus()
        {
            WaitForComplete();
            var text = FindElement(By.XPath("//td/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Plant Renewal Status (DB-ST-P)", text))
            {
                throw new NotFoundException("Expected text!");
            }
            var text1 = FindElement(By.XPath("//td[5]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("13153", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//tr[12]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("413929", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//td[9]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Polhemus Cleaners and Laundry", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//td[6]/table/tbody/tr/td/div/div/span")).Text;
            if (!string.Equals("Thomas S Kim", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports ClickInvoiceNumber()
        {
            FindElement(By.XPath("//tr[3]/td/div/a")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));
            return this;
        }
        public Reports VerifyInvoicePaymentReportisDisplayed()
        {
            WaitForComplete();
            var text = FindElement(By.XPath("//td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Invoice Payment Report", text))
            {
                throw new NotFoundException("Expected text!");
            }
            var text1 = FindElement(By.XPath("//tr[4]/td[3]/div")).Text;
            if (!string.Equals("$177.00", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            WaitForComplete();
            return this;
        }
         public Reports VerifyAnnualRegistrationRenewalInvoice()
        {
            FindElement(By.LinkText("Re-Generate Invoice")).Click();
            Sleep(8000);
            var text2 = FindElement(By.XPath("//td/table/tbody/tr/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("220330", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//tr[4]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("413935", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//tr[5]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("200103", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            var text5 = FindElement(By.XPath("//td[5]/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("$0.00", text5))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyInvoicePaymentReport()
        {
           
            FindElement(By.LinkText("Re-Generate Invoice")).Click();
            Sleep(8000);
            var text2 = FindElement(By.XPath("//td/table/tbody/tr/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("217472", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//tr[4]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("413079", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//tr[5]/td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("12840", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            var text5 = FindElement(By.XPath("//div[4]/span[2]")).Text;
            if (!string.Equals("Royal Cleaners", text5))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports EnterFacilityName(string FacName = "Dry Cleaner")
        {
            //click drop down
            FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).SendKeys(FacName);
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            //Wait until we have the facility name.
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));
            Sleep(3000);
            var text5 = FindElement(By.XPath("//td[6]/div/div/div/span")).Text;
            if (!string.Equals("Drycleaning and Laundry Services (except Coin-Operated)", text5))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyFeeSchedulePageisDisplayed()
        {
            var text = FindElement(By.XPath("//td/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Fee Schedule", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifySearchResultIsDisplayed()
        {
            ////cleck view report
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).GetAttribute("class")));
            Sleep(1000);
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl00']")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));
            Sleep(1000);
            return this;
        }

        public Reports VerifyPermitToOperateReportisDisplayed()
        {
            Sleep(8000);

            var text1 = FindElement(By.XPath("//span[2]")).Text;
            if (!string.Equals("200103", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//td[2]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Rocky Vallabh", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//td[3]/table/tbody/tr[2]/td[2]/div")).Text;
            if (!string.Equals("Registered", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//tr[2]/td/div/div/div/span[2]")).Text;
            if (!string.Equals("100052", text4))
            {
                throw new NotFoundException("Expected text!");
            }

            return this;
        }


        public Reports VerifyCertificateOfRegistrationReportisDisplayed()
        {
            Sleep(5000);
           
            var text1 = FindElement(By.XPath("//span[2]")).Text;
            if (!string.Equals("200103", text1))
            {
                throw new NotFoundException("Expected text!");
            }
            var text2 = FindElement(By.XPath("//td[2]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Rocky Vallabh", text2))
            {
                throw new NotFoundException("Expected text!");
            }
            var text3 = FindElement(By.XPath("//td[3]/table/tbody/tr[2]/td[2]/div")).Text;
            if (!string.Equals("Registered", text3))
            {
                throw new NotFoundException("Expected text!");
            }
            var text4 = FindElement(By.XPath("//div/div/div/span[2]")).Text;
            if (!string.Equals("100052", text4))
            {
                throw new NotFoundException("Expected text!");
            }
            
            return this;
        }
        public Reports VerifyInspectionHistoryReport()
        {
            //click Employee (or former employee
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).Click();
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl03_divDropDown_ctl00")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_ddDropDownButton']")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_ctl04_ctl07_txtValue")).GetAttribute("value")));
            WaitForComplete();

            return this;
        }
        public Reports VerifyInspectionHistoryReportisDisplayed()
        {
            Sleep(2000);
            var text = FindElement(By.XPath("//td[2]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Inspection History Report", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports EnterInvoiceNumber(string InvoiceNumber = "217472")
        {
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).SendKeys(InvoiceNumber);
            return this;
        }
        public Reports SelectAsssinedEngineerAndApplicationStatusType()
        {
            //select assigned engineeer
            FindElement(By.Id("ctl32_ctl04_ctl03_txtValue")).Click();
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl03_divDropDown_ctl00")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl03_ddDropDownButton']")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_AsyncWait_Wait")).GetAttribute("class")));
            Sleep(3000);
            //select Application status type
            FindElement(By.Id("ctl32_ctl04_ctl05_txtValue")).Click();
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl05_divDropDown_ctl00")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl05_ddDropDownButton']")).Click();
            WaitFor(() => !string.IsNullOrEmpty(FindElement(By.Id("ctl32_ctl04_ctl07_txtValue")).GetAttribute("value")));


            return this;
        }

        public Reports VerifyApplicationLatestStatus()
        {
            var text = FindElement(By.XPath("//td/div/table/tbody/tr/td/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Application Latest Status", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
              
        public Reports VerifyFacilityOwnerHistoryReportisDisplayed()
        {
            Sleep(2000);
            var text = FindElement(By.XPath("//td[2]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Facility Owner History Report", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports VerifyPermitApplicationWorkloadPageisDisplayed()
        {
            Sleep(15000);
            var text = FindElement(By.XPath("//td[3]/table/tbody/tr/td/div")).Text;
            if (!string.Equals("Permit Application Workload", text))
            {
                throw new NotFoundException("Expected text!");
            }
            return this;
        }
        public Reports ApplicationStatus()
        {
            
            //Application Status 
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl07_ddDropDownButton")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl07_divDropDown']/span/div[1]")).FindElement(By.XPath("//input[@id='ctl32_ctl04_ctl07_divDropDown_ctl20']")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ctl32_ctl04_ctl07_txtValue")).GetAttribute("name"))));
            //Contains Component Category-Gasoline
            WaitForComplete();
            FindElement(By.Id("ctl32_ctl04_ctl09_ddDropDownButton")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='ctl32_ctl04_ctl09_divDropDown']/span/div[1]")).FindElement(By.XPath("//tr[18]/td/span/label")).Click();
            WaitForComplete();
              return this;
        }
        public Reports ShowPermitApplicationDetail()
        {
            //select application number
            FindElement(By.XPath("//tr[6]/td[2]/div/a")).Click();
            WaitForComplete();
            //select facility number
            var FacilityNum = FindElement(By.XPath("//tr[11]/td[4]/table/tbody/tr/td/div/a")).Text;
            SeleniumUITests.SharedProperties["FacilityNumber"] = FacilityNum;
            return this;
        }
        public Reports ClickTaskNumber()
        {
            //save task number and facility number
            var taskNumber = FindElement(By.XPath("//tr[6]/td/div/a/div/div/span")).Text;
            var facilityNumberEnforcement = FindElement(By.XPath("//tr[6]/td[7]/div")).Text;

            SeleniumUITests.SharedProperties["TaskNumber"] = taskNumber;
            SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumberEnforcement;
            //click task Number
            FindElement(By.XPath("//tr[6]/td/div/a/div/div/span")).Click();
            WaitForComplete();
            return this;
        }
        public Reports Reassign()
        {
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_Button']")).Click();
            FindElement(By.XPath(".//*[@id='MainContent_cbTaskStatus_cbTaskStatus_OptionList']/li[4]")).Click();

            WaitForEnabled(By.Id("MainContent_txtName"));
            FindElement(By.Id("MainContent_txtName")).Click();
            FindElement(By.Id("MainContent_txtName")).Clear();
            FindElement(By.Id("MainContent_txtName")).SendKeys("Test GDF");
            FindElement(By.XPath(".//*[@id='MainContent_txtName_AutoCompleteExtender_completionListElem']/li[1]")).Click();
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='MainContent_btnUpdateTask']")).Click();
            Sleep(5000);
            return this;
        }


    }
}