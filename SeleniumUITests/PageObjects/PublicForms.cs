using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{

    public class PublicForms : PageObject
    {
        private IWebDriver driver
        {
            get
            {
                return webDriver;
            }
        }

        /// <summary>
        /// This is the URL of the Public Forms page.
        /// </summary>

        public override string PageRelativeUrl
        {
            get
            {
               
                return "PublicForms/ComplaintWizardSelection";
            }
        }


        public PublicForms ComplaintOverview()
        {
            var driver = this;
            Navigate();
            if (!driver.FindElement(By.Id("PublicComplaintType_GasStation")).Selected)
            {
                driver.FindElement(By.Id("PublicComplaintType_GasStation")).Click();
            }
            driver.FindElement(By.LinkText("CONTINUE TO NEXT STEP")).Click();
            return this;
        }
        public PublicForms AllegedSourceLocation(string AllegedSourceLocation_Name = "District", string Address_StreetAddress1 = "939 Ellis St", string Address_City = "San Francisco",
            string Address_PostalCode = "94104", string AllegedSourceNozzleLocation = "23")
        {
            var driver = this;
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).SendKeys(AllegedSourceLocation_Name);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).SendKeys(Address_StreetAddress1);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).SendKeys(Address_City);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).SendKeys(Address_PostalCode);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).SendKeys(AllegedSourceNozzleLocation);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            return this;
        }
        public PublicForms ObservationLocation(string Address_StreetAddress1 = "Van Ness and Ellis St", string Address_City = "San Francisco", string Address_PostalCode = "94109",
            string Description = "This is additional information")
        {
            var driver = this;
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_StreetAddress1")).Click();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_StreetAddress1")).Clear();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_StreetAddress1")).SendKeys(Address_StreetAddress1);
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_City")).Click();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_City")).Clear();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_City")).SendKeys(Address_City);
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_PostalCode")).Click();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_PostalCode")).Clear();
            driver.FindElement(By.Id("ObservationInformationStep_ObservationLocation_Address_PostalCode")).SendKeys(Address_PostalCode);
            if (!driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_IsOngoing_True")).Selected)
            {
                driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_IsOngoing_True")).Click();
            }
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).Click();
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).Clear();
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).SendKeys(Description);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            return this;
        }

        public PublicForms FollowUpOptions(string ComplainantFirstName = "Bill", string ComplainantLastName = "Smith", string StreetAddress1 = "424 32nd Ave", string City = "San Francisco",
            string PostalCode = "94121", string ComplainantPrimaryPhone = "(415)555-5555", string ComplainantEmail = "test@test.com")
        {
            var driver = this;
            if (!driver.FindElement(By.Id("FollowUpOptionsStep_PublicFollowUpOption_ComplainantRequestsContact")).Selected)
            {
                driver.FindElement(By.Id("FollowUpOptionsStep_PublicFollowUpOption_ComplainantRequestsContact")).Click();
            }
            driver.FindElement(By.Id("complainantContactInfo")).Click();
            driver.FindElement(By.XPath("//div[@id='complainantContactInfo']//label[.='Your Contact Information']")).Click();
            //if (driver.FindElement(By.XPath(".//*[@id='complainantContactInfo']/div[2]/label")).Text != "Your Contact Information")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='contactInfoSection']/div[4]/label")).Text != "Your Mailing Address")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).SendKeys(ComplainantFirstName);
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).SendKeys(ComplainantLastName);
            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).SendKeys(StreetAddress1);
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).SendKeys(City);
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).SendKeys(PostalCode);
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).SendKeys(ComplainantPrimaryPhone);
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).SendKeys(ComplainantEmail);
            if (!driver.FindElement(By.Id("FollowUpOptionsStep_HasRequestedHardCopy_True")).Selected)
            {
                driver.FindElement(By.Id("FollowUpOptionsStep_HasRequestedHardCopy_True")).Click();
            }
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmisionStepMailingAddress']/div[2]")).Text != "Please enter a mailing address:")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).SendKeys(StreetAddress1);
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).SendKeys(City);
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).SendKeys(PostalCode);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            return this;
        }

        public PublicForms ReviewSubmitComplaint()
        {
            var driver = this;

            //if (driver.FindElement(By.XPath(".//*[@id='ComplaintSubmissionStepWizardStep']/p/span[3]")).Text != "Review & Submit Complaint")
            //{
            //    Console.Error.WriteLine("verifyTitle failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[1]/div[1]")).Text != "ALLEGED SOURCE LOCATION")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[1]/div[3]/div[1]")).Text != "939 Ellis St")
            //{
            //   // driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[1]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[2]/div[1]")).Text != "OBSERVATION LOCATION")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[2]/div[3]/div[1]")).Text != "Van Ness and Ellis St")
            //{
            //   // driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[2]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[3]/div[1]")).Text != "ADDITIONAL INFORMATION")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[3]/div[2]")).Text != "This is additional information")
            //{
            //  //  driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[3]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[4]/div[1]")).Text != "FOLLOW UP OPTIONS")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[4]/div[2]/div/span")).Text != "I want to provide my contact information and be contacted by an inspector.")
            //{
            //   // driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[4]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[1]/div[1]")).Text != "CONTACT INFORMATION")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[1]/div[2]")).Text != "Bill Smith")
            //{
            //  //  driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[1]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[2]/div[1]")).Text != "MAILING ADDRESS")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            ////if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[2]/div[2]/div[1]")).Text != "424 32nd Ave")
            ////{
            ////    //driver.Close();
            ////    throw new SystemException("assertText failed");
            ////}
            //if (!(driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[2]/a")).Displayed))
            //{
            //    Console.Error.WriteLine("verifyElementPresent failed");
            //}
            return this;
        }

        public PublicForms EditAllegedSourceLocation(string AllegedSourceLocation_Name = "Tester", string Address_StreetAddress1 = "949 Ellis stt", string Address_City = "San Francisco",
            string Address_PostalCode = "94104", string AllegedSourceNozzleLocation = "54")
        {

            var driver = this;

            driver.FindElement(By.CssSelector("a.editButton")).Click();
            //if (!driver.FindElement(By.TagName("html")).Text.Contains("Alleged Source Information"))
            //{
            //    Console.Error.WriteLine("verifyTextPresent failed");
            //}
            Sleep(5000);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Name")).SendKeys(AllegedSourceLocation_Name);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_StreetAddress1")).SendKeys(Address_StreetAddress1);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_City")).SendKeys(Address_City);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceLocation_Address_PostalCode")).SendKeys(Address_PostalCode);
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).Click();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).Clear();
            driver.FindElement(By.Id("AllegedSourceInformationStep_AllegedSourceNozzleLocation")).SendKeys(AllegedSourceNozzleLocation);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            //Edit Observation Location Continue button
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            //Continue button for follow up option
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            return this;
        }



        public PublicForms EditAdditionalInformation(string Description = "This is additional information")
        {
            var driver = this;
            WaitForComplete();
            driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[3]/a")).Click();
         
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_OccurrenceDateTime")).GetAttribute("Value"))));
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).Click();
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).Clear();
            driver.FindElement(By.Id("ObservationInformationStep_AdditionalComplaintInformation_Description")).SendKeys(Description);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            return this;
        }

        public PublicForms EditFollowUpOption()
        {
            var driver = this;
            WaitForComplete();

            driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[1]/div[4]/a")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).GetAttribute("value"))));
            if (!driver.FindElement(By.Id("FollowUpOptionsStep_PublicFollowUpOption_ComplainantHasRequestedNoContact")).Selected)
            {
                driver.FindElement(By.Id("FollowUpOptionsStep_PublicFollowUpOption_ComplainantHasRequestedNoContact")).Click();
            }

            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            WaitForComplete();
            return this;
        }

        public PublicForms EditContactInformation(string ComplainantFirstName = "Jake", string ComplainantLastName = "Ryan", string StreetAddress1 = "424 32nd Ave", string City = "San Francisco",
            string PostalCode = "94121", string ComplainantPrimaryPhone = "(415)555-5555", string ComplainantEmail = "testbaaqmd@test.com")
        {
            var driver = this;
            //  WaitForComplete();        
            driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepPage']/div[3]/div[1]/a")).Click();
            WaitFor(() => (!string.IsNullOrEmpty(FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).GetAttribute("value"))));
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantFirstName")).SendKeys(ComplainantFirstName);
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_ComplainantLastName")).SendKeys(ComplainantLastName);

            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_StreetAddress1")).SendKeys(StreetAddress1);
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_City")).SendKeys(City);
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PostalCode")).SendKeys(PostalCode);
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantPrimaryPhone")).SendKeys(ComplainantPrimaryPhone);
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_PublicComplaint_ComplainantEmail")).SendKeys(ComplainantEmail);
           
            return this;

        }

        public PublicForms EditMailingAddress(string StreetAddress1 = "477 32ND AVE", string City = "San Francisco",
            string PostalCode = "94121")
        {
            var driver = this;
        //    WaitForComplete();
            driver.FindElement(By.Id("FollowUpOptionsStep_HasRequestedHardCopy_True")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_StreetAddress1")).SendKeys(StreetAddress1);
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_City")).SendKeys(City);
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).Click();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).Clear();
            driver.FindElement(By.Id("FollowUpOptionsStep_MailingAddress_PostalCode")).SendKeys(PostalCode);
            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();
            WaitForComplete();
            return this;
        }

        public PublicForms SubmitComplaint()
        {
            var driver = this;

            WaitForComplete();
            driver.FindElement(By.XPath(".//*[@id='TestCode']")).Click();
            driver.FindElement(By.Id("ComplaintWizardSubmitButton")).Click();
            WaitForComplete();
            return this;
        }

        public PublicForms CloseComplientForm()
        {
            var driver = this;
            WaitForComplete();
            //if (driver.FindElement(By.XPath(".//*[@id='ComplaintSubmissionStepWizardStep']/p/span[3]")).Text != "Review & Submit Complaint")
            //{
            //    Console.Error.WriteLine("verifyText failed");
            //}
            //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepConfirmation']/div/div[2]")).Text != "Thank you, your complaint has been received.")
            //{
            //    // driver.Close();
            //    throw new SystemException("assertText failed");
            //}
            Sleep(5000);
           // //if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepConfirmation']/div/div[5]")).Text != "Date submitted is")
           // //{
           // //    //  driver.Close();
           // //    throw new SystemException("assertText failed");
           // //}
           // if (!(driver.FindElement(By.Id("complaintstatus_icon")).Displayed))
           // {
           //     Console.Error.WriteLine("verifyElementPresent failed");
           // }
           // if (!(driver.FindElement(By.Id("CloseComplaintWizardButton")).Displayed))
           // {
           //     //  driver.Close();
           //     throw new SystemException("assertElementPresent failed");
           // }
           // if (driver.FindElement(By.XPath(".//*[@id='complaintSubmissionStepConfirmation']/div/div[11]")).Text != "If requested and contact information was provided, you will be contacted by an Air Quality Inspector.")
           // {
           //     //  driver.Close();
           //     throw new SystemException("assertText failed");
           // }   
           //// driver.FindElement(By.Id("CloseComplaintWizardButton")).Click();
            
            return this;
        }
    public PublicForms CloseComplaintWizardButton() {
            var driver = this;
    //        WaitForComplete();
           FindElement(By.Id("CloseComplaintWizardButton")).Click();
          //  webDriver.Quit();
           return this;

        }


}
}

