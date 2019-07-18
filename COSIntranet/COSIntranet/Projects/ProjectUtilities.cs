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
        /// Create project opening milestones List
        /// </summary>
        /// <returns>Lists with milestones tasks</returns>
        public static List<ProjectTask> CreateMilestoneTasks(int projectTaskId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Project finished - handover to relevant departments", Duration = 14, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = -14, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "HQ kick off meeting", Duration = 35, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 61, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "LATEST Premise handover (3 sets keys to RM, BC, Contractor)", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 17, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Building starts", Duration = 10, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 17, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Whitebox handover at 10 AM", Duration = 0, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 7, ParentId = projectTaskId });
            tasks.Add(new ProjectTask { Title = "Grand opening", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 0, ParentId = projectTaskId });

            return tasks;
        }

        /// <summary>
        /// logistik tasks
        /// </summary>
        /// <returns>Lists with all logistik tasks</returns>
        public static List<ProjectTask> LogistikTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Order CLUB Change cards (when new Country/region)", Duration = 90, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 90, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date / Shop furnitures incl wallpaper", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date / fitting/ mannequin/ store supply", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Flooring, carpet for fitting room if applicable", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Light", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Facade signs to store", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Costumer counter if applicable", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Products", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Arrival date/ Marketing POS", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Prepack/ reserve overview updated for OM", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Basic order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fashion order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Old fashion Order created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Planning, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order floor, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note if some products are not ordered in the shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Light, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Facade sign, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order wallpaper", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order furniture", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order sound system, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Alarmgates", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Lightbox, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order TV, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Wardrope, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order costumer counter", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Mannequin", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order carpet, note ETA warehouse in shipping overview", Duration = 21, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 47, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order fittings", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order store supply (incl. Store kit)", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
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
            tasks.Add(new ProjectTask { Title = "Order VM POS, note ETA Warehouse in Shipping overview", Duration = 28, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 44, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place print order in Marketing if print is needed", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 46, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "1 st shipment Mesurements and weight of all the goods are send to Order Management", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Warehouse, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Retail confirmed first delivery", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 19, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "1st order shipped from Warehouse", Duration = shippingDays, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "2st order shipped from Warehouse", Duration = shippingDays, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 16, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "2nd shipmentMesurements and weight of all the goods are send to Order Management", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Warehouse, TimeBeforeGrandOpening = 18, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "1st shipment Upload Pictures and Pallet info", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Warehouse, TimeBeforeGrandOpening = 26, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Retail confirmed second delivery", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 9, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Post Grand opening tasks
        /// </summary>
        /// <returns>Lists with all Post Grand opening tasks</returns>
        public static List<ProjectTask> PostGrandOpeningTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Sign QS document", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = -1, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Evaluation meetings with departments", Duration = 15, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = -6, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Reparing issues, if any", Duration = 14, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = -14, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Preperation of store
        /// </summary>
        /// <returns>Lists with all Preperation of store tasks</returns>
        public static List<ProjectTask> PreperationOfStoreTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "VM opening guide uploaded", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Purchase, bathroom & Kitchen tasks
        /// </summary>
        /// <returns>Lists with all Purchase, bathroom & Kitchen tasks</returns>
        public static List<ProjectTask> PurchaseBathroomKitchenTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Soap", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Towels", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Kithsentovels", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Knifes, forkes, spoons", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Coffemachine", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Plates", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Glasses", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Cups", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Garbage bags big and small ", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Toilet paper", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Toilet cleaner", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Dish washing", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Purchase, Cleaning tasks
        /// </summary>
        /// <returns>Lists with all Purchase, Cleaning tasks tasks</returns>
        public static List<ProjectTask> PurchaseCleaningTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Vacuumcleaner and bags", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Swab for the floor plus proper cleaning products for the woodenfloors", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Windowcleaner", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Windowwiper", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Dusters", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Purchase, office equipment tasks
        /// </summary>
        /// <returns>Lists with all Purchase, office equipment tasks tasks</returns>
        public static List<ProjectTask> PurchaseOfficeEquipmentTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Tape", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Stapler", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "A4 paper", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Calendar", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Scissors", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Carpenterknife", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Pens", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Calculator", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Dymo", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Rebuilding period - builing tasks
        /// </summary>
        /// <returns>Lists with all Rebuilding period - builing tasks</returns>
        public static List<ProjectTask> RebuildingPeriodBuilingTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Fill existing Celing", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 13, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Paint existing ceiling", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount new ceiling", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount floor", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 11, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Build new walls", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fine filler the walls", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 13, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Rough filler the walls", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 13, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Paint walls", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Stock in backroom", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Facade Paint the Facade", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 12, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Mount foil on the window (hoarding)", Duration = 0, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 7, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Rebuilding period - Electricity tasks
        /// </summary>
        /// <returns>Lists with all Rebuilding period - Electricity tasks</returns>
        public static List<ProjectTask> RebuildingPeriodElectricityTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Mount light", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount EPOS", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fitting room Mount Mirror", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Facade Mount Facade sign", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Facade Mount wing sign", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Facade Mount new awning", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Set up TV", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Set up lightbox", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Mount Roller blind", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Mount alarmgates and test these", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Mount costumer counter and test this", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Rebuilding period - Mounting tasks
        /// </summary>
        /// <returns>Lists with all Rebuilding period - Mounting tasks</returns>
        public static List<ProjectTask> RebuildingPeriodMountingTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Mount all interior", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount Wardrope", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Counter", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount wallpaper in store", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Mount logo in store", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fitting room Mount wallpaper", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fitting room Mount frames (only in NO)", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fitting room Mount carpet", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Fitting room Mount hooks", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Window Mount grid in the window for posters", Duration = 2, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 10, ParentId = parentTaskId, ParentTitle = parentTitle });
           
            return tasks;
        }

        /// <summary>
        /// Rebuilding period - demolition tasks
        /// </summary>
        /// <returns>Lists with all Rebuilding period - demolition tasks</returns>
        public static List<ProjectTask> RebuildingPeriodDemolitionTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Remove existing floor", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Remove existing light", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Remove existing walls", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 15, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Store preperation
        /// </summary>
        /// <returns>Lists with all Store preperation tasks</returns>
        public static List<ProjectTask> StorePreperationTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Cleaning", Duration = 1, Responsible = DepartmentUtilities.StoreManager, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 6, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Visual Merchandising", Duration = 4, Responsible = DepartmentUtilities.StoreManager, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 8, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Final cleaning", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.StoreManager, TimeBeforeGrandOpening = 1, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Ensure exchange money", Duration = 3, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 3, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// System preperation
        /// </summary>
        /// <returns>Lists with all System preperation tasks</returns>
        public static List<ProjectTask> SystemPreperationTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Adding the store KPIs and sales budget in PBI / AX", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Country / region setup in system (only when new market)", Duration = 0, ResponsibleDepartment = DepartmentUtilities.IS, TimeBeforeGrandOpening = 61, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create store in AX", Duration = 0, ResponsibleDepartment = DepartmentUtilities.IS, TimeBeforeGrandOpening = 61, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create Store Email", Duration = 7, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create store manager Email", Duration = 7, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order phone, note Store ETD in Shipment overview", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order phone line and add number in information sheet", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Internet and add IP adress in Information sheet", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Phone, note Store ETD in Shipment overview", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 60, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Replanishment setup", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Set assortment (when new Country/region)", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order POS package", Duration = 14, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 39, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order Printer", Duration = 14, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 39, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "POS package ETA Warehouse", Duration = 2, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 25, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Prepare POS system", Duration = 7, ResponsibleDepartment = DepartmentUtilities.IT, TimeBeforeGrandOpening = 23, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "POS invoice created", Duration = 2, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create store in EDM database", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Ecommerce, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Internet installation in store", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 9, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create/change address on WEB", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Ecommerce, TimeBeforeGrandOpening = 0, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create Store to google maps", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Ecommerce, TimeBeforeGrandOpening = 0, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create store so they are visible on the WEBsite map", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Ecommerce, TimeBeforeGrandOpening = 0, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order card terminal, note Store ETD in Shipment overview", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create Store in HR system", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create employees in HR system", Duration = 1, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Project preperation tasks
        /// </summary>
        /// <returns>Lists with all Project preperation tasks</returns>
        public static List<ProjectTask> ProjectPreperationTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Location search end (get DWG drawing, take pictures, premise condition at takeover)", Duration = 1,Responsible = DepartmentUtilities.RegionalManager, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 69, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "P/L signed ", Duration = 1,Responsible = DepartmentUtilities.RegionalManager, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 69, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Premise contract signed", Duration = 1,Responsible = DepartmentUtilities.RegionalManager, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 69, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Location visit (measurements etc.)", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 50, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Initial building/renovation budget", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 68, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings period", Duration = 7, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 49, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, In store", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, Facade", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, Light", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, Ventilation", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, AC", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by CHANGE, Firesystem", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, In store", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, Facade", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, Light", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 42, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, Ventilation", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, AC", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Drawings approved by Center, Firesystem", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Project schedule finish and Frontpage ready", Duration = 2, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 40, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Collect final renovation offer from contractor", Duration = 1, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 62, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Finalize budget for Project", Duration = 1, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 62, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Budget approved", Duration = 1, Responsible = DepartmentUtilities.RegionalManager, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 62, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Collect contractor offers", Duration = 7, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 68, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Contactor offer approved", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 62, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Contractor start up meeting (timetable, drawings etc.)", Duration = 7, Responsible = DepartmentUtilities.ProjectCoordinator, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Documents send to the center for approval", Duration = 7, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Handover dwg files to architect", Duration = 1, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 69, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Building permit granted", Duration = 7, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Administration tasks
        /// </summary>
        /// <returns>Lists with all Administration tasks</returns>
        public static List<ProjectTask> AdministrationTasks(int parentTaskId, string parentTitle, int shippingDays)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Fill out FrontPage", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 68, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Introduction to CHANGE (Presentation of POS, AX and Intranet) (Only when new market)", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create pricelist (Only when new market)", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Provided Project number (assets under construction) in AX, created by Accounting, for project for leasing agreament, use this for orders", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Accounting, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create bank account in AX ", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Accounting, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Noted that turnover are to be delivered to the landlord according to rental agreement", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Accounting, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Noted that  Auditor endorsement of annual revenue are to be delivered to landlord according to the rental agreement", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Accounting, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Leasing COS AS to Retail/Franchisee - Validate if sale or lease from COS AS", Duration = 7, Responsible = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Leasing - financing by COS AS: validate if sale and leaseback from leasing company (Sydleasing or other)", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Note weekly order date for the store in Information sheet (Order management, planning and Warehouse decide", Duration = 7, Responsible = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create Marketing plan for Grand opening period", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create bank account", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create night deposit agreement", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order bank bags and codes to the bank", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order music licence to play music in the store", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager,  TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order ensurance", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Hired store manager", Duration = 35, ResponsibleDepartment = DepartmentUtilities.HR , TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Hired store staff", Duration = 35, ResponsibleDepartment = DepartmentUtilities.HR, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order theft alarm", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create work scedule for store", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Finance, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create sales budget for the store", Duration = 21, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Order renovation and garbage aggreements", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Register electricity, heat and water", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 54, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Marketing plan approved by International HQ", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place order in Marketing department", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Place Marketing order at local vendors", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Reinvoice from external vendors", Duration = 2, ResponsibleDepartment = DepartmentUtilities.OrderManagement, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "2nd shipment  Upload Pictures and Pallet info", Duration = 0, ResponsibleDepartment = DepartmentUtilities.Warehouse, TimeBeforeGrandOpening = 16, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Upload Center marketing plan and posiibilies", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 28, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create Marketing plan for the rest of the year", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Retail, Responsible = DepartmentUtilities.RegionalManager, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Deliver marketing material to retail", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 21, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create content for WEB", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create all EDM", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Create content for social media", Duration = 14, ResponsibleDepartment = DepartmentUtilities.Marketing, TimeBeforeGrandOpening = 14, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Onboard all staff", Duration = 7, ResponsibleDepartment = DepartmentUtilities.Retail, TimeBeforeGrandOpening = 35, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Upload banner on WEB", Duration = 2, ResponsibleDepartment = DepartmentUtilities.Ecommerce, TimeBeforeGrandOpening = 2, ParentId = parentTaskId, ParentTitle = parentTitle });
            tasks.Add(new ProjectTask { Title = "Handover combined invoice overview for leasing/evaluate ttl. cost", Duration = 14, ResponsibleDepartment = DepartmentUtilities.RetailDevelopment, Responsible = DepartmentUtilities.ProjectCoordinator, TimeBeforeGrandOpening = -14, ParentId = parentTaskId, ParentTitle = parentTitle });

            return tasks;
        }

        /// <summary>
        /// Rebuilding period
        /// </summary>
        /// <returns>Lists with all Rebuilding period tasks</returns>
        public static List<ProjectTask> RebuildingPeriod(int parentTaskId, string parentTitle, int shippingDays)
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

        /// <summary>
        /// Archive project releted folder to zip file
        /// </summary>
        /// <param name="web">Busines dev web</param>
        /// <param name="itemId">Project item id</param>
        public static string GetProjectsFolderName(SPWeb web, int projectId)
        {
            string projectFolderName = string.Empty;
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, ListUtilities.Urls.StoreOpenings));
            SPListItem projectItem = list.GetItemById(projectId);

            SPFieldLookupValue store = new SPFieldLookupValue(Convert.ToString(projectItem[Fields.Store]));
            SPFieldLookupValue storeCountry = new SPFieldLookupValue(Projects.ProjectUtilities.GetStoreCountry(projectItem.Web, store.LookupId));
            string type = Convert.ToString(projectItem[Fields.ChangeProjectCategory]);
            projectFolderName = string.Format("{0}_{1}_{2}_{3}", projectItem.ID, store.LookupValue, storeCountry.LookupValue, type);

            return projectFolderName;
        }
    }
}
