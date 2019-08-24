using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Change.Intranet.Common;
using Microsoft.SharePoint;

namespace Change.Intranet.Features.ChangeContentOrganizerBD
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("60454f6b-c357-471f-8fbd-1b949135c88a")]
    public class ChangeContentOrganizerBDEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
               Upgradeto11(web);
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

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            Logger.WriteLog(Logger.Category.Medium, this.GetType().Name, string.Format("upgrading - web:{0}, action:{1}", web.Url, upgradeActionName));

            switch (upgradeActionName)
            {

                case "UpgradeToV1.1":
                    Upgradeto11(web);
                    break;

            }
        }

        private void Upgradeto11(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Medium, typeof(ChangeContentOrganizerBDEventReceiver).FullName, string.Format("Upgradeto11 web:{0}", web.Url));
            if (web != null)
            {

            }

            Logger.WriteLog(Logger.Category.Medium, typeof(ChangeContentOrganizerBDEventReceiver).FullName, string.Format("Upgradeto11 fnished web:{0}", web.Url));
        }
    }
}
