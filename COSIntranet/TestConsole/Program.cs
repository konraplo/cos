using Change.Contracts.Common;
using Change.Intranet.Model;
using Change.Intranet.Projects;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static Change.Contracts.Common.ListUtilities;
using System.Xml;

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
        private const string BA = "Procurement General[de]Allgemeiner Einkauf";
        private const string BC = "Connectivity, eMobility&Fahrerassistenz[de]Connectivitaet, eMobilitaet&Fahrerassistenz";
        private const string BI = "Interior[de]Interieur[es]Interior[hu]belső[pl]wnętrze[ru]интерьер[zh]室内[cs]interiér[fr]intérieur[it]interno[pt]interior[sk]interié";
        private const string BE = "Electric[de]Elektrik[es]Eléctrico[hu]Elektromos[pl]Elektryczny[ru]электрический[zh]电动[cs]Elektrický[fr]électrique[it]elettrico[pt]elétrico[sk]elektrický";
        private const string BM = "Metal[de]Metall[es]metal[hu]fém[pl]metal[ru]металл[zh]金属[cs]kov[fr]métal[it]metallo[pt]metal[sk]kov";
        private const string BN = "New Product Launches[de]Neue Produktanlaeufe";
        private const string BP = "Powertrain[de]Antriebswelle[es]Tren motriz[hu]Erőátvitel[pl]Powertrain[ru]Силовой агрегат[zh]动力总成[cs]Hnací ústrojí[fr]Groupe motopropulseur[it]Powertrain[pt]Powertrain[sk]Powertrai";
        private const string BX = "Exterior[de]Exterieur[es]exterior[hu]külső[pl]powierzchowność[ru]экстерьер[zh]外观[cs]exteriér[fr]extérieur[it]esterno[pt]exterior[sk]exteriér";
        static void Main(string[] args)
        {
            string test = Path.GetFileNameWithoutExtension("dupa.json");
            string testBool = Convert.ToString(true);
            DateTime warningDate = DateTime.Parse("7/23/2019 8:33:21 AM");
            DateTime endDate = DateTime.Parse("8/19/2019 10:10:11 AM");
            int diffMonth = ((endDate.Year - warningDate.Year) * 12) + endDate.Month - warningDate.Month;
            Console.WriteLine("Months: " + diffMonth);
            string testString = "<Lists/Drawings>/Center before opening";
            Regex regex = new Regex("<([^>]+)>");
            Match match = regex.Match(testString);
            if (match.Success)
            {
                string cleanString = regex.Replace(testString, "$1/dupa");
                Console.WriteLine("MATCH VALUE: " + regex.Replace(match.Value, "$1") );
                Console.WriteLine(cleanString);

            }
            //var dateSpan = DateTimeSpan.CompareDates(compareTo, now);
            //Console.WriteLine("Years: " + dateSpan.Years);
            //Console.WriteLine("Days: " + dateSpan.Days);
            //Console.WriteLine("Hours: " + dateSpan.Hours);
            //Console.WriteLine("Minutes: " + dateSpan.Minutes);
            //Console.WriteLine("Seconds: " + dateSpan.Seconds);
            //Console.WriteLine("Milliseconds: " + dateSpan.Milliseconds);

            //DateTime grandOpening = DateTime.Now;
            //DateTime firstDelivery = grandOpening.AddDays(-13);
            //DateTime secondDelivery = grandOpening.AddDays(-7);
            //Console.WriteLine(string.Format("{0},{1},{2}", string.Format("{0:MMMM dd, yyyy}", grandOpening), string.Format("{0:dd-MM-yyyy}", firstDelivery), string.Format("{0:dd-MM-yyyy}", secondDelivery)));
            ReadCsvCommodities();
            //TestSetContractStatus(@"http://sharcha-p15/sites/contracts");
            //TestProjectTemplate();
            //TestCreateProjectTemplate(@"http://sharcha-p15/sites/cos/bd", 16);
            //TestCreateProjectTemplate(@"http://spvm/sites/cos/bd", 1);
            //SetFieldSchema(@"http://spvm/sites/cos/");
            //TestCopyFolderStrcutre(@"http://sharcha-p15/sites/cos/bd");
            //TestCopyFolderStrcutreRef(@"http://spvm/sites/kplmain");
            //TestUpdateFolderStrucutreProjectTemplate(@"http://sharcha-p15/sites/cos/bd");
            //CreateFolderStructure(@"http://sharcha-p15/sites/cos/bd", "11_tst01_Canada_Opening");
            //Upgradeto12Test(@"http://sharcha-p15/sites/cos/bd");
            //CreateZipFile();
        }

        private static void ReadCsvCommodities()
        {
            using (StreamReader sr = new StreamReader(@"C:\kpl\commodities.csv"))
            {
                Regex regex = new Regex(@"\d{4}");
                string currentLine;
                Dictionary<string, Comm> values = new Dictionary<string, Comm>();
                Dictionary<string, string> commodities = new Dictionary<string, string>();
                commodities.Add("BA", BA);
                commodities.Add("BC", BC);
                commodities.Add("BI", BI);
                commodities.Add("BE", BE);
                commodities.Add("BM", BM);
                commodities.Add("BN", BN);
                commodities.Add("BP", BP);
                commodities.Add("BX", BX);
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] coulumns = currentLine.Split(new char[] { ';' });
                    Match match = regex.Match(coulumns[1]);
                    if (match.Success)
                    {
                        string commValue = coulumns[0];
                        if (!commodities.ContainsKey(commValue))
                        {
                            continue;
                        }


                        string id = match.Value;
                        Comm comm;
                        if (values.ContainsKey(id))
                        {
                            comm = values[id];
                        }
                        else {
                            comm = new Comm();
                            values.Add(id, comm);
                        }

                        comm.Id = id;
                        comm.Translation = commodities[commValue];
                        //if (coulumns[2].Equals("US", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //    comm.Us = coulumns[3];
                        //}
                        //else if (coulumns[2].Equals("D", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //    comm.De = coulumns[3];
                        //}
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (Comm item in values.Values)
                {
                    //if (string.IsNullOrEmpty(item.Us))
                    //{
                    //    continue;
                    //}

                    string commodityLine = string.Empty;
                    //if (string.IsNullOrEmpty(item.De)) {
                    // commodityLine = string.Format("{0};{1}", item.Us, item.Id);
                    //}
                    //else
                    //{
                    //    commodityLine = string.Format("{0}[de]{1};{2}", item.Us, item.De, item.Id);
                    //}
                    commodityLine = string.Format("{0};{1}", item.Translation, item.Id);

                    sb.AppendLine(commodityLine);
                }

                File.WriteAllText(@"C:\kpl\commoditiesFile.txt",sb.ToString());
            }
        }

        private static void ReadCsvSearchDocs()
        {
            using (StreamReader sr = new StreamReader(@"C:\kpl\SearchDocId.csv"))
            {
                Regex regex = new Regex(@"\d{4}");
                string currentLine;
                Dictionary<string, Comm> values = new Dictionary<string, Comm>();
                Dictionary<string, string> commodities = new Dictionary<string, string>();
                commodities.Add("BA", BA);
                commodities.Add("BC", BC);
                commodities.Add("BI", BI);
                commodities.Add("BE", BE);
                commodities.Add("BM", BM);
                commodities.Add("BN", BN);
                commodities.Add("BP", BP);
                commodities.Add("BX", BX);
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] coulumns = currentLine.Split(new char[] { ';' });
                    Match match = regex.Match(coulumns[1]);
                    if (match.Success)
                    {
                        string commValue = coulumns[0];
                        if (!commodities.ContainsKey(commValue))
                        {
                            continue;
                        }


                        string id = match.Value;
                        Comm comm;
                        if (values.ContainsKey(id))
                        {
                            comm = values[id];
                        }
                        else
                        {
                            comm = new Comm();
                            values.Add(id, comm);
                        }

                        comm.Id = id;
                        comm.Translation = commodities[commValue];
                        //if (coulumns[2].Equals("US", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //    comm.Us = coulumns[3];
                        //}
                        //else if (coulumns[2].Equals("D", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //    comm.De = coulumns[3];
                        //}
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (Comm item in values.Values)
                {
                    //if (string.IsNullOrEmpty(item.Us))
                    //{
                    //    continue;
                    //}

                    string commodityLine = string.Empty;
                    //if (string.IsNullOrEmpty(item.De)) {
                    // commodityLine = string.Format("{0};{1}", item.Us, item.Id);
                    //}
                    //else
                    //{
                    //    commodityLine = string.Format("{0}[de]{1};{2}", item.Us, item.De, item.Id);
                    //}
                    commodityLine = string.Format("{0};{1}", item.Translation, item.Id);

                    sb.AppendLine(commodityLine);
                }

                File.WriteAllText(@"C:\kpl\commoditiesFile.txt", sb.ToString());
            }
        }

        private static void Upgradeto12Test(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    Upgradeto12(web);
                }
            }
        }

        private static void SetFieldSchema(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPFieldLookup lookUp = (SPFieldLookup)web.Fields.GetFieldByInternalName("ChangeProjectDepartment");
                    string bla = "<Field DisplayName='Department' Type='Lookup' Required='FALSE' List='{eae5405f-dbe5-4cc4-9f55-012aadcb3822}' WebId='a7915e2d-0e70-4538-9d51-aae67e9cfc56' ID='{23e58fe2-e13e-4bf6-bee5-c99b44921b9e}' SourceID='{ec7597cd-5b09-472f-b510-c95decd8b857}' StaticName='ChangeProjectDepartment' Name='ChangeProjectDepartment' Version='4' Group='CHANGE Fields' ShowField='Title' />";
                    bla = lookUp.SchemaXml;
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(bla);
                    xDoc.DocumentElement.Attributes["DisplayName"].Value = "Dupa";
                    bla = xDoc.OuterXml;

                    lookUp.SchemaXml = bla;
                    //lookUp.Update(true);
                }
            }
        }

        private static void SetTaskLink(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList projectList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));
                    SPListItem project = projectList.GetItemById(1);

                    string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.ProjectTasks);
                    SPList tasksList = web.GetList(tasksUrl);
                    SPView view = Change.Intranet.Common.ProjectHelper.AddProjectTaskView(project, tasksList);
                    string allTaskViewUrl = view.Url;
                    allTaskViewUrl = string.Format("{0}/{1}",web.Url, allTaskViewUrl);
                    SPFieldUrlValue hyper = new SPFieldUrlValue();
                    hyper.Description = "Tasks";
                    hyper.Url = allTaskViewUrl;
                    project[Change.Intranet.Common.Fields.ChangeProjectTasksLink] = hyper;
                    project.Update();
                }
            }
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
            if (projectRootTask != null)
            {
                FillProjectTasksTree(projectRootTask, tasks);
            }

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
            string templatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.ProjectTemplates);
            SPList templatesList = web.GetList(templatesUrl);
            SPListItem templateItem = templatesList.GetItemById(2);
            templateItem.File.OpenBinary();
            string content = Encoding.UTF8.GetString(templateItem.File.OpenBinary());

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

        private static void SaveProjectTemplate(SPWeb web, ProjectTask projectRootTask)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(projectRootTask);
            var template = serializer.Deserialize(json, typeof(ProjectTask));
            byte[] content = System.Text.Encoding.ASCII.GetBytes(json);
            string fileName = "template1.json";
            string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.ProjectTemplatesDocuments);
            SPList projectTemplatesList = web.GetList(projectTemplatesUrl);
            CommonUtilities.AddDocumentToLibrary((SPDocumentLibrary)projectTemplatesList, string.Empty, content, fileName, new Hashtable());
            //string path = @"D:\kpl\template1.json";
            //File.WriteAllText(path, json);
        }

        private static void TestProjectTemplate()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string path = @"D:\kpl\template1aa.json";
            string json = File.ReadAllText(path);
            var template = serializer.Deserialize(json, typeof(ProjectTask));

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
                    //SaveProjectTemplate(web, result);
                    using (DisableEventFiringScope scope = new DisableEventFiringScope())
                    {
                        ImportProjectTasksTree(web, projectItemId);
                    }
                }
            }
        }

        private static void TestCopyFolderStrcutre(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    //string srcUrl = SPUtility.ConcatUrls(web.Url,string.Format(@"Lists/Marketing/{0}", @"11_tst01_Canada_Opening/From Marketing to partner"));
                    //string destUrl = SPUtility.ConcatUrls(web.Url, @"tests/11_tst01_Canada_Opening");
                    //SPMoveCopyUtil.CopyFolder(srcUrl, destUrl);
                }
            }
        }
        private static void TestCopyFolderStrcutreRef(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    string srcUrl = SPUtility.ConcatUrls(web.Url, string.Format(@"Marketing/{0}", @"dupa"));
                    string destUrl = SPUtility.ConcatUrls(web.Url, @"Shared Documents/dupa");

                    string pathToDomain = @" C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\16\ISAPI\Microsoft.SharePoint.dll";
                    //Assembly domainAssembly = Assembly.LoadFrom(pathToDomain);
                    Assembly domainAssembly = Assembly.Load("Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                    Type customerType = domainAssembly.GetType("Microsoft.SharePoint.SPMoveCopyUtil");


                    MethodInfo copyFolder = customerType.GetMethod("CopyFolder", BindingFlags.Static | BindingFlags.NonPublic); //Type.GetType("Microsoft.SharePoint.SPMoveCopyUtil").GetMethod("CopyFolder");
                    copyFolder.Invoke(null, new object[] { srcUrl, destUrl });
                }
            }
        }
        private static void TestUpdateFolderStrucutreProjectTemplate(string siteUrl)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(SPUtility.ConcatUrls(web.Url,"tests"));//web.GetList(projectsUrl);

                    UpdateFolderStrucutreMarketingLib(list);
                    UpdateFolderStrucutreDrawingsLib(list);
                    UpdateFolderStrucutreGeneralInformationLib(list);
                    UpdateFolderStrucutreLogisticLib(list);
                    UpdateFolderStrucutrePicturesLib(list);
                    UpdateFolderStrucutreEvaluationLib(list);
                }
            }
        }

        private static void CreateFolderStructure(string siteUrl,string projectName)
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, "tests"));//web.GetList(projectsUrl);
                    SPFolderCollection folderColl = list.RootFolder.SubFolders;
                    foreach (SPFolder folder in folderColl)
                    {
                        SPList projectDocumentList = null;
                        string listUrl = folder.Name;
                        try
                        {
                            projectDocumentList = web.GetList(SPUrlUtility.CombineUrl(web.Url, listUrl));
                        }
                        catch (Exception)
                        {
                            Logger.WriteLog(Logger.Category.Information, "CreateFolderStructure", string.Format("List:{0} not found", listUrl));
                        }

                        if (projectDocumentList == null)
                        {
                            listUrl = string.Format("Lists/{0}", folder.Name);
                            try
                            {
                                projectDocumentList = web.GetList(SPUrlUtility.CombineUrl(web.Url, listUrl));
                            }
                            catch (Exception)
                            {
                                Logger.WriteLog(Logger.Category.Information, "CreateFolderStructure", string.Format("List:{0} not found", listUrl));

                                return;
                            }
                        }

                        // copy folder structure
                        string srcUrl = SPUtility.ConcatUrls(web.Url, folder.Url);
                        string destUrl = SPUtility.ConcatUrls(web.Url, string.Format(@"{0}/{1}",projectDocumentList.RootFolder.Url, projectName));
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

        private static void Upgradeto12(SPWeb web)
        {
            if (web != null)
            {
                // add project template ct
                string projectsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), Change.Intranet.Common.ListUtilities.Urls.StoreOpenings);
                //web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));
                SPList projectsList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.StoreOpenings));//web.GetList(projectsUrl);
                SPContentType projectContentType = web.Site.RootWeb.ContentTypes[Change.Intranet.Common.ContentTypeIds.Project];
                SPField prjectTemplateLookup = web.Site.RootWeb.Fields.GetFieldByInternalName(Change.Intranet.Common.Fields.ProjectTemplate);
                CommonUtilities.AddFieldToContentType(web, projectContentType, prjectTemplateLookup, false, false, string.Empty);

                
            }

            Logger.WriteLog(Logger.Category.Medium, "Upgradeto12 finished", string.Format("web:{0}", web.Url));
        }

        private static void UpdateFolderStrucutreMarketingLib(SPList list)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "Start update Marketing");
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Marketing";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);

            string fromMarketingToPartnerFolderUrl = "From Marketing to partner";
            SPFolder fromMarketingToPartner = projectFolderObj.SubFolders.Add(fromMarketingToPartnerFolderUrl);
            folderUrl = "Center Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);
            folderUrl = "Own Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);
            folderUrl = "External Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);

            string fromPartnerToMarketingFolderUrl = "From partner to Marketing";
            SPFolder fromPartnerToMarketing = projectFolderObj.SubFolders.Add(fromPartnerToMarketingFolderUrl);
            folderUrl = "Center information";
            fromPartnerToMarketing.SubFolders.Add(folderUrl);
            list.Update();

            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\MarketingTemplates");
            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketin_order.xlsx".TrimStart('\\'));
            string trargetFolderRelativeUrl = string.Format(@"Marketing/{0}/{1}", fromPartnerToMarketingFolderUrl, folderUrl);
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_Timeline.xlsx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_overview.xlsx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "End update Marketing");
        }

        private static void UpdateFolderStrucutreDrawingsLib(SPList list)
        {
            // Drawings
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "Start update Drawings");

            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Drawings";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "Center before opening";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Recieved from center";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Final - PDF";
            SPFolder finalPDF = projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Final - CAD";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Not approved";
            finalPDF.SubFolders.Add(folderUrl);

            list.Update();

            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "End update Drawings");
        }

        private static void UpdateFolderStrucutreGeneralInformationLib(SPList list)
        {
            // GeneralInformation
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "Start update GeneralInformation");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "GeneralInformation";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\GeneralInformationTemplates");
            list.Update();

            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Costruction_Scope_of_Work.docx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Frontpage.xlsx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);

            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "End update GeneralInformation");
        }

        private static void UpdateFolderStrucutreLogisticLib(SPList list)
        {
            // Logistic
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "Start update Logistic");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Logistic";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "Order";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Order confirmation";
            projectFolderObj.SubFolders.Add(folderUrl);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "End update Logistic");
        }
        private static void UpdateFolderStrucutrePicturesLib(SPList list)
        {
            // Pictures
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "Start update Pictures");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Pictures";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "From Warehouse";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Center, after opening";
            projectFolderObj.SubFolders.Add(folderUrl);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "End update Pictures");
        }

        private static void UpdateFolderStrucutreEvaluationLib(SPList list)
        {
            // Evaluation
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "Start update Evaluation");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Evaluation";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);

            list.Update();

            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\EvaluationTemplates");

            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_contractor.doc".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_departments.xls".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "End update Evaluation");
        }

    }
}
