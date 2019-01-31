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
            }
        }

        /// <summary>
        /// Add fields, content types end event receivers to list
        /// </summary>
        /// <param name="web"></param>
        private void AddFieldsCtErToLists(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "Find lists");

            string deptUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", deptUrl));
            SPList deptList = web.GetList(deptUrl);

            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", storesUrl));
            SPList storetList = web.GetList(storesUrl);

            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList tasksList = web.GetList(tasksUrl);

            string projectsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.StoreOpenings);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", projectsUrl));
            SPList projectsList = web.GetList(projectsUrl);

            string countriesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Countries);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList countriesList = web.GetList(countriesUrl);

            // add lookups
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "Add lookups");

            SPFieldLookup deptLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Department, "$Resources:COSIntranet,ChangeColDeparment", Fields.Title, deptList, false, false);
            SPFieldLookup storeOpeningLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.StoreOpening, "$Resources:COSIntranet,ChangeColStoreOpening", Fields.Title, tasksList, false, false);
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

            SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Project];
            projectContentType.FieldLinks.Delete(SPBuiltInFieldId.Predecessors);
            projectContentType.FieldLinks[SPBuiltInFieldId.Title].DisplayName = "$Resources:COSIntranet,ChangeProjectTitle";
            projectContentType.FieldLinks[SPBuiltInFieldId.TaskDueDate].DisplayName = "$Resources:COSIntranet,ChangeOpeningDate";
            projectContentType.FieldLinks[SPBuiltInFieldId.AssignedTo].DisplayName = "$Resources:COSIntranet,ChangeProjectCoordinator";
            CommonUtilities.AddFieldToContentType(web, projectContentType, storeLookup, true, false, string.Empty);

            CommonUtilities.AddFieldToContentType(web, projectContentType, countryLookup, false, true, string.Empty);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectContentType.Name, projectsUrl));
            CommonUtilities.AttachContentTypeToList(projectsList, projectContentType, true, false);

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

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", deptUrl));
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.TaskListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.TaskListEventReceiver", false);
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

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
