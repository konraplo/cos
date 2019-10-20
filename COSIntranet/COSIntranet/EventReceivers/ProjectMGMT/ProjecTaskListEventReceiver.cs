namespace Change.Intranet.EventReceivers.ProjectMGMT
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;

    /// <summary>
    /// Event receivers for CHANGE task list
    /// </summary>
    public class ProjecTaskListEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
            UpdateProjectTaskInforamtions(properties.ListItem);
        }

        /// <summary>
        /// Ein Element wurde aktualisiert..
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemUpdated");
            UpdateProjectTaskInforamtions(properties.ListItem);
        }

        private void UpdateProjectTaskInforamtions(SPListItem item)
        {
            try
            {
                if (item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
                {
                    Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project dept mgr for id:{0}, title:{1}", item.ID, item.Title));
                    EventFiringEnabled = false;
                    SPFieldLookupValue project = new SPFieldLookupValue(Convert.ToString(item[Fields.Project]));
                    SPFieldLookupValue dept = new SPFieldLookupValue(Convert.ToString(item[Fields.ProjectDepartment]));

                    SPFieldLookupValue parent = new SPFieldLookupValue(Convert.ToString(item[SPBuiltInFieldId.ParentID]));
                    if (parent.LookupId > 0)
                    {
                        SPListItem parentItem = item.ParentList.GetItemById(parent.LookupId);
                        project = new SPFieldLookupValue(Convert.ToString(parentItem[Fields.Project]));
                        dept = new SPFieldLookupValue(Convert.ToString(parentItem[Fields.ProjectDepartment]));
                    }
                    else if (!Convert.ToBoolean(item[Fields.StoreOpeningTask]))
                    {
                        SPListItem projectRootTask = ProjectHelper.GetProjectRootTask(item.ParentList, project.LookupId);
                        if (projectRootTask != null)
                        {
                            item[SPBuiltInFieldId.ParentID] = projectRootTask.ID;
                        }
                    }

                    if (dept.LookupId > 0)
                    {
                        string deptUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
                        SPList deptList = item.Web.GetList(deptUrl);
                        SPListItem deptItem = deptList.GetItemById(dept.LookupId);
                        item[Fields.ChangeDeparmentmanager] = deptItem[Fields.ChangeDeparmentmanager];
                        item[Fields.ProjectDepartment] = dept;
                    }

                    item[Fields.ChangeTaskDisplayNameId] = string.Format("({0}) {1}", project.LookupValue, item.Title);
                    item.Update();
                }
            }
            finally
            {
                EventFiringEnabled = true;
            }

        }

    }
}
