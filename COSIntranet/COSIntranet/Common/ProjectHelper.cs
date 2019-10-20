namespace Change.Intranet.Common
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using Change.Intranet.Projects;
    using System.IO;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Change.Intranet.Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Functionality connteced with projects opereations
    /// </summary>
    public static class ProjectHelper
    {
        public const string GET_STORE_OPENING_TASK = @"<Where>
                                                                  <And>
                                                                    <Eq>
                                                                      <FieldRef Name='{0}'  LookupId='True'/>
                                                                      <Value Type='Lookup'>{1}</Value>
                                                                    </Eq>
                                                                    <Eq>
                                                                      <FieldRef Name='{2}' />
                                                                      <Value Type='Boolean'>1</Value>
                                                                    </Eq>
                                                                  </And>
                                                                </Where>";
        /// <summary>
        /// Get all tasks for specified project
        /// </summary>
        private const string QueryProjectTasks =
                                  @"<Where>
                                      <Eq>
                                                                      <FieldRef Name='{0}'  LookupId='True'/>
                                                                      <Value Type='Lookup'>{1}</Value>
                                                                    </Eq>
                                   </Where>";


        private static string[] storeOpeningLibrarieUrls = { "Marketing", "Drawings", "GeneralInformation", "Logistic", "Pictures", "Evaluation" };
        public static string[] projectLibrarieUrls = { "Finance", "HR", "Marketing"};

        /// <summary>
        /// Remove project releted folder from list
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="listUrl">Project library url</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProjectFolder(SPWeb web, string listUrl, int itemId)
        {
            Logger.WriteLog(Logger.Category.Information, "RemoveProjectFolder", string.Format("Remove project folder:{0} from {1}", itemId, listUrl));
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, string.Format("Lists/{0}", listUrl)));
                SPListItemCollection items = CommonUtilities.GetFoldersByPrefix(web, list, string.Format("{0}_", itemId));

                if (items.Count > 0)
                {
                    SPListItem firstItem = items[0];
                    firstItem.Delete();
                    list.Update();
                }
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProjectFolder", "List not found");
                return;
            }
        }

        /// <summary>
        /// Remove all store opening releted folders form libs
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveAllStoreOpeningReletedFolder(SPWeb web, int itemId)
        {
            foreach (string listUrl in storeOpeningLibrarieUrls)
            {
                RemoveProjectFolder(web, listUrl, itemId);
            }
        }

        // <summary>
        /// Remove all project releted folders form libs
        /// </summary>
        /// <param name="web">Project web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveAllProjectFolder(SPWeb web, int itemId)
        {
            foreach (string listUrl in projectLibrarieUrls)
            {
                RemoveProjectFolder(web, listUrl, itemId);
            }
        }

        public static void RemoveProject(SPWeb web, int itemId)
        {
            try
            {
                SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, ListUtilities.Urls.StoreOpenings));
                Logger.WriteLog(Logger.Category.Information, "RemoveProject", string.Format("Remove project :{0} from {1}", itemId, list.RootFolder.Url));
                SPListItem project = list.GetItemById(itemId);
                project.Delete();
                list.Update();
            }
            catch (Exception e)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProject", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Remove project root task from taks list.
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveStoreOpeningRootTask(SPWeb web, int itemId)
        {
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, ListUtilities.Urls.ProjectTasks));
            RemoveStoreOpeningRootTask(list, itemId);
        }

        /// <summary>
        /// Remove project root task from taks list.
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProjectRootTask(SPWeb web, int itemId)
        {
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, ListUtilities.Urls.ProjectTasks));
            RemoveProjectRootTask(list, itemId);
        }

        /// <summary>
        /// Remove project root task from taks list.
        /// </summary>
        /// <param name="list">Project tasks lists</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveStoreOpeningRootTask(SPList list, int itemId)
        {
            try
            {
                SPListItem projectRootTask = GetStoreOpeningRootTask(list, itemId);
                if (projectRootTask != null)
                {
                    Logger.WriteLog(Logger.Category.Information, "RemoveProjectRootTask", string.Format("Remove project rootTask:{0} from {1}", itemId, list.RootFolder.Url));
                    using (DisableEventFiring scope = new DisableEventFiring())
                    {
                        projectRootTask.Delete();
                        list.Update();
                    }

                }

            }
            catch (Exception e)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProjectRootTask", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Remove project root task from taks list.
        /// </summary>
        /// <param name="list">Project tasks lists</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProjectRootTask(SPList list, int itemId)
        {
            try
            {
                SPListItem projectRootTask = GetProjectRootTask(list, itemId);
                if (projectRootTask != null)
                {
                    Logger.WriteLog(Logger.Category.Information, "RemoveProjectRootTask", string.Format("Remove project rootTask:{0} from {1}", itemId, list.RootFolder.Url));
                    using (DisableEventFiring scope = new DisableEventFiring())
                    {
                        projectRootTask.Delete();
                        list.Update();
                    }

                }

            }
            catch (Exception e)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProjectRootTask", e.Message);
                throw;
            }
        }
        /// <summary>
        /// Archive project. Create ziped project file and save in Assets library
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        /// <param name="fileSavingPlace">Place where the archiv file is saved</param>
        /// <returns>Name of created zip file</returns>
        public static string ArchiveProject(SPWeb web, int itemId, UIHelper.ZipFileSavingPlace fileSavingPlace)
        {
            string createdZipName = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite elevSite = new SPSite(web.Site.ID))
                {
                    using (SPWeb elevWeb = elevSite.OpenWeb(web.ID))
                    {
                        elevWeb.AllowUnsafeUpdates = true;
                        Logger.WriteLog(Logger.Category.Information, "ArchiveProject", string.Format("Archive project for project id: {0} from web: {1}", itemId, web.Url));

                        SPList list = web.GetList(SPUrlUtility.CombineUrl(elevWeb.Url, ListUtilities.Urls.StoreOpenings));
                        SPListItem project = list.GetItemById(itemId);
                        string projectFolderName = ProjectUtilities.GetProjectsFolderName(elevWeb, itemId);
                        ZipUtility zip = new ZipUtility(projectFolderName);

                        foreach (string listUrl in storeOpeningLibrarieUrls)
                        {
                            ArchiveProjectData(elevWeb, listUrl, itemId, zip);
                        }

                        if (fileSavingPlace == UIHelper.ZipFileSavingPlace.LocalServerTempFolder)
                        {
                            createdZipName = zip.SavePackageToTempFolder();
                        }
                        else
                        {
                            createdZipName = zip.SavePackageToSharePoint(web, "siteassets/archives");
                        }
                    }
                }
            });

            return createdZipName;
        }

        /// <summary>
        /// Archive project releted folder to zip file
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="listUrl">Project library url</param>
        /// <param name="itemId">Project item id</param>
        public static void ArchiveProjectData(SPWeb web, string listUrl, int itemId, ZipUtility zipUtility)
        {
            Logger.WriteLog(Logger.Category.Information, "ArchiveProjectData", string.Format("Archive project folder for project id: {0} from library: {1}", itemId, listUrl));
            SPList list = null;

            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, string.Format("Lists/{0}", listUrl)));
                SPListItemCollection items = CommonUtilities.GetFoldersByPrefix(web, list, string.Format("{0}_", itemId));



                if (items.Count > 0)
                {
                    // create folder for project content in ziped file
                    zipUtility.AddDirectoryByName(listUrl);

                    SPListItem firstItem = items[0];

                    SPQuery query = new SPQuery();
                    query.Folder = firstItem.Folder;
                    query.ViewAttributes = "Scope='RecursiveAll'";
                    SPListItemCollection projectItems = list.GetItems(query);

                    if (projectItems.Count > 0)
                    {

                        foreach (SPListItem item in projectItems)
                        {
                            try
                            {
                                if (item.FileSystemObjectType == SPFileSystemObjectType.File)
                                {
                                    string folderPath = item.File.ParentFolder.Url.Replace(firstItem.Folder.Url, string.Empty);
                                    folderPath = Path.Combine(listUrl, folderPath.Replace("/", "\\").TrimStart(('\\')));
                                    zipUtility.AddFile(item.File, folderPath);
                                }
                                else if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                                {
                                    string folderPath = item.Folder.Url.Replace(firstItem.Folder.Url, string.Empty);
                                    folderPath = Path.Combine(listUrl, folderPath.Replace("/", "\\").TrimStart(('\\')));
                                    zipUtility.AddDirectoryByName(folderPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLog(Logger.Category.Unexpected, "ArchiveProjectFolder", "Iterate project folder files");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "ArchiveProjectFolder", "List not found");
                return;
            }
        }

        public static SPListItem GetStoreOpeningRootTask(SPList projectTasksList, int storeOpeningItemId)
        {
            SPQuery findProjectTask = new SPQuery();
            findProjectTask.Query = string.Format(GET_STORE_OPENING_TASK, Fields.StoreOpening, storeOpeningItemId, Fields.StoreOpeningTask);
            SPListItemCollection items = projectTasksList.GetItems(findProjectTask);
            if (items.Count == 1)
            {
                return items[0];
            }

            return null;
        }

        public static SPListItem GetProjectRootTask(SPList projectTasksList, int projectItemId)
        {
            SPQuery findProjectTask = new SPQuery();
            findProjectTask.Query = string.Format(GET_STORE_OPENING_TASK, Fields.Project, projectItemId, Fields.StoreOpeningTask);
            SPListItemCollection items = projectTasksList.GetItems(findProjectTask);
            if (items.Count == 1)
            {
                return items[0];
            }

            return null;
        }
        /// <summary>
        /// Save project plan (all project tasks) as template
        /// </summary>
        /// <param name="web">Web with project template lib</param>
        /// <param name="projectItemId"></param>
        /// <param name="templateName"></param>
        public static void SaveProjectTemplate(SPWeb web, int projectItemId, string templateName)
        {
            ProjectTask projectRootTask = ExportProjectTasksTree(web, projectItemId);
            templateName = string.Format("{0}.json", templateName);
            SaveProjectTemplate(web, projectRootTask, templateName);
        }

        /// <summary>
        /// Add/update view to the project tasks filtered by project
        /// </summary>
        /// <param name="projectItem">Project List item</param>
        /// <param name="tasksList">project tasks view</param>
        /// <returns>created/updated view</returns>
        public static SPView AddProjectTaskView(SPListItem projectItem, SPList tasksList)
        {
            string ViewDecisionsName = string.Format("project_{0}_tasks", projectItem.ID);

            //create view
            //SPList projects = projectItem.ParentList;
            SPViewCollection allviews = tasksList.Views;
            SPView viewProjectTasks = null;
            if (CommonUtilities.HasView(tasksList, ViewDecisionsName))
            {
                Logger.WriteLog(Logger.Category.Unexpected, "AddProjectTaskView", string.Format("Remove fields from View:{0} from list: {1}", ViewDecisionsName, tasksList.Title));

                viewProjectTasks = allviews[ViewDecisionsName];
                //remove all fields from default view
                viewProjectTasks.ViewFields.DeleteAll();
            }

            System.Collections.Specialized.StringCollection viewFields = new System.Collections.Specialized.StringCollection();
            viewFields.Add("Checkmark");
            viewFields.Add("LinkTitle");
            viewFields.Add("DueDate");
            viewFields.Add("AssignedTo");
            viewFields.Add("ChangeStoreOpening");
            string myquery = string.Format(QueryProjectTasks, Fields.StoreOpening, projectItem.ID);

            if (viewProjectTasks != null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "AddProjectTaskView", string.Format("Add Fields to View:{0} to list: {1}", ViewDecisionsName, tasksList.Title));

                for (int i = 0; i < viewFields.Count; i++)
                {
                    viewProjectTasks.ViewFields.Add(viewFields[i]);
                }

                viewProjectTasks.Query = myquery;
                viewProjectTasks.Update();
            }
            else
            {
                Logger.WriteLog(Logger.Category.Unexpected, "AddProjectTaskView", string.Format("Add View:{0} to list: {1}", ViewDecisionsName, tasksList.Title));

                viewProjectTasks = allviews.Add(ViewDecisionsName, viewFields, myquery, 100, true, false);
            }

            return viewProjectTasks;
        }

        private static List<ProjectTask> ExportProjectTasks(SPWeb web, int projectItemId)
        {
            List<ProjectTask> result = new List<ProjectTask>();
            SPList projectList = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.StoreOpenings));
            SPListItem project = projectList.GetItemById(projectItemId);
            DateTime grandOpening = Convert.ToDateTime(project[SPBuiltInFieldId.TaskDueDate]);
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(project[Fields.Store]));
            SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(web, store.LookupId));

            string countryUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Countries);
            SPList countryList = web.GetList(countryUrl);
            List<Country> regions = new List<Country>();
            foreach (SPListItem regionIem in countryList.GetItems(new SPQuery()))
            {
                regions.Add(new Country { Id = regionIem.ID, Title = regionIem.Title, Manager = Convert.ToString(regionIem[Fields.ChangeCountrymanager]) });
            }

            string storeMgr = ProjectUtilities.GetStoreManager(web, store.LookupId);

            string projectCoordinator = Convert.ToString(project[SPBuiltInFieldId.AssignedTo]);
            string regionalMgr = regions.FirstOrDefault(x => x.Id.Equals(storeCountry.LookupId)).Manager;

            SPList tasksList = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.ProjectTasks));
            SPQuery query = new SPQuery();

            // tasks
            query.Query = string.Format(QueryProjectTasks, Fields.StoreOpening, projectItemId); ;
            SPListItemCollection tasks = tasksList.GetItems(query);
            foreach (SPListItem taskItem in tasks)
            {
                DateTime endDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.TaskDueDate]);
                DateTime startDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.StartDate]);
                ProjectTask task = new ProjectTask();
                task.Id = taskItem.ID;
                task.Title = taskItem.Title;
                task.IsStoreOpeningTask = Convert.ToBoolean(taskItem[Fields.StoreOpeningTask]);
                SPFieldLookupValue department = new SPFieldLookupValue(Convert.ToString(taskItem[Fields.Department]));
                task.ResponsibleDepartment = department.LookupValue;
                task.Responsible = Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                task.Duration = (endDate - startDate).Days;
                task.TimeBeforeGrandOpening = (grandOpening - endDate).Days;

                SPFieldLookupValue parent = new SPFieldLookupValue(Convert.ToString(taskItem[SPBuiltInFieldId.ParentID]));
                if (parent.LookupId > 0)
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

        private static void SaveProjectTemplate(SPWeb web, ProjectTask projectRootTask, string fileName)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(projectRootTask);
            byte[] content = System.Text.Encoding.ASCII.GetBytes(json);
            string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplates);
            SPList projectTemplatesList = web.GetList(projectTemplatesUrl);
            Hashtable prop = new Hashtable(1);
            prop["Title"] = Path.GetFileNameWithoutExtension(fileName);
            CommonUtilities.AddDocumentToLibrary((SPDocumentLibrary)projectTemplatesList, string.Empty, content, fileName, prop);
        }

        private static ProjectTask ExportProjectTasksTree(SPWeb web, int projectItemId)
        {
            List<ProjectTask> tasks = ExportProjectTasks(web, projectItemId);
            ProjectTask projectRootTask = tasks.FirstOrDefault(x => x.IsStoreOpeningTask == true);
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

        public static void ImportProjectTasksTree(SPWeb web, SPListItem project, int projectTemplateItemId, int rootTaskId,DateTime grandOpening, string projectCoordinator, string storeMgr, List<Country> regions, List<Department> departments, SPContentType foundedProjectTaskCT, SPFieldLookupValue storeCountry, SPFieldLookupValue store, ref DateTime projectStartDate, ref DateTime projectDueDate)
        {
            string templatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplates);
            SPList templatesList = web.GetList(templatesUrl);
            SPListItem templateItem = templatesList.GetItemById(projectTemplateItemId);
            string content = Encoding.UTF8.GetString(templateItem.File.OpenBinary());

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ProjectTask rootTask = (ProjectTask)serializer.Deserialize(content, typeof(ProjectTask));
            List<ProjectTask> tasksToCreate = new List<ProjectTask>();

            SPList tasksList = web.GetList(SPUtility.ConcatUrls(web.Url, Change.Intranet.Common.ListUtilities.Urls.ProjectTasks));

            rootTask.Id = rootTaskId; 
            CreateMainTasks(grandOpening, projectCoordinator, storeMgr, regions, departments, tasksList, foundedProjectTaskCT, rootTask, storeCountry, store, project, tasksToCreate);

            // create subtasks
            List<string> formatedUpdateBatchCommands = SubTasksToCreate(tasksList, foundedProjectTaskCT, storeCountry, store, projectCoordinator, storeMgr, regions, tasksToCreate, ref projectStartDate, ref projectDueDate, grandOpening, project);
            string result = CommonUtilities.BatchAddListItems(web, formatedUpdateBatchCommands);
        }

        private static void CreateMainTasks(DateTime grandOpening, string projectCoordinator, string storeMgr, List<Country> regions, List<Department> departments, SPList tasksList, SPContentType foundedProjectTaskCT, ProjectTask task, SPFieldLookupValue storeCountry, SPFieldLookupValue store, SPListItem project, List<ProjectTask> tasks)
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
            else if (!task.IsStoreOpeningTask)
            {
                tasks.Add(task);
            }

        }

        private static List<string> SubTasksToCreate(SPList tasksList, SPContentType projectTaskCT, SPFieldLookupValue storeCountry, SPFieldLookupValue store, string projectCoordinator, string storeMgr, List<Country> regions, List<ProjectTask> tasks, ref DateTime projectStartDate, ref DateTime projectDueDate, DateTime grandOpening, SPListItem projectItem)
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
                        Convert.ToString(projectTaskCT.Id)));
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

    }

}
