namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;

    /// <summary>
    /// Functionality connteced with projects opereations
    /// </summary>
    public static class ProjectHelper
    {
        public static string[] projectLibrarieUrls = { "Marketing" , "Drawings", "GeneralInformation", "Logistic", "Pictures", "Evaluation" };

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
        /// <param name="listUrl">Project library url</param>
        /// <param name="itemId">Project item id</param>
        public static void RemoveAllProjectFolder(SPWeb web, int itemId)
        {
            foreach (string listUrl in projectLibrarieUrls)
            {
                RemoveProjectFolder(web, listUrl, itemId);
            }
        }
    }
}
