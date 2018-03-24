using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Globalization;

namespace Change.Intranet.Features.ChangeComposedLook
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("3bd61603-df9c-4203-bb90-7f0769103fbb")]
    public class ChangeComposedLookEventReceiver : SPFeatureReceiver
    {
        private const string CustomLookName = "Change of Scandinavia";
        private const string ThemeUrl = "_catalogs/theme/15/cos_pal015.spcolor";
        private const string MasterPageUrl = "_catalogs/masterpage/{0}";
        private const string ImageUrl = "SiteAssets/Images/mainBackground.JPG";
        private const string MasterPageOslo = "oslo.master";
        private const string MasterPageSeatle = "seattle.master";

        /// <summary>
        /// Query to get all composed looks for specified title
        /// </summary>
        private const string GetComposedLookByTitle = @"<Where>
                                                                <Eq>
                                                                  <FieldRef Name='Title' />
                                                                  <Value Type='Text'>{0}</Value>
                                                                </Eq>
                                                              </Where>";

        /// <summary>
        /// Query to get all composed looks for specified title
        /// </summary>
        private const string GetComposedLookByOrder = @"<Where><Eq><FieldRef Name='DisplayOrder'/><Value Type='Number'>{0}</Value></Eq></Where>";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                string serverRelativeUrl = web.ServerRelativeUrl;
                string rootServerRelativeUrl = web.Site.RootWeb.ServerRelativeUrl;
                //SPList list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, "_catalogs/design"));
                SPList designGallery = web.GetCatalog(SPListTemplateType.DesignCatalog);
                string queryString = string.Format(GetComposedLookByTitle, CustomLookName);
                SPQuery query = new SPQuery();
                query.Query = queryString;

                SPListItemCollection itmes = designGallery.GetItems(query);
                bool match = itmes.Count > 0;
                string materUrlValue = string.Format(MasterPageUrl, MasterPageOslo);
                if (!web.IsRootWeb)
                {
                    materUrlValue = string.Format(MasterPageUrl, MasterPageSeatle);
                }

                if (!match)
                {
                    SPListItem item = designGallery.AddItem();

                    UpdateComposedLookItem(item, serverRelativeUrl, rootServerRelativeUrl, materUrlValue, CustomLookName, 199);
                    
                    item.Update();
                }

                web.ApplyTheme(SPUtility.ConcatUrls(rootServerRelativeUrl, ThemeUrl) , null, SPUtility.ConcatUrls(rootServerRelativeUrl, ImageUrl), false);
                UpdateCurrentItem(designGallery, serverRelativeUrl, rootServerRelativeUrl, materUrlValue);
            }
        }


        private void UpdateCurrentItem(SPList designGallery, string serverRelativeUrl, string rootServerRelativeUrl, string materUrlValue)
        {
            SPQuery query = new SPQuery();
            query.RowLimit = 1;
            query.Query = string.Format(GetComposedLookByOrder, 0); 
            query.ViewFields = "<FieldRef Name='DisplayOrder'/>";
            query.ViewFieldsOnly = true;

            SPListItemCollection currentItems = designGallery.GetItems(query);

            if (currentItems.Count == 1)
            {
                // Remove the old Current item.
                currentItems[0].Delete();
            }

            SPListItem currentItem = designGallery.AddItem();
            UpdateComposedLookItem(currentItem, serverRelativeUrl, rootServerRelativeUrl, materUrlValue, SPResource.GetString(CultureInfo.CurrentUICulture, Strings.DesignGalleryCurrentItemName), 0);
            currentItem.Update();
        }

        private void UpdateComposedLookItem(SPListItem item, string serverRelativeUrl, string rootServerRelativeUrl, string materUrlValue, string customLookName, int displayOrderd)
        {
            item[SPBuiltInFieldId.Title] = customLookName;
            item["Name"] = customLookName;

            SPFieldUrlValue masterUrl = new SPFieldUrlValue();
           
            masterUrl.Url = SPUtility.ConcatUrls(serverRelativeUrl, materUrlValue);
            masterUrl.Description = SPUtility.ConcatUrls(serverRelativeUrl, materUrlValue);
            item["MasterPageUrl"] = masterUrl;

            SPFieldUrlValue themeUrl = new SPFieldUrlValue();
            themeUrl.Url = SPUtility.ConcatUrls(rootServerRelativeUrl, ThemeUrl);
            themeUrl.Description = SPUtility.ConcatUrls(rootServerRelativeUrl, ThemeUrl);
            item["ThemeUrl"] = themeUrl;

            SPFieldUrlValue imageUrl = new SPFieldUrlValue();
            imageUrl.Url = SPUtility.ConcatUrls(rootServerRelativeUrl, ImageUrl);
            imageUrl.Description = SPUtility.ConcatUrls(rootServerRelativeUrl, ImageUrl);
            item["ImageUrl"] = imageUrl;

            item["DisplayOrder"] = displayOrderd;
            item.Update();
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
