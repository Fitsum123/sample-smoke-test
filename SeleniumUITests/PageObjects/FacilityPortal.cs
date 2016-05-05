using System;
using System.Linq;
using FluentAutomation;
using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class FacilityPortal:PageObject
    {
       

       

        public FacilityPortal FindFacilityByFacilityNumber(string facilityNumber)
        {
            wd.FindElement(By.Id("FacilityNumber")).Clear();
            wd.FindElement(By.Id("FacilityNumber")).SendKeys(facilityNumber);
            wd.FindElement(By.Id("searchSubmit")).Click();
            WaitForComplete();
            var url = string.Format("/IC2/Facilities/Portal/{0}", facilityNumber);

            return NavigateTo<FacilityPortal>(url);
        }

        public FacilityPortal ShowInspectionHistory(string expectedTaskNumber, string expectedTaskStatus="Closed")
        {
            wd.FindElement(By.CssSelector("#leftpanel > ul > li.static.selected > ul > li:nth-child(7) > a")).Click();
            Sleep(500);

          

            string dateInspectedShownUnderInspectionHistory=null ,
             deviceInspectedShownUnderInspectionHistory =null,
             tasknumberShownUnderInspectionHistory=null ,
            taskstatusShownUnderInspectionHistory=null;


            var taskDateInspectedColumn = wd.FindElements(By.CssSelector("#inspectionHistory_table >tbody td:nth-child(1)"));
            var taskDeviceInspectedColumn = wd.FindElements(By.CssSelector("#inspectionHistory_table >tbody td:nth-child(2)"));
            var taskNumberColumn = wd.FindElements(By.CssSelector("#inspectionHistory_table >tbody td:nth-child(3)"));
            var taskStatusColumn = wd.FindElements(By.CssSelector("#inspectionHistory_table >tbody td:nth-child(4)"));
           for (var i = 0; i < taskNumberColumn.Count; i++)
           {
               if (taskNumberColumn[i].Text != expectedTaskNumber) continue;
               dateInspectedShownUnderInspectionHistory = taskDateInspectedColumn[i].Text;
               deviceInspectedShownUnderInspectionHistory = taskDeviceInspectedColumn[i].Text;
               tasknumberShownUnderInspectionHistory = taskNumberColumn[i].Text;
               taskstatusShownUnderInspectionHistory = taskStatusColumn[i].Text;
               break;
           }



            Assert.AreEqual(tasknumberShownUnderInspectionHistory, expectedTaskNumber, "Inspection History doesn't show expected task number");
            Assert.AreEqual(taskstatusShownUnderInspectionHistory, expectedTaskStatus, "Inspection History doesn't show expected task status");
            Assert.AreEqual(false,string.IsNullOrWhiteSpace(deviceInspectedShownUnderInspectionHistory),"Device Inspected should be displayed");
            Assert.AreEqual(false,string.IsNullOrWhiteSpace(dateInspectedShownUnderInspectionHistory),"Date Inspected should be displayed");

            return this;

        }

        public  FacilityPortal FacilityDetail()
        {
            FindElement(By.XPath("//*[@id='1']/td[1]/a")).Click();
            return this;
        }

        public FacilityPortal ShowComplianceHistory(string expectedTaskNumber)
        {
            //ToDo:To be improved when the actual functionality starts populating the Compliance History table Properly
            wd.FindElement(By.CssSelector("#leftpanel > ul > li.static.selected > ul > li:nth-child(2) > a")).Click();
            Sleep(300);
            var tasknumberShownUnderInspectionHistory = FindElement(By.XPath("//*[@id='1']/td[2]")).Text;
            var minorShownUnderInspectionHistory = FindElement(By.XPath("//*[@id='1']/td[3]")).Text;
            var tagShownUnderInspectionHistory = FindElement(By.XPath("//*[@id='2']/td[3]")).Text;


            Assert.AreEqual(tasknumberShownUnderInspectionHistory, expectedTaskNumber, "Compliance History doesn't show expected task number");
            Assert.AreEqual(minorShownUnderInspectionHistory, "Minor", "Compliance History doesn't show expected Minor");
            Assert.AreEqual(tagShownUnderInspectionHistory, "Tag", "Compliance History doesn't show expected Tag");
            return this;

        }

    }
}