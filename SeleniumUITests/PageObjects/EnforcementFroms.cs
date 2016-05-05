using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gov.Baaqmd.Tests.WebUITests;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class EnforcementFroms : PageObject
    {
        public string TaskNumber { get; set; }
        public string FacilityNumber { get; set; }

        public EnforcementFroms PopulateAndSubmitEnforcementForm(string inspectionDescription= "general test",string contactName="name", string title="title", string phone="5157792502",
                                                                 string email= "test@test.com", string rule0Status="Compliance Assistance", string rule1Status = "Compliant",
                                                                 string rule2Status = "Not Applicable", string rule3Status = "Not Compliant", string initialLetterforConditionParamDialog="D",
                                                                 string condiotionParamDialogInput= "Dispensing Rate",string conditionParameter="Hours", string permitedVal="12", string actualVal= "12",
                                                                 string note="test", string sourceType="Phase I", string sourceSubType="All", string souceIssueProblem="Failed", string tagId="123",
                                                                 string tagtype="Hose",string tagSubtype= "Hose Condition", string minorType="Phase I",string minorSubType= "Loose adapter",
                                                                 string fuelPoint="1",string nozzleSerialId="12", string gasGrade="89")
        {

            wd.SwitchTo().Window(wd.WindowHandles.Last());
            wd.FindElement(By.XPath("//*[@id='InspectionSection']/div[1]/div/div/div[1]/div[2]/div/fieldset[2]/span/img")).Click();
            Sleep(1000);
            wd.FindElement(By.LinkText("7")).Click();
            wd.FindElement(By.Id("GeneralInformationTab_Inspection_Description")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_Inspection_Description")).SendKeys(inspectionDescription);
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Name")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Name")).SendKeys(contactName);
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Title")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Title")).SendKeys(title);
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Phone")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Phone")).SendKeys(phone);
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Email")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_InspectionContacts_Items_0__Email")).SendKeys(email);


            wd.FindElement(By.CssSelector("#InspectionTabButton > span")).Click();
            wd.SwitchTo().ActiveElement();
            wd.FindElement(By.Id("InspectionTab_IsCurrentPermitAvailable_True")).Click();
            new SelectElement(wd.FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_0__Status"))).SelectByText(rule0Status);
            new SelectElement(wd.FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_1__Status"))).SelectByText(rule1Status);
            new SelectElement(wd.FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_2__Status"))).SelectByText(rule2Status);
            new SelectElement(wd.FindElement(By.Id("InspectionTab_DistrictRules_Items_0__Rules_3__Status"))).SelectByText(rule3Status);
            wd.FindElement(By.XPath("//div[@id='ConditionsSection']/div[2]/a/span")).Click();
            new SelectElement(wd.FindElement(By.Id("AddConditionDialog_SelectedCondition"))).SelectByIndex(1);
            wd.FindElement(By.LinkText("OK")).Click();

            var inputElements =
                wd.FindElements(
                    By.CssSelector("#ConditionsSection > div.content > div:nth-child(1) > div.content tbody  tr input"));
            inputElements.ForEach(x => {
                x.Clear();
                x.SendKeys(conditionParameter);
            });
            Sleep(2000);
            var numberOfRowsForConditionValue =
               wd.FindElements(
                   By.CssSelector("#ConditionsSection > div.content > div:nth-child(1) > div.content tbody  tr")).Count;
           
         
            wd.FindElement(By.XPath("//div[@id='ConditionsSection']/div[2]/div/div[2]/a/span")).Click();
            wd.FindElement(By.Id("AddConditionParameterDialog_ParameterName")).SendKeys(initialLetterforConditionParamDialog);
            wd.FindElement(By.Id("ui-active-menuitem")).Click();
            wd.FindElement(By.Id("AddConditionParameterDialog_ParameterName")).Clear();
            wd.FindElement(By.Id("AddConditionParameterDialog_ParameterName")).SendKeys(condiotionParamDialogInput);
            wd.FindElement(By.LinkText("OK")).Click();
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__PermittedValue", numberOfRowsForConditionValue))).Clear();
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__PermittedValue", numberOfRowsForConditionValue))).SendKeys(permitedVal);
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__ActualValue", numberOfRowsForConditionValue))).Clear();
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__ActualValue", numberOfRowsForConditionValue))).SendKeys(actualVal);
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__Units", numberOfRowsForConditionValue))).Clear();
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__Units", numberOfRowsForConditionValue))).SendKeys(conditionParameter);

            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__Notes", numberOfRowsForConditionValue))).Clear();
            wd.FindElement(By.Id(string.Format("InspectionTab_Conditions_Items_0__Parameters_{0}__Notes", numberOfRowsForConditionValue))).SendKeys(note);

            
            wd.FindElement(By.LinkText("SOURCE TEST ISSUE")).Click();
            new SelectElement(wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__SourceTestType"))).SelectByText(sourceType);
            wd.FindElement(By.CssSelector("#InspectionTab_SourceTestIssues_Items_0__SourceTestType > option[value=\"Phase1\"]")).Click();
            Sleep(5000);
            new SelectElement(wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__SourceTestSubType"))).SelectByText(sourceSubType);
            new SelectElement(wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__Problem"))).SelectByText(souceIssueProblem);
            wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__Contractor")).Clear();
            wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__Contractor")).SendKeys(note);
            wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__Notes")).Clear();
            wd.FindElement(By.Id("InspectionTab_SourceTestIssues_Items_0__Notes")).SendKeys(note);

            wd.FindElement(By.CssSelector("#EnforcementActionsTabButton > span")).Click();
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_EnforcementActionsRecipient_Name"))).SelectByText(contactName);
            wd.FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionIdentifier")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionIdentifier")).SendKeys("7");
            wd.FindElement(By.CssSelector("span.datetime > img.ui-datepicker-trigger")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='EnforcementActionsSection']/div[2]/div/div[2]/table/tbody/tr/td[4]/fieldset[2]/span/img")).Click();
            wd.FindElement(By.LinkText("Prev")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='EnforcementActionsSection']/div[2]/div/div[2]/table/tbody/tr/td[4]/fieldset[3]/span/img")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.Id("EnforcementActionsTab_DistrictEnforcementActions_Items_0__Items_0__EnforcementActionType_NOV")).Click();
            wd.FindElement(By.LinkText("TAG")).Click();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__EnforcementActionIdentifier")).SendKeys(tagId);
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__TagType"))).SelectByText(tagtype);
            wd.FindElement(By.CssSelector("option[value=\"Hose\"]")).Click();
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__TagSubType"))).SelectByText(tagSubtype);
            wd.FindElement(By.CssSelector("div > span.datetime > img.ui-datepicker-trigger")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='TagsSection']/div[2]/table/tbody/tr[2]/td[3]/div/span/img")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='TagsSection']/div[2]/table/tbody/tr[3]/td[3]/div/span/img")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__FuelingPoint")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__FuelingPoint")).SendKeys(fuelPoint);
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__NozzleSerialId")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__NozzleSerialId")).SendKeys(nozzleSerialId);
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__GasGrade"))).SelectByText(gasGrade);
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__Description")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Tags_Items_0__Description")).SendKeys(note);
            wd.FindElement(By.LinkText("MINOR")).Click();
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__MinorType"))).SelectByText(minorType);
            wd.FindElement(By.CssSelector("option[value=\"PhaseI\"]")).Click();
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__MinorSubType"))).SelectByText(minorSubType);
            wd.FindElement(By.XPath("//div[@id='MinorsSection']/div[2]/table/tbody/tr/td[2]/div/span/img")).Click();
            wd.FindElement(By.LinkText("Prev")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='MinorsSection']/div[2]/table/tbody/tr[2]/td[2]/div/span/img")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.XPath("//div[@id='MinorsSection']/div[2]/table/tbody/tr[3]/td[2]/div/span/img")).Click();
            wd.FindElement(By.CssSelector("span.ui-icon.ui-icon-circle-triangle-w")).Click();
            wd.FindElement(By.LinkText("1")).Click();
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__FuelingPoint")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__FuelingPoint")).SendKeys(fuelPoint);
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__NozzleSerialId")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__NozzleSerialId")).SendKeys(nozzleSerialId);
            new SelectElement(wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__GasGrade"))).SelectByText(gasGrade);
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__Description")).Clear();
            wd.FindElement(By.Id("EnforcementActionsTab_Minors_Items_0__Description")).SendKeys(note);
            wd.FindElement(By.CssSelector("#FollowUpsTabButton > span")).Click();
           
            Sleep(3000);
            wd.FindElement(By.CssSelector("#ReviewInspectionTabButton > span")).Click();
            Sleep(5000);
          
            wd.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div/a[5]/span")).Click();
        
            var headerText = wd.FindElement(By.CssSelector("body > div.page > div.header > div.mast > h1")).Text;
            var length = headerText.Length;
            var indexOfOpeningbracket = headerText.IndexOf("(", StringComparison.Ordinal);
            this.TaskNumber = headerText.Substring(indexOfOpeningbracket +1 , length - indexOfOpeningbracket - 2);

            headerText =
              wd.FindElement(By.CssSelector("div.banner:nth-child(2) > div:nth-child(1) > h1:nth-child(1)")).Text;
            indexOfOpeningbracket = headerText.IndexOf("(", StringComparison.Ordinal);
            var indexOfClosingbracket= headerText.IndexOf(")", StringComparison.Ordinal);
             
            this.FacilityNumber = headerText.Substring(indexOfOpeningbracket + 1,
                indexOfClosingbracket - indexOfOpeningbracket - 1);

           //WaitForComplete();
            return this;
        }

        public void MakeRevisionRequest(string revisionNote= "review requested")
        {
            
            wd.SwitchTo().Window(wd.WindowHandles.Last());
            Sleep(8000);
            wd.FindElement(By.CssSelector(".buttons > a:nth-child(1)")).Click();
            wd.FindElement(By.Id("RevisionsRequestedDialog_RequestedNotes")).Clear();
            wd.FindElement(By.Id("RevisionsRequestedDialog_RequestedNotes")).SendKeys(revisionNote);
            wd.FindElement(By.LinkText("OK")).Click();
        }

        public void MakeRevisionResponse(string comment= "general test\ncomment accepted")
        {
            wd.SwitchTo().Window(wd.WindowHandles.Last());
            Sleep(3000);

            wd.FindElement(By.Id("GeneralInformationTab_Inspection_Description")).Clear();
            wd.FindElement(By.Id("GeneralInformationTab_Inspection_Description")).SendKeys(comment); //"general test\ncomment accepted"
            wd.FindElement(By.XPath("//a[5]/span")).Click();

        }

        public void ApproveRevisionResponse()
        {
            wd.SwitchTo().Window(wd.WindowHandles.Last());
            Sleep(7000);
            wd.FindElement(By.CssSelector(".buttons > a:nth-child(5) > span:nth-child(1)")).Click();
            WaitForComplete(5000);

        }
    }
}
