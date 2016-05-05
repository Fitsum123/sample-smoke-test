using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Gov.Baaqmd.Tests
{

    internal static class Environment
    {
        /// <summary>
        /// If true, an alert is inserted.
        /// </summary>
        public static bool DemoMode = false;



#if SAUCE
        public static bool IsInSauce { get { return bool.Parse("%IsInSauce%".ExpandEnvironmentalVariables((key) => "true")); } }
#else
        public static bool IsInSauce { get { return bool.Parse("%IsInSauce%".ExpandEnvironmentalVariables((key) => "false")); } }
#endif

#if LOCAL
        static private string testEnvironment = null;
        static public string TestEnvironment {  get
            {
                //We do this to make the test dynamic - run locally
                //against azure emulator full or express.  the test doesnt know
                if (testEnvironment == null)
                {
                    try {
                        //1st try to connect on :8080, if fail set to 80
                        var wc = new System.Net.WebClient();
                        wc.DownloadString("http://localhost:8080");
                        testEnvironment = "localhost:8080";
                    } catch (Exception)
                    {
                        testEnvironment = "localhost";
                    }
                }

                return testEnvironment;
            }
        }
#endif

#if AzureDev
        static public string TestEnvironment = "ps-dev.baaqmd.gov";
#endif

#if AzureTest
        static public string TestEnvironment = "ps-uat.baaqmd.gov";
#endif

#if AzureStaging
        static public string TestEnvironment = "ps-uat.baaqmd.gov";
#endif



        public static string UserName { get { return "%SauceUserName%".ExpandEnvironmentalVariables((key) => "fitsum"); } }
        public static string AccessKey { get { return "%SaucePassword%".ExpandEnvironmentalVariables((key) => "fa44aa33-b552-42db-b69a-777a03d5625a"); } }

        public static NetworkCredential TestEngineer1 = new NetworkCredential("testengineer1@baaqmd.gov", "Possum1");
        public static NetworkCredential TestInspector1 = new NetworkCredential("TestGDFInspector1@baaqmd.gov", "Possum1");
        public static NetworkCredential Testsupervisor2 = new NetworkCredential("TestEngSupervisor2@baaqmd.gov", "Possum1");


    }
}
