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
        public static ProjectTask GrandOpening = new ProjectTask { Title = "Grand opening", Duration = 0, Responsible = DepartmentUtilities.StoreManager };
        public static ProjectTask EnsureExchangeMoney = new ProjectTask { Title = "Ensure exchange money", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 1};
        public static ProjectTask FinalCleaning = new ProjectTask { Title = "Final cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 1};

        /// <summary>
        /// Create project opening main tasks List
        /// </summary>
        /// <returns>Lists with main project opening tasks</returns>
        public static List<ProjectTask> CreateStoreOpeningTasks(int projectTaskId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Grand opening", Duration = 0, Responsible = DepartmentUtilities.StoreManager, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Ensure exchange money", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 1, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Final cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 1, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Visual Merchandising", Duration = 2, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 3, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 4, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Opening guide handover to Shop manager", Duration = 14, ResponsibleDepartment = DepartmentUtilities.VisualMerchandise, TimeBeforeGrandOpening = 18, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Rebuilding start", Duration = 7, Responsible = DepartmentUtilities.Contractor, TimeBeforeGrandOpening = 11, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "LATEST Premise handover (3 sets keys to RM, BC, Contractor)", Duration = 3, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening =14, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "HQ kick off meeting", Duration = 1, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 61, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Contractor start up meeting (timetable, drawings etc.)", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 62, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Final project approved (P/L, drawings, plan, Sydleasing number)", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 62, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Collect final renovation offer from contractor", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 64, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Project schedule finish", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 64, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Drawings approved", Duration = 2, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 64, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Drawings finish", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 66, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Drawings begin", Duration = 2, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 68, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Premise contract signed", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 69, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Initial P/L signed", Duration = 4, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 73, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Initial building/renovation budget", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 74, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Store design location visit (measurements etc.)", Duration = 1, Responsible = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 75, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Location search end (get DWG drawing, take pictures, premise condition at takeover)", Duration = 4, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 77, ParentId = projectTaskId });

            return tasks;
        }

        /// <summary>
        /// When new partner tasks
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> LogistikTasks(int parentTaskId, string parentTitle)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Order CLUB Change cards (when new Country/region)", Duration = 90, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 90, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date / Shop furnitures incl wallpaper", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date / fitting/ mannequin/ store supply", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Flooring, carpet for fitting room if applicable", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Light", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Facade signs to store", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Costumer counter if applicable", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Products", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Marketing POS", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Prepack/ reserve overview updated for OM", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Basic order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fashion order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Old fashion Order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order floor, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note if some products are not ordered in the shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Light, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Facade sign, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order wallpaper", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order furniture", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order sound system, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Alarmgates", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Lightbox, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order TV, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Wardrope, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order costumer counter", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Mannequin", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order carpet, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order fittings", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order store supply (incl. Store kit)", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note picking date for all Products in the shipping overview", Duration = 7, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note packing date for all Products in the shipping overview", Duration = 7, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note Shipping date for all Products in the shipping overview", Duration = 7, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note Arrival date for all Products in the shipping overview", Duration = 7, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note order numbers for all Products in the shipping overview", Duration = 0, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place basic order in AX", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place furniture order in AX", Duration = 7, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place Store supply order in AX", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place fitting order in AX", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place Mannequin order in AX", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place Fashion order in AX", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Redirect preorders in system", Duration = 10, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });

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

            tasks.Add(new ProjectTask { Title = "Order furniture", Duration = 23, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 33, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Furniture ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Furniture ETA shop", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order furniture", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 33, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Sound system ETA Farum", Duration = 2, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Sound system ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Sound system ETA Farum", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Alarmgates", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 33, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Alarmgates ETA Farum", Duration = 2, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Alarmgates ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Alarmgates ETA shop", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order costumer counter", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 33, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Costumer counters ETA Farum", Duration = 2, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Costumer counter ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Costumer counter ETA shop", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order costumer carpet", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 33, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Carpet ETA Farum", Duration = 2, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Carpet ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Carpet ETA shop", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Door mat", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Storedesign, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Door mat ETA shop", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 5, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order store fittings", Duration = 21, ResponsibleDepartment = DepartmentUtilities.VisualMerchandise, TimeBeforeGrandOpening = 31, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Store fittings ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Store fittings ETA shop", Duration = 4, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order POS and terminal system", Duration = 42, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 55, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "POS system ETA Farum", Duration = 1, TimeBeforeGrandOpening = 13, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Prepare POS system", Duration = 2, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "IT systems ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "IT systems ETA Shop", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Setup POS system in Shop", Duration = 3, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 7, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order FG products (incl. Alarms and hangers)", Duration = 21, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "FG products (incl. Alarms and hangers) ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 7, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "FG products (incl. Alarms and hangers) ETA shop", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 5, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order store supply (incl. Store kit)", Duration = 21, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Store supply (incl. Store kit) ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 7, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Store supply (incl. Store kit) ETA shop", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 5, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order VM products", Duration = 21, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 30, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "VM materials ETA Farum", Duration = 2, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 9, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "VM products ETD Farum", Duration = 2, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 7, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "VM products ETA shop", Duration = 1, ResponsibleDepartment = DepartmentUtilities.CostumersService, TimeBeforeGrandOpening = 5, ParentId = parentTaskId, ParentTitle = parentTitle });

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
