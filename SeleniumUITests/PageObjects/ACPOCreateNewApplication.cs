using Gov.Baaqmd.BusinessObjects.GenericLookups;
using Gov.Baaqmd.Tests.WebUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{
    public class ACPOCreateNewApplication : PageObject
    {
        /// <summary>
        /// This is the URL of the ACPO page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "ACPO";
            }
        }

        /*ACPOCreateNewApplication driver { get { return this; } }*/

        public ACPOCreateNewApplication PopulateApplicationHeader(string applicationTitle = "Application Title", bool hasCEQA = false)
        {
            FindElement(By.Id("ApplicationTitle")).Clear();
            FindElement(By.Id("ApplicationTitle")).SendKeys(applicationTitle);
            FindElement(By.Id("contactPickerChoiceLabel")).Click();
            FindElement(By.CssSelector("span.addressSummaryIconLabel")).Click();

            //need to wait until we have an address t o validate before we can click next.
            while (string.IsNullOrEmpty(FindElement(By.Id("ContactInfo_Entity_EmailAddress")).GetAttribute("Value")))
                Sleep(new TimeSpan(0, 0, 1));

            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            FindElement(By.Id("ExceedsMaxEmployees")).Click();
            FindElement(By.Id("ExceedsMaxIncome")).Click();
            FindElement(By.XPath("(//input[@name='IsAnAffiliate'])[2]")).Click();
            FindElement(By.XPath("(//input[@name='IsWithinProximityToSchool'])[2]")).Click();
            FindElement(By.XPath("(//input[@name='IsGreenBusiness'])[2]")).Click();
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            if (!hasCEQA)
                FindElement(By.Id("HasCEQADocumentFalse")).Click();
            else
            {
                FindBy("#HasCEQADocumentTrue").Click();
                new SelectElement(FindBy("#CEQADocumentation_Entity_DocumentTypeID"))
                    .SelectByValue( ((int)CEQADocumentType.CEQADocumentTypeEnum.OtherCEQADocumenation).ToString() );

                UploadFile("CEQADocumentation_Entity_upload_uploadInput", fileName: "CEQA.TXT", waitForComplete: false);

                FindBy("#CEQADocumentation_Entity_DocumentDate").Clear();
                FindBy("#CEQADocumentation_Entity_DocumentDate").SendKeys(DateTime.Today.AddDays(-1).ToShortDateString());

                FindBy("#CEQADocumentation_Entity_LeadAgencyName").Clear();
                FindBy("#CEQADocumentation_Entity_LeadAgencyName").SendKeys("Lead Agency Name");

                FindBy("#CEQADocumentation_Entity_ContactName").Clear();
                FindBy("#CEQADocumentation_Entity_ContactName").SendKeys("Lead Contact Agency Name");

                FindBy("#CEQADocumentation_Entity_PhoneNumber").Clear();
                FindBy("#CEQADocumentation_Entity_PhoneNumber").SendKeys("4155551212");

                FindBy("#CEQADocumentation_Entity_Email").Click();
                FindBy("#CEQADocumentation_Entity_Email").Clear();
                FindBy("#CEQADocumentation_Entity_Email").SendKeys("test@agency.gov");

            }


            FindElement(By.Id("HasRelatedProjectFalse")).Click();
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            return this;
        }


        public ACPOCreateNewApplication AddAutobody(string deviceName = "AB", DateTime? startupDate = null)
        {
            this.PushDelay(100);

            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(14);

            FindElement(By.Id("addDevice")).Click();
            FindElement(By.Id("AutoBodyDeviceSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            Sleep(1000);

            FindElement(By.CssSelector("#Components_0__Entity_MaterialUsages_SurfaceCoatingMaterialUsageModel_addButton > span")).Click();


            //800 Gal
            FindElement(By.CssSelector("div.materialContainer:nth-child(1)")).Click();
            Sleep(1000);
            //no cleanup

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            FindElement(By.CssSelector("#form_SaveAndCloseButton > span")).Click();

            this.PopDelay();



            return this;
        }


        public ACPOCreateNewApplication AddEmissionPoint(string deviceName = "EP", DateTime? startupDate = null, string parentSource = "TS1")
        {
            this.PushDelay(100);

            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(14);

            FindElement(By.Id("addDevice")).Click();
            FindElement(By.Id("EmissionPointSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());

            FindBy("#ComponentTypeEmissionPoint").Click();

            Sleep(1000);

            FindBy("#IsStandAlone").Click();
            Sleep(500);

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();

            //Connect emission point
            FindBy("div.optionListOption:nth-child(2) > input:nth-child(1)").Click();
            WaitForEnabled(By.CssSelector("a.addExistingEquipmentUpstreamButton"));
            FindBy("a.addExistingEquipmentUpstreamButton").Click();
            WaitForEnabled(By.CssSelector("#wizardDoneButton"));
            ExecuteScript($"$('td[Title=\"{parentSource}\"]').click()");
            FindBy("#wizardDoneButton").Click();



            Sleep(1000);

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();

            FindBy(".olControlDrawFeaturePointItem").Click();
            FindBy("canvas.ol-unselectable").Click();

            FindElement(By.CssSelector("#form_SaveAndCloseButton > span")).Click();

            this.PopDelay();

            return this;
        }

        public ACPOCreateNewApplication AddBackupDiesel(string deviceName = "AB", DateTime? startupDate = null,
            string manufacturer = "Cummings", string model = "QSX15-69",
            string EpaCarbEngineFamilyName = "ECEXL015.AAJ",
            string serialNumber = "sn12345",
            int modelYear = 2014,
            int breakHP = 755,
            CombustionPrimaryUseType.CombustionPrimaryUseTypeEnum primaryUseType = CombustionPrimaryUseType.CombustionPrimaryUseTypeEnum.ElectricalGeneration,
            EngineTierCertificationType.EngineTierCertificationTypeEnum engineTierCertificationType = EngineTierCertificationType.EngineTierCertificationTypeEnum.Tier4,
            CombustionBurnType.CombustionBurnTypeEnum combustionBurnType = CombustionBurnType.CombustionBurnTypeEnum.LeanBurn,
            EngineIgnitionType.EngineIgnitionTypeEnum engineIgnitionType = EngineIgnitionType.EngineIgnitionTypeEnum._4StrokeCompression,
            bool hasTurbocharger = true, MobilityType.MobilityTypeEnum mobilityType = MobilityType.MobilityTypeEnum.Stationary
            )
        {
            this.PushDelay(100);

            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(14);

            FindElement(By.Id("addDevice")).Click();
            FindElement(By.Id("InternalCombustionEngineSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());

            //Make/Model
            FindElement(By.CssSelector("#Device_Entity_Manufacturer")).Clear();
            FindElement(By.CssSelector("#Device_Entity_Manufacturer")).SendKeys("Cummings");
            FindElement(By.CssSelector("#Device_Entity_Model")).Clear();
            FindElement(By.CssSelector("#Device_Entity_Model")).SendKeys("QSX15-G9");



            //Emergency Standby
            FindElement(By.CssSelector("#Components_0__Entity_SubTypes > ul:nth-child(1) > li:nth-child(1) > span:nth-child(2) > label:nth-child(1)")).Click();
            Sleep(1000);

            FindBy("#Components_0__Entity_SourceInfo_Value_EpacarbEngineFamilyNameTypeId_autocomplete").Clear();
            FindBy("#Components_0__Entity_SourceInfo_Value_EpacarbEngineFamilyNameTypeId_autocomplete").SendKeys(EpaCarbEngineFamilyName);
            Sleep(1000);
            FindBy("#Components_0__Entity_SourceInfo_Value_EpacarbEngineFamilyNameTypeId_autocomplete").SendKeys(Keys.Enter);
            //just click another control
            FindElement(By.Id("FacilityDeviceName")).Click();

            if (!string.IsNullOrEmpty(serialNumber))
            {
                FindBy("#Components_0__Entity_SourceInfo_Value_Entity_SerialNumber").Clear();
                FindBy("#Components_0__Entity_SourceInfo_Value_Entity_SerialNumber").SendKeys(serialNumber);
            }

            FindBy("#Components_0__Entity_SourceInfo_Value_Entity_Horsepower").Clear();
            FindBy("#Components_0__Entity_SourceInfo_Value_Entity_Horsepower").SendKeys(breakHP.ToString());


            new SelectElement(FindBy("#Components_0__Entity_SourceInfo_Value_Entity_ModelYearTypeId")).SelectByText(((int)2014).ToString());
            new SelectElement(FindBy("#Components_0__Entity_SourceInfo_Value_Entity_CombustionPrimaryUseTypeId")).SelectByValue(((int)primaryUseType).ToString());
            new SelectElement(FindBy("#Components_0__Entity_SourceInfo_Value_Entity_EngineTierCertificationTypeId")).SelectByValue(((int)engineTierCertificationType).ToString());
            new SelectElement(FindBy("#Components_0__Entity_SourceInfo_Value_Entity_CombustionBurnTypeId")).SelectByValue(((int)combustionBurnType).ToString());
            new SelectElement(FindBy("#Components_0__Entity_SourceInfo_Value_Entity_EngineIgnitionTypeId")).SelectByValue(((int)engineIgnitionType).ToString());

            if (hasTurbocharger)
                FindBy("#Turbocharger_checkbox").Click();

            FindBy($"input[value=\"{(int)mobilityType}\"]").Click();


            //Hrmm need something?
            FindBy("#Components_0__Entity_EquipmentActivityInfo_ICEAbatementOperationModel__Activities  #None_checkbox").Click();

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            Sleep(1000);

            FindBy("#Components_0__Entity_MaterialUsages_StandbyEmergencyICEFuel_addButton > span:nth-child(1)").Click();


            //Disel 50 Hr
            FindBy("div.materialContainer:nth-child(1) > div:nth-child(3) > div:nth-child(2)").Click();
            Sleep(1000);

            
            Retry(() => UploadFile("Components_0__Entity_Deviceupload_uploadInput", fileName: "FuelData.txt", waitForComplete: false));

            FindBy("#Components_0__Entity_MaterialUsages_StandbyEmergencyICEFuel__0__Entity_MaterialUsage_Value_Entity_MaximumFuelConsumptionRate").Clear();
            FindBy("#Components_0__Entity_MaterialUsages_StandbyEmergencyICEFuel__0__Entity_MaterialUsage_Value_Entity_MaximumFuelConsumptionRate").SendKeys("39.2");
            var val = MaximumFuelConsumptionRateUnitOfMeasureType.MaximumFuelConsumptionRateUnitOfMeasureTypeEnum.GallonsPerHour;
            new SelectElement(FindBy("#Components_0__Entity_MaterialUsages_StandbyEmergencyICEFuel__0__Entity_MaterialUsage_Value_Entity_MaximumFuelConsumptionRateUnitOfMeasureTypeId")).SelectByValue(((int)val).ToString());
            FindBy("#Components_0__Entity_MaterialUsages_StandbyEmergencyICEFuel__0__Entity_MaterialUsage_Value_Entity_MaximumFuelConsumptionRateUnitOfMeasureTypeId").Click();


            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();

            //Emissions train page
            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            FindElement(By.CssSelector("#form_SaveAndCloseButton > span")).Click();

            this.PopDelay();



            return this;
        }

        public ACPOCreateNewApplication AddHalogenatedDryCleaningMachine(string deviceName = "DCP", DateTime? startupDate = null,
            DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum manufacturer = DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum.AMA, MachineType.MachineTypeEnum machineType = MachineType.MachineTypeEnum.ClosedLoop,
            string drumCapacity = "50", DryCleaningFacilityType.DryCleaningFacilityTypeEnum dryCleaningFacilityType = DryCleaningFacilityType.DryCleaningFacilityTypeEnum.StandAlone,
            string materialName = "Perchloroethylene", string annualUsage = "10")
        {
            this.PushDelay(100);

            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(14);


            FindElement(By.Id("addDevice")).Click();
            FindElement(By.Id("DryCleanerSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());
            FindElement(By.Id("Components_0__Entity_ComponentTypeId_HalogenatedDryCleaningMachine")).Click();

            FindBy("input[value='{0}']".FormatString((int)dryCleaningFacilityType)).Click();

            new SelectElement(FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DryCleaningManufacturerTypeId"))).SelectByValue(((int)manufacturer).ToString());
            new SelectElement(FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_MachineTypeId"))).SelectByValue(((int)machineType).ToString());

            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DrumCapacity")).Clear();
            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DrumCapacity")).SendKeys(drumCapacity);

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();

            Click("#Components_0__Entity_MaterialUsages_DryCleanerHalogenatedMaterialUsageModel_addButton");
            FindBy("div[data-materialname=\"{0}\"]".FormatString(materialName)).Click();

            FindElement(By.Id("Components_0__Entity_MaterialUsages_DryCleanerHalogenatedMaterialUsageModel__0__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).Clear();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_DryCleanerHalogenatedMaterialUsageModel__0__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).SendKeys(annualUsage);

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();
            FindElement(By.CssSelector("#form_SaveAndCloseButton > span")).Click();

            this.PopDelay();

            return this;
        }

        public ACPOCreateNewApplication AddNonHalogenatedDryCleaningMachine(string deviceName = "DCR", DateTime? startupDate = null,
            DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum manufacturer = DryCleaningManufacturerType.DryCleaningManufacturerTypeEnum.AeroTech,
            string model = "Model",
            string drumCapacity = "100")
        {
            this.PushDelay(100);

            startupDate = startupDate ?? DateTime.Today.AddDays(14);

            FindElement(By.Id("addDevice")).Click();
            FindElement(By.Id("DryCleanerSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());
            FindElement(By.Id("FacilityDeviceName")).Click();

            Click(By.Id("Components_0__Entity_ComponentTypeId_NonHalogenatedDryCleaningMachine"));

            Sleep(1000);
            Click(By.Id("Components_0__Entity_Entity_ComponentTypeId_RegisteredDryCleaningMachine"));
            Sleep(1000);

            new SelectElement(FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DryCleaningManufacturerTypeId"))).SelectByValue(((int)manufacturer).ToString());
            Sleep(1000);

            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_Model")).Clear();
            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_Model")).SendKeys(model);
            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DrumCapacity")).Clear();
            FindElement(By.Id("Components_0__Entity_SourceInfo_Value_Entity_DrumCapacity")).SendKeys(drumCapacity);
            Sleep(1000);

            FindElement(By.CssSelector("#form_ContinueButton > span")).Click();

            Click("#Components_0__Entity_MaterialUsages_DryCleanerNonHalogenatedMaterialUsageModel_addButton > span");

            FindElement(By.CssSelector("div.materialInfoDesc")).Click();

            //Need to wait until box is populated
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("Components_0__Entity_MaterialUsages_DryCleanerNonHalogenatedMaterialUsageModel__0___MaterialNameDisplay")).GetAttribute("Value"))));


            Click(By.CssSelector("#form_ContinueButton > span"));
            Click(By.CssSelector("#form_ContinueButton > span"));
            Click(By.CssSelector("#form_SaveAndCloseButton > span"));

            this.PopDelay();

            return this;
        }

        public void WaitForEvaluatingPermitApplicationCompleteness()
        {

            Retry(() =>
                {
                    var verify = FindElement(By.CssSelector("#applicationStatusHeaderStatus"));
                    Assert.IsTrue(verify.Text.ToUpper().Contains("EVALUATING PERMIT APPLICATION COMPLETENESS"));

                    var appNumber = FindElement(By.CssSelector("#applicationDetails > div:nth-child(1) > span:nth-child(2)")).Text;

                    SeleniumUITests.SharedProperties["ApplicationNumber"] = appNumber;
                },
                onFailure: () => { Sleep(1000); wd.Navigate().Refresh(); },
                retrySeconds: 120
                );
        }


        public ACPOCreateNewApplication SubmitAndPay(bool populateHrsa = true)
        {

            Click(By.CssSelector("#wizardNextButton > span"));
            WaitForComplete();
            Click(By.CssSelector("#wizardNextButton > span"));
            WaitForComplete();
            Click(By.CssSelector("#wizardNextButton > span"));
            WaitForComplete();

            if (populateHrsa)
                PopulateHRSA();

            Int64 obj = 0;

            do
            {
                var error = (Int64)ExecuteScript("return $('#validation-summary-errors').length");
                if (error > 0)
                    throw new BaaqmdException("UI Error");

                obj = (Int64)ExecuteScript("return $('#wizardNextButton > span').length");

                if (obj > 0)
                {
                    Click(By.CssSelector("#wizardNextButton > span"));
                    WaitForComplete();
                }
            } while (obj > 0);


            /*
            Click(By.CssSelector("#wizardNextButton > span"));
            WaitForComplete();
            Click(By.CssSelector("#wizardNextButton > span"));
            WaitForComplete();
            */

            FindElement(By.XPath("//div[@id='applicationFooter']/a[2]/span")).Click();
            WaitForComplete();

            FindElement(By.Id("contactPickerSelector")).Click();
            FindElement(By.XPath("//div[@id='materialList']/div[4]/div/div[2]/div/div[2]/div")).Click();
            FindElement(By.Id("IsSubmittalCertification")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();
            FindElement(By.Id("isOnlinePaymentYes")).Click();
            FindElement(By.Id("CardOwnerName")).Clear();
            FindElement(By.Id("CardOwnerName")).SendKeys("Jack Spade");
            FindElement(By.Id("cardNumber")).Clear();
            FindElement(By.Id("cardNumber")).SendKeys("4111111111111111");
            FindElement(By.Id("expirationDate")).Clear();
            FindElement(By.Id("expirationDate")).SendKeys("1219");
            FindElement(By.Id("cardCode")).Clear();
            FindElement(By.Id("cardCode")).SendKeys("123");
            FindElement(By.Id("UseContactAddress")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();

            WaitForComplete();

            return this;
        }

        public ACPOCreateNewApplication PopulateHRSA()
        {
            //Populate HRSA
            //need to wait until button is ready.
            FindElement(By.CssSelector("#wizardNextButton > span"));


            try
            {
                //Upload a temp file to the HRSA control.
                UploadFile("HRSA_Entity_upload_uploadInput", fileName: "HRSA.TXT");

                int buildings = 0;
                while (buildings <= 10)
                {
                    var bld = "HRSA_Entity_DeviceLocations_{0}__DeviceLocationId".FormatString(buildings);

                    //some js engines return int64, lets just do string.
                    var cnt = ExecuteScript("return $('#{0}').length".FormatString(bld)).ToString();
                    if (cnt == "0")
                        break;

                    new SelectElement(wd.FindElement(By.Id(bld))).SelectByText("Outside");
                    wd.FindElement(By.CssSelector("#HRSA_Entity_DeviceLocations_{0}__DeviceLocationId > option[value=\"-2147381758\"]".FormatString(buildings))).Click();
                    buildings++;
                }
            }
            catch (OpenQA.Selenium.NotFoundException)
            {

            }

            return this;
        }
        public ACPOCreateNewApplication CreateHeaderApplicationTitleGDF(string applicationTitle = "Application Title", string Note = "test123")
        {
            FindElement(By.Id("ApplicationTitle")).Clear();
            FindElement(By.Id("ApplicationTitle")).SendKeys(applicationTitle);
            FindElement(By.Id("contactPickerChoiceLabel")).Click();
            FindElement(By.CssSelector("span.addressSummaryIconLabel")).Click();

            //need to wait until we have an address t o validate before we can click next.
            while (string.IsNullOrEmpty(FindElement(By.Id("ContactInfo_Entity_EmailAddress")).GetAttribute("Value")))
                Sleep(new TimeSpan(0, 0, 1));

            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            FindElement(By.XPath(".//*[@id='exceedsMaxEmployees']/label[2]/input")).Click();
            FindElement(By.XPath(".//*[@id='exceedsMaxIncome']/label[2]/input")).Click();
            FindElement(By.XPath(".//*[@id='isAnAffiliate']/label[2]/input")).Click();
            FindElement(By.XPath(".//*[@id='schoolProximityQuestion']/label[2]/input")).Click();
            FindElement(By.XPath(".//*[@id='greenBusiness']/label[2]/input")).Click();
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            FindElement(By.Id("HasCEQADocumentFalse")).Click();
            FindElement(By.Id("HasRelatedProjectTrue")).Click();
            FindElement(By.Id("RelatedProjectsDoc_0__Entity_ProjectName")).SendKeys(Note);
            FindElement(By.Id("RelatedProjectsDoc_0__Entity_ProjectDescription")).SendKeys(Note);

            return this;
        }
        public ACPOCreateNewApplication UploadRELATEDPROJECTS()
        {
            FindElement(By.XPath(".//*[@id='RelatedProjectsDoc_0__Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();
            return this;
        }
        public ACPOCreateNewApplication CreateDeviceInformationForGDFApplication(string deviceName = "PUMPS ", DateTime? startupDate = null,
             GasolineDispensingActivityType.GasolineDispensingActivityTypeEnum RefuelingMotorVehicles = GasolineDispensingActivityType.GasolineDispensingActivityTypeEnum.RefuelingMotorVehiclesRetail,
               GDFTankMaterialType.GDFTankMaterialTypeEnum Gasolineunleaded = GDFTankMaterialType.GDFTankMaterialTypeEnum._551,
            TankType.TankTypeEnum TankType = TankType.TankTypeEnum.UnderGround,
            string TankVolume = "123",
             Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum Phase1VaporRecoveryType = Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum.PhilTiteVR101,
            Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum Phase2VaporRecoveryType = Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum.HirtVCS200,
            GDFTankMaterialType.GDFTankMaterialTypeEnum Diesel = GDFTankMaterialType.GDFTankMaterialTypeEnum._98,
            TankType.TankTypeEnum TankType2 = TankType.TankTypeEnum.AboveGround,
            Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum Phase1VaporRecoveryType2 = Phase1VaporRecoveryType.Phase1VaporRecoveryTypeEnum.OPWVR102,
            Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum Phase2VaporRecoveryType2 = Phase2VaporRecoveryType.Phase2VaporRecoveryTypeEnum.Healy400ORVR,
            NozzleProductType.NozzleProductTypeEnum ProductType = NozzleProductType.NozzleProductTypeEnum.GasolineTripleProductNozzle, string NumberofNozzels = "1",
            NozzleProductType.NozzleProductTypeEnum ProductType2 = NozzleProductType.NozzleProductTypeEnum.Diesel,
            BuildingLocationType.BuildingLocationTypeEnum Location = BuildingLocationType.BuildingLocationTypeEnum.Outside)
        {
            
            if (!startupDate.HasValue)
                startupDate = DateTime.Today.AddDays(-1);

            FindElement(By.XPath(".//*[@id='addDevice']/span")).Click();
            FindElement(By.Id("GasolineDispensingSelect")).Click();
            FindElement(By.CssSelector("#wizardFooter > #wizardNextButton > span")).Click();
            FindElement(By.Id("FacilityDeviceName")).Clear();
            FindElement(By.Id("FacilityDeviceName")).SendKeys(deviceName);
            FindElement(By.Id("StartupDate")).Clear();
            FindElement(By.Id("StartupDate")).SendKeys(startupDate.Value.ToShortDateString());

            Click("input[value='{0}']".FormatString((int)RefuelingMotorVehicles));
            WaitForComplete();
            //click add thank
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel_addButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_GdfTankMaterialTypeId']/option[2]".FormatString((int)Gasolineunleaded))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_TankTypeId']/option[3]".FormatString((int)TankType))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_TankVolume']")).SendKeys(TankVolume);
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_Phase1VaporRecoveryTypeId']/option[2]".FormatString((int)Phase1VaporRecoveryType))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_GdfTankMaterialTypeId']/option[2]".FormatString((int)Phase1VaporRecoveryType))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__0__Entity_Phase2VaporRecoveryTypeId']/option[20]".FormatString((int)Phase2VaporRecoveryType))).Click();
            //click add thank
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel_addButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_GdfTankMaterialTypeId']/option[4]".FormatString((int)Diesel))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_TankTypeId']/option[2]".FormatString((int)TankType2))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_TankVolume']")).SendKeys(TankVolume);
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_Phase1VaporRecoveryTypeId']/option[3]".FormatString((int)Phase1VaporRecoveryType2))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentGasolineDispensingModel__1__Entity_Phase2VaporRecoveryTypeId']/option[18]".FormatString((int)Phase2VaporRecoveryType2))).Click();
            //click add product type
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel_addButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__0__Entity_NozzleProductTypeId']/option[4]".FormatString((int)ProductType))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__0__Entity_NozzleNumber']")).SendKeys(NumberofNozzels);

            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel_addButton']/span")).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__1__Entity_NozzleProductTypeId']/option[7]".FormatString((int)ProductType2))).Click();
            FindElement(By.XPath(".//*[@id='Components_0__Entity_EquipmentActivityInfo_EquipmentNozzleModel__1__Entity_NozzleNumber']")).SendKeys(NumberofNozzels);
            //click downloadable GDF PDF file
            //FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[1]/a")).Click();
            //FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[3]/a")).Click();
            //FindElement(By.XPath(".//*[@id='Components_0__Entity_download']/div[4]/a")).Click();
            // upload a file
            FindElement(By.XPath(".//*[@id='Components_0__Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            FindElement(By.XPath(".//*[@id='form_ContinueButton']")).Click();
            //WaitForComplete();
            return this;
        }
        public ACPOCreateNewApplication materialUsage(string materialName1 = "Gasoline - Unleaded", string annualUsage = "6000000", 
            string info = "Please provide a justification for making this information a Trade Secret (255 character minimum) Please provide a justification for making this information a Trade Secret (255 character minimum):Please provide a justification for making this information a Trade Secret (255 character minimum):",
            string materialName2 = "Ethanol (E85)", string materialName3 = "Diesel fuel", string annualUsage1 = "123")
        {

            Click("#Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton");
            FindBy("div[data-materialname=\"{0}\"]".FormatString(materialName1)).Click();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).Clear();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_MaterialUsage_Value_Entity_BackingEntity_AnnualUsage")).SendKeys(annualUsage);
            //click + sign
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity__tradeSecret")).Click();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_IsUsageInformationTradeSecret")).Click();
            FindElement(By.Id("Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_TradeSecretReason")).SendKeys(info);
            FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity__tradeSecretSaveAndClose']/span")).Click();
            // upload a file
            FindElement(By.XPath(".//*[@id='Components_0__Entity_MaterialUsages_GDMaterialUsageModel__0__Entity_EmissionFactorDocuments_Entity_upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TEST.TXT"));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Sleep(5000);
            Click("#Components_0__Entity_MaterialUsages_GDMaterialUsageModel_addButton");
            FindBy("div[data-materialname=\"{0}\"]".FormatString(materialName2)).Click();
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

    }
}
