using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Gov.Baaqmd.BusinessObjects.GenericLookups;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class ACPOAsbestos : PageObject
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

        /*ACPOAsbestos driver { get { return this; } }*/

        public ACPOAsbestos VerifyApplicationStatus(string targetStatus = "Awaiting Final Fees")
        {

            Retry(() =>
            {
                var verify = FindElement(By.CssSelector("#applicationStatusHeaderStatus"));
                Assert.AreEqual(targetStatus.ToUpper(), verify.Text.ToUpper());
            }, onFailure: ()=> {
                Sleep(1000);
                this.Refresh();
            });

            var applicationNumber = FindElement(By.CssSelector("#applicationDetails > div:nth-child(1) > span:nth-child(2)")).Text;

            SeleniumUITests.SharedProperties["ApplicationNumber"] = applicationNumber;
            SeleniumUITests.SharedProperties["AsbestosJobPage"] = new Uri(wd.Url).PathAndQuery;

            PopDelay();
            return this;
        }


        public ACPOAsbestos AddManualPayment()
        {
            PauseForDemo();
            FindElement(By.CssSelector("div.contactPickerSelectorIcon > img")).Click();
            FindElement(By.CssSelector("div.contactInfoContainer")).Click();
            FindElement(By.Id("IsSubmittalCertification")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();
            WaitForComplete();

            FindElement(By.Id("isOnlinePaymentNo")).Click();
            FindElement(By.CssSelector("#addressCheckNext > span")).Click();
            WaitForComplete();

            return this;
        }

       
        public ACPOAsbestos AddDemoCertification()
        {
            PauseForDemo();
            FindElement(By.Id("DemolitionCertification1")).Click();
            FindElement(By.XPath("//div[@id='applicationFooter']/a[2]/span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddRenoCertification()
        {
            PauseForDemo();
            FindElement(By.CssSelector("#RenovationCertification1")).Click();
            FindElement(By.CssSelector("#RenovationCertification2")).Click();
            FindElement(By.XPath("//div[@id='applicationFooter']/a[2]/span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddStartEndDates(DateTime? startDate = null, DateTime? endDate = null, bool isNightWork = false, bool isWeekendWork = false)
        {
            PauseForDemo();
            startDate = startDate ?? DateTime.Today.AddDays(10);
            endDate = endDate ?? DateTime.Today.AddDays(12);

            FindElement(By.Id("Schedule_0__StartDate")).Click();
            FindElement(By.Id("Schedule_0__StartDate")).Clear();
            FindElement(By.Id("Schedule_0__StartDate")).SendKeys(startDate.Value.ToShortDateString());
            FindElement(By.Id("Schedule_0__EndDate")).Clear();
            FindElement(By.Id("Schedule_0__EndDate")).SendKeys(endDate.Value.ToShortDateString());

            if (isNightWork) Click("#Schedule_0__IsNightWork");
            if (isWeekendWork) Click("#Schedule_0__IsWeekendWork");


            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddLabAnalysis(
                        string labName = "Lab Name",
                        string address1 = "500 25th Ave",
                        string city = "San Francisco",
                        string postalCode = "94121",
                        string email = "nospam@nono.no", string phoneNumber = "4155551212"
            )
        {
            PauseForDemo();
            FindElement(By.XPath("(//input[@id='WasAsbestosSurveyCompleted'])[2]")).Click();

            FindElement(By.Id("AsbestosSurveyReportContactInfo_CompanyName")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_CompanyName")).SendKeys(labName);
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_AddressLine1")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_AddressLine1")).SendKeys(address1);
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_City")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_City")).SendKeys(city);
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_ZipCode")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_Address_ZipCode")).SendKeys(postalCode);
            FindElement(By.Id("AsbestosSurveyReportContactInfo_EmailAddress")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_EmailAddress")).SendKeys(email);
            FindElement(By.Id("AsbestosSurveyReportContactInfo_PhoneNumber")).Clear();
            FindElement(By.Id("AsbestosSurveyReportContactInfo_PhoneNumber")).SendKeys(phoneNumber);

            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddFireTrainingDetails(bool isFireTraining = false, ActivityMethodType.ActivityMethodTypeEnum removalMethod = ActivityMethodType.ActivityMethodTypeEnum.ByHand)
        {
            PauseForDemo();
            if (isFireTraining)
            {
                FindElement(By.XPath("(//input[@id='DemolitionIsFireTraining'])[1]")).Click();
            }
            else {
                FindElement(By.XPath("(//input[@id='DemolitionIsFireTraining'])[2]")).Click();
                FindElement(By.Id("{0}".FormatString((int) removalMethod))).Click();
            }
            return this;
        }

        public ACPOAsbestos AddLocationDetails(string locationBuildingType = "Single Family Dwelling or 4 units or less",
            string locationName = "Location Name",
            string address1 = "500 25th Ave",
            string city = "San Francisco",
            string postalCode = "94121")
        {
            PauseForDemo();
            new SelectElement(FindElement(By.Id("LocationBuildingType"))).SelectByText(locationBuildingType);
            FindElement(By.Id("LocationName")).Clear();
            
            FindElement(By.Id("LocationName")).SendKeys(locationName);

            FindElement(By.Id("LocationAddress_AddressLine1")).Clear();
            
            FindElement(By.Id("LocationAddress_AddressLine1")).SendKeys(address1);
            FindElement(By.Id("LocationAddress_City")).Clear();
            FindElement(By.Id("LocationAddress_City")).SendKeys(city);
            FindElement(By.Id("LocationAddress_ZipCode")).Clear();
            FindElement(By.Id("LocationAddress_ZipCode")).SendKeys(postalCode);
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddPayOnline()
        {
            PauseForDemo();
            FindElement(By.Id("contactPickerSelector")).Click();
            Click(".addressSummaryContractorContactInfoIcon");
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

            return this;
        }

        public ACPOAsbestos ClickPayFeeAsbestos()
        {
            PauseForDemo();

            Click(".payFees");
            WaitForComplete();

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


            return this;
        }

        public ACPOAsbestos AddRenovationDetails(RenovationType.RenovationTypeEnum renovationType = RenovationType.RenovationTypeEnum.Renovation,
            AsbestosMaterialDescriptionType.AsbestosMaterialDescriptionTypeEnum materialDescriptionType = AsbestosMaterialDescriptionType.AsbestosMaterialDescriptionTypeEnum.FireProofing,
            ActivityMethodType.ActivityMethodTypeEnum removalMethod = ActivityMethodType.ActivityMethodTypeEnum.FloorMasticRemovalUsingMechanicalBuffersAndSolvent,
            int totalRemovalAmount = 300,
            RemovalAmountUnitOfMeasureType.RemovalAmountUnitOfMeasureTypeEnum units = RemovalAmountUnitOfMeasureType.RemovalAmountUnitOfMeasureTypeEnum.SquareFeet,

            string wasteTransporterName = "Waste Transporter", string epaId = "123",
            string wAddress1 = "500 25th Ave", string wCity = "San Francisco", string wPostalCode = "94121",
            string wEmail = "nospam@nono.no", string wPhoneNumber = "4155551212",

            AsbestosLandfillType.AsbestosLandfillTypeEnum landfill = AsbestosLandfillType.AsbestosLandfillTypeEnum.AltamontSanitaryLandfill,
            string lAddress1 = "500 25th Ave", string lCity = "San Francisco", string lPostalCode = "94121",

            bool isEmergency = true,
            EmergencyType.EmergencyTypeEnum? emergencyType = EmergencyType.EmergencyTypeEnum.ConversionToFriableMaterial,
            DateTime? emergencyDate = null, string emergencyDescription = "test desc."
            )
        {
            PauseForDemo();

            Click("input[value='{0}']".FormatString((int)renovationType));

            new SelectElement(FindElement(By.Id("MaterialDescriptionType"))).SelectByValue(((int)materialDescriptionType).ToString());

            FindElement(By.Id("{0}".FormatString((int)removalMethod))).Click();

            FindElement(By.CssSelector("#AsbestosSquareFeetRemovalAmount")).Clear();
            FindElement(By.CssSelector("#AsbestosSquareFeetRemovalAmount")).SendKeys(totalRemovalAmount.ToString());
            new SelectElement(FindBy("#OtherRemovalAmountUnitOfMeasureType")).SelectByValue(((int)units).ToString());

            this.SendKeys("#WasteTransporterContactInfo_CompanyName", wasteTransporterName);
            this.SendKeys("#WasteTransportContactEpaIdBusinessAttribute_Value", epaId);
            this.SendKeys("#WasteTransporterContactInfo_Address_AddressLine1", wAddress1);
            this.SendKeys("#WasteTransporterContactInfo_Address_City", wCity);
            this.SendKeys("#WasteTransporterContactInfo_Address_ZipCode", wPostalCode);
            this.SendKeys("#WasteTransporterContactInfo_EmailAddress", wEmail);
            this.SendKeys("#WasteTransporterContactInfo_PhoneNumber", wPhoneNumber);


            new SelectElement(FindBy("#AsbestosLandfillType")).SelectByValue(((int)landfill).ToString());
            this.SendKeys("#DisposalAddress_AddressLine1", lAddress1);
            this.SendKeys("#DisposalAddress_City", lCity);
            this.SendKeys("#DisposalAddress_ZipCode", lPostalCode);

            if (isEmergency)
            {
                emergencyDate = emergencyDate ?? DateTime.Now.AddDays(1);
                Click("#IsEmergencyEvent[value='True']");
                new SelectElement(FindBy("#EmergencyType")).SelectByValue(((int)emergencyType.Value).ToString());

                SendKeys("#EmergencyDate", emergencyDate.Value.ToShortDateString());
                SendKeys("#EmergencyTime", emergencyDate.Value.ToShortTimeString());
                SendKeys("#EmergencyDescription", emergencyDescription);

            }
            else
                Click("#IsEmergencyEvent[value='False']");

            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos AddContractorDetails(string companyName = "New Company Name", 
            string licenseNumber = "123", string typeOfBusiness = "Individual",
            string contractorFirstName = "Connie", string contractorLastName = "CLast",
            string address1 = "500 25th Ave", string City = "San Francisco", string postalCode = "94121",
            string email = "nospam@nono.no", string phoneNumber = "4155551212"
            )
        {
            PauseForDemo();
            FindElement(By.Id("ContractorCompanyName")).Clear();
            FindElement(By.Id("ContractorCompanyName")).SendKeys(companyName);
            FindElement(By.Id("ContractorLicenseNumber")).Clear();
            FindElement(By.Id("ContractorLicenseNumber")).SendKeys(licenseNumber);
            new SelectElement(FindElement(By.Id("ContractorTypeOfBusiness"))).SelectByText(typeOfBusiness);
            FindElement(By.XPath("(//input[@id='ContractorHasSubmittedRequestsForOtherSites'])[2]")).Click();
            FindElement(By.Id("contactPickerChoiceLabel")).Click();
            FindElement(By.CssSelector("div.newContactDesc")).Click();
            FindElement(By.Id("ContractorContactInfo_FirstName")).Clear();
            FindElement(By.Id("ContractorContactInfo_FirstName")).SendKeys(contractorFirstName);
            FindElement(By.Id("ContractorContactInfo_LastName")).Clear();
            FindElement(By.Id("ContractorContactInfo_LastName")).SendKeys(contractorLastName);
            FindElement(By.Id("ContractorContactInfo_Address_AddressLine1")).Clear();
            FindElement(By.Id("ContractorContactInfo_Address_AddressLine1")).SendKeys(address1);
            FindElement(By.Id("ContractorContactInfo_Address_City")).Clear();
            FindElement(By.Id("ContractorContactInfo_Address_City")).SendKeys(City);
            FindElement(By.Id("ContractorContactInfo_Address_ZipCode")).Clear();
            FindElement(By.Id("ContractorContactInfo_Address_ZipCode")).SendKeys(postalCode);
            FindElement(By.Id("ContractorContactInfo_EmailAddress")).Clear();
            FindElement(By.Id("ContractorContactInfo_EmailAddress")).SendKeys(email);
            FindElement(By.Id("ContractorContactInfo_PhoneNumber")).Clear();
            FindElement(By.Id("ContractorContactInfo_PhoneNumber")).SendKeys(phoneNumber);
            FindElement(By.CssSelector("#wizardNextButton > span")).Click();
            WaitForComplete();

            return this;
        }

        public ACPOAsbestos WaitForApproved()
        {
           return Retry(() =>
           {
               wd.Navigate().Refresh();
               WaitForComplete();
               var verify = FindElement(By.CssSelector("#applicationStatusHeaderStatus"));
               Assert.IsTrue(verify.Text.ToUpper().Contains("APPROVED"));
               PauseForDemo();
               return this;
           });
            
        }

        public ACPOAsbestos CreateAsbestosJobHeader(string asbestosJobTitle = "New Asbestos Demolition - Unit Test",
            BusinessEventSubType.BusinessEventSubTypeEnum asbestosType = BusinessEventSubType.BusinessEventSubTypeEnum.AsbestosDemolition,
            bool isNotificationForPastOrCurrentJob = false)
        {
            PauseForDemo();

            //Asbestos needs a delay for server code - vertigo notified.
            PushDelay(100);

            FindElement(By.Id("AsbestosJobRequestTitle")).Clear();
            FindElement(By.Id("AsbestosJobRequestTitle")).SendKeys(asbestosJobTitle);

            Click("input[value='{0}']".FormatString((int) asbestosType));
            //div.sourceInfoItem:nth-child(3) > div:nth-child(1) > div:nth-child(2) > label:nth-child(1) > input:nth-child(1)
            if (isNotificationForPastOrCurrentJob)
                FindElement(By.XPath("(//input[@id='IsNotificationForPastOrCurrentJob'])[1]")).Click();
            else
                FindElement(By.XPath("(//input[@id='IsNotificationForPastOrCurrentJob'])[2]")).Click();


            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            return this;
        }
        public ACPOAsbestos CreateAsbestosJobHeaderRenovation(string asbestosJobTitle = "New Asbestos Reno - Unit Test",
           BusinessEventSubType.BusinessEventSubTypeEnum asbestosType = BusinessEventSubType.BusinessEventSubTypeEnum.AsbestosRenovation,
           bool isNotificationForPastOrCurrentJob = false)
        {
            PauseForDemo();

            //Asbestos needs a delay for server code - vertigo notified.
            PushDelay(100);

            FindElement(By.Id("AsbestosJobRequestTitle")).Clear();
            FindElement(By.Id("AsbestosJobRequestTitle")).SendKeys(asbestosJobTitle);

            Click("input[value='{0}']".FormatString((int)asbestosType));
            //div.sourceInfoItem:nth-child(3) > div:nth-child(1) > div:nth-child(2) > label:nth-child(1) > input:nth-child(1)
            if (isNotificationForPastOrCurrentJob)
                FindElement(By.XPath("(//input[@id='IsNotificationForPastOrCurrentJob'])[1]")).Click();
            else
                FindElement(By.XPath("(//input[@id='IsNotificationForPastOrCurrentJob'])[2]")).Click();


            FindElement(By.CssSelector("#wizardNextButton > span")).Click();

            return this;
        }
    }
}
