using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;


namespace Gov.Baaqmd.Tests.SeleniumUITests.PageObjects
{


    public class ACPOSignin : PageObject
    {
        /// <summary>
        /// This is the URL of the signin page.
        /// </summary>
        public override string PageRelativeUrl
        {
            get
            {
                return "ACPO/Account/Login?ReturnUrl=%2fACPO%2f";
            }
        }

        /// <summary>
        /// Log into ACPO web site.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ACPO SignAsExternalNewUse(string userName, string password)
        {
            /*

            This is the original code from Selenium builder:


                [Test()]
                public void TestCase() {
                  IWebDriver wd = new RemoteWebDriver(DesiredCapabilities.Firefox());
                  try {
                    wd.Navigate().GoToUrl("http://ps-dev.baaqmd.gov/");
                    if (!wd.FindElement(By.TagName("html")).Text.Contains("LOG")) {
                        Console.Error.WriteLine("verifyTextPresent failed");
                    }
                    wd.FindElement(By.Id("UserName")).Click();
                    wd.FindElement(By.Id("UserName")).Clear();
                    wd.FindElement(By.Id("UserName")).SendKeys("jroselle@baaqmd.gov");
                    wd.FindElement(By.Id("Password")).Click();
                    wd.FindElement(By.Id("Password")).Clear();
                    wd.FindElement(By.Id("Password")).SendKeys("password");
                    wd.FindElement(By.Id("LogInSubmit")).Click();
                    if (!wd.FindElement(By.TagName("html")).Text.Contains("CUSTOMER PORTAL HOME")) {
                        Console.Error.WriteLine("verifyTextPresent failed");
                    }
                  } finally { wd.Quit(); }
                }

            I took the selenium builder code and tossed what I didnt need.
            
            For example, the web dirver is set up for us, and we don't want to terminate it.
            So we really just need what's in the Try block.

            I then extracted the username/password to come up with this function


            */

            //Ensure we are on the right page
            Navigate();

            //Entry criteria
            if (!wd.FindElement(By.TagName("html")).Text.Contains("LOG"))
            {
                Console.Error.WriteLine("verifyTextPresent failed");
            }

            wd.FindElement(By.Id("UserName")).Click();
            wd.FindElement(By.Id("UserName")).Clear();
            wd.FindElement(By.Id("UserName")).SendKeys(userName);
            wd.FindElement(By.Id("Password")).Click();
            wd.FindElement(By.Id("Password")).Clear();
            wd.FindElement(By.Id("Password")).SendKeys(password);
            wd.FindElement(By.Id("LogInSubmit")).Click();

            //This test ends with us on the ACPO page
            //Success criteria
            if (!wd.FindElement(By.TagName("html")).Text.Contains("CUSTOMER PORTAL HOME"))
            {
                Console.Error.WriteLine("verifyTextPresent failed");
            }

            return NavigateTo<ACPO>();
        }


