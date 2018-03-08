using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Change.Intranet.Features.ChangeListsForDivisions
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("a53b7b35-d1c1-434b-917a-a398fb2b9f64")]
    public class ChangeListsForDivisionsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                // add folder strucure

                // visual Merchandise
                SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/VisualMerchandise"));
                SPFolderCollection folderColl = list.RootFolder.SubFolders;

                string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBookingOfCraftsmen", "COSIntranet", web.Language);
                SPFolder newFolder = folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBookingOfLights", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSignsForPrint", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMountingOfFoil", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleThingsOnHangers", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleManuals", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMGuidelines", "COSIntranet", web.Language);
                folderColl.Add(folderUrl);

                list.OnQuickLaunch = true;
                list.Update();
            }
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
