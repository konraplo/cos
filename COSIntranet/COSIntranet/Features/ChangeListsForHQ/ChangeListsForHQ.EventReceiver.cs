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

        private void AddLookupsToLists(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", "add Lookups to lists");

            string deptUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", deptUrl));
            SPList deptList = web.GetList(deptUrl);
            //ListUtilities.CreateLookupFieldAtList(web, "ParentDepartment", Fields.Department, deptList, Fields.Title, deptList, false, false);
            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", storesUrl));
            SPList storestList = web.GetList(storesUrl);

            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForHQEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList tasksList = web.GetList(tasksUrl);
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
