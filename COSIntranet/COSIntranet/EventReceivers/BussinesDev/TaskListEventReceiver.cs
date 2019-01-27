namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Change.Intranet.Model;
    using Change.Intranet.Projects;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
        }

        private void SetLocalization(SPListItem item)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("SetLocalization for id:{0}, ct:{1}", item.ID, item.ContentType.Name));
            if (item.ContentType.Parent.Id == ContentTypeIds.Project)
            {
                EventFiringEnabled = false;

                EventFiringEnabled = true;
            }
            else if (item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
            {
                EventFiringEnabled = false;
                EventFiringEnabled = true;
            }
        }

        private void UpdateProjectTaskInforamtions(SPListItem item)
        {
            if (item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
            {
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project store/country for id:{0}, title:{1}", item.ID, item.Title));
                SPFieldLookupValue project = new SPFieldLookupValue(Convert.ToString(item[Fields.StoreOpening]));
                SPListItem projectItem = item.ParentList.GetItemById(project.LookupId);
                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(projectItem[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);
                item[Fields.Country] = storeCountry;
                item[Fields.Store] = store;
                item.Update();
            }
        }
    }
}
