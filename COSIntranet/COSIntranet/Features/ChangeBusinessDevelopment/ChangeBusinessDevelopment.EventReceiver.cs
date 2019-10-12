namespace Change.Intranet.Features.ChangeBusinessDevelopment
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("313988d9-d9fa-436a-9711-7405c5680067")]
    public class ChangeBusinessDevelopmentEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "add CT and ER");
                AddFieldsCtErToLists(web);

                // add folder strucure
                Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "add folder strucure");
                LogisticLib(web);
                MarketingLib(web);

                Upgradeto12(web);
                Upgradeto13(web);
            }
        }

        private static void LogisticLib(SPWeb web)
        {
            // Logistic
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Start add Logistic");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Logistic"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleLogisticOrderTemplates", "COSIntranet", web.Language);
            SPFolder orderTemplates = folderColl.Add(folderUrl);
           
            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "End add Logistic");
        }

        private static void MarketingLib(SPWeb web)
        {
            // Logistic
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Start add Marketing");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Marketing"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMarketingTemplate", "COSIntranet", web.Language);
            SPFolder orderTemplates = folderColl.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "End add Marketing");
        }

        /// <summary>
        /// Add fields, content types end event receivers to list
        /// </summary>
        /// <param name="web"></param>
        private void AddFieldsCtErToLists(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Find lists");

            string deptUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", deptUrl));
            SPList deptList = web.GetList(deptUrl);

            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", storesUrl));
            SPList storetList = web.GetList(storesUrl);

            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList tasksList = web.GetList(tasksUrl);

            string projectsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.StoreOpenings);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", projectsUrl));
            SPList projectsList = web.GetList(projectsUrl);

            string countriesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Countries);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList countriesList = web.GetList(countriesUrl);

            // add lookups
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Add lookups");

            SPFieldLookup deptLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Department, "$Resources:COSIntranet,ChangeColDeparment", Fields.Title, deptList, false, false);
            SPFieldLookup storeOpeningLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.StoreOpening, "$Resources:COSIntranet,ChangeColStoreOpening", Fields.Title, projectsList, false, false);
            SPFieldLookup storeLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Store, "$Resources:COSIntranet,ChangeColStore", Fields.StoreId, storetList, false, false);
            SPFieldLookup countryLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Country, "$Resources:COSIntranet,ChangeColCountry", Fields.Title, countriesList, false, false);

            // add ct to lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add ct to lists");
            SPContentType storeContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Store];
            CommonUtilities.AddFieldToContentType(web, storeContentType, countryLookup, true, false, string.Empty);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", storeContentType.Name, storesUrl));
            CommonUtilities.AttachContentTypeToList(storetList, storeContentType, true, false);

            SPContentType countryContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Country];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", countryContentType.Name, countriesUrl));
            CommonUtilities.AttachContentTypeToList(countriesList, countryContentType, true, false);

            SPContentType deptContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Department];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", deptContentType.Name, deptUrl));
            SPContentType deptListContentType = CommonUtilities.AttachContentTypeToList(deptList, deptContentType, true, false);
            CommonUtilities.AddFieldToContentType(web, deptListContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColParentdeparment");

            SPContentType storeOpeningContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectStoreOpening];
            storeOpeningContentType.FieldLinks.Delete(SPBuiltInFieldId.Predecessors);
            storeOpeningContentType.FieldLinks[SPBuiltInFieldId.Title].DisplayName = "$Resources:COSIntranet,ChangeProjectTitle";
            storeOpeningContentType.FieldLinks[SPBuiltInFieldId.TaskDueDate].DisplayName = "$Resources:COSIntranet,ChangeOpeningDate";
            storeOpeningContentType.FieldLinks[SPBuiltInFieldId.AssignedTo].DisplayName = "$Resources:COSIntranet,ChangeProjectCoordinator";
            storeOpeningContentType.FieldLinks[SPBuiltInFieldId.StartDate].ReadOnly = true;

            CommonUtilities.AddFieldToContentType(web, storeOpeningContentType, storeLookup, true, false, string.Empty);        
            CommonUtilities.AddFieldToContentType(web, storeOpeningContentType, countryLookup, false, true, string.Empty);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", storeOpeningContentType.Name, projectsUrl));
            CommonUtilities.AttachContentTypeToList(projectsList, storeOpeningContentType, true, false);
            //projectsList.Fields[Fields.ChangeShippingDays].ShowInEditForm = false;
            //projectsList.Fields[Fields.ChangeProjectCategory].ShowInEditForm = false;
            //projectsList.Fields[SPBuiltInFieldId.PercentComplete].ShowInNewForm = false;
            //projectsList.Fields[SPBuiltInFieldId.TaskDueDate].ShowInEditForm = false;
            //projectsList.Update();

            SPContentType projectTaskContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectTask];
            projectTaskContentType.FieldLinks[Fields.ChangeDeparmentmanager].ReadOnly = true;

            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, storeOpeningLookup, true, false, "$Resources:COSIntranet,ChangeColParentProject");
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, storeLookup, false, true, string.Empty);
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColResponsibleDepartment");
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, countryLookup, false, true, string.Empty);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectTaskContentType.Name, tasksUrl));
            CommonUtilities.AttachContentTypeToList(tasksList, projectTaskContentType, true, false);

            //add ER to Lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", storesUrl));
            CommonUtilities.AddListEventReceiver(storetList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(storetList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreListEventReceiver", false);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", deptUrl));
            CommonUtilities.AddListEventReceiver(deptList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.DeptListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(deptList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.DeptListEventReceiver", false);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", tasksUrl));
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.TaskListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.TaskListEventReceiver", false);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", projectsUrl));
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreOpeningEventReceiver", false);
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreOpeningEventReceiver", false);
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemDeleted, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreOpeningEventReceiver", false);

        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            Logger.WriteLog(Logger.Category.Medium, this.GetType().Name, string.Format("upgrading - web:{0}, action:{1}", web.Url, upgradeActionName));

            switch (upgradeActionName)
            {

                case "UpgradeToV1.2":
                    Upgradeto12(web);
                    break;
                case "UpgradeToV1.3":
                    Upgradeto13(web);
                    break;

            }
        }

        private void Upgradeto12(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Medium, this.GetType().Name, string.Format("Upgradeto12 web:{0}", web.Url));
            if (web != null)
            {
                // add project template ct
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add project template ct");
                string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplates);
                SPList projectTemplatesList = web.GetList(projectTemplatesUrl);
                SPContentType projectTemplateContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectTemplate];

                CommonUtilities.AttachContentTypeToList(projectTemplatesList, projectTemplateContentType, true, false);

                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "create/add project template lookup to project ct");
                string projectsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.StoreOpenings);
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add Lookups to:{0}", projectsUrl));
                SPFieldLookup projecttemplateLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.ProjectTemplate, "$Resources:COSIntranet,ChangeColProjectTemplate", Fields.Title, projectTemplatesList, false, false);

                SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectStoreOpening];
                CommonUtilities.AddFieldToContentType(web, projectContentType, projecttemplateLookup, false, false, string.Empty);
            }

            Logger.WriteLog(Logger.Category.Medium, "Upgradeto12 finished", string.Format("web:{0}", web.Url));
        }

        private void Upgradeto13(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Medium, this.GetType().Name, string.Format("Upgradeto13 web:{0}", web.Url));
            if (web != null)
            {
                // add project tasks link to project ct
                SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectStoreOpening];
                SPField projectTasksLink = web.Site.RootWeb.Fields[Fields.ChangeProjectTasksLink];
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add fild:{0} to ct:{1}", projectTasksLink.Title, projectContentType.Name));
                CommonUtilities.AddFieldToContentType(web, projectContentType, projectTasksLink, false, true, string.Empty);
            }

            Logger.WriteLog(Logger.Category.Medium, "Upgradeto13 finished", string.Format("web:{0}", web.Url));
        }

    }
}
