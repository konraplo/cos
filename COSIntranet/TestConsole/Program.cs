﻿using Change.Contracts.Common;
using Change.Intranet.Model;
using Change.Intranet.Projects;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
            TestCreateProjectTemplate(@"http://sharcha-p15/sites/cos/bd", 11);

            //CreateZipFile();
        }

        private static List<ProjectTask> ExportProjectTasks(SPWeb web, int projectItemId)
        {
            List<ProjectTask> result = new List<ProjectTask>();
            SPList projectList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));
            SPListItem project = projectList.GetItemById(projectItemId);
            DateTime grandOpening = Convert.ToDateTime(project[SPBuiltInFieldId.TaskDueDate]);
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(project[Change.Intranet.Common.Fields.Store]));
            SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(web, store.LookupId));

            string countryUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.Countries);
            SPList countryList = web.GetList(countryUrl);
            List<Country> regions = new List<Country>();
            foreach (SPListItem regionIem in countryList.GetItems(new SPQuery()))
            {
                regions.Add(new Country { Id = regionIem.ID, Title = regionIem.Title, Manager = Convert.ToString(regionIem[Change.Intranet.Common.Fields.ChangeCountrymanager]) });
            }

            string storeMgr = ProjectUtilities.GetStoreManager(web, store.LookupId);

            string projectCoordinator = Convert.ToString(project[SPBuiltInFieldId.AssignedTo]);
            string regionalMgr = regions.FirstOrDefault(x => x.Id.Equals(storeCountry.LookupId)).Manager;

            SPList tasksList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.ProjectTasks));
            SPQuery query = new SPQuery();

            // tasks
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
                task.TimeBeforeGrandOpening = (grandOpening - endDate).Days;

                SPFieldLookupValue parent = new SPFieldLookupValue(Convert.ToString(taskItem[SPBuiltInFieldId.ParentID]));
                if(parent.LookupId > 0)
                {
                    task.ParentId = parent.LookupId;
                    task.ParentTitle = parent.LookupValue;
                }
                result.Add(task);
            }

            foreach (ProjectTask projectTask in result.Where(x => !string.IsNullOrEmpty(x.Responsible) && x.Responsible.Equals(storeMgr)))
            {
                projectTask.Responsible = DepartmentUtilities.StoreManager;
            }
            foreach (ProjectTask projectTask in result.Where(x => !string.IsNullOrEmpty(x.Responsible) && x.Responsible.Equals(projectCoordinator)))
            {
                projectTask.Responsible = DepartmentUtilities.ProjectCoordinator;
            }
            foreach (ProjectTask projectTask in result.Where(x => !string.IsNullOrEmpty(x.Responsible) && x.Responsible.Equals(regionalMgr)))
            {
                projectTask.Responsible = DepartmentUtilities.RegionalManager;
            }

            //clean up other responsibilities
            foreach (ProjectTask projectTask in result.Where(x => !string.IsNullOrEmpty(x.Responsible) && x.Responsible.Contains(";#")))
            {
                projectTask.Responsible = string.Empty;
            }

            return result;
        }

        private static ProjectTask ExportProjectTasksTree(SPWeb web, int projectItemId)
        {
            List<ProjectTask> tasks = ExportProjectTasks(web, projectItemId);
            ProjectTask projectRootTask = tasks.FirstOrDefault(x=> x.IsStoreOpeningTask == true);
            FillProjectTasksTree(projectRootTask, tasks);

            return projectRootTask;
        }

        private static void FillProjectTasksTree(ProjectTask parentTask, List<ProjectTask> tasks)
        {
            List<ProjectTask> subtasks = tasks.Where(x => x.ParentId.Equals(parentTask.Id)).ToList();
            parentTask.Subtasks = subtasks;
            foreach (ProjectTask task in subtasks)
            {
                FillProjectTasksTree(task, tasks);
            }
        }

        private static void ImportProjectTasksTree(SPWeb web, int projectItemId)
        {
            string path = @"D:\kpl\template1.json";
            string template = File.ReadAllText(path);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ProjectTask rootTask = (ProjectTask)serializer.Deserialize(template, typeof(ProjectTask));
            List<ProjectTask> tasksToCreate = new List<ProjectTask>();

            SPList projectList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));
            SPListItem project = projectList.GetItemById(projectItemId);
            SPList tasksList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.ProjectTasks));
            SPContentType foundedProjectTaskCT = tasksList.ContentTypes[tasksList.ContentTypes.BestMatch(Change.Intranet.Common.ContentTypeIds.ProjectTask)];
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(project[Change.Intranet.Common.Fields.Store]));
            SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(web, store.LookupId));

            string countryUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.Countries);
            SPList countryList = web.GetList(countryUrl);
            List<Country> regions = new List<Country>();
            foreach (SPListItem regionIem in countryList.GetItems(new SPQuery()))
            {
                regions.Add(new Country { Id = regionIem.ID, Title = regionIem.Title, Manager = Convert.ToString(regionIem[Change.Intranet.Common.Fields.ChangeCountrymanager]) });
            }

            string storeMgr = ProjectUtilities.GetStoreManager(web, store.LookupId);
            string projectCoordinator = Convert.ToString(project[SPBuiltInFieldId.AssignedTo]);
            DateTime grandOpening = Convert.ToDateTime(project[SPBuiltInFieldId.TaskDueDate]);

            // todo: fill root task values from created project taks in ER
            rootTask.Id = 0; // only for test
            List<Department> departments = DepartmentUtilities.GetDepartments(web);

            CreateMainTasks(grandOpening, projectCoordinator, storeMgr, regions, departments, tasksList, foundedProjectTaskCT, rootTask, storeCountry, store, project, tasksToCreate);

            // create subtasks
            DateTime projectStartDate = DateTime.MinValue;
            DateTime projectDueDate = DateTime.MaxValue;
            List<string> formatedUpdateBatchCommands = SubTasksToCreate(tasksList, foundedProjectTaskCT, storeCountry, store, projectCoordinator, storeMgr, regions, tasksToCreate, projectStartDate, projectDueDate, grandOpening, project);
            string result = CommonUtilities.BatchAddListItems(web, formatedUpdateBatchCommands);


        }

        private static void CreateMainTasks(DateTime grandOpening, string projectCoordinator, string storeMgr, List<Country> regions, List<Department> departments, SPList tasksList, SPContentType foundedProjectTaskCT, ProjectTask task, SPFieldLookupValue storeCountry, SPFieldLookupValue store, SPListItem project,List<ProjectTask> tasks)
        {
            if (task.Subtasks.Count > 0)
            {
                // create task, read Id
                SPListItem projectTask = null;
                if (!task.IsStoreOpeningTask)
                {
                    projectTask = tasksList.AddItem();
                    projectTask[SPBuiltInFieldId.Title] = task.Title;
                    projectTask[SPBuiltInFieldId.ContentTypeId] = foundedProjectTaskCT.Id;
                    projectTask[Change.Intranet.Common.Fields.Country] = storeCountry;
                    projectTask[Change.Intranet.Common.Fields.StoreOpening] = string.Format("{0};#{1}", project.ID, project.Title);
                    projectTask[Change.Intranet.Common.Fields.Store] = string.Format("{0};#{1}", store.LookupId, store.LookupValue);
                    if (task.ParentId > 0)
                    {
                        projectTask[SPBuiltInFieldId.ParentID] = new SPFieldLookupValue(string.Format("{0};#{1}", task.ParentId, task.ParentTitle));

                    }
                    projectTask[Change.Intranet.Common.Fields.ChangeTaskDisplayNameId] = string.Format("({0}) {1}", project.Title, task.Title);

                    if (!string.IsNullOrEmpty(task.ResponsibleDepartment))
                    {
                        Department responsibleDepartment = departments.FirstOrDefault(x => x.Title.Equals(task.ResponsibleDepartment));
                        if (responsibleDepartment != null)
                        {
                            projectTask[Change.Intranet.Common.Fields.Department] = string.Format("{0};#{1}", responsibleDepartment.Id, responsibleDepartment.Title);
                            projectTask[Change.Intranet.Common.Fields.ChangeDeparmentmanager] = responsibleDepartment.Manager;
                           
                            if (responsibleDepartment.Title.Equals(DepartmentUtilities.Retail))
                            {
                                task.Responsible = DepartmentUtilities.RegionalManager;
                            }
                        }
                    }
                    string responsible = string.Empty;
                    if (task.Responsible != null)
                    {
                        if (task.Responsible.Equals(DepartmentUtilities.StoreManager))
                        {
                            responsible = storeMgr;
                        }
                        else if (task.Responsible.Equals(DepartmentUtilities.RegionalManager))
                        {
                            responsible = regions.FirstOrDefault(x => x.Id.Equals(storeCountry.LookupId)).Manager;
                        }
                        else if (task.Responsible.Equals(DepartmentUtilities.ProjectCoordinator))
                        {
                            responsible = projectCoordinator;
                        }
                    }

                    if (!string.IsNullOrEmpty(responsible))
                    {
                        projectTask[SPBuiltInFieldId.AssignedTo] = responsible;
                    }

                    projectTask.Update();

                    task.Id = projectTask.ID;
                }


                // check all subtasks
                foreach (ProjectTask subTask in task.Subtasks)
                {
                    // set parent id
                    subTask.ParentId = task.Id;
                    subTask.ParentTitle = task.Title;
                    CreateMainTasks(grandOpening, projectCoordinator, storeMgr, regions, departments, tasksList, foundedProjectTaskCT, subTask, storeCountry, store, project, tasks);
                }

                if (projectTask != null)
                {
                    int lastTaskTBGO = task.Subtasks.Min(x => x.TimeBeforeGrandOpening);
                    DateTime dueDate = grandOpening.AddDays(-lastTaskTBGO);
                    DateTime startDate = dueDate.AddDays(-task.Duration);
                    projectTask[SPBuiltInFieldId.StartDate] = startDate;
                    projectTask[SPBuiltInFieldId.TaskDueDate] = dueDate;
                    projectTask.Update();
                }


            }
            else
            {
                tasks.Add(task);
            }
            
        }

        private static List<string> SubTasksToCreate(SPList tasksList, SPContentType foundedProjectTaskCT, SPFieldLookupValue storeCountry, SPFieldLookupValue store, string projectCoordinator, string storeMgr, List<Country> regions, List<ProjectTask> tasks, DateTime projectStartDate, DateTime projectDueDate, DateTime grandOpening, SPListItem projectItem)
        {
            List<string> formatedUpdateBatchCommands = new List<string>();

            List<Department> departments = DepartmentUtilities.GetDepartments(projectItem.Web);

            int counter = 1;
            foreach (ProjectTask task in tasks)
            {
                DateTime dueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                DateTime startDate = dueDate.AddDays(-task.Duration);

                if (projectStartDate.Equals(DateTime.MinValue))
                {
                    projectStartDate = startDate;
                }
                else if (DateTime.Compare(projectStartDate, startDate) > 0)
                {
                    projectStartDate = startDate;
                }

                if (projectDueDate.Equals(DateTime.MaxValue))
                {
                    projectDueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                }
                else if (DateTime.Compare(projectDueDate, grandOpening.AddDays(-task.TimeBeforeGrandOpening)) < 0)
                {
                    projectDueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                }

                StringBuilder batchItemSetVar = new StringBuilder();
                batchItemSetVar.Append(string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                                                    projectItem.ParentList.Fields[SPBuiltInFieldId.Title].InternalName,
                                                    task.Title));
                batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       tasksList.Fields[Change.Intranet.Common.Fields.ChangeTaskDisplayNameId].InternalName,
                       string.Format("({0}) {1}", projectItem.Title, task.Title)));

                batchItemSetVar.Append(
                        string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                        projectItem.ParentList.Fields[SPBuiltInFieldId.ContentTypeId].InternalName,
                        Convert.ToString(foundedProjectTaskCT.Id)));
                batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       Change.Intranet.Common.Fields.StoreOpening,
                       string.Format("{0};#{1}", projectItem.ID, projectItem.Title)));
                batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       Change.Intranet.Common.Fields.Store,
                       string.Format("{0};#{1}", store.LookupId, store.LookupValue)));

                if (!string.IsNullOrEmpty(task.ResponsibleDepartment))
                {
                    Department responsibleDepartment = departments.FirstOrDefault(x => x.Title.Equals(task.ResponsibleDepartment));
                    if (responsibleDepartment != null)
                    {
                        batchItemSetVar.Append(
                          string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                          Change.Intranet.Common.Fields.Department,
                          string.Format("{0};#{1}", responsibleDepartment.Id, responsibleDepartment.Title)));
                        batchItemSetVar.Append(
                          string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                          tasksList.Fields[Change.Intranet.Common.Fields.ChangeDeparmentmanager].InternalName,
                          responsibleDepartment.Manager));
                        if (responsibleDepartment.Title.Equals(DepartmentUtilities.Retail))
                        {
                            task.Responsible = DepartmentUtilities.RegionalManager;
                        }
                    }
                }

                batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       Change.Intranet.Common.Fields.Country,
                       storeCountry));
                batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       tasksList.Fields[Change.Intranet.Common.Fields.ChangeTaskDurationId].InternalName,
                       task.Duration));
                string responsible = string.Empty;

                if (task.Responsible != null)
                {
                    if (task.Responsible.Equals(DepartmentUtilities.StoreManager))
                    {
                        responsible = storeMgr;
                    }
                    else if (task.Responsible.Equals(DepartmentUtilities.RegionalManager))
                    {
                        responsible = regions.FirstOrDefault(x => x.Id.Equals(storeCountry.LookupId)).Manager;
                    }
                    else if (task.Responsible.Equals(DepartmentUtilities.ProjectCoordinator))
                    {
                        responsible = projectCoordinator;
                    }
                }

                if (!string.IsNullOrEmpty(responsible))
                {
                    batchItemSetVar.Append(
                    string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                    tasksList.Fields[SPBuiltInFieldId.AssignedTo].InternalName,
                    responsible));
                }

                batchItemSetVar.Append(
                  string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                  tasksList.Fields[SPBuiltInFieldId.TaskDueDate].InternalName,
                  SPUtility.CreateISO8601DateTimeFromSystemDateTime(dueDate)));

                batchItemSetVar.Append(
                  string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                  tasksList.Fields[SPBuiltInFieldId.StartDate].InternalName,
                  SPUtility.CreateISO8601DateTimeFromSystemDateTime(startDate)));

                if (task.ParentId > 0)
                {
                    batchItemSetVar.Append(
                       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                       tasksList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                       string.Format("{0};#{1}", task.ParentId, task.ParentTitle)));
                }

                formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, tasksList.ID.ToString(), batchItemSetVar));
                counter++;
            }

            return formatedUpdateBatchCommands;
        }

        private static void SaveProjectTemplate(ProjectTask projectRootTask)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(projectRootTask);
            var template = serializer.Deserialize(json, typeof(ProjectTask));
            string path = @"D:\kpl\template1.json";
            File.WriteAllText(path, json);
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
                    //ProjectTask result = ExportProjectTasksTree(web, projectItemId);
                    //SaveProjectTemplate(result);
                    using (DisableEventFiringScope scope = new DisableEventFiringScope())
                    {
                        ImportProjectTasksTree(web, projectItemId);
                    }
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
