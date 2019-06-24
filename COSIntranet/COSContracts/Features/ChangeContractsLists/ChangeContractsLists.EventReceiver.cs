using System;
using System.Reflection;
using System.Runtime.InteropServices;
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
                Upgradeto112(web);
                Upgradeto113(web);
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
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Add lookups");

            SPFieldLookup custLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.Customer, "$Resources:COSContracts,ChangeColCustomer", Fields.Title, customersList, true, false);
            SPFieldLookup custProfitCenterLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.CustomerProfitCenter, "$Resources:COSContracts,ChangeColCustPCenter", Fields.Title, customerProfitCenterList, true, false);
            SPFieldLookup groupEntityLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.GroupEntity, "$Resources:COSContracts,ChangeColGroupEntity", Fields.Title, groupEntityList, false, false);
            SPFieldLookup contractSubtypeLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.ContractSubtype, "$Resources:COSContracts,ChangeColContractSubtype", Fields.Title, contractSubtypeList, false, false);
            SPFieldLookup vendorLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.Vendor, "$Resources:COSContracts,ChangeColVendor", Fields.Title, vendorsList, true, false);
            SPFieldLookup externalCustContactLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.ExternalContactCust, "$Resources:COSContracts,ChangeColExtCustContact", Fields.Title, externalContactsList, false, false);
            SPFieldLookup externalVendorContactLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeContractsFieldsGroup, Fields.ExternalContactVendor, "$Resources:COSContracts,ChangeColExtVendorContact", Fields.Title, externalContactsList, false, false);

            // add ct to lists
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add ct to lists");
            SPContentType contractContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Contract];
            SPContentType contractDocumentContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ContractDocument];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", contractDocumentContentType.Name, contractUrl));
            CommonUtilities.AttachContentTypeToList(contractsList, contractDocumentContentType, false, false);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", contractContentType.Name, contractUrl));
            CommonUtilities.AddFieldToContentType(web, contractContentType, custLookup, true, false, "$Resources:COSContracts,ChangeColCustomer");
            CommonUtilities.AddFieldToContentType(web, contractContentType, externalCustContactLookup, false, false, "$Resources:COSContracts,ChangeColExtCustContact");
            CommonUtilities.AddFieldToContentType(web, contractContentType, contractSubtypeLookup, false, false, "$Resources:COSContracts,ChangeColContractSubtype");
            CommonUtilities.AddFieldToContentType(web, contractContentType, vendorLookup, true, false, "$Resources:COSContracts,ChangeColVendor");
            CommonUtilities.AddFieldToContentType(web, contractContentType, externalVendorContactLookup, false, false, "$Resources:COSContracts,ChangeColExtVendorContact");
            SPContentType contractListContentType = CommonUtilities.AttachContentTypeToList(contractsList, contractContentType, true, false);

            SPContentType contractSubtypeContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ContractSubtype];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", contractSubtypeContentType.Name, contractSubtypeUrl));
            CommonUtilities.AttachContentTypeToList(contractSubtypeList, contractSubtypeContentType, true, false);

            SPContentType customerProfitCenterContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.CustomerProfitCenter];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", customerProfitCenterContentType.Name, customerProfitCenterUrl));
            CommonUtilities.AttachContentTypeToList(customerProfitCenterList, customerProfitCenterContentType, true, false);

            SPContentType customerContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Customer];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", customerContentType.Name, customersUrl));
            CommonUtilities.AddFieldToContentType(web, customerContentType, custProfitCenterLookup, true, false, "$Resources:COSContracts,ChangeColCustPCenter");
            CommonUtilities.AddFieldToContentType(web, customerContentType, groupEntityLookup, false, false, "$Resources:COSContracts,ChangeColGroupEntity");
            CommonUtilities.AttachContentTypeToList(customersList, customerContentType, true, false);
            

            SPContentType externalContactsContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ExternalContact];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", externalContactsContentType.Name, externalContactsUrl));
            CommonUtilities.AttachContentTypeToList(externalContactsList, externalContactsContentType, true, false);

            SPContentType groupEntityContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.GroupEntity];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", groupEntityContentType.Name, groupEntityUrl));
            CommonUtilities.AttachContentTypeToList(groupEntityList, groupEntityContentType, true, false);

            SPContentType vendorsContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Vendor];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", vendorsContentType.Name, vendorsUrl));
            SPContentType vendorListContentType = CommonUtilities.AttachContentTypeToList(vendorsList, vendorsContentType, true, false);
            CommonUtilities.AddFieldToContentType(web, vendorListContentType, groupEntityLookup, false, false, "$Resources:COSContracts,ChangeColGroupEntity");


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
            Logger.WriteLog(Logger.Category.Unexpected, this.GetType().Name, string.Format("Upgrading Feature:{0}", upgradeActionName));
            try
            {

                SPWeb web = properties.Feature.Parent as SPWeb;

                switch (upgradeActionName)
                {

                    case "UpgradeToV1.2":
                        Upgradeto112(web);
                        break;
                    case "UpgradeToV1.3":
                        Upgradeto113(web);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, this.GetType().Name, string.Format("Error while Upgrading Feature:{0}", ex.Message));
                throw;
            }
        }

        private void Upgradeto112(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Upgradeto112");
            SPField groupEntityValue = web.Fields.GetFieldByInternalName(Fields.GroupEntityValue);
            groupEntityValue.ReadOnlyField = true;
            groupEntityValue.ShowInDisplayForm = false;
            groupEntityValue.Update();
            SPField custPCValue = web.Fields.GetFieldByInternalName(Fields.CustPCValue);
            custPCValue.ReadOnlyField = true;
            custPCValue.ShowInDisplayForm = false;
            custPCValue.Update();

            SPContentType vendor = web.Site.RootWeb.ContentTypes[ContentTypeIds.Vendor];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add filed:{0} to ct:{1}", groupEntityValue.InternalName, vendor.Name));
            CommonUtilities.AddFieldToContentType(web, vendor, groupEntityValue, false, true, "Vendor group entity value");

            SPContentType customer = web.Site.RootWeb.ContentTypes[ContentTypeIds.Customer];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add filed:{0} to ct:{1}", groupEntityValue.InternalName, customer.Name));
            CommonUtilities.AddFieldToContentType(web, customer, groupEntityValue, false, true, "Customer group entity value");
            CommonUtilities.AddFieldToContentType(web, customer, custPCValue, false, true, "Customer profit center");            
        }

        private void Upgradeto113(SPWeb web)
        {
            // update all exsitng vendors
            SPQuery query = new SPQuery();
            SPList listVendors = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.Vendors));

            SPListItemCollection vendors = listVendors.GetItems(query);

            foreach (SPListItem listItem in vendors)
            {
                string groupEntity = Convert.ToString(listItem[Fields.GroupEntity]);
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("List:{0}, item:{1}, value:{2}", ListUtilities.Urls.Vendors, listItem.Title, groupEntity));
                if (!string.IsNullOrEmpty(groupEntity))
                {
                    SPFieldLookupValue geLookupValue = new SPFieldLookupValue(groupEntity);
                    listItem[Fields.GroupEntityValueId] = geLookupValue.LookupValue;
                    listItem.SystemUpdate(false);
                }
            }

            SPList listCustomers = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.Customers));

            SPListItemCollection customers = listCustomers.GetItems(query);

            foreach (SPListItem listItem in customers)
            {
                string groupEntity = Convert.ToString(listItem[Fields.GroupEntity]);
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("List:{0}, item:{1}, value:{2}", ListUtilities.Urls.Customers, listItem.Title, groupEntity));
                if (!string.IsNullOrEmpty(groupEntity))
                {
                    SPFieldLookupValue geLookupValue = new SPFieldLookupValue(groupEntity);
                    listItem[Fields.GroupEntityValueId] = geLookupValue.LookupValue;
                }

                string profitCenter = Convert.ToString(listItem[Fields.CustomerProfitCenter]);
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("List:{0}, item:{1}, value:{2}", ListUtilities.Urls.Customers, listItem.Title, profitCenter));
                if (!string.IsNullOrEmpty(profitCenter))
                {
                    SPFieldLookupValue pcLookupValue = new SPFieldLookupValue(profitCenter);
                    listItem[Fields.CustPCValueId] = pcLookupValue.LookupValue;
                }

                listItem.SystemUpdate(false);

            }

            string vendorsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Vendors);
            SPList vendorsList = web.GetList(vendorsUrl);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", vendorsUrl));
            CommonUtilities.AddListEventReceiver(vendorsList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Contracts.EventReceivers.VendorsListEventReceiver.VendorsListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(vendorsList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Contracts.EventReceivers.VendorsListEventReceiver.VendorsListEventReceiver", false);

            string customersUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Customers);
            SPList customersList = web.GetList(customersUrl);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", customersUrl));
            CommonUtilities.AddListEventReceiver(customersList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Contracts.EventReceivers.CustomersListEventReceiver.CustomersListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(customersList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Contracts.EventReceivers.CustomersListEventReceiver.CustomersListEventReceiver", false);

        }

    }
}
