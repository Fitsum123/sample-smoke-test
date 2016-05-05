using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;

public class ACPOSaveStartUpInformationForDCR : PageObject
{
    public override string PageRelativeUrl
    {
        get
        {
          
           return "/ACPO/";
        }
    }
    // External User - Save Start-Up information
    public void testACPOSaveStartUpInformationForDCR()
    {
        var driver = this;
        Navigate();

        //Click Open Application button
       
        driver.FindElement(By.XPath("html/body/div[1]/div/div/div/div/div/div[4]/div[2]/div[2]/div[1]/div/div[3]/div[3]/div/table/tbody/tr[1]/td[9]/span")).Click();
        //startup infor
        driver.FindElement(By.XPath(".//*[@id='authorityToConstruct']/div[2]/div/div[8]/div[1]/span[1]")).Click();
       // driver.FindElement(By.LinkText("UPLOAD DOCUMENT")).Click();

        driver.FindElement(By.XPath(".//*[@id='Device_0__upload_uploadButton']/span")).SendKeys("C:/Users/fitsum/Desktop/QA Info/qa.txt");
        driver.FindElement(By.LinkText("SAVE INFORMATION")).Click(); 
    




    }

}
