using Change.Contracts.Common;
using Change.Intranet.Model;
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

        private const string queryProjectTasks =
                                   @"<Where>
                                      <Eq>
                                                                      <FieldRef Name='{0}'  LookupId='True'/>
                                                                      <Value Type='Lookup'>{1}</Value>
                                                                    </Eq>
                                   </Where>";


        public static Guid ChangeContractContractStatus = new Guid("{8c222fe8-f4a9-4e59-a75c-bf111672c947}");

        static void Main(string[] args)
        {
            DateTime warningDate = DateTime.Parse("7/23/2019 8:33:21 AM");
            DateTime endDate = DateTime.Parse("8/19/2019 10:10:11 AM");
            int diffMonth = ((endDate.Year - warningDate.Year) * 12) + endDate.Month - warningDate.Month;
            //var dateSpan = DateTimeSpan.CompareDates(compareTo, now);
            //Console.WriteLine("Years: " + dateSpan.Years);
            Console.WriteLine("Months: " + diffMonth);
            //Console.WriteLine("Days: " + dateSpan.Days);
            //Console.WriteLine("Hours: " + dateSpan.Hours);
            //Console.WriteLine("Minutes: " + dateSpan.Minutes);
            //Console.WriteLine("Seconds: " + dateSpan.Seconds);
            //Console.WriteLine("Milliseconds: " + dateSpan.Milliseconds);

            //DateTime grandOpening = DateTime.Now;
            //DateTime firstDelivery = grandOpening.AddDays(-13);
            //DateTime secondDelivery = grandOpening.AddDays(-7);
            //Console.WriteLine(string.Format("{0},{1},{2}", string.Format("{0:MMMM dd, yyyy}", grandOpening), string.Format("{0:dd-MM-yyyy}", firstDelivery), string.Format("{0:dd-MM-yyyy}", secondDelivery)));

            //TestSetContractStatus(@"http://sharcha-p15/sites/contracts");
            TestCreateProjectTemplate(@"http://sharcha-p15/sites/cos/bd", 1);

            //CreateZipFile();
        }

        private static List<ProjectTask> ExportProjectTasks(SPWeb web, int projectItemId)
        {
            List<ProjectTask> result = new List<ProjectTask>();
            SPList projectList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));
            SPListItem project = projectList.GetItemById(projectItemId);
            DateTime grandOpening = Convert.ToDateTime(project[SPBuiltInFieldId.TaskDueDate]);

            SPList tasksList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.ProjectTasks));
            SPQuery query = new SPQuery();

            // late contracts
            query.Query = string.Format(queryProjectTasks, Change.Intranet.Common.Fields.StoreOpening, projectItemId); ;
            SPListItemCollection tasks = tasksList.GetItems(query);
            foreach (SPListItem taskItem in tasks)
            {
                DateTime endDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.TaskDueDate]);
                DateTime startDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.StartDate]);
                ProjectTask task = new ProjectTask();
                task.Id = taskItem.ID;
                task.Title = taskItem.Title;
                task.IsStoreOpeningTask = Convert.ToBoolean(taskItem[Change.Intranet.Common.Fields.StoreOpeningTask]);
                SPFieldLookupValue department = new SPFieldLookupValue(Convert.ToString(taskItem[Change.Intranet.Common.Fields.Department]));
                task.ResponsibleDepartment = department.LookupValue;
                task.Responsible = Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                task.Duration = (endDate - startDate).Days;
                task.TimeBeforeGrandOpening = (grandOpening - startDate).Days;

                SPFieldLookupValue parent = new SPFieldLookupValue(Convert.ToString(taskItem[SPBuiltInFieldId.ParentID]));
                if(parent.LookupId > 0)
                {
                    task.ParentId = parent.LookupId;
                    task.ParentTitle = parent.LookupValue;
                }
                result.Add(task);
            }
            return result;
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

        private static void TestCreateProjectTemplate(string siteUrl, int projectItemId)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    List<ProjectTask> result = ExportProjectTasks(web, projectItemId);
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
