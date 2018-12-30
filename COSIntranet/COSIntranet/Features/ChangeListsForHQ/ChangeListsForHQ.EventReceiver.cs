using System;
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
                // add folder strucure
                Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "add folder strucure");
              
            }
        }

        private void AddFieldsAndCTToLists(SPWeb web)
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
            SPContentType storeContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Store];
            CommonUtilities.AttachContentTypeToList(storestList, storeContentType, true, false);

            SPContentType deptContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Department];
            SPContentType deptListContentType = CommonUtilities.AttachContentTypeToList(storestList, storeContentType, true, false);
            CommonUtilities.AddFieldToContentType(web, deptListContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColParentdeparment");

            SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Project];
            projectContentType.FieldLinks.Delete(SPBuiltInFieldId.Predecessors);
            CommonUtilities.AddFieldToContentType(web, projectContentType, storeLookup, true, false, string.Empty);

            SPContentType projectTaskContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectTask];
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, taskLookup, false, false, "$Resources:COSIntranet,ChangeColParentProject");
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, storeLookup, true, true, string.Empty);

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
