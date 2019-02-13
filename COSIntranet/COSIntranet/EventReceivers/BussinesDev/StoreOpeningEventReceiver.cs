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
                SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(item.Web, store.LookupId));
                item[Fields.Country] = storeCountry;
                item.Update();

                // create project plan
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateProjectTasks for id:{0}, title:{1}", item.ID, item.Title));
                DateTime grandOpening = Convert.ToDateTime(item[SPBuiltInFieldId.TaskDueDate]);
                string storeMgr = ProjectUtilities.GetStoreManager(item.Web, store.LookupId);
                string projectCoordinator = Convert.ToString(item[SPBuiltInFieldId.AssignedTo]);

                string tasksUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
                SPList tasksList = item.Web.GetList(tasksUrl);
                SPContentType foundedProjectTask = tasksList.ContentTypes[tasksList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];

                SPListItem whiteBoxHandover = tasksList.AddItem();
                whiteBoxHandover[SPBuiltInFieldId.Title] = "White box handover";
                whiteBoxHandover[SPBuiltInFieldId.ContentTypeId] = foundedProjectTask.Id;
                List<ProjectTask> whiteBoxHandoverTasks = ProjectUtilities.WhiteBoxHandoverTasks(whiteBoxHandover.ID, whiteBoxHandover.Title);
                DateTime dueDate = DateTime.MaxValue;
                DateTime startDate = DateTime.MinValue;
                foreach (ProjectTask task in whiteBoxHandoverTasks.OrderByDescending(x => x.TimeBeforeGrandOpening))
                {
                    if (dueDate.Equals(DateTime.MaxValue))
                    {
                        dueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                    }
                    else if(DateTime.Compare(dueDate, grandOpening.AddDays(-task.TimeBeforeGrandOpening)) < 0)
                    {
                        dueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                    }

                    if (startDate.Equals(DateTime.MinValue))
                    {
                        startDate = dueDate.AddDays(-task.Duration);
                    }
                    else if (DateTime.Compare(startDate, dueDate.AddDays(-task.Duration)) > 0)
                    {
                        startDate = dueDate.AddDays(-task.Duration);
                    }
                }

                whiteBoxHandover[SPBuiltInFieldId.StartDate] = startDate;
                whiteBoxHandover[SPBuiltInFieldId.TaskDueDate] = dueDate;
                whiteBoxHandover[Fields.ChangeTaskDurationId] = (dueDate - startDate).TotalDays;
                whiteBoxHandover[Fields.Country] = storeCountry;
                whiteBoxHandover[Fields.StoreOpening] = string.Format("{0};#{1}", item.ID, item.Title);
                whiteBoxHandover[Fields.Store] = string.Format("{0};#{1}", store.LookupId, store.LookupValue);
                whiteBoxHandover.Update();

                List<Department> departments = DepartmentUtilities.GetDepartments(item.Web);

                string countryUrl = SPUrlUtility.CombineUrl(item.Web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Countries);
                SPList countryList = item.Web.GetList(countryUrl);
                List<Country> regions = new List<Country>();
                foreach (SPListItem regionIem in countryList.GetItems(new SPQuery()))
                {
                    regions.Add(new Country { Id = regionIem.ID, Title = regionIem.Title, Manager = Convert.ToString(regionIem[Fields.ChangeCountrymanager]) });
                }

                List<string> formatedUpdateBatchCommands = new List<string>();
                int counter = 1;

                foreach (ProjectTask task in ProjectUtilities.CreateStoreOpeningTasks().Union(whiteBoxHandoverTasks).OrderByDescending(x => x.TimeBeforeGrandOpening))
                {
                    dueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                    startDate = dueDate.AddDays(-task.Duration);

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
                           Fields.StoreOpening,
                           string.Format("{0};#{1}", item.ID, item.Title)));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.Store,
                           string.Format("{0};#{1}", store.LookupId, store.LookupValue)));

                    if (!string.IsNullOrEmpty(task.ResponsibleDepartment))
                    {
                        Department responsibleDepartment = departments.FirstOrDefault(x => x.Title.Equals(task.ResponsibleDepartment));
                        if (responsibleDepartment != null)
                        {
                            batchItemSetVar.Append(
                              string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                              Fields.Department,
                              string.Format("{0};#{1}", responsibleDepartment.Id, responsibleDepartment.Title)));
                            batchItemSetVar.Append(
                              string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                              tasksList.Fields[Fields.ChangeDeparmentmanager].InternalName,
                              responsibleDepartment.Manager));
                        }
                    }

                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           Fields.Country,
                           storeCountry));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           tasksList.Fields[Fields.ChangeTaskDurationId].InternalName,
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
                           item.ParentList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                           string.Format("{0};#{1}", task.ParentId, task.ParentTitle)));
                    }
                    
                    formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, tasksList.ID.ToString(), batchItemSetVar));
                    counter++;
                }

                string result = CommonUtilities.BatchAddListItems(item.Web, formatedUpdateBatchCommands);
                EventFiringEnabled = true;
            }

        }


    }
}
