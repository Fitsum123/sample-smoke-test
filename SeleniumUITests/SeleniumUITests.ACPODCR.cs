using Gov.Baaqmd.Tests.SeleniumUITests.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Baaqmd.Tests.SeleniumUITests
{
    public partial class SeleniumUITests
    {
        [TestMethod] //<== This indicates that this is a test method and should show up in Test Explorer
        [TestCategory("UI")] //<== This is a UI Test
        [TestCategory("Nightly")] //<== This is to be run nightly
        [DeploymentItem("TestData.xls")] //<== Use the Excel spreadsheet for data
        [DataSource("ACPOSignIn")] //<== Use this tab in Excel
        public void SeleniumACPOSigninDCR()
        {
            //Read Data from Excel, each column is a field. 
            string UserName = context.DataRow["UserName"].ToString();
            string Password = context.DataRow["Password"].ToString();

            //Root Page of this test
            var page = new ACPOSignin();

            //Run login
            page.SignAsExternalNewUse(UserName, Password);


            //Test for Console.Error messages -- Every test should end with this line.
            CheckForErrors();
        }
    }
}
