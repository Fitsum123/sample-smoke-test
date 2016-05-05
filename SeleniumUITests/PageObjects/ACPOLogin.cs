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


    public class ACPOLogin : PageObject
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
        public ACPO Login(string userName, string password)
        {
            SeleniumUITests.SharedProperties["UserName"] = userName;
            SeleniumUITests.SharedProperties["Password"] = password;

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
            SeleniumUITests.SharedProperties["UserName"] = emailId;
            SeleniumUITests.SharedProperties["Password"] = password;

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
        public ACPO VerifyCreateExternalUserAccount(string emailId, string password)
        {
            var rnd = new Random();
            var number = rnd.Next(1000, 9999);
            //Read Data from Excel, each column is a field. 
            emailId = "test" + number.ToString() + "@test.com";
            password = "Possum100";

         //   string accessCode = "F675YHZQ";
            string accessCode = SeleniumUITests.SharedProperties["AccessCode"];
            Navigate();
            FindElement(By.XPath(".//*[@id='SignUpMessageControlContainer']/div/div[4]/a")).Click();
            FindElement(By.XPath(".//*[@id='NewAccountSubmit']")).Click();

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

            var emailaddress = FindElement(By.XPath(".//*[@id='SignUpForm']/div[2]/div/p[1]/span")).Text;
            SeleniumUITests.SharedProperties["EmailAddress"] = emailaddress;
            
            FindElement(By.Id("BypassEmailConfirm")).Click();
            FindElement(By.XPath(".//*[@id='NewAccountSubmit']")).Click();

            FindElement(By.XPath(".//*[@id='addressValidationUI0']/div[3]/input")).Click();
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
            FindElement(By.XPath("id('ContactInfo_FirstName')")).SendKeys("TestFirstName");
            FindElement(By.XPath("id('ContactInfo_LastName')")).Clear();
            FindElement(By.XPath("id('ContactInfo_LastName')")).SendKeys("TestLastName");
            FindElement(By.Id("NewAccountSubmit")).Click();

            FindElement(By.XPath(".//*[@id='FacilityCodes_0_']")).SendKeys(accessCode);

            int backspace = 8;  // if we exceed this, we're in a bad place.
            while (backspace-- > 0)
            {
                FindElement(By.XPath(".//*[@id='FacilityCodes_0_']")).SendKeys(Keys.Left);
            }
            Sleep(3000);
            int delete = 15;  // if we exceed this, we're in a bad place.
            while (delete-- > 0)
            {
                FindElement(By.XPath(".//*[@id='FacilityCodes_0_']")).SendKeys(Keys.Backspace);
            }
            Sleep(3000);
            FindElement(By.XPath("id('DoneSubmit')")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='accountInfoLink']/span")).Click();
            WaitForComplete();
            FindElement(By.XPath(".//*[@id='accountInfoContainer']/div[3]/div/div[1]/a")).Click();
            WaitForComplete();
            FindElement(By.XPath("//div/table/tbody/tr/td[6]/span")).Click();
            WaitForComplete();
            FindElement(By.XPath("//form/div/div[2]/div/div/div[2]/a/span")).Click();
            WaitForComplete();
            FindElement(By.XPath("//div[9]/div/div[2]/div/a/span")).Click();
            WaitForComplete();
            FindElement(By.XPath("//a[@id='wizardCancelButton']/span")).Click();
            WaitForComplete();
            return NavigateTo<ACPO>();
        }
      
    }
}
