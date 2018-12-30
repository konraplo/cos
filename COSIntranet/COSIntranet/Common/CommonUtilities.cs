namespace Change.Intranet.Common
{
    using System;
    using Microsoft.SharePoint;

    /// <summary>
    /// Helpermethods with solutionwide accessible methods and functions.
    /// </summary>
    public static class CommonUtilities
    {
        /// <summary>
        /// This method creates a lookup site column if not exists.
        /// </summary>
        /// <param name="web">The Web object.</param>
        /// <param name="groupName">Name of the group, where the filed should be listet.</param>
        /// <param name="fieldName">Internal name of the field.</param>
        /// <param name="lookupFieldDisplayName">Name of the Field, from that the lookup should get its data.</param>
        /// <param name="lookupFieldName"> </param>
        /// <param name="lookupList">List object that should be used as the lookup data source.</param>
        /// <param name="required">Flag, if the field is required.</param>
        /// <param name="allowMultipleValues">Flaf if multiple values are allowed.</param>
        public static SPFieldLookup CreateLookupField(SPWeb web, string groupName, string fieldName, string lookupFieldDisplayName, string lookupFieldName, SPList lookupList, bool required, bool allowMultipleValues)
        {
            try
            {
                if (web == null)
                {
                    Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName, "Helper.CreateLookupField: Parameter 'web' is Null or empty.");
                    throw new ArgumentNullException("web");
                }
                if (String.IsNullOrEmpty(groupName))
                {
                    Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName,"Helper.CreateLookupField:Parameter 'groupName' is Null or empty.");
                }
                if (String.IsNullOrEmpty(fieldName))
                {
                    Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName,"Helper.CreateLookupField:Parameter 'fieldName' is Null or empty.");
                    throw new ArgumentNullException("fieldName");
                }
                if (String.IsNullOrEmpty(lookupFieldDisplayName))
                {
                    Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName,String.Format("Helper.CreateLookupField:Parameter {0} is Null or empty.", "lookupFieldName"));
                    throw new ArgumentNullException("lookupFieldName", String.Format("Parameter {0} is Null or empty.", "lookupFieldName"));
                }
                if (lookupList == null)
                {
                    Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName,"Helper.CreateLookupField:Parameter 'lookupList' is Null or empty.");
                    throw new ArgumentNullException("lookupList");
                }

                SPFieldLookup lookUp = null;

                using (SPSite site = new SPSite(web.Site.ID))
                {
                    using (SPWeb rootWeb = site.OpenWeb(site.RootWeb.ID))
                    {


                        if (!rootWeb.Fields.ContainsField(fieldName))
                        {
                            fieldName = rootWeb.Fields.AddLookup(fieldName, lookupList.ID, lookupList.ParentWeb.ID, required);
                        }

                        lookUp = (SPFieldLookup)rootWeb.Fields[fieldName];
                        lookUp.AllowMultipleValues = allowMultipleValues;
                        lookUp.Group = groupName;
                        lookUp.Title = lookupFieldDisplayName;
                        lookUp.LookupField = lookupFieldName;//rootWeb.Fields[SPBuiltInFieldId.Title].Title;
                        lookUp.Update(true);

                        Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName,String.Format("Lookupfield {0} created and updated.lookUp.LookupField = {1}", lookUp.Title, lookUp.LookupField));
                    }
                }

                return lookUp;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,String.Format("Helper.CreateLookupField: Error during lookupfieldcreation:{0}", ex.Message));
                throw ex;

            }
        }


    }
}
