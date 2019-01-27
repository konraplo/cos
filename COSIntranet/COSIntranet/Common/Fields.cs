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
    }
}
