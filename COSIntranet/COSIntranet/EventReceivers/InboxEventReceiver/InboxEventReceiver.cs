using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Threading.Tasks;
using Change.Intranet.Common;
using System.IO;

namespace Change.Intranet
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class InboxEventReceiver : SPItemEventReceiver
    {
        private Guid sitelLookupId = new Guid("{4dcbc8cf-ebd9-4e87-a332-d42aa7edb5ae}");
        private Guid folderlLookupId = new Guid("{BD708A9B-98AE-4A32-9981-5011326C5428}");
        private Guid urlFieldId = new Guid("{5B0F68F2-8B2E-4C9D-B2B4-157BB8205052}");

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("item id:{0}", properties.ListItem.ID));
            SPListItem item = properties.ListItem;
            SPFile file = item.File;
            if (file != null)
            {
                bool canDelete = true;
                using (var stream = file.OpenBinaryStream())
                {
                    SPFieldLookupValueCollection sitesUrl = new SPFieldLookupValueCollection(item[sitelLookupId].ToString());
                    SPFieldLookupValueCollection foldersUrl = new SPFieldLookupValueCollection(item[folderlLookupId].ToString());
                    Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("Prepeare to provision file:{0}", file.Name));
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(properties.SiteId))
                        {
                            SPList siteList = site.RootWeb.GetList(SPUrlUtility.CombineUrl(properties.Web.ServerRelativeUrl.TrimEnd('/'), "/Lists/Sites"));
                            SPList folderList = site.RootWeb.GetList(SPUrlUtility.CombineUrl(properties.Web.ServerRelativeUrl.TrimEnd('/'), "/Lists/Folders"));

                            //Parallel.ForEach(sitesUrl, (siteUrl) =>
                            foreach (var siteUrl in sitesUrl)
                            {
                                Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("copy to site:{0}", siteUrl.LookupValue));
                                if (!CopyFileToDetinations(site, foldersUrl, file.Name, stream, folderList, siteList, siteUrl.LookupId))
                                {
                                    canDelete = false;
                                }
                            }
                        }
                    });
                }
                if (canDelete)
                {
                    file.Delete();
                }
            }
            base.ItemUpdated(properties);
        }

        private bool CopyFileToDetinations(SPSite site, SPFieldLookupValueCollection foldersUrl, string filename, Stream stream, SPList folderList, SPList siteList, int siteUrlId)
        {
            SPListItem itemSite = siteList.GetItemById(siteUrlId);
            string urlField = itemSite[urlFieldId].ToString();
            bool sucess = true;
            using (SPWeb web = site.OpenWeb(urlField))
            {
                Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("web:{0}, opened - start provision file:{1}", web.Url, filename));         
                foreach (var folderUrl in foldersUrl)
                {
                    try
                    {
                        SPListItem itemFolder = folderList.GetItemById(folderUrl.LookupId);
                        string urlFolder = itemFolder[urlFieldId].ToString();
                        Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("copy to folder:{0}, file:{1}", urlFolder, filename));

                        int counter = 0;
                        SPList destinationList = null;
                        SPFolder destinationFolder = null;

                        foreach (string folderUrlToken in urlFolder.Split('/'))
                        {
                            if (counter == 0)
                            {
                                string listUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), string.Format("/Lists/{0}", ListUtilities.ListUrlMappings[folderUrlToken]));
                                destinationList = web.GetList(listUrl);
                                destinationFolder = destinationList.RootFolder;
                            }
                            else
                            {
                                string translatedToken = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", folderUrlToken), "COSIntranet", web.Language);
                                destinationFolder = destinationFolder.SubFolders[translatedToken];
                            }
                            counter++;
                        }
                        destinationFolder.Files.Add(filename, stream, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(Logger.Category.Unexpected, "InboxEventReceiver - CopyFileToDetinations", string.Format("Errory by copy to File:{0} - {1}", filename, ex.Message));
                        // handle error
                        sucess = false;
                    }
                }
            }
            return sucess;
        }
    }
}