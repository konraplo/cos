namespace Change.Intranet.Projects
{
    using Change.Intranet.Common;
    using Change.Intranet.Model;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helpermethods with project related methods and functions.
    /// </summary>
    public static class ProjectUtilities
    {
        /// <summary>
        /// Regional manager
        /// </summary>
        private const string RegionalManager = "Regional manager";

        /// <summary>
        /// Storedesign
        /// </summary>
        private const string Storedesign = "Storedesign";

        /// <summary>
        /// Create project opening tasks List
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> CreateStoreOpeningTasks()
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Location search end (get DWG drawing, take pictures, premise condition at takeover)", Duration = 2, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Store design location visit (measurements etc.)", Duration = 1, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Initial building/renovation budget", Duration = 2, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Initial P/L signed", Duration = 1, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Premise contract signed", Duration = 4, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Drawings begin", Duration = 1, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Drawings finish", Duration = 2, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Drawings approved", Duration = 2, Responsible = RegionalManager });
            return tasks;
        }

        public static string GetStoreCountry(SPWeb web, int storeItemId)
        {
            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            SPList storestList = web.GetList(storesUrl);
            SPListItem storeItem = storestList.GetItemById(storeItemId);
            
            return Convert.ToString(storeItem[Fields.ChangeCountryId]);
        }

        public static string GetProjectCountry(SPWeb web, int projectItemId)
        {
            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            SPList tasksList = web.GetList(tasksUrl);
            SPListItem storeItem = tasksList.GetItemById(projectItemId);
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(storeItem[Fields.Store]));
            return GetStoreCountry(web, store.LookupId);
        }
    }
}
