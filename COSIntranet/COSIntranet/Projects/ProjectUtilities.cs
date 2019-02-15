﻿namespace Change.Intranet.Projects
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
        public static ProjectTask GrandOpening = new ProjectTask { Title = "Grand opening", Duration = 0, Responsible = DepartmentUtilities.StoreManager };
        public static ProjectTask EnsureExchangeMoney = new ProjectTask { Title = "Ensure exchange money", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 1};
        public static ProjectTask FinalCleaning = new ProjectTask { Title = "Final cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 1};

        /// <summary>
        /// Create project opening tasks List
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> CreateStoreOpeningTasks()
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Grand opening", Duration = 0, Responsible = DepartmentUtilities.StoreManager });
            tasks.Add(new ProjectTask { Title = "Ensure exchange money", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 1 });
            tasks.Add(new ProjectTask { Title = "Final cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 1});
            tasks.Add(new ProjectTask { Title = "Visual Merchandising", Duration = 2, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 3 });
            tasks.Add(new ProjectTask { Title = "Cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 4 });
            tasks.Add(new ProjectTask { Title = "Opening guide handover to Shop manager", Duration = 14, ResponsibleDepartment = DepartmentUtilities.VisualMerchandise, TimeBeforeGrandOpening = 18 });
            tasks.Add(new ProjectTask { Title = "Rebuilding start", Duration = 7, Responsible = DepartmentUtilities.Contractor, TimeBeforeGrandOpening = 11 });
            tasks.Add(new ProjectTask { Title = "LATEST Premise handover (3 sets keys to RM, BC, Contractor)", Duration = 3, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening =14 });
            tasks.Add(new ProjectTask { Title = "HQ kick off meeting", Duration = 1, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 61 });
            tasks.Add(new ProjectTask { Title = "Contractor start up meeting (timetable, drawings etc.)", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 62 });
            tasks.Add(new ProjectTask { Title = "Final project approved (P/L, drawings, plan, Sydleasing number)", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 62 });
            tasks.Add(new ProjectTask { Title = "Collect final renovation offer from contractor", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 64 });
            tasks.Add(new ProjectTask { Title = "Project schedule finish", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 64 });
            tasks.Add(new ProjectTask { Title = "Drawings approved", Duration = 2, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 64 });
            tasks.Add(new ProjectTask { Title = "Drawings finish", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 66 });
            tasks.Add(new ProjectTask { Title = "Drawings begin", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 68 });
            tasks.Add(new ProjectTask { Title = "Premise contract signed", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 69 });
            tasks.Add(new ProjectTask { Title = "Initial P/L signed", Duration = 4, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 73 });
            tasks.Add(new ProjectTask { Title = "Initial building/renovation budget", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 74 });
            tasks.Add(new ProjectTask { Title = "Store design location visit (measurements etc.)", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 75 });
            tasks.Add(new ProjectTask { Title = "Location search end (get DWG drawing, take pictures, premise condition at takeover)", Duration = 4, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 77 });

            return tasks;
        }

        /// <summary>
        /// When new partner tasks
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> WhenNewPartnerTasks(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Country/region setup in system", Duration = 2, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Presentation of POS, Navision and Intranet", Duration = 2, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create pricelist", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Setup sales reports", Duration = 2, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order CLUB Change cards (when new region)", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// White Box Handover tasks
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> WhiteBoxHandoverTasks(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Sign QS document", Duration = 0, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 4, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "FG delivery sheet", Duration = 0, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 4, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Create costumer in system tasks
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> CreateCostumerInSystemTasks(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Create costumer in Navision (add similar shop)", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create costumer in Infostore (Incl. Replanishment setup, mailaddress)", Duration = 1, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create/order phone line/data line (Update project sheet)", Duration = 42, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 49, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Ensure first FASHION order", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create/change address in Globase ", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Administration tasks
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> AdministrationTasks(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Create bank account", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 50, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create night deposit agreement", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order bank bads and codes to the bank", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order KODA/licence to play music", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order ensurance", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Staff hiring", Duration = 2, ResponsibleDepartment = DepartmentUtilities.HR, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order theft alarm", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order renovation and garbage aggreements", Duration = 2, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Register electricity, heat and water", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order ad for local media (Grand opening + 3 months)", Duration = 2, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 59, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create EDM", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Rebuilding period
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> RebuildingPeriod(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Order floor", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 39, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Floor ETA Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Floor ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Floor ETA shop", Duration = 3, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Light", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 39, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Light ETA Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Light ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 16, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Light ETA shop", Duration = 3, Responsible = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order all signs", Duration = 42, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Signs ETA Farum", Duration = 2, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "All signs ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 16, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "All signs ETA shop", Duration = 3, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "All signs ETA shop", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 37, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Wallpaper ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 16, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Wallpaper ETA shop", Duration = 3, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Get store country lookup value
        /// </summary>
        /// <param name="web"></param>
        /// <param name="storeItemId"></param>
        /// <returns></returns>
        public static string GetStoreCountry(SPWeb web, int storeItemId)
        {
            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            SPList storestList = web.GetList(storesUrl);
            SPListItem storeItem = storestList.GetItemById(storeItemId);
            SPFieldLookupValue country = new SPFieldLookupValue(Convert.ToString(storeItem[Fields.Country]));
            return string.Format("{0};#{1}", country.LookupId, country.LookupValue);
        }

        /// <summary>
        /// Get store country lookup value
        /// </summary>
        /// <param name="web"></param>
        /// <param name="storeItemId"></param>
        /// <returns></returns>
        public static string GetStoreManager(SPWeb web, int storeItemId)
        {
            string storesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Stores);
            SPList storestList = web.GetList(storesUrl);
            SPListItem storeItem = storestList.GetItemById(storeItemId);
            return Convert.ToString(storeItem[Fields.ChangeStoremanager]);
        }

        /// <summary>
        /// Get project country lookup value
        /// </summary>
        /// <param name="web"></param>
        /// <param name="projectItemId"></param>
        /// <returns></returns>
        public static string GetProjectCountry(SPWeb web, int projectItemId)
        {
            string storeOpeningsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.StoreOpenings);
            SPList storeOpeningsList = web.GetList(storeOpeningsUrl);
            SPListItem storeItem = storeOpeningsList.GetItemById(projectItemId);
            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(storeItem[Fields.Store]));
            return GetStoreCountry(web, store.LookupId);
        }
    }
}
