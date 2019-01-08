using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.Intranet.Common
{
    /// <summary>
    /// Ids and names of the most used site columns.
    /// </summary>
    public static class Fields
    {
        public const string ChangeFieldsGroup = "CHANGE Fields";
        public const string Department = "ChangeDepartment";
        public const string ProjectTask = "ChangeProjectTask";
        public const string Store = "ChangeStore";
        public const string StoreId = "Storeid";
        public const string Title = "Title";

        /// <summary>
        /// GUID of ChangeCountry ItemId
        /// </summary>
        public static Guid ChangeCountryId = new Guid("{B5F2AFCA-C006-4EF9-AE52-5390C6865D2D}");
    }
}
