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


        public static string[] projectLibrarieUrls = { "Marketing", "Drawings", "GeneralInformation", "Logistic", "Pictures", "Evaluation" };

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
        /// Remove all project releted folders form libs
        /// </summary>
        /// <param name="web">Busines dev web</param>
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
        public static void RemoveProjectRootTask(SPWeb web, int itemId)
        {
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, ListUtilities.Urls.ProjectTasks));
            RemoveProjectRootTask(list, itemId);
        }

        /// <summary>
        /// Remove project root task from taks list.
        /// </summary>
        /// <param name="web">Project tasks lists</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProjectRootTask(SPList list, int itemId)
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

                        foreach (string listUrl in projectLibrarieUrls)
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
            var template = serializer.Deserialize(json, typeof(ProjectTask));
            byte[] content = System.Text.Encoding.ASCII.GetBytes(json);
            string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplates);
            SPList projectTemplatesList = web.GetList(projectTemplatesUrl);
            CommonUtilities.AddDocumentToLibrary((SPDocumentLibrary)projectTemplatesList, string.Empty, content, fileName, new Hashtable());
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


    }

}
