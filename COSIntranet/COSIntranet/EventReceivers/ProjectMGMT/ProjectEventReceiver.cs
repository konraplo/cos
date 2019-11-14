namespace Change.Intranet.EventReceivers.ProjectMGMT
{
    using Change.Intranet.Common;
    using Change.Intranet.Model;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

     /// <summary>
    /// Event receivers for project list
    /// </summary>
    public class ProjectEventReceiver : SPItemEventReceiver
    {
        public delegate List<ProjectTask> CreateProjectTasksList(int parentTaskId, string parentTitle, int shippingDays);

        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
            CreateProjectTasks(properties.ListItem);
            this.UpdateFolderStrucutre(properties.ListItem);
        }

        /// <summary>
        /// Ein Element wurde aktualisiert..
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemUpdated");

            this.UpdateFolderStrucutre(properties.ListItem);
        }

        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemDeleted");
            ProjectHelper.RemoveAllProjectFolder(properties.Web, properties.ListItemId);
            ProjectHelper.RemoveProjectRootTask(properties.Web, properties.ListItemId);
        }

        private void UpdateFolderStrucutre(SPListItem item)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "UpdateFolderStrucutre");
            SPFieldLookupValue dept = new SPFieldLookupValue(Convert.ToString(item[Fields.ProjectDepartment]));
            string type = string.Format(ProjectHelper.LaunchInStoreDateFormat, Convert.ToDateTime(item[SPBuiltInFieldId.TaskDueDate])); 
            string projectFolderName = string.Format("{0}_{1}_{2}_{3}", item.ID, dept.LookupValue, item.Title, type);
            // todo: create project folders in libs
            foreach (string listUrl in ProjectHelper.projectLibrarieUrls)
            {
                CreateFolderStructure(item.Web, projectFolderName, item.ID, listUrl);
            }
        }

        private static void CreateFolderStructure(SPWeb web, string projectFolder, int itemId, string listUrl)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, "CreateFolderStructure", "Start");
            SPList list;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, string.Format("Lists/{0}", listUrl)));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, string.Format("Lists/{0}", listUrl), "List not found");
                return;
            }

            SPListItemCollection items = CommonUtilities.GetFoldersByPrefix(web, list, string.Format("{0}_", itemId));

            //Get the name and Url for the folder 
            if (items.Count > 0)
            {
                SPListItem firstItem = items[0];
                firstItem[SPBuiltInFieldId.FileLeafRef] = projectFolder;
                firstItem.Update();
            }
            else
            {
                SPFolderCollection folderColl = list.RootFolder.SubFolders;

                string folderUrl = projectFolder;
                SPFolder projectFolderObj = folderColl.Add(folderUrl);
            }

            Logger.WriteLog(Logger.Category.Information, "CreateFolderStructure", "End");
        }

        private void CreateProjectTasks(SPListItem item)
        {
            if (item.ContentType.Parent.Id == ContentTypeIds.Project)
            {
                EventFiringEnabled = false;
                string tasksUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
                SPList tasksList = item.Web.GetList(tasksUrl);

                // update project taks link
                string allTaskViewUrl = tasksList.Views["All Tasks"].Url;
                allTaskViewUrl = string.Format("{0}/{1}?FilterField1=ChangeProject&FilterValue1={2}", item.Web.Url, allTaskViewUrl, item.Title);

                SPFieldUrlValue hyper = new SPFieldUrlValue();
                hyper.Description = "Tasks";
                hyper.Url = allTaskViewUrl;
                item[Fields.ChangeProjectTasksLink] = hyper;
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project task lin for id:{0}, title:{1}", item.ID, item.Title));

                item.SystemUpdate();

                // create project plan
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateProjectTasks for id:{0}, title:{1}", item.ID, item.Title));
                DateTime launchInStore = Convert.ToDateTime(item[SPBuiltInFieldId.TaskDueDate]);
                string projectCoordinator = Convert.ToString(item[SPBuiltInFieldId.AssignedTo]);


                SPContentType foundedProjectTaskCT = tasksList.ContentTypes[tasksList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];
                // create root project task
                SPListItem projectTask = tasksList.AddItem();
                projectTask[SPBuiltInFieldId.Title] = item.Title;
                projectTask[SPBuiltInFieldId.ContentTypeId] = foundedProjectTaskCT.Id;
                projectTask[Fields.StoreOpeningTask] = true;
                projectTask[SPBuiltInFieldId.StartDate] = item[SPBuiltInFieldId.StartDate];
                projectTask[SPBuiltInFieldId.TaskDueDate] = item[SPBuiltInFieldId.TaskDueDate];
                projectTask[Fields.Project] = string.Format("{0};#{1}", item.ID, item.Title);
                projectTask[Fields.ProjectDepartment] = item[Fields.ProjectDepartment];
                projectTask[Fields.ChangeTaskDisplayNameId] = item.Title;
                projectTask.Update();
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("created project root task id:{0}, title:{1}", projectTask.ID, projectTask.Title));
                SPFieldLookupValue projectTaskValue = new SPFieldLookupValue(string.Format("{0};#{1}", projectTask.ID, projectTask.Title));


                List<Department> departments = DepartmentUtilities.GetDepartments(item.Web);

                
                DateTime projectStartDate = DateTime.MinValue;
                DateTime projectDueDate = DateTime.MaxValue;

                string customTemplate = string.Empty;
                if (item.ParentList.Fields.ContainsField(Fields.ProjectMgmtTemplate))
                {
                    customTemplate = Convert.ToString(item[Fields.ProjectMgmtTemplate]);
                }

                if (!string.IsNullOrEmpty(customTemplate))
                {
                    SPFieldLookupValue templateValue = new SPFieldLookupValue(customTemplate);
                    if (templateValue.LookupId > 0)
                    {
                        ProjectHelper.ImportProjectTasksTree(item.Web, item, templateValue.LookupId, projectTask.ID, launchInStore, projectCoordinator, departments, foundedProjectTaskCT, ref projectStartDate, ref projectDueDate);
                    }
                }
                

                if (!projectStartDate.Equals(DateTime.MinValue))
                {
                    projectTask[SPBuiltInFieldId.StartDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(projectStartDate);
                    projectTask.Update();

                    item[SPBuiltInFieldId.StartDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(projectStartDate);
                    item.Update();
                }

                if (!projectDueDate.Equals(DateTime.MaxValue))
                {
                    projectTask[SPBuiltInFieldId.TaskDueDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(projectDueDate);
                    projectTask.Update();
                }

                EventFiringEnabled = true;
            }

        }

        private void SendNotification(SPListItem projectItem)
        {
            string projectCoordinator = Convert.ToString(projectItem[SPBuiltInFieldId.AssignedTo]);
            string projectName = projectItem.Title;


            if (!string.IsNullOrEmpty(projectCoordinator))
            {
                SPFieldUserValue user = new SPFieldUserValue(projectItem.Web, projectCoordinator);
                projectCoordinator = user.LoginName;
            }

            SPListItemCollection tasks = ProjectHelper.GetAllProjectTasks(projectItem.Web, projectItem.ID);
            foreach (SPListItem taskItem in tasks)
            {
                string responsible = Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);

                if (!string.IsNullOrEmpty(responsible))
                {
                    SPFieldUserValue user = new SPFieldUserValue(projectItem.Web, responsible);
                    if (!string.IsNullOrEmpty(user.User.Email))
                    {
                        // send notification
                        Logger.WriteLog(Logger.Category.Information, typeof(ProjectEventReceiver).FullName, string.Format("send notification to :{0}", user.User.Email));
                        string subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ListUtilities.ChangeProjectMGMTCreatedMailSubject), "COSIntranet", projectItem.Web.Language);
                        string body = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ListUtilities.ChangeProjectMGMTCreatedMailBody), "COSIntranet", projectItem.Web.Language);

                        subject = string.Format(body, subject);
                        body = string.Format(body, projectName, projectItem.Web.Url, projectCoordinator);

                        CommonUtilities.SendEmail(projectItem.Web, user.User.Email, body, subject);
                    }
                }
            }
        }
    }
}
