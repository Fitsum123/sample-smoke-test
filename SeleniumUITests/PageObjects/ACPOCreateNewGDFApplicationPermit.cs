using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Gov.Baaqmd.Tests.WebUITests.PageObjects;
using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Gov.Baaqmd.Tests.SeleniumUITests;

public class ACPOCreateNewGDFApplicationPermit : PageObject
{
    public override string PageRelativeUrl
    {
        get
        {

            //   return "/ACPO/Portal/";
            return "/ACPO/";
        }
    }

    public void CreateHeaderApplicationTitleGDF()
    {
        var driver = this;
        Navigate();

        driver.FindElement(By.XPath(".//*[@id='startNewApplication2']/span")).Click();
        driver.FindElement(By.Id("ApplicationTitle")).Click();
        driver.FindElement(By.Id("ApplicationTitle")).SendKeys("GDF");
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
        driver.FindElement(By.Id("HasRelatedProjectTrue")).Click();
        driver.FindElement(By.Id("RelatedProjectsDoc_0__Entity_ProjectName")).SendKeys("abc");
        driver.FindElement(By.Id("RelatedProjectsDoc_0__Entity_ProjectDescription")).SendKeys("def");
        Thread.Sleep(5000);
        //UPLOAD DOCUMENT
        driver.FindElement(By.XPath(".//*[@id='RelatedProjectsDoc_0__Entity_upload_uploadButton']/span")).Click();
        System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        Thread.Sleep(8000);
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

    }
   
