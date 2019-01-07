using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Change.Intranet.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Change.Intranet.Features.ChangeListsForHQ
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("938634aa-6e5a-4bbf-8d1e-44b56a089b22")]
    public class ChangeListsForHQEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "add CT and ER");
                AddFieldsCtErToLists(web);

                // add folder strucure
                Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "add folder strucure");
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
            SPList storestList = web.GetList(storesUrl);

            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList tasksList = web.GetList(tasksUrl);

            // add lookups
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "Add lookups");

            SPFieldLookup deptLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Department, "$Resources:COSIntranet,ChangeColDeparment", Fields.Title, deptList, false, false);
            SPFieldLookup taskLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.ProjectTask, "$Resources:COSIntranet,ChangeColProjectTask", Fields.Title, tasksList, false, false);
            SPFieldLookup storeLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Store, "$Resources:COSIntranet,ChangeColStore", Fields.StoreId, storestList, false, false);

            // add ct to lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add ct to lists");
            SPContentType storeContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Store];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", storeContentType.Name, storesUrl));
            CommonUtilities.AttachContentTypeToList(storestList, storeContentType, true, false);

            SPContentType deptContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Department];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", deptContentType.Name, deptUrl));
            SPContentType deptListContentType = CommonUtilities.AttachContentTypeToList(deptList, deptContentType, true, false);
            CommonUtilities.AddFieldToContentType(web, deptListContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColParentdeparment");

            SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Project];
            projectContentType.FieldLinks.Delete(SPBuiltInFieldId.Predecessors);
            projectContentType.FieldLinks[SPBuiltInFieldId.Title].DisplayName = "$Resources:COSIntranet,ChangeProjectTitle";
            projectContentType.FieldLinks[SPBuiltInFieldId.TaskDueDate].DisplayName = "$Resources:COSIntranet,ChangeOpeningDate";
            projectContentType.FieldLinks[SPBuiltInFieldId.AssignedTo].DisplayName = "$Resources:COSIntranet,ChangeProjectCoordinator";
            projectContentType.FieldLinks[SPBuiltInFieldId.TaskStatus].Hidden = true;
            CommonUtilities.AddFieldToContentType(web, projectContentType, storeLookup, true, false, string.Empty);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectContentType.Name, tasksUrl));
            CommonUtilities.AttachContentTypeToList(tasksList, projectContentType, true, true);

            SPContentType projectTaskContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectTask];
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, taskLookup, false, false, "$Resources:COSIntranet,ChangeColParentProject");
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, storeLookup, false, true, string.Empty);
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColResponsibleDepartment");

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectTaskContentType.Name, tasksUrl));
            CommonUtilities.AttachContentTypeToList(tasksList, projectTaskContentType, false, true);

            //add ER to Lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", storesUrl));
            CommonUtilities.AddListEventReceiver(storestList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(storestList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.BussinesDev.StoreListEventReceiver", false);

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
