namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Change.Intranet.Model;
    using Change.Intranet.Projects;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Event receivers for store openings list
    /// </summary>
    public class StoreOpeningEventReceiver : SPItemEventReceiver
    {
        private const string GrandOpeningDateFormat = "{0:MMMM dd, yyyy}";
        private const string DeliveryDateFormat = "{0:dd-MM-yyyy}";
        public delegate List<ProjectTask> CreateProjectTasksList(int parentTaskId, string parentTitle, int shippingDays);

        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
            CreateProjectTasks(properties.ListItem);
            this.UpdateFolderStrucutre(properties.ListItem);
            this.SendNotification(properties.ListItem);
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
        }

        private void SendNotification(SPListItem storeOpeningItem)
        {
            string projectCoordinator = Convert.ToString(storeOpeningItem[SPBuiltInFieldId.AssignedTo]);
            string projectName = storeOpeningItem.Title;
            Logger.WriteLog(Logger.Category.Information, typeof(StoreOpeningEventReceiver).FullName, string.Format("project:{0}, owner:{1}", projectName, projectCoordinator));
            if (!string.IsNullOrEmpty(projectCoordinator))
            {
                SPFieldUserValue user = new SPFieldUserValue(storeOpeningItem.Web, projectCoordinator);
                if (!string.IsNullOrEmpty(user.User.Email))
                {
                    // send reminder
                    Logger.WriteLog(Logger.Category.Information, typeof(StoreOpeningEventReceiver).FullName, string.Format("send reminder to :{0}", user.User.Email));
                    string subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ListUtilities.ChangeProjectCreatedMailSubject), "COSIntranet", storeOpeningItem.Web.Language);
                    string body = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ListUtilities.ChangeProjectCreatedMailBody), "COSIntranet", storeOpeningItem.Web.Language);
                    string category = Convert.ToString(storeOpeningItem[Fields.ChangeProjectCategory]);
                    subject = string.Format(subject, category, projectName);

                    DateTime grandOpening = Convert.ToDateTime(storeOpeningItem[SPBuiltInFieldId.TaskDueDate]);
                    DateTime firstDelivery = grandOpening.AddDays(-13);
                    DateTime secondDelivery = grandOpening.AddDays(-7);
                    body = string.Format(body, projectName, string.Format(GrandOpeningDateFormat, grandOpening), string.Format(DeliveryDateFormat, firstDelivery), string.Format(DeliveryDateFormat, secondDelivery));

                    CommonUtilities.SendEmail(storeOpeningItem.Web, user.User.Email, body, subject);
                }
            }
        }

        private void UpdateFolderStrucutre(SPListItem item)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "UpdateFolderStrucutre");
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(item[Fields.Store]));
            SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(item.Web, store.LookupId));
            string type = Convert.ToString(item[Fields.ChangeProjectCategory]);
            string projectFolderName = string.Format("{0}_{1}_{2}_{3}", item.ID, store.LookupValue, storeCountry.LookupValue, type);
            UpdateFolderStrucutreMarketingLib(item.Web, projectFolderName, item.ID);
            UpdateFolderStrucutreDrawingsLib(item.Web, projectFolderName, item.ID);
            UpdateFolderStrucutreGeneralInformationLib(item.Web, projectFolderName, item.ID);
            UpdateFolderStrucutreLogisticLib(item.Web, projectFolderName, item.ID);
            UpdateFolderStrucutrePicturesLib(item.Web, projectFolderName, item.ID);
            UpdateFolderStrucutreEvaluationLib(item.Web, projectFolderName, item.ID);
        }

        
        private static void UpdateFolderStrucutreMarketingLib(SPWeb web, string projectFolder,int itemId)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "Start update Marketing");
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Marketing"));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "UpdateFolderStrucutreMarketingLib", "List not found");
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

                string fromMarketingToPartnerFolderUrl = "From Marketing to partner";
                SPFolder fromMarketingToPartner = projectFolderObj.SubFolders.Add(fromMarketingToPartnerFolderUrl);
                folderUrl = "Center Channels";
                fromMarketingToPartner.SubFolders.Add(folderUrl);
                folderUrl = "Own Channels";
                fromMarketingToPartner.SubFolders.Add(folderUrl);
                folderUrl = "External Channels";
                fromMarketingToPartner.SubFolders.Add(folderUrl);

                string fromPartnerToMarketingFolderUrl = "From partner to Marketing";
                SPFolder fromPartnerToMarketing = projectFolderObj.SubFolders.Add(fromPartnerToMarketingFolderUrl);
                folderUrl = "Center information";
                fromPartnerToMarketing.SubFolders.Add(folderUrl);
                list.Update();

                string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\MarketingTemplates");
                string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketin_order.xlsx".TrimStart('\\'));
                string trargetFolderRelativeUrl = string.Format(@"{0}/{1}", projectFolder, fromPartnerToMarketingFolderUrl); 
                AddFileToLibrary(list, trargetFolderRelativeUrl, docPath);
                docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_Timeline.xlsx".TrimStart('\\'));
                AddFileToLibrary(list, trargetFolderRelativeUrl, docPath);
                docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_overview.xlsx".TrimStart('\\'));
                AddFileToLibrary(list, trargetFolderRelativeUrl, docPath);
            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "End update Marketing");
        }

        private static void AddFileToLibrary(SPList list, string fromMarketingToPartnerFolderUrl, string docPath)
        {
            if (File.Exists(docPath))
            {
                Logger.WriteLog(Logger.Category.Information, "AddFileToLibrary", string.Format("library:{0} folder:{1} file{2}", list.Title, fromMarketingToPartnerFolderUrl, docPath));

                string fileName = Path.GetFileName(docPath);
                FileStream stream = new FileStream(docPath, FileMode.Open, FileAccess.Read);
                BinaryReader myReader = new BinaryReader(stream);
                byte[] content = myReader.ReadBytes((int)stream.Length);
                myReader.Close();
                stream.Close();

                CommonUtilities.AddDocumentToLibrary((SPDocumentLibrary)list, fromMarketingToPartnerFolderUrl, content, fileName, new Hashtable());
            }
        }

        private static void UpdateFolderStrucutreDrawingsLib(SPWeb web, string projectFolder, int itemId)
        {
            // Drawings
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "Start update Drawings");
           
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Drawings"));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "UpdateFolderStrucutreMarketingLib", "List not found");
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
                folderUrl = "Center before opening";
                projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Recieved from center";
                projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Final - PDF";
                SPFolder finalPDF = projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Final - CAD";
                projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Not approved";
                finalPDF.SubFolders.Add(folderUrl);

                list.Update();
            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "End update Drawings");
        }

        private static void UpdateFolderStrucutreGeneralInformationLib(SPWeb web, string projectFolder, int itemId)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "Start update GeneralInformation");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/GeneralInformation"));
            if (list == null)
            {
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
                string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\GeneralInformationTemplates");
                list.Update();

                string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Costruction_Scope_of_Work.docx".TrimStart('\\'));
                AddFileToLibrary(list, folderUrl, docPath);
                docPath = string.Format(@"{0}\{1}", rootDirectory, @"Frontpage.xlsx".TrimStart('\\'));
                AddFileToLibrary(list, folderUrl, docPath);

            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "End update GeneralInformation");
        }

        private static void UpdateFolderStrucutreLogisticLib(SPWeb web, string projectFolder, int itemId)
        {
            // Logistic
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "Start update Logistic");
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Logistic"));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "UpdateFolderStrucutreLogisticLib", "List not found");
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
                folderUrl = "Order";
                projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Order confirmation";
                projectFolderObj.SubFolders.Add(folderUrl);

                list.Update();
            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "End update Logistic");
        }

        private static void UpdateFolderStrucutrePicturesLib(SPWeb web, string projectFolder, int itemId)
        {
            // Pictures
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "Start update Pictures");
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Pictures"));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "UpdateFolderStrucutrePicturesLib", "List not found");
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
                folderUrl = "From Warehouse";
                projectFolderObj.SubFolders.Add(folderUrl);
                folderUrl = "Center, after opening";
                projectFolderObj.SubFolders.Add(folderUrl);

                list.Update();
            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "End update Pictures");
        }

        private static void UpdateFolderStrucutreEvaluationLib(SPWeb web, string projectFolder, int itemId)
        {
            // Evaluation
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "Start update Evaluation");
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Evaluation"));
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "UpdateFolderStrucutreEvaluationLib", "List not found");
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

                list.Update();

                string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\EvaluationTemplates");

                string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_contractor.doc".TrimStart('\\'));
                AddFileToLibrary(list, folderUrl, docPath);
                docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_departments.xls".TrimStart('\\'));
                AddFileToLibrary(list, folderUrl, docPath);
            }


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "End update Evaluation");
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
                SPContentType foundedProjectTaskCT = tasksList.ContentTypes[tasksList.ContentTypes.BestMatch(ContentTypeIds.ProjectTask)];

                // create store opening task
                SPListItem projectTask = tasksList.AddItem();
                projectTask[SPBuiltInFieldId.Title] = item.Title;
                projectTask[SPBuiltInFieldId.ContentTypeId] = foundedProjectTaskCT.Id;
                projectTask[Fields.Country] = storeCountry;
                projectTask[Fields.StoreOpeningTask] = true;
                projectTask[SPBuiltInFieldId.StartDate] = item[SPBuiltInFieldId.StartDate];
                projectTask[SPBuiltInFieldId.TaskDueDate] = item[SPBuiltInFieldId.TaskDueDate];
                projectTask[Fields.StoreOpening] = string.Format("{0};#{1}", item.ID, item.Title);
                projectTask[Fields.Store] = string.Format("{0};#{1}", store.LookupId, store.LookupValue);
                projectTask[Fields.ChangeTaskDisplayNameId] = item.Title;
                projectTask.Update();
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("created store opening task id:{0}, title:{1}", projectTask.ID, projectTask.Title));
                SPFieldLookupValue projectTaskValue = new SPFieldLookupValue(string.Format("{0};#{1}", projectTask.ID, projectTask.Title));

                int shippingDays = Convert.ToInt32(item[Fields.ChangeShippingDays]);
                List<ProjectTask> logistikTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.LogistikTasks, "Logistik", shippingDays) ;
                List<ProjectTask> purchaseBathroomKitchenTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.PurchaseBathroomKitchenTasks, "Purchase, bathroom & Kitchen", shippingDays);
                List<ProjectTask> purchaseCleaningTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.PurchaseCleaningTasks, "Purchase, Cleaning", shippingDays);
                List<ProjectTask> purchaseOfficeEquipmentTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.PurchaseOfficeEquipmentTasks, "Purchase, office equipment", shippingDays);
                List<ProjectTask> projectPreperationTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.ProjectPreperationTasks, "Project preperation", shippingDays);
                List<ProjectTask> administrationTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.AdministrationTasks, "Administration", shippingDays);
                List<ProjectTask> preperationOfStoreTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.PreperationOfStoreTasks, "Preperation of store", shippingDays);
                List<ProjectTask> rebuildingPeriodBuilingTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.RebuildingPeriodBuilingTasks, "Rebuilding period - builing", shippingDays);
                List<ProjectTask> rebuildingPeriodDemolitionTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.RebuildingPeriodDemolitionTasks, "Rebuilding period - demolition", shippingDays);
                List<ProjectTask> rebuildingPeriodElectricityTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.RebuildingPeriodElectricityTasks, "Rebuilding period - Electricity", shippingDays);
                List<ProjectTask> rebuildingPeriodMountingTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.RebuildingPeriodMountingTasks, "Rebuilding period - Mounting", shippingDays);
                List<ProjectTask> storePreperationTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.StorePreperationTasks, "Store preperation", shippingDays);
                List<ProjectTask> postGrandOpeningTasks = CreateSubTasks(item, projectTaskValue, store, storeCountry, grandOpening, tasksList, foundedProjectTaskCT, ProjectUtilities.PostGrandOpeningTasks, "Post Grand opening", shippingDays);

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
                DateTime projectStartDate = DateTime.MinValue;
                DateTime projectDueDate = DateTime.MaxValue;

                foreach (ProjectTask task in ProjectUtilities.CreateMilestoneTasks(projectTask.ID).
                                             Union(logistikTasks).
                                             Union(administrationTasks).
                                             Union(projectPreperationTasks).
                                             Union(preperationOfStoreTasks).
                                             Union(purchaseBathroomKitchenTasks).
                                             Union(purchaseCleaningTasks).
                                             Union(purchaseOfficeEquipmentTasks).
                                             Union(rebuildingPeriodBuilingTasks).
                                             Union(rebuildingPeriodDemolitionTasks).
                                             Union(rebuildingPeriodElectricityTasks).
                                             Union(rebuildingPeriodMountingTasks).
                                             Union(storePreperationTasks).
                                             Union(postGrandOpeningTasks).OrderByDescending(x => x.TimeBeforeGrandOpening))
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
                                                        item.ParentList.Fields[SPBuiltInFieldId.Title].InternalName,
                                                        task.Title));
                    batchItemSetVar.Append(
                           string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                           tasksList.Fields[Fields.ChangeTaskDisplayNameId].InternalName,
                           string.Format("({0}) {1}", item.Title, task.Title)));

                    batchItemSetVar.Append(
                            string.Format(CommonUtilities.BATCH_ITEM_SET_VAR,
                            item.ParentList.Fields[SPBuiltInFieldId.ContentTypeId].InternalName,
                            Convert.ToString(foundedProjectTaskCT.Id)));
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
                            if (responsibleDepartment.Title.Equals(DepartmentUtilities.Retail))
                            {
                                task.Responsible = DepartmentUtilities.RegionalManager;
                            }
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
                           tasksList.Fields[SPBuiltInFieldId.ParentID].InternalName,
                           string.Format("{0};#{1}", task.ParentId, task.ParentTitle)));
                    }

                    formatedUpdateBatchCommands.Add(string.Format(CommonUtilities.BATCH_ADD_ITEM_CMD, counter, tasksList.ID.ToString(), batchItemSetVar));
                    counter++;
                }

                string result = CommonUtilities.BatchAddListItems(item.Web, formatedUpdateBatchCommands);

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

        /// <summary>
        /// Create grouping task with sub task list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="store"></param>
        /// <param name="storeCountry"></param>
        /// <param name="grandOpening"></param>
        /// <param name="tasksList"></param>
        /// <param name="foundedProjectTask"></param>
        /// <param name="subTasks"></param>
        /// <param name="mainTaskTitle"></param>
        /// <returns></returns>        
        private List<ProjectTask> CreateSubTasks(SPListItem item, SPFieldLookupValue projectTaskValue, SPFieldLookupValue store, SPFieldLookupValue storeCountry, DateTime grandOpening, SPList tasksList, SPContentType foundedProjectTask, CreateProjectTasksList subTasks, string mainTaskTitle, int shippingDays)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("CreateSubTasks project:{0}, parent title:{1}", projectTaskValue.LookupValue, mainTaskTitle));
            SPListItem projectTask = tasksList.AddItem();
            projectTask[SPBuiltInFieldId.Title] = mainTaskTitle;
            projectTask[SPBuiltInFieldId.ContentTypeId] = foundedProjectTask.Id;
            projectTask[Fields.Country] = storeCountry;
            projectTask[Fields.StoreOpening] = string.Format("{0};#{1}", item.ID, item.Title);
            projectTask[Fields.Store] = string.Format("{0};#{1}", store.LookupId, store.LookupValue);
            projectTask[SPBuiltInFieldId.ParentID] = projectTaskValue;
            projectTask[Fields.ChangeTaskDisplayNameId] = string.Format("({0}) {1}", item.Title, mainTaskTitle);
            projectTask.Update();

            // compute time period
            List<ProjectTask> subTasksList = subTasks(projectTask.ID, projectTask.Title, shippingDays);
            DateTime dueDate = DateTime.MaxValue;
            DateTime startDate = DateTime.MinValue;
            foreach (ProjectTask task in subTasksList.OrderByDescending(x => x.TimeBeforeGrandOpening))
            {
                if (dueDate.Equals(DateTime.MaxValue))
                {
                    dueDate = grandOpening.AddDays(-task.TimeBeforeGrandOpening);
                }
                else if (DateTime.Compare(dueDate, grandOpening.AddDays(-task.TimeBeforeGrandOpening)) < 0)
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

            projectTask[SPBuiltInFieldId.StartDate] = startDate;
            projectTask[SPBuiltInFieldId.TaskDueDate] = dueDate;
            projectTask[Fields.ChangeTaskDurationId] = (dueDate - startDate).TotalDays;

            projectTask.Update();

            return subTasksList;
        }
     }
}
