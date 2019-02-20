using Change.Intranet.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;

namespace Change.Intranet.Model
{
    /// <summary>
    /// Helpermethods with project related methods and functions.
    /// </summary>
    public static class DepartmentUtilities
    {
        /// <summary>
        /// Regional manager
        /// </summary>
        public const string RegionalManager = "Regional manager";

        /// <summary>
        /// Store manager
        /// </summary>
        public const string StoreManager = "Store manager";

        /// <summary>
        /// Project coordinator
        /// </summary>
        public const string ProjectCoordinator = "Project coordinator";

        /// <summary>
        /// Contractorr
        /// </summary>
        public const string Contractor = "Contractor";

        /// <summary>
        /// Storedesign
        /// </summary>
        public const string Storedesign = "Storedesign";

        /// <summary>
        /// IT
        /// </summary>
        public const string IT = "IT";

        /// <summary>
        /// HR
        /// </summary>
        public const string HR = "HR";

        /// <summary>
        /// Costumers service
        /// </summary>
        public const string CostumersService = "Costumers service";

        /// <summary>
        /// Retail
        /// </summary>
        public const string Retail = "Retail";

        /// <summary>
        /// Accounting
        /// </summary>
        public const string Accounting = "Accounting";

        /// <summary>
        /// e-commerce
        /// </summary>
        public const string Ecommerce = "E-commerce";

        /// <summary>
        /// Warehouse
        /// </summary>
        public const string Warehouse = "Warehouse";

        /// <summary>
        /// Marketing
        /// </summary>
        public const string Marketing = "Marketing";

        /// <summary>
        /// Finance
        /// </summary>
        public const string Finance = "Finance";

        /// <summary>
        /// Planning
        /// </summary>
        public const string Planning = "Planning";

        /// <summary>
        /// Order Management
        /// </summary>
        public const string OrderManagement = "Order Management";

        /// <summary>
        /// Visual Merchandise
        /// </summary>
        public const string VisualMerchandise = "Visual Merchandise";
              
        /// <summary>
        /// Get all departments and convert to objects
        /// </summary>
        /// <param name="web">Department web</param>
        /// <returns></returns>
        public static List<Department> GetDepartments(SPWeb web)
        {
            string deptUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
            SPList deptList = web.GetList(deptUrl);
            List<Department> departments = new List<Department>();
            foreach (SPListItem deptIem in deptList.GetItems(new SPQuery()))
            {
                departments.Add(new Department { Id = deptIem.ID, Title = deptIem.Title, Manager = Convert.ToString(deptIem[Fields.ChangeDeparmentmanager]) });
            }

            return departments;
        }
    }
}
