using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

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
        private const string MasterPageUrl = "_catalogs/masterpage/oslo.master";
        private const string ImageUrl = "SiteAssets/Images/mainBackground.JPG";

        /// <summary>
        /// Query to get all composed looks for specified title
        /// </summary>
        private const string GetComposedLookByTitle = @"<Where>
                                                                <Eq>
                                                                  <FieldRef Name='Title' />
                                                                  <Value Type='Text'>{0}</Value>
                                                                </Eq>
                                                              </Where>";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                string serverRelativeUrl = web.ServerRelativeUrl;
                SPList list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, "_catalogs/design"));
                string queryString = string.Format(GetComposedLookByTitle, CustomLookName);
                SPQuery query = new SPQuery();
                query.Query = queryString;

                SPListItemCollection itmes = list.GetItems(query);
                bool match = itmes.Count > 0;

                if (!match)
                {
                    SPListItem item = list.AddItem();

                    item[SPBuiltInFieldId.Title] = CustomLookName;
                    item["Name"] = CustomLookName;

                    SPFieldUrlValue masterUrl = new SPFieldUrlValue();
                    masterUrl.Url = SPUtility.ConcatUrls(serverRelativeUrl, MasterPageUrl);
                    masterUrl.Description = SPUtility.ConcatUrls(serverRelativeUrl, MasterPageUrl);
                    item["MasterPageUrl"] = masterUrl;

                    SPFieldUrlValue themeUrl = new SPFieldUrlValue();
                    themeUrl.Url = SPUtility.ConcatUrls(serverRelativeUrl, ThemeUrl);
                    themeUrl.Description = SPUtility.ConcatUrls(serverRelativeUrl, ThemeUrl);
                    item["ThemeUrl"] = themeUrl;

                    SPFieldUrlValue imageUrl = new SPFieldUrlValue();
                    imageUrl.Url = SPUtility.ConcatUrls(serverRelativeUrl, ImageUrl);
                    imageUrl.Description = SPUtility.ConcatUrls(serverRelativeUrl, ImageUrl);
                    item["ImageUrl"] = imageUrl;

                    item["DisplayOrder"] = 199;
                    item.Update();
                }

                //site.RootWeb.ApplyTheme(serverRelativeUrl + ThemeUrl,
                //                          null, null, true);
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
