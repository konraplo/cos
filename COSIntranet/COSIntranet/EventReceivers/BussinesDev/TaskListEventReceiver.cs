namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Change.Intranet.Model;
    using Change.Intranet.Projects;
    using Microsoft.SharePoint;
    using System;
    using System.Collections.Generic;
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
            CreateProjectTasks(properties.ListItem);
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

        private void CreateProjectTasks(SPListItem item)
        {
            if (item.ContentType.Parent.Id == ContentTypeIds.Project)
            {
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateProjectTasks for id:{0}, title:{1}", item.ID, item.Title));
                EventFiringEnabled = false;

                SPContentType foundedProjectTask = item.ParentList.ContentTypes[item.ParentList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];

                List<string> formatedUpdateBatchCommands = new List<string>();
                int counter = 1;
                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(item[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);

                foreach (ProjectTask task in ProjectUtilities.CreateStoreOpeningTasks())
                {
                    StringBuilder batchItemSetVar = new StringBuilder();
                    batchItemSetVar.Append(string.Format(CommonUtilities.BATCH_ITEM_SET_VAR, 
                                                        item.ParentList.Fields[SPBuiltInFieldId.Title].InternalName,
                                                        task.Title));
                    batchItemSetVar.Append(
                            string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                            item.ParentList.Fields[SPBuiltInFieldId.ContentTypeId].InternalName,
                            Convert.ToString(foundedProjectTask.Id)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.ProjectTask,
                           string.Format("{0};#{1}", item.ID, item.Title)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.Store,
                           string.Format("{0};#{1}", store.LookupId, store.LookupValue)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           item.ParentList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                           string.Format("{0};#{1}", item.ID, item.Title)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           item.ParentList.Fields[Fields.ChangeCountryId].InternalName,
                           storeCountry));

                    formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, item.ParentList.ID.ToString(), batchItemSetVar));
                    counter++;
                }

                CommonUtilities.BatchAddListItems(item.Web, formatedUpdateBatchCommands);
                EventFiringEnabled = true;
            }
        }
    }
}
