namespace Change.Intranet.Common
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using Change.Intranet.Projects;
    using System.IO;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;

    /// <summary>
    /// Functionality connteced with projects opereations
    /// </summary>
    public static class ProjectHelper
    {
        public static string[] projectLibrarieUrls = { "Marketing", "Drawings", "GeneralInformation", "Logistic", "Pictures", "Evaluation" };

        /// <summary>
        /// Remove project releted folder from list
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="listUrl">Project library url</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProjectFolder(SPWeb web, string listUrl, int itemId)
        {
            Logger.WriteLog(Logger.Category.Information, "RemoveProjectFolder", string.Format("Remove project folder:{0} from {1}", itemId, listUrl));
            SPList list = null;
            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, string.Format("Lists/{0}", listUrl)));
                SPListItemCollection items = CommonUtilities.GetFoldersByPrefix(web, list, string.Format("{0}_", itemId));

                if (items.Count > 0)
                {
                    SPListItem firstItem = items[0];
                    firstItem.Delete();
                    list.Update();
                }
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProjectFolder", "List not found");
                return;
            }
        }

        /// <summary>
        /// Remove all project releted folders form libs
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveAllProjectFolder(SPWeb web, int itemId)
        {
            foreach (string listUrl in projectLibrarieUrls)
            {
                RemoveProjectFolder(web, listUrl, itemId);
            }
        }

        /// <summary>
        /// Remove project. WARNNING! After delete fire ER!
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveProject(SPWeb web, int itemId)
        {
            try
            {
                SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, ListUtilities.Urls.StoreOpenings));
                Logger.WriteLog(Logger.Category.Information, "RemoveProject", string.Format("Remove project :{0} from {1}", itemId, list.RootFolder.Url));
                SPListItem project = list.GetItemById(itemId);
                project.Delete();
                list.Update();
            }
            catch (Exception e)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveProject", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Archive project. Create ziped project file and save in Assets library
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        /// <param name="fileSavingPlace">Place where the archiv file is saved</param>
        /// <returns>Name of created zip file</returns>
        public static string ArchiveProject(SPWeb web, int itemId, UIHelper.ZipFileSavingPlace fileSavingPlace)
        {
            string createdZipName = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite elevSite = new SPSite(web.Site.ID))
                {
                    using (SPWeb elevWeb = elevSite.OpenWeb(web.ID))
                    {
                        elevWeb.AllowUnsafeUpdates = true;
                        Logger.WriteLog(Logger.Category.Information, "ArchiveProject", string.Format("Archive project for project id: {0} from web: {1}", itemId, web.Url));

                        SPList list = web.GetList(SPUrlUtility.CombineUrl(elevWeb.Url, ListUtilities.Urls.StoreOpenings));
                        SPListItem project = list.GetItemById(itemId);
                        string projectFolderName = ProjectUtilities.GetProjectsFolderName(elevWeb, itemId);
                        ZipUtility zip = new ZipUtility(projectFolderName);

                        foreach (string listUrl in projectLibrarieUrls)
                        {
                            ArchiveProjectData(elevWeb, listUrl, itemId, zip);
                        }

                        if (fileSavingPlace == UIHelper.ZipFileSavingPlace.LocalServerTempFolder)
                        {
                            createdZipName = zip.SavePackageToTempFolder();
                        }
                        else
                        {
                            createdZipName = zip.SavePackageToSharePoint(web, "siteassets/archives");
                        }
                    }
                }
            });

            return createdZipName;
        }

        /// <summary>
        /// Archive project releted folder to zip file
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="listUrl">Project library url</param>
        /// <param name="itemId">Project item id</param>
        public static void ArchiveProjectData(SPWeb web, string listUrl, int itemId, ZipUtility zipUtility)
        {
            Logger.WriteLog(Logger.Category.Information, "ArchiveProjectData", string.Format("Archive project folder for project id: {0} from library: {1}", itemId, listUrl));
            SPList list = null;

            try
            {
                list = web.GetList(SPUrlUtility.CombineUrl(web.Url, string.Format("Lists/{0}", listUrl)));
                SPListItemCollection items = CommonUtilities.GetFoldersByPrefix(web, list, string.Format("{0}_", itemId));



                if (items.Count > 0)
                {
                    // create folder for project content in ziped file
                    zipUtility.AddDirectoryByName(listUrl);

                    SPListItem firstItem = items[0];

                    SPQuery query = new SPQuery();
                    query.Folder = firstItem.Folder;
                    query.ViewAttributes = "Scope='RecursiveAll'";
                    SPListItemCollection projectItems = list.GetItems(query);

                    if (projectItems.Count > 0)
                    {

                        foreach (SPListItem item in projectItems)
                        {
                            try
                            {
                                if (item.FileSystemObjectType == SPFileSystemObjectType.File)
                                {
                                    string folderPath = item.File.ParentFolder.Url.Replace(firstItem.Folder.Url, string.Empty);
                                    folderPath = Path.Combine(listUrl, folderPath.Replace("/", "\\").TrimStart(('\\')));
                                    zipUtility.AddFile(item.File, folderPath);
                                }
                                else if (item.FileSystemObjectType == SPFileSystemObjectType.Folder)
                                {
                                    string folderPath = item.Folder.Url.Replace(firstItem.Folder.Url, string.Empty);
                                    folderPath = Path.Combine(listUrl, folderPath.Replace("/", "\\").TrimStart(('\\')));
                                    zipUtility.AddDirectoryByName(folderPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLog(Logger.Category.Unexpected, "ArchiveProjectFolder", "Iterate project folder files");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "ArchiveProjectFolder", "List not found");
                return;
            }
        }
    }

}
