﻿namespace Change.Intranet.EventReceivers.BussinesDev
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
                EventFiringEnabled = false;

               // update project store
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project store for id:{0}, title:{1}", item.ID, item.Title));
                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(item[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);
                item[Fields.Country] = storeCountry;
                item.Update();

                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateProjectTasks for id:{0}, title:{1}", item.ID, item.Title));
                SPContentType foundedProjectTask = item.ParentList.ContentTypes[item.ParentList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];

                string deptUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
                SPList deptList = item.Web.GetList(deptUrl);
                List<Department> departments = new List<Department>();
                foreach (SPListItem deptIem in deptList.GetItems(new SPQuery()))
                {
                    departments.Add(new Department { Id = deptIem.ID, Title = deptIem.Title });
                    //item.Web.EnsureUser();
                }

                List<string> formatedUpdateBatchCommands = new List<string>();
                int counter = 1;

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

                    if (!string.IsNullOrEmpty(task.ResponsibleDepartment))
                    {
                        Department responsibleDepartment = departments.FirstOrDefault(x => x.Title.Equals(task.ResponsibleDepartment));
                        batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.Department,
                           string.Format("{0};#{1}", responsibleDepartment.Id, responsibleDepartment.Title)));
                    }

                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           item.ParentList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                           string.Format("{0};#{1}", item.ID, item.Title)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.Country,
                           string.Format("{0};#{1}", store.LookupId, store.LookupValue)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           item.ParentList.Fields[Fields.ChangeTaskDurationId].InternalName,
                           task.Duration));
                    formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, item.ParentList.ID.ToString(), batchItemSetVar));
                    counter++;
                }

                CommonUtilities.BatchAddListItems(item.Web, formatedUpdateBatchCommands);
                EventFiringEnabled = true;
            }
            else if (item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
            {
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project store/country for id:{0}, title:{1}", item.ID, item.Title));
                SPFieldLookupValue project = new SPFieldLookupValue(Convert.ToString(item[Fields.ProjectTask]));
                SPListItem storeItem = item.ParentList.GetItemById(project.LookupId);
                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(storeItem[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);
                item[Fields.Country] = storeCountry;
                item[Fields.Store] = storeItem[Fields.Store];
                item[SPBuiltInFieldId.ParentID] = string.Format("{0};#{1}", project.LookupId, project.LookupValue);
                item.Update();
            }
        }
    }
}
