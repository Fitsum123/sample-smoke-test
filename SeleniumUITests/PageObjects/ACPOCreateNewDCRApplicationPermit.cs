using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.SeleniumUITests;

public class ACPOCreateNewDCRApplicationPermit : PageObject
{
    public override string PageRelativeUrl
    {
        get
        {
           
            return "/ACPO/Portal/";

        }
    }
   

    public void CreateHeaderApplicationTitleDCR()
    {
        var driver = this;
        Navigate();

        driver.FindElement(By.XPath(".//*[@id='startNewApplication2']/span")).Click();
        driver.FindElement(By.Id("ApplicationTitle")).Click();
        driver.FindElement(By.Id("ApplicationTitle")).SendKeys("AB");
        driver.FindElement(By.Id("contactPickerSelector")).Click();
        driver.FindElement(By.XPath(".//*[@id='materialList']/div[5]")).Click();
        Thread.Sleep(5000);
        driver.FindElement(By.Id("EquipmentDescription")).SendKeys("ABCDEFG");
   
        driver.FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
        driver.FindElement(By.XPath(".//*[@id='exceedsMaxEmployees']/label[2]/input")).Click();
        driver.FindElement(By.XPath("//div[@id='exceedsMaxIncome']/label[2]/input")).Click();

        driver.FindElement(By.XPath("//div[@id='isAnAffiliate']/label[2]/input")).Click();
        driver.FindElement(By.XPath("//div[@id='schoolProximityQuestion']/label[2]/input")).Click();
        driver.FindElement(By.XPath("//div[@id='greenBusiness']/label[2]/input")).Click();
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        driver.FindElement(By.Id("HasCEQADocumentFalse")).Click();

        driver.FindElement(By.Id("HasRelatedProjectFalse")).Click();

        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

    }


    public void CreateDeviceInformationForDCRApplication()
    {
        var driver = this;

        driver.FindElement(By.XPath(".//*[@id='addDevice']/span")).Click();
        driver.FindElement(By.XPath(".//*[@id='DryCleanerSelect']")).Click();
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

        driver.FindElement(By.Id("FacilityDeviceName")).Click();
        driver.FindElement(By.Id("FacilityDeviceName")).Clear();
        driver.FindElement(By.Id("FacilityDeviceName")).SendKeys("dev1");
        driver.FindElement(By.Id("StartupDate")).Click();
        driver.FindElement(By.XPath(".//*[@id='StartupDate']")).SendKeys(DateTime.Today.AddDays(30).ToShortDateString());
        //Non-Halogenated Dry Cleaning 
        driver.FindElement(By.Id("Components_0__Entity_ComponentTypeId_NonHalogenatedDryCleaningMachine")).Click();
        //Registered Dry Cleaning Machine
        driver.FindElement(By.Id("Components_0__Entity_Entity_ComponentTypeId_RegisteredDryCleaningMachine")).Click();
        Thread.Sleep(5000);
        //Manufacturer Name
        driver.FindElement(By.XPath("//select[@id='Components_0__Entity_SourceInfo_Value_Entity_DryCleaningManufacturerTypeId']//option[2]")).Click();
        // Device information
        driver.FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_ManufacturerDescription")).SendKeys("123");
        driver.FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_Model")).SendKeys("123");
        driver.FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DrumCapacity")).SendKeys("123");
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        // Material Usage
        Thread.Sleep(5000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_DryCleanerNonHalogenatedMaterialUsageModel_addButton']/span")).Click();
        // click D5 Siloxane -Green Earth
        Thread.Sleep(5000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_DryCleanerNonHalogenatedMaterialUsageModel__materialList']/div[1]")).Click();
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //Emissions Trains Upstream 
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        Thread.Sleep(5000);
        //OPERATION LOCATION
        driver.FindElement(By.LinkText("SAVE AND CLOSE")).Click();
        Thread.Sleep(5000);
        //Done with form
        driver.FindElement(By.XPath(".//*[@id='wizardNextButton']")).Click();
        Thread.Sleep(5000);
        // ESTIMATED EMISSIONS
        driver.FindElement(By.Id("wizardNextButton")).Click();
        //EVALUATIONS TRIGGERED
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //HRSA FORM
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //Conditions & Regulations
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //Application Fees
        driver.FindElement(By.XPath(".//*[@id='submitApplicationFeesInTabs']/a")).Click();
    }

    public void PaymentForDCRApplication()
    {
        var driver = this;   
        //PAYMENT INFORMATION
        driver.FindElement(By.Id("contactPickerSelector")).Click();
        driver.FindElement(By.XPath(".//*[@id='materialList']/div[6]")).Click();
        driver.FindElement(By.XPath(".//*[@id='IsSubmittalCertification']")).Click();

        driver.FindElement(By.Id("addressCheckNext")).Click();
        driver.FindElement(By.Id("isOnlinePaymentYes")).Click();

        driver.FindElement(By.Id("CardOwnerName")).Click();
        driver.FindElement(By.Id("CardOwnerName")).Clear();
        driver.FindElement(By.Id("CardOwnerName")).SendKeys("FIT");
        driver.FindElement(By.Id("cardNumber")).Click();
        driver.FindElement(By.Id("cardNumber")).Clear();
        driver.FindElement(By.Id("cardNumber")).SendKeys("4111111111111111");
        driver.FindElement(By.Id("expirationDate")).Click();
        driver.FindElement(By.Id("expirationDate")).Clear();
        driver.FindElement(By.Id("expirationDate")).SendKeys("1124");
        driver.FindElement(By.Id("cardCode")).Click();
        driver.FindElement(By.Id("cardCode")).Clear();
        driver.FindElement(By.Id("cardCode")).SendKeys("1233");

        driver.FindElement(By.Id("UseContactAddress")).Click();

        Thread.Sleep(3000);
        driver.FindElement(By.LinkText("SUBMIT PAYMENT")).Click();
        Thread.Sleep(6000);
       // driver.FindElement(By.LinkText("VIEW INVOICE")).Click();

        driver.FindElement(By.XPath(".//*[@id='facilityLink']/a")).Click();

        var facilityNumber = FindElement(By.XPath(".//*[@id='facilityDetails']/div[1]/div[2]/span[2]")).Text;

        SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;
        SeleniumUITests.SharedProperties["DCRJobPage"] = new Uri(wd.Url).PathAndQuery;
        return;



    }
   
}


