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
    /// Event receivers for store openings list
    /// </summary>
    public class StoreOpeningEventReceiver : SPItemEventReceiver
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

        private void CreateProjectTasks(SPListItem item)
        {
            if (item.ContentType.Parent.Id == ContentTypeIds.Project)
            {
                EventFiringEnabled = false;

                // update project country
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("update project store for id:{0}, title:{1}", item.ID, item.Title));
                SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(item[Fields.Store]));
                string storeCountry = ProjectUtilities.GetStoreCountry(item.Web, store.LookupId);
                item[Fields.Country] = storeCountry;
                item.Update();

                // create project plan
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateProjectTasks for id:{0}, title:{1}", item.ID, item.Title));
                DateTime grandOpening = Convert.ToDateTime(item[SPBuiltInFieldId.TaskDueDate]);

                string tasksUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
                SPList tasksList = item.Web.GetList(tasksUrl);
                SPContentType foundedProjectTask = tasksList.ContentTypes[tasksList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];

                string deptUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
                SPList deptList = item.Web.GetList(deptUrl);
                List<Department> departments = new List<Department>();
                foreach (SPListItem deptIem in deptList.GetItems(new SPQuery()))
                {
                    departments.Add(new Department { Id = deptIem.ID, Title = deptIem.Title, Manager = Convert.ToString(deptIem[Fields.ChangeDeparmentmanager])  });
                }

                List<string> formatedUpdateBatchCommands = new List<string>();
                int counter = 1;

                // add grand opening task
                SPListItem gradOpeningTask = tasksList.AddItem();
                gradOpeningTask[SPBuiltInFieldId.ContentTypeId] = foundedProjectTask.Id;
                gradOpeningTask[SPBuiltInFieldId.TaskDueDate] = grandOpening;
                gradOpeningTask[SPBuiltInFieldId.StartDate] = grandOpening;
                gradOpeningTask[SPBuiltInFieldId.Title] = ProjectUtilities.GrandOpening.Title;
                gradOpeningTask[Fields.StoreOpening] = string.Format("{0};#{1}", item.ID, item.Title);
                gradOpeningTask[Fields.Store] = string.Format("{0};#{1}", store.LookupId, store.LookupValue);
                gradOpeningTask[Fields.ChangeTaskDurationId] = ProjectUtilities.GrandOpening.Duration;
                gradOpeningTask[Fields.Country] = storeCountry;
                gradOpeningTask.Update();

                //foreach (ProjectTask task in ProjectUtilities.CreateStoreOpeningTasks())
                //{
                //    StringBuilder batchItemSetVar = new StringBuilder();
                //    batchItemSetVar.Append(string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //                                        item.ParentList.Fields[SPBuiltInFieldId.Title].InternalName,
                //                                        task.Title));
                //    batchItemSetVar.Append(
                //            string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //            item.ParentList.Fields[SPBuiltInFieldId.ContentTypeId].InternalName,
                //            Convert.ToString(foundedProjectTask.Id)));
                //    batchItemSetVar.Append(
                //           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //           Fields.StoreOpening,
                //           string.Format("{0};#{1}", item.ID, item.Title)));
                //    batchItemSetVar.Append(
                //           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //           Fields.Store,
                //           string.Format("{0};#{1}", store.LookupId, store.LookupValue)));

                //    if (!string.IsNullOrEmpty(task.ResponsibleDepartment))
                //    {
                //        Department responsibleDepartment = departments.FirstOrDefault(x => x.Title.Equals(task.ResponsibleDepartment));
                //        batchItemSetVar.Append(
                //           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //           Fields.Department,
                //           string.Format("{0};#{1}", responsibleDepartment.Id, responsibleDepartment.Title)));
                //        batchItemSetVar.Append(
                //          string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //          tasksList.Fields[Fields.ChangeDeparmentmanager].InternalName,
                //          responsibleDepartment.Manager));
                //    }

                //    //batchItemSetVar.Append(
                //    //       string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //    //       item.ParentList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                //    //       string.Format("{0};#{1}", item.ID, item.Title)));
                //    batchItemSetVar.Append(
                //           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //           Fields.Country,
                //           storeCountry));
                //    batchItemSetVar.Append(
                //           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                //           tasksList.Fields[Fields.ChangeTaskDurationId].InternalName,
                //           task.Duration));
                //    formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, tasksList.ID.ToString(), batchItemSetVar));
                //    counter++;
                //}

                //string result = CommonUtilities.BatchAddListItems(item.Web, formatedUpdateBatchCommands);
                EventFiringEnabled = true;
            }
          
        }
    }
}
