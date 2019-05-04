using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;

namespace Change.Intranet.Common
{
    /// <summary>
    /// Uiltis for list operations
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// resx key for project created notification subject
        /// </summary>
        public const string ChangeProjectCreatedMailSubject = "ChangeProjectCreatedMailSubject";

        /// <summary>
        /// resx key for project created notification body
        /// </summary>
        public const string ChangeProjectCreatedMailBody = "ChangeProjectCreatedMailBody";

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
            public const string Stores = "Lists/Stores";
            public const string Departments = "Lists/Departments";
            public const string ProjectTasks = "Lists/ProjectTasks";
            public const string StoreOpenings = "Lists/StoreOpenings";
            public const string Countries = "Lists/Countries";
        }

        /// <summary>
        /// This Method creates a lookupfield at a specified list.
        /// </summary>
        /// <param name="web">The Web where the list with the lookup values is stored</param>
        /// <param name="internalFieldName">name of the lookup field that should be created</param>
        /// <param name="fieldResource">Resource key of</param>
        /// <param name="currentList">List, where the lookupfield has to be added.</param>
        /// <param name="lookupFieldName">Name of the Field where the lookup values are stored in</param>
        /// <param name="lookupList">List where the lookup values are stored in</param>
        /// <param name="required">True, if required.</param>
        /// <param name="allowMultipleValues">If the selection of multiple values is allowed this parameter has to be set to true.</param>
        /// <returns>The newly created field. In Case of an Exception this method returns NULL.</returns>
        public static SPFieldLookup CreateLookupFieldAtList(SPWeb web, string internalFieldName, string fieldResource, SPList currentList, string lookupFieldName, SPList lookupList, bool required, bool allowMultipleValues)
        {
            try
            {
                if (web == null)
                {
                    throw new ArgumentNullException("web", String.Format("Parameter {0} is Null or empty.", "web"));
                }
                if (String.IsNullOrEmpty(internalFieldName))
                {
                    throw new ArgumentNullException("internalFieldName", String.Format("Parameter {0} is Null or empty.", "internalFieldName"));
                }
                if (String.IsNullOrEmpty(fieldResource))
                {
                    throw new ArgumentNullException("fieldResource", String.Format("Parameter {0} is Null or empty.", "fieldResource"));
                }

                if (null == currentList)
                {
                    throw new ArgumentNullException("currentList", String.Format("Parameter {0} is Null or empty.", "currentlistUrl"));
                }

                if (String.IsNullOrEmpty(lookupFieldName))
                {
                    throw new ArgumentNullException("lookupFieldName", String.Format("Parameter {0} is Null or empty.", "lookupFieldName"));
                }

                if (null == lookupList)
                {
                    throw new ArgumentNullException("lookupList", String.Format("Parameter {0} is Null or empty.", "lookupListUrl"));
                }


                SPFieldLookup lookUp = null;
                Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Instantiate Lists");

                Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Check if the lookup field is currently existing.");
                if (currentList.Fields.ContainsField(internalFieldName))
                {
                    try
                    {
                        Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Try to delete the field");
                        lookUp = (SPFieldLookup)currentList.Fields[internalFieldName];
                        lookUp.Delete();
                        currentList.Update();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Error trying to delete lookupfield.");
                    }
                }

                Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Start creating lookupfield");
                currentList.Fields.AddLookup(internalFieldName, lookupList.ID, required);
                lookUp = (SPFieldLookup)currentList.Fields[internalFieldName];
                lookUp.AllowMultipleValues = allowMultipleValues;
                lookUp.LookupField = lookupFieldName;
                lookUp.Title = fieldResource;
                lookUp.Update(true);

                return lookUp;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Information, "CreateLookupFieldAtList", "Error during lookupfieldcreation:" + ex.Message);
                throw;
            }
        }
    }
}
