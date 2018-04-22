using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Reflection;
using Change.Intranet.Common;
using Change.Intranet.EventReceivers.PreventFolderDeleteEventReceiver;
using System.Collections.Generic;

namespace Change.Intranet.Features.ChangeContentOrganizer
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("02839299-9d83-47e1-9732-64070cdd5140")]
    public class ChangeContentOrganizerEventReceiver : SPFeatureReceiver
    {
        private SPContentType fetchContentType(SPContentTypeCollection contentTypeCollection, string ID)
        {
            SPContentType publContentType = null;
            foreach (SPContentType contentType in contentTypeCollection)
            {

                if (string.Equals(contentType.Id.ToString(), ID, StringComparison.InvariantCultureIgnoreCase))
                {
                    publContentType = contentType;
                    break;
                }
            }
            return publContentType;
        }

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeContentOrganizerEventReceiver - FeatureActivated", "Start feature acitivation");
            SPWeb web = properties.Feature.Parent as SPWeb;
            Upgradeto11(web);
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeContentOrganizerEventReceiver - FeatureDeactivating", "Start feature deacitivation");
            SPWeb web = properties.Feature.Parent as SPWeb;
            SPWeb rootWeb = web.Site.RootWeb;
            SPContentType folderCT = rootWeb.ContentTypes[SPBuiltInContentTypeId.Folder];
            List<SPEventReceiverDefinition> ersToDelete = new List<SPEventReceiverDefinition>();

            foreach (SPEventReceiverDefinition er in folderCT.EventReceivers)
            {
                if (er.Id.Equals(ContentOrganizerUtilities.ItemDeletingERID) || er.Name.Equals(ContentOrganizerUtilities.ItemDeletingERName) ||
                    er.Id.Equals(ContentOrganizerUtilities.ItemUpdatingERID) || er.Name.Equals(ContentOrganizerUtilities.ItemUpdatingERName))
                {
                    ersToDelete.Add(er);
                }
            }

            foreach (SPEventReceiverDefinition er in ersToDelete)
            {
                er.Delete();
            }

            //if (folderCT.EventReceivers.EventReceiverDefinitionExist(ContentOrganizerUtilities.ItemDeletingERID))
            //{
            //    Logger.WriteLog(Logger.Category.Information, "ChangeContentOrganizerEventReceiver - FeatureDeactivating", string.Format("Remove ItemDeleting ER:{0}", ContentOrganizerUtilities.ItemDeletingERID));
            //    folderCT.EventReceivers[ContentOrganizerUtilities.ItemDeletingERID].Delete();
            //}

            //if (folderCT.EventReceivers.EventReceiverDefinitionExist(ContentOrganizerUtilities.ItemUpdatingERID))
            //{
            //    Logger.WriteLog(Logger.Category.Information, "ChangeContentOrganizerEventReceiver - FeatureDeactivating", string.Format("Remove ItemUpdating ER:{0}", ContentOrganizerUtilities.ItemUpdatingERID));
            //    folderCT.EventReceivers[ContentOrganizerUtilities.ItemUpdatingERID].Delete();
            //}
            folderCT.Update(true, false);

            if (!folderCT.Sealed)
            {
                folderCT.Sealed = true;
                folderCT.Update(true, false);
            }

            Logger.WriteLog(Logger.Category.Information, "ChangeContentOrganizerEventReceiver - FeatureDeactivating", "End feature deacitivation");
        }


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
            Logger.WriteLog(Logger.Category.Medium, "ChangeContentOrganizerEventReceiver - FeatureUpgrading", string.Format("upgrading - web:{0}, action:{1}", web.Url, upgradeActionName));

            switch (upgradeActionName)
            {

                case "UpgradeToV1.1":
                    Upgradeto11(web);
                    break;

            }
        }

        private void Upgradeto11(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Medium, "ChangeContentOrganizerEventReceiver - Upgradeto11", string.Format("web:{0}", web.Url));
            SPContentType folderCT = web.Site.RootWeb.ContentTypes[SPBuiltInContentTypeId.Folder];
            Logger.WriteLog(Logger.Category.Medium, "Upgradeto11 add ER", string.Format("CT:{0}", folderCT.Name));
            if (folderCT.Sealed)
            {
                folderCT.Sealed = false;
                folderCT.Update(true);
            }

            string assemblyName = Assembly.GetExecutingAssembly().FullName;
            string receiverClassName = typeof(PreventFolderDeleteEventReceiver).FullName;
            if (!folderCT.EventReceivers.EventReceiverDefinitionExist(ContentOrganizerUtilities.ItemDeletingERID))
            {
                Logger.WriteLog(Logger.Category.Medium, "ChangeContentOrganizerEventReceiver - Upgradeto11", string.Format("Add ItemDeleting ER:{0}", ContentOrganizerUtilities.ItemDeletingERID));
                SPEventReceiverDefinition erDef = folderCT.EventReceivers.Add(ContentOrganizerUtilities.ItemDeletingERID);
                erDef.Type = SPEventReceiverType.ItemDeleting;
                erDef.Assembly = assemblyName;
                erDef.Class = receiverClassName;
                erDef.Name = ContentOrganizerUtilities.ItemDeletingERName;
                erDef.Update();
            }

            if (!folderCT.EventReceivers.EventReceiverDefinitionExist(ContentOrganizerUtilities.ItemUpdatingERID))
            {
                Logger.WriteLog(Logger.Category.Medium, "ChangeContentOrganizerEventReceiver - Upgradeto11", string.Format("Add ItemUpdating ER:{0}", ContentOrganizerUtilities.ItemUpdatingERID));
                SPEventReceiverDefinition erDef = folderCT.EventReceivers.Add(ContentOrganizerUtilities.ItemUpdatingERID);
                erDef.Type = SPEventReceiverType.ItemUpdating;
                erDef.Assembly = assemblyName;
                erDef.Class = receiverClassName;
                erDef.Name = ContentOrganizerUtilities.ItemUpdatingERName;
                erDef.Update();
            }

            folderCT.Update(true, false);

            Logger.WriteLog(Logger.Category.Medium, "Upgradeto11 fnished", string.Format("web:{0}", web.Url));
        }
    }
}
