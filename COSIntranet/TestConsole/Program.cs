using Change.Contracts.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Change.Contracts.Common.ListUtilities;

namespace TestConsole
{
    class Program
    {
        private const string queryLateContracts =
                                   @"<Where>
                                     <And>
                                      <Lt>
                                        <FieldRef Name='ChangeContractWarnDate' />
                                        <Value Type='DateTime'>
                                          <Today/>
                                        </Value>
                                      </Lt>
                                      <Eq>
                                        <FieldRef Name='ChangeContractContractStatus' />
                                        <Value Type='Text'>Active</Value>
                                      </Eq>
                                    </And>
                                   </Where>";

        public static Guid ChangeContractContractStatus = new Guid("{8c222fe8-f4a9-4e59-a75c-bf111672c947}");

        static void Main(string[] args)
        {
            //DateTime grandOpening = DateTime.Now;
            //DateTime firstDelivery = grandOpening.AddDays(-13);
            //DateTime secondDelivery = grandOpening.AddDays(-7);
            //Console.WriteLine(string.Format("{0},{1},{2}", string.Format("{0:MMMM dd, yyyy}", grandOpening), string.Format("{0:dd-MM-yyyy}", firstDelivery), string.Format("{0:dd-MM-yyyy}", secondDelivery)));

            TestSetContractStatus(@"http://sharcha-p15/sites/contracts");

            //CreateZipFile();
        }

        private static void TestSetContractStatus(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SetContractStatus(web);
                }
            }
        }

        private static void SetContractStatus(SPWeb web)
        {
            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, Urls.Contracts));
            SPQuery query = new SPQuery();

            // late contracts
            query.Query = queryLateContracts;
            SPListItemCollection contracts = list.GetItems(query);

            using (DisableEventFiringScope scope = new DisableEventFiringScope())
            {
                foreach (SPListItem contractItem in contracts)
                {
                    contractItem[ChangeContractContractStatus] = Fields.StatusExpired;
                    contractItem.SystemUpdate(false);
                }
            }
        }

        private static void CreateZipFile()
        {
            ZipUtility zipUtility = new ZipUtility("ZipDefaultName", "zip");

            zipUtility.AddDirectoryByName("DUPA1_Folder");
            zipUtility.AddDirectoryByName("DUPA2_Folder");

            FileStream file = File.Open("testDupa.txt", FileMode.Open);
            zipUtility.AddFile("TESTDUPA", file);

            file = File.Open("testDupa1.txt", FileMode.Open);
            //zipUtility.AddFile("testdupa1", file);
            zipUtility.AddFile("DUPA2_Folder\\testdupa1", file);

            //zipUtility.AddFile("testdupa", file, "DUPA2_Folder");
            //zipUtility.AddFile("DUPA2_Folder\\testdupa", file);

            zipUtility.SavePackageToFile(Directory.GetCurrentDirectory());
        }
    }
}