        /// <summary>
        /// Copied from Roopa
        /// </summary>
        /// <returns></returns>
        public ACPO CreateExternalUser(string emailId, string password)
        {
            var driver = wd;

            Navigate(); //uses PageRelativeUrl driver.Navigate().GoToUrl(AcpoConstants.fullUrl);
            Assert.AreEqual("Login", driver.Title);
            Assert.AreEqual("SIGN UP NOW", FindElement(By.CssSelector("div.signUpButtonContainer > a.button.primaryButton > span")).Text);
            FindElement(By.CssSelector("div.signUpButtonContainer > a.button.primaryButton > span")).Click();
            Assert.AreEqual("SIGN UP FOR A NEW ACCOUNT", FindElement(By.CssSelector("#SignUpTitle > span")).Text);

            //None of this is necessary
            //////DateTime localDate = DateTime.Now;
            //////DateTime firstJan = new DateTime(localDate.Year, 1, 1);

            //////int daysSinceFirstJan = (localDate - firstJan).Days + 1;
            //////CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");

            //////Console.WriteLine("Creating unique email id");
            //////String uniqueId = daysSinceFirstJan.ToString() + localDate.ToString("hhmms_ff", ci);
            //////String emailId = "test" + uniqueId + "@test.com";
            //////String password = AcpoConstants.password;
            //////FileStream fs = new FileStream("C:\\out\\EmailID.txt", FileMode.Append);
            //////TextWriter tmp = Console.Out;
            //////StreamWriter sw = new StreamWriter(fs);
            //////Console.SetOut(sw);
            //////Console.WriteLine(emailId);
            //////Console.SetOut(tmp);
            //////Console.WriteLine(emailId);
            //////sw.Close();

            FindElement(By.Id("UserName")).Clear();
            FindElement(By.Id("UserName")).SendKeys(emailId);
            FindElement(By.Id("ConfirmUserName")).Clear();
            FindElement(By.Id("ConfirmUserName")).SendKeys(emailId);
            FindElement(By.Id("Password")).Clear();
            FindElement(By.Id("Password")).SendKeys(password);
            FindElement(By.Id("ConfirmPassword")).Clear();
            FindElement(By.Id("ConfirmPassword")).SendKeys(password);
            FindElement(By.Id("TestCode")).Click();
            FindElement(By.CssSelector("#NewAccountSubmit > span")).Click();
            //Thread.Sleep(5000);
            // Assert.AreEqual("SIGN UP FOR A NEW ACCOUNT", driver.FindElement(By.CssSelector("#SignUpTitle > span")).Text);
            FindElement(By.Id("BypassEmailConfirm")).Click();
            //Thread.Sleep(5000);
            // String expectedText = "Congratulations! You have successfully activated your account.\nNext please complete the form below.";
            //        String actualText = driver.FindElement(By.CssSelector("div.successCreatedMessage")).Text;
            //        Assert.AreEqual(expectedText, actualText);
            //Thread.Sleep(5000);
            //   driver.FindElement(By.XPath("id('ContactInfo_FirstName')")).Clear();
            FindElement(By.XPath("id('ContactInfo_FirstName')")).SendKeys("TestFirstName");
            FindElement(By.XPath("id('ContactInfo_LastName')")).Clear();
            FindElement(By.XPath("id('ContactInfo_LastName')")).SendKeys("TestLastName");
            FindElement(By.XPath("id('ContactInfo_Address_AddressLine1')")).Clear();
            FindElement(By.XPath("id('ContactInfo_Address_AddressLine1')")).SendKeys("401 Jacklin Road");
            FindElement(By.XPath("id('ContactInfo_Address_City')")).Clear();
            FindElement(By.XPath("id('ContactInfo_Address_City')")).SendKeys("Milpitas");
            //driver.FindElement(By.XPath("id('ContactInfo_Address_State')")).Clear();
            FindElement(By.XPath("id('ContactInfo_Address_State')")).SendKeys("CA");
            FindElement(By.XPath("id('ContactInfo_Address_ZipCode')")).Clear();
            FindElement(By.XPath("id('ContactInfo_Address_ZipCode')")).SendKeys("95035");
            FindElement(By.XPath("id('ContactInfo_PhoneNumber')")).Clear();
            FindElement(By.XPath("id('ContactInfo_PhoneNumber')")).SendKeys("5103245678");
            FindElement(By.XPath("id('AcceptTerms')")).Click();
            FindElement(By.Id("NewAccountSubmit")).Click();
            FindElement(By.XPath("id('DoneSubmit')")).Click();
            Assert.AreEqual("Welcome to Bay Area Air Quality Management District", driver.Title);

            return NavigateTo<ACPO>();

        }
        public void testACPOSaveStartUpInformationForDCR()
        {
            var driver = this;
         //   Navigate();

            //Click Open Application button

            driver.FindElement(By.XPath("//span[@class='applicationFinishedOpen']")).Click();
            //startup infor
            driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[1]/span[1]")).Click();
            Thread.Sleep(3000);
            // driver.FindElement(By.LinkText("UPLOAD DOCUMENT")).Click();
            // driver.FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).SendKeys("C:/Users/fitsum/Desktop/QA Info/qa.txt");
            // Thread.Sleep(5000);
            driver.FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).Click();
            System.Windows.Forms.SendKeys.SendWait("C:\\Users\\fitsum\\Desktop\\QA Info\\qa.txt");
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Thread.Sleep(7000);

