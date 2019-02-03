using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Change.Intranet.Common;
using System.Collections.Generic;

namespace Change.Intranet.EventReceivers.PreventFolderDeleteEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class PreventFolderDeleteEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Medium, "PreventFolderDeleteEventReceiver - ItemDeleting", string.Format("ListItem.ID:{0}, List:{1}", properties.ListItem.ID, properties.List.Title));
            if (!properties.Web.UserIsSiteAdmin)
            {
                string itemUrl = string.Concat('/', properties.ListItem.Url);
               
                List<string> result = GetFolderUrls(properties.SiteId, properties.Web);
                if (result.Contains(itemUrl))
                {
                    properties.Cancel = true;
                    properties.ErrorMessage = ContentOrganizerUtilities.PreventFolderDeleteErrorMsg;
                    Logger.WriteLog(Logger.Category.Medium, "PreventFolderDeleteEventReceiver - ItemDeleting", string.Format("{0}, ListItem.ID:{1}, List:{2}", properties.ErrorMessage, properties.ListItem.ID, properties.List.Title));
                }
            }
            else
            {
                base.ItemDeleting(properties);
            }
        }

        /// <summary>
        /// Item is being updated
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            if (properties.ListItem == null)
            {
                base.ItemUpdating(properties);
                return;
            }

            Logger.WriteLog(Logger.Category.Medium, "PreventFolderDeleteEventReceiver - ItemUpdating", string.Format("ListItem.ID:{0}, List:{1}", properties.ListItem.ID, properties.List.Title));
            if (!properties.Web.UserIsSiteAdmin)
            {
                string itemUrl = string.Concat('/', properties.ListItem.Url);

                List<string> result = GetFolderUrls(properties.SiteId, properties.Web);
                if (result.Contains(itemUrl))
                {
                    properties.Cancel = true;
                    properties.ErrorMessage = ContentOrganizerUtilities.PreventFolderDeleteErrorMsg;
                    Logger.WriteLog(Logger.Category.Medium, "PreventFolderDeleteEventReceiver - ItemUpdating", string.Format("{0}, ListItem.ID:{1}, List:{2}", properties.ErrorMessage, properties.ListItem.ID, properties.List.Title));
                }
            }
            else
            {
                base.ItemUpdating(properties);
            }
        }

        private List<string> GetFolderUrls(Guid siteId, SPWeb currentWeb)
        {
            List<string> result = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    SPList folderList = site.RootWeb.GetList(SPUrlUtility.CombineUrl(site.RootWeb.ServerRelativeUrl.TrimEnd('/'), "/Lists/Folders"));
                    SPListItemCollection folders =  folderList.GetItems(new SPQuery());
                    //Parallel.ForEach(sitesUrl, (siteUrl) =>
                    foreach (SPListItem itemFolder in folders)
                    {
                        Logger.WriteLog(Logger.Category.Medium, "GetFolderUrls - translate folder", string.Format("folder:{0}", itemFolder.Title));
                        string urlFolder = itemFolder[ContentOrganizerUtilities.UrlFieldId].ToString();
                        string translatedUrlFolder = string.Empty;
                        int counter = 0;
                        foreach (string folderUrlToken in urlFolder.Split('/'))
                        {
                            translatedUrlFolder = string.Concat(translatedUrlFolder, '/');
                            string translatedToken = string.Empty;
                            if(counter == 0)
                            {
                                translatedToken = string.Format("Lists/{0}", ListUtilities.ListUrlMappings[folderUrlToken]);
                            }
                            else
                            {
                                translatedToken = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", folderUrlToken), "COSIntranet", currentWeb.Language);
                            }
                            counter++;
                            translatedUrlFolder = string.Concat(translatedUrlFolder, translatedToken);
                        }
                        result.Add(translatedUrlFolder.Trim());
                    }
                }
            });

            return result;
        }

    }
}