    public void CreateDeviceInformationForGDFApplication()
    {
        var driver = this;



        driver.FindElement(By.XPath(".//*[@id='addDevice']/span")).Click();
        driver.FindElement(By.Id("GasolineDispensingSelect")).Click();
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

        driver.FindElement(By.Id("FacilityDeviceName")).Click();
        driver.FindElement(By.Id("FacilityDeviceName")).Clear();
        driver.FindElement(By.Id("FacilityDeviceName")).SendKeys("PUMPS");
        driver.FindElement(By.XPath(".//*[@id='StartupDate']")).SendKeys(DateTime.Today.AddDays(-1).ToShortDateString());

        //CLICK Refueling Motor Vehicles (retail)
        driver.FindElement(By.Id("RefuelingMotorVehiclesRetail_checkbox")).Click();
        //CLICK Add Thank
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel_addButton']/span")).Click();
        //dropdown select material-gasoline    
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_GdfTankMaterialTypeId']/option[2]")).Click();
        //dropdown select Tank Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_TankTypeId']/option[3]")).Click();
        //Enter Tank volume      
        driver.FindElement(By.Id("Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_TankVolume")).SendKeys("123123");
        //select dropdown Phase 1 Vapor Recovery Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_Phase1VaporRecoveryTypeId']/option[2]")).Click();
        //select dropdown Phase 2 Vapor Recovery Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_Phase2VaporRecoveryTypeId']/option[20]")).Click();


        //CLICK Add 2nd TANK
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel_addButton']/span")).Click();
        //dropdown select material-diesel
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_GdfTankMaterialTypeId']/option[4]")).Click();
        //dropdown select Tank Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_TankTypeId']/option[2]")).Click();
        //Enter Tank volume
        driver.FindElement(By.Id("Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_TankVolume")).SendKeys("123");
        //select dropdown Phase 1 Vapor Recovery Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_Phase1VaporRecoveryTypeId']/option[3]")).Click();
        //select dropdown Phase 2 Vapor Recovery Type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_Phase2VaporRecoveryTypeId']/option[18]")).Click();

        //Click Add Product Type button 
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel_addButton']/span")).Click();
        //select dropdown product type
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__0__Entity_NozzleProductTypeId']/option[4]")).Click();
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__0__Entity_NozzleNumber']")).Click();
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__0__Entity_NozzleNumber']")).SendKeys("1");
        //Click ADD PRODUCT TYPE    

        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel_addButton']/span")).Click();
        //click product type-Diesel
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__1__Entity_NozzleProductTypeId']/option[7]")).Click();
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__1__Entity_NozzleNumber']")).Click();
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__1__Entity_NozzleNumber']")).SendKeys("1");
                                     
        // CLICK Please download and complete the GDF Equipment Information Form:
        //driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[1]/a")).Click();
        //driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[3]/a")).Click();
        //driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[4]/a")).Click();
        //Upload a document
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_upload_uploadButton']/span")).Click();
        System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
        Thread.Sleep(3000);
        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        Thread.Sleep(5000);

        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        Thread.Sleep(3000);
        // Material Usage
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton']/span")).Click();
        Thread.Sleep(5000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__materialList']/div[1]")).Click();
        Thread.Sleep(3000);                            
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage']")).SendKeys("6000000");
        //Click Trade secret icon (shaped like a +) 
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity__tradeSecret']")).Click();
        //check material usage box
        Thread.Sleep(3000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_IsUsageInformationTradeSecret']")).Click();
        //Descritpion
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_TradeSecretReason']")).SendKeys("Please provide a justification for making this information a Trade Secret (255 character minimum):Please provide a justification for making this information a Trade Secret (255 character minimum):Please provide a justification for making this information a Trade Secret (255 character minimum):");
        Thread.Sleep(5000);
        //CLOSE
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity__tradeSecretSaveAndClose']/span")).Click();
        Thread.Sleep(3000);
        //Upload a document
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_EmissionFactorDocuments_Entity_upload_uploadButton']/span")).Click();
        System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
        Thread.Sleep(3000);
        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        Thread.Sleep(5000);
        // Add Another Material button 
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton']/span")).Click();
        Thread.Sleep(3000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__materialList']/div[4]")).Click();
        Thread.Sleep(3000);
        //click red x to remove
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__RepeaterWrapper']/div[1]/div[3]/span")).Click();
        Thread.Sleep(3000);
        //Are you sure?
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel_deleteConfirmYesButton']")).Click();
        Thread.Sleep(3000);
        // Add Another Material button 
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton']/span")).Click();
        Thread.Sleep(3000);
        //click desiel fuel
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__materialList']/div[3]")).Click();
        Thread.Sleep(3000);
        driver.FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__1__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage']")).SendKeys("123");
        Thread.Sleep(3000);

        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        Thread.Sleep(3000);
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        
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
        //upload a document
        driver.FindElement(By.XPath(".//*[@id='HRSA_Entity_upload_uploadButton']/span")).Click();
        System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
        Thread.Sleep(3000);
        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        Thread.Sleep(5000);
        //device location
        driver.FindElement(By.XPath(".//*[@id='HRSA_Entity_DeviceLocations_0__DeviceLocationId']/option[4]")).Click();

        //  driver.FindElement(By.XPath(".//*[@id='HRSA_Entity_DeviceLocations_0__DeviceLocationId']")).Click();
        // driver.FindElement(By.XPath(".//*[@id='HRSA_Entity_DeviceLocations_0__DeviceLocationId']/option[3]']")).Click();

        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //Conditions & Regulations
        driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
        //Application Fees
       
        driver.FindElement(By.XPath(".//*[@id='submitApplicationFeesInTabs']/a")).Click();
        Thread.Sleep(7000);
    }

    public void PaymentForGDFApplication()
    {
        var driver = this;
        //PAYMENT INFORMATION
        driver.FindElement(By.Id("contactPickerSelector")).Click();
        driver.FindElement(By.XPath(".//*[@id='materialList']/div[6]")).Click();
        driver.FindElement(By.XPath(".//*[@id='IsSubmittalCertification']")).Click();
        Thread.Sleep(5000);
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

        Thread.Sleep(5000);
        driver.FindElement(By.LinkText("SUBMIT PAYMENT")).Click();
        Thread.Sleep(6000);

         //   driver.FindElement(By.LinkText("VIEW INVOICE")).Click();
        var applicationNumber = FindElement(By.XPath(".//*[@id='applicationDetails']/div[1]/span[2]")).Text;
        SeleniumUITests.SharedProperties["FacilityNumber"] = applicationNumber;
       

        driver.FindElement(By.XPath(".//*[@id='facilityLink']/a")).Click();
        var facilityNumber = FindElement(By.XPath(".//*[@id='facilityDetails']/div[1]/div[2]/span[2]")).Text;
        SeleniumUITests.SharedProperties["FacilityNumber"] = facilityNumber;

        SeleniumUITests.SharedProperties["DCRJobPage"] = new Uri(wd.Url).PathAndQuery;



        return;

    }
}


