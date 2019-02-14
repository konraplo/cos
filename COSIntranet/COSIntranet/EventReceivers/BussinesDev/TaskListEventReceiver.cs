namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Change.Intranet.Projects;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;

    /// <summary>
    /// Event receivers for CHANGE task list
    /// </summary>
    public class TaskListEventReceiver : SPItemEventReceiver
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
            if (item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
            {
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project store/country/dept mgr for id:{0}, title:{1}", item.ID, item.Title));
                EventFiringEnabled = false;
                SPFieldLookupValue project = new SPFieldLookupValue(Convert.ToString(item[Fields.StoreOpening]));
                SPFieldLookupValue dept = new SPFieldLookupValue(Convert.ToString(item[Fields.Department]));

                SPFieldLookupValue parent = new SPFieldLookupValue(Convert.ToString(item[SPBuiltInFieldId.ParentID]));
                if (parent.LookupId > 0)
                {
                    SPListItem parentItem = item.ParentList.GetItemById(parent.LookupId);
                    project = new SPFieldLookupValue(Convert.ToString(parentItem[Fields.StoreOpening]));
                    dept = new SPFieldLookupValue(Convert.ToString(parentItem[Fields.Department]));
                }

                string projectsUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.StoreOpenings);
                SPList projectsList = item.Web.GetList(projectsUrl);
                SPListItem projectItem = projectsList.GetItemById(project.LookupId);

                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(projectItem[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);

                if (dept.LookupId > 0)
                {
                    string deptUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
                    SPList deptList = item.Web.GetList(deptUrl);
                    SPListItem deptItem = deptList.GetItemById(dept.LookupId);
                    item[Fields.ChangeDeparmentmanager] = deptItem[Fields.ChangeDeparmentmanager]; 
                }

                item[Fields.Country] = storeCountry;
                item[Fields.Store] = store;
                item.Update();
                EventFiringEnabled = true;
            }
        }
    }
}
