using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Change.Contracts.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Change.Contracts.Features.ChangeContractsLists
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("b7fb40c7-7c2b-47e0-8e26-83fabe699679")]
    public class ChangeContractsListsEventReceiver : SPFeatureReceiver
    {
        private const string ReceiverName = "ChangeBusinessDevelopmentEventReceiver";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                Logger.WriteLog(Logger.Category.Information, ReceiverName, "add CT and ER");
                AddFieldsCtErToLists(web);
            }
        }

        /// <summary>
        /// Add fields, content types end event receivers to list
        /// </summary>
        /// <param name="web"></param>
        private void AddFieldsCtErToLists(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, ReceiverName, "Find lists");

            string contractUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Contracts);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", contractUrl));
            SPList contractsList = web.GetList(contractUrl);

            string contractSubtypeUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ContractSubtype);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", contractSubtypeUrl));
            SPList contractSubtypeList = web.GetList(contractSubtypeUrl);

            string customerProfitCenterUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.CustomerProfitCenter);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", customerProfitCenterUrl));
            SPList customerProfitCenterList = web.GetList(customerProfitCenterUrl);

            string customersUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Customers);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", customersUrl));
            SPList customersList = web.GetList(customersUrl);

            string externalContactsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ExternalContacts);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", externalContactsUrl));
            SPList externalContactsList = web.GetList(externalContactsUrl);

            string groupEntityUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.GroupEntity);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", groupEntityUrl));
            SPList groupEntityList = web.GetList(groupEntityUrl);

            string vendorsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Vendors);
            Logger.WriteLog(Logger.Category.Information, ReceiverName, string.Format("add Lookups to:{0}", vendorsUrl));
            SPList vendorsList = web.GetList(vendorsUrl);

            // add lookups
            //Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Add lookups");

            //SPFieldLookup deptLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Department, "$Resources:COSIntranet,ChangeColDeparment", Fields.Title, contractsList, false, false);
            //SPFieldLookup storeOpeningLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.StoreOpening, "$Resources:COSIntranet,ChangeColStoreOpening", Fields.Title, externalContactsList, false, false);
            //SPFieldLookup storeLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.ContractSubtype, "$Resources:COSIntranet,ChangeColStore", Fields.StoreId, contractSubtypeList, false, false);
            //SPFieldLookup countryLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Country, "$Resources:COSIntranet,ChangeColCountry", Fields.Title, groupEntityList, false, false);

            // add ct to lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add ct to lists");
            SPContentType contractContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Contract];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", contractContentType.Name, contractUrl));
            SPContentType contractListContentType = CommonUtilities.AttachContentTypeToList(contractsList, contractContentType, true, false);
            //CommonUtilities.AddFieldToContentType(web, deptListContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColParentdeparment");

            SPContentType contractSubtypeContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ContractSubtype];
            //CommonUtilities.AddFieldToContentType(web, storeContentType, countryLookup, true, false, string.Empty);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", contractSubtypeContentType.Name, contractSubtypeUrl));
            CommonUtilities.AttachContentTypeToList(contractSubtypeList, contractSubtypeContentType, true, false);

            SPContentType customerProfitCenterContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.CustomerProfitCenter];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", customerProfitCenterContentType.Name, customerProfitCenterUrl));
            CommonUtilities.AttachContentTypeToList(customerProfitCenterList, customerProfitCenterContentType, true, false);

            SPContentType customerContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Customer];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", customerContentType.Name, customersUrl));
            CommonUtilities.AttachContentTypeToList(customersList, customerContentType, true, false);

            SPContentType externalContactsContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ExternalContact];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", externalContactsContentType.Name, externalContactsUrl));
            CommonUtilities.AttachContentTypeToList(externalContactsList, externalContactsContentType, true, false);

            SPContentType groupEntityContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.GroupEntity];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", groupEntityContentType.Name, groupEntityUrl));
            CommonUtilities.AttachContentTypeToList(groupEntityList, groupEntityContentType, true, false);

            SPContentType vendorsContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Vendor];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", vendorsContentType.Name, vendorsUrl));
            CommonUtilities.AttachContentTypeToList(vendorsList, vendorsContentType, true, false);           

            
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
