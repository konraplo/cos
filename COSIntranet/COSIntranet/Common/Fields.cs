namespace Change.Intranet.Common
{
    using System;

    /// <summary>
    /// Ids and names of the most used site columns.
    /// </summary>
    public static class Fields
    {
        public const string ChangeFieldsGroup = "CHANGE Fields";
        public const string Department = "ChangeDepartment";
        public const string ProjectTask = "ChangeProjectTask";
        public const string StoreOpening = "ChangeStoreOpening";
        public const string Store = "ChangeStore";
        public const string Country = "ChangeCountry";
        public const string StoreId = "Storeid";
        public const string Title = "Title";

        /// <summary>
        /// GUID of ChangeTaskDuration
        /// </summary>
        public static Guid ChangeTaskDurationId = new Guid("{F3C19AC2-4AE7-45BB-940B-0AFC1CB1A05C}");

        /// <summary>
        /// GUID of ChangeDeparmentmanager
        /// </summary>
        public static Guid ChangeDeparmentmanager = new Guid("{320DC330-92FE-480B-967E-0B205A94AA7D}");

        /// <summary>
        /// GUID of Storemanager
        /// </summary>
        public static Guid ChangeStoremanager = new Guid("{71F7422B-335A-4ADC-B63A-68E8A27A546A}");

        /// <summary>
        /// GUID of ChangeTasksLink
        /// </summary>
        public static Guid ChangeProjectTasksLink = new Guid("{56587AA8-12EB-43D0-B4DA-83FB0B92CF0B}");
    }
}
