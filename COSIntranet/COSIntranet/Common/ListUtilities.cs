using System.Collections.Generic;

namespace Change.Intranet.Common
{
    /// <summary>
    /// Uiltis for list operations
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Dictionrary with mapping ListTitleResxKey - List url
        /// </summary>
        public static Dictionary<string, string> ListUrlMappings =  new Dictionary<string, string>
                                                                    {
                                                                        { "ChangeLibTitleVisualMerchandise", Urls.VisualMerchandise },
                                                                        { "ChangeLibTitleChangeAcademy", Urls.ChangeAcademy },
                                                                        { "ChangeLibTitleDailyOperation", Urls.DailyOperation },
                                                                        { "ChangeLibTitleFinance", Urls.Finance },
                                                                        { "ChangeLibTitleHR", Urls.HR },
                                                                        { "ChangeLibTitleIT", Urls.IT },
                                                                        { "ChangeLibTitleManagers", Urls.Managers },
                                                                        { "ChangeLibTitleMarketing", Urls.Marketing },
                                                                        { "ChangeLibTitleProductAssortment", Urls.ProductAssortment },
                                                                        { "ChangeLibTitleSalesTraining", Urls.SalesTraining }
                                                                    };

        /// <summary>
        /// Url for list/document libs in National subsites
        /// </summary>
        public static class Urls
        {
            public const string VisualMerchandise = "VisualMerchandise";
            public const string ChangeAcademy = "ChangeAcademy";
            public const string DailyOperation = "DailyOperation";
            public const string Finance = "Finance";
            public const string HR = "HR";
            public const string IT = "IT";
            public const string Managers = "Managers";
            public const string Marketing = "Marketing";
            public const string ProductAssortment = "ProductAssortment";
            public const string SalesTraining = "SalesTraining";
        }
    }
}