            driver.FindElement(By.LinkText("SAVE INFORMATION")).Click();

        }
        public void testACPOVerifyRegistrationCertificationDCR()
        {
            var driver = this;
            
            //Click Facility page
            driver.FindElement(By.XPath("//span[@class='facilityPageOpen']")).Click();
            //view permit to operate
            driver.FindElement(By.XPath("//a[@id='viewPermit']//span[.='VIEW PERMIT TO OPERATE']")).Click();



        }
        public void testACPOCheckRenewalBarBoxDCR()
        {
            var driver = this;
            
            //Click on facility link 
            driver.FindElement(By.XPath("//span[@class='facilityPageOpen']")).Click();


        }
        public void testACPOPayRenewalDCR()
        {
            var driver = this;
            //Navigate();
            //  Thread.Sleep(50000);
            Thread.Sleep(5000);
            //login ACPO and click facility page 
            driver.FindElement(By.XPath("//div[1]/div/div/div/div/div/div[4]/div[2]/div[2]/div[1]/div/div[3]/div[3]/div/table/tbody/tr[1]/td[9]/span")).Click();
            Thread.Sleep(5000);
            //  Thread.Sleep(10000);
            //Click on Renew Permit button 
            driver.FindElement(By.LinkText("RENEW FACILITY PERMIT")).Click();

            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

            driver.FindElement(By.LinkText("CONTINUE TO NEXT SECTION")).Click();

            driver.FindElement(By.LinkText("VIEW INVOICE")).Click();

            driver.FindElement(By.Id("applicationFeesTabControl")).Click();

            driver.FindElement(By.XPath("//div[@class='topPayFeesSection']//a[.='PAY FEES & RENEW PERMIT']")).Click();

            driver.FindElement(By.CssSelector("div.contactPickerSelectorIcon > img")).Click();

            driver.FindElement(By.XPath("//div[@id='materialList']/div[4]/div/div[2]/div/div[2]/div")).Click();

            driver.FindElement(By.Id("IsSubmittalCertification")).Click();

            driver.FindElement(By.LinkText("CONTINUE")).Click();
            // Proceed to payment steps and pay for renewal.
            driver.FindElement(By.Id("isOnlinePaymentYes")).Click();


            driver.FindElement(By.Id("CardOwnerName")).Click();
            driver.FindElement(By.Id("CardOwnerName")).Clear();

            driver.FindElement(By.Id("CardOwnerName")).SendKeys("DSF");
            driver.FindElement(By.Id("cardNumber")).Click();
            driver.FindElement(By.Id("cardNumber")).Clear();
            driver.FindElement(By.Id("cardNumber")).SendKeys("4111111111111111");
            driver.FindElement(By.Id("expirationDate")).Click();
            driver.FindElement(By.Id("expirationDate")).Clear();
            driver.FindElement(By.Id("expirationDate")).SendKeys("1123");
            driver.FindElement(By.Id("cardCode")).Click();
            driver.FindElement(By.Id("cardCode")).Clear();
            driver.FindElement(By.Id("cardCode")).SendKeys("1234");

            driver.FindElement(By.Id("UseContactAddress")).Click();

            driver.FindElement(By.LinkText("SUBMIT PAYMENT")).Click();
            driver.FindElement(By.LinkText("Facility Information Page")).Click();



        }
    }
}
