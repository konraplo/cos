using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Change.Intranet.Common;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Change.Intranet.Projects;
using System.Text.RegularExpressions;
using System.Collections;

namespace Change.Intranet.EventReceivers.BussinesDev.InboxBDEventReceiver
{

    /// <summary>
    /// List Item Events
    /// </summary>
    public class InboxBDEventReceiver : SPItemEventReceiver
    {
        public class EventFiring : SPItemEventReceiver
        {
            public void DisableHandleEventFiring()
            {
                this.EventFiringEnabled = false;
            }


            public void EnableHandleEventFiring()
            {
                this.EventFiringEnabled = true;
            }
        }

        private Guid folderlLookupId = new Guid("{26c49b5f-fdf7-4cb8-b986-aef0d4e65eb3}");
        private Guid folderUrlId = new Guid("{5B0F68F2-8B2E-4C9D-B2B4-157BB8205052}");
        private Guid logId = new Guid("{f9cdeded-c94e-4fd7-8cea-b32cd6d4924c}");
        private Guid statusId = new Guid("{a2a6f77f-5d45-4830-a0e5-7e86e94b7ad1}");


        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("item id:{0}", properties.ListItem.ID));
            SPListItem item = properties.ListItem;
            SPFile file = item.File;
            string status = Convert.ToString(item[statusId]);
            EventFiring eventFiring = new EventFiring();
            if (status == "4")
            {
                eventFiring.DisableHandleEventFiring();
                item[statusId] = null;
                item.Update();
                eventFiring.EnableHandleEventFiring();
            }
            else if (status != "1")
            {
                eventFiring.DisableHandleEventFiring();
                item[statusId] = 1;
                item.SystemUpdate();
                eventFiring.EnableHandleEventFiring();

                StringBuilder sb = new StringBuilder();
                if (file != null)
                {
                    bool canDelete = true;
                    using (var stream = file.OpenBinaryStream())
                    {
                        SPFieldLookupValueCollection destUrls = new SPFieldLookupValueCollection(item[folderlLookupId].ToString());
                        Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("Prepeare to provision file:{0}", file.Name));
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(properties.SiteId))
                            {
                                using (SPWeb web = site.OpenWeb(properties.Web.ID))
                                {
                                    SPList destList = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), "/Lists/FoldersBD"));
                                    List<string> folders = new List<string>();
                                    SPList stores = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), "/Lists/StoreOpenings"));
                                    foreach (SPListItem storeItem in stores.Items)
                                    {
                                        SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(storeItem[Fields.Store]));
                                        SPFieldLookupValue storeCountry = new SPFieldLookupValue(ProjectUtilities.GetStoreCountry(web, store.LookupId));
                                        string type = Convert.ToString(storeItem[Fields.ChangeProjectCategory]);
                                        string projectFolderName = string.Format("{0}_{1}_{2}_{3}", storeItem.ID, store.LookupValue, storeCountry.LookupValue, type);
                                        folders.Add(projectFolderName);
                                    }

                                    foreach (var destUrl in destUrls)
                                    {
                                        Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - Itemupdated", string.Format("copy to folder:{0}", destUrl.LookupValue));
                                        string error = string.Empty;
                                        if (!CopyFileToDetinations(web, destUrl, folders, file.Name, stream, destList, ref sb))
                                        {
                                            canDelete = false;
                                        }
                                    }
                                }
                            }
                        });
                    }
                    if (canDelete)
                    {
                        item[statusId] = 2;
                        //file.Delete();
                    }
                    else
                    {
                        item[statusId] = 3;
                    }
                    eventFiring.DisableHandleEventFiring();
                    item[logId] = sb.ToString();
                    item.SystemUpdate();
                    eventFiring.EnableHandleEventFiring();
                }
            }
            base.ItemUpdated(properties);
        }

        public override void ItemUpdating(SPItemEventProperties properties)
        {
            string fileUrl = properties.BeforeUrl;

            string fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
            if (IsUploaded(properties.List, fileName))
            {
                properties.AfterProperties["Status"] = 4;
            }
            base.ItemUpdating(properties);
        }

        private bool IsUploaded(SPList list, string fileName)

        {
            bool isExisted = false;
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='FileLeafRef'></FieldRef><Value Type='Text'>{0}</Value></Eq></Where>", fileName);
            query.ViewAttributes = "Scope=\"Recursive\"";
            SPListItemCollection itemColl = list.GetItems(query);
            if (itemColl.Count > 0)
            {
                string status = Convert.ToString(itemColl[0][statusId]);
                if (status != "")
                {
                    isExisted = true;
                }
            }

            return isExisted;

        }

        private bool CopyFileToDetinations(SPWeb web, SPFieldLookupValue destUrl, List<string> subfolders, string filename, Stream stream, SPList folderList, ref StringBuilder sb)
        {
            bool sucess = true;
            Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("web:{0}, opened - start provision file:{1}", web.Url, filename));
            SPListItem folderItem = folderList.GetItemById(destUrl.LookupId);
            string destUrlValue = Convert.ToString(folderItem[folderUrlId]);
            string docliburl = string.Empty;
            Regex regex = new Regex("<([^>]+)>");
            Match match = regex.Match(destUrlValue);
            if (match.Success)
            {
                docliburl = regex.Replace(match.Value, "$1");
            }
            else
            {
                sucess = false;
                sb.AppendLine(string.Format("error while copying file to {0} - wrong destination folder syntax", destUrlValue));
                return sucess;
            }

            foreach (var subfolder in subfolders)
            {
                string targetUrl = regex.Replace(destUrlValue, subfolder);
                try
                {
                    Logger.WriteLog(Logger.Category.Medium, "InboxEventReceiver - CopyFileToDetinations", string.Format("copy to dest:{0}/{1} file:{2}", docliburl, targetUrl, filename));

                    string listUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl, docliburl);
                    SPList destinationList = web.GetList(listUrl);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        CommonUtilities.AddDocumentToLibrary((SPDocumentLibrary)destinationList, targetUrl, ms.ToArray(), filename, new Hashtable());
                    }

                    sb.AppendLine(string.Format("{0}/{1}/{2}", docliburl, targetUrl, filename));
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, "InboxEventReceiver - CopyFileToDetinations", string.Format("Errory by copy to File:{0} - {1}", filename, ex.Message));
                    // handle error
                    sucess = false;
                    sb.AppendLine(string.Format("error while copying file to {0}/{1}. {2}", docliburl, targetUrl, ex.Message));
                }
            }
            if (subfolders.Count == 0)
            {
                sucess = false;
                sb.AppendLine(string.Format("error while copying file to {0}. No projects defined", destUrlValue));
            }
            return sucess;
        }
    }
}