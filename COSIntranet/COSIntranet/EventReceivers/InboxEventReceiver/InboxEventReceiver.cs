using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Threading.Tasks;
using Change.Intranet.Common;

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
            //byte[] bytes = file.OpenBinary();
            //Microsoft.Office.RecordsManagement.RecordsRepository.OfficialFileCore.SubmitFile()
            SPFieldLookupValueCollection sitesUrl = new SPFieldLookupValueCollection(item[sitelLookupId].ToString());
            SPFieldLookupValueCollection foldersUrl = new SPFieldLookupValueCollection(item[folderlLookupId].ToString());
            Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("Prepeare to provision file:{0}", file.Name));
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(properties.SiteId))
                {
                    SPList siteList = site.RootWeb.GetList(SPUrlUtility.CombineUrl(properties.Web.ServerRelativeUrl.TrimEnd('/'), "/Lists/Sites"));
                    SPList folderList = site.RootWeb.GetList(SPUrlUtility.CombineUrl(properties.Web.ServerRelativeUrl.TrimEnd('/'), "/Lists/Folders"));

                    foreach (var siteUrl in sitesUrl)
                    {
                        Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("copy to site:{0}", siteUrl.LookupValue));
                        CopyFileToDetinations(site, foldersUrl, file, folderList, siteList, siteUrl.LookupId);
                    }

                    //Parallel.ForEach(sitesUrl, (siteUrl) =>
                    //{
                    //    CopyFileToDetinations(site, foldersUrl, file, folderList, siteList, siteUrl.LookupId);
                    //});
                    
                 }
            });
            
            base.ItemUpdated(properties);
        }

        private void CopyFileToDetinations(SPSite site, SPFieldLookupValueCollection foldersUrl, SPFile file, SPList folderList, SPList siteList, int siteUrlId)
        {
            SPListItem itemSite = siteList.GetItemById(siteUrlId);
            string urlField = itemSite[urlFieldId].ToString();
            using (SPWeb web = site.OpenWeb(urlField))
            {
                Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("web:{0}, opened - start provision file:{2}", web.Url, file.Name));
                foreach (var folderUrl in foldersUrl)
                {
                    try
                    {
                        SPListItem itemFolder = folderList.GetItemById(folderUrl.LookupId);
                        string urlFolder = itemFolder[urlFieldId].ToString();
                        Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("copy to folder:{0}, file:{1}", urlFolder, file.Name));

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
                        destinationFolder.Files.Add(file.Name, file.OpenBinaryStream());
                    }
                    catch (Exception ex)
                    {
                        // handle error
                        Logger.WriteLog(Logger.Category.Unexpected, "InboxEventReceiver - CopyFileToDetinations", string.Format("Errory by copy to File:{0} - {1}", file.Name, ex.Message));
                    }
                }
            }


        }
    }
}