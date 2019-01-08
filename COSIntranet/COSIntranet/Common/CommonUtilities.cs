﻿namespace Change.Intranet.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.SharePoint;

    /// <summary>
    /// Helpermethods with solutionwide accessible methods and functions.
    /// </summary>
    public static class CommonUtilities
    {
        /// <summary>
        /// batch comand to delete items
        /// </summary>
        private const string BATCH_DELETE_ITEM_CMD = "<Method><SetList Scope=\"Request\">{0}</SetList><SetVar Name=\"ID\">{1}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";

        /// <summary>
        /// batch comand to update items
        /// </summary>
        public const string BATCH_UPDATE_ITEM_CMD = "<Method ID=\"{0}\">" +
                    "<SetList>{1}</SetList>" +
                    "<SetVar Name=\"Cmd\">Save</SetVar>" +
                    "<SetVar Name=\"ID\">{2}</SetVar>" +
                    "{3}" +
                    "</Method>";
        /// <summary>
        /// batch row used to update items
        /// </summary>
        public const string BATCH_ADD_ITEM_CMD = "<Method ID=\"{0}\">" +
                   "<SetList>{1}</SetList>" +
                   "<SetVar Name=\"Cmd\">Save</SetVar>" +
                   "<SetVar Name=\"ID\">New</SetVar>" +
                   "{2}" +
                   "</Method>";

        /// <summary>
        /// batch row used to set values for item in batch commands
        /// </summary>
        public const string BATCH_ITEM_SET_VAR = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";

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

                        lookUp = (SPFieldLookup)rootWeb.Fields.GetFieldByInternalName(fieldName);
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

        /// <summary>
        /// Attach specified content type to the specified list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="contentType"></param>
        /// <param name="pSetAsDefualtContentType">indicates if this content type should be default one in list</param>
        /// <param name="pContentTypesEnabled">indicates if after this content types management in list is active or not</param>
        public static SPContentType AttachContentTypeToList(SPList list, SPContentType contentType, bool pSetAsDefualtContentType, bool pContentTypesEnabled)
        {
            //check preconditions
            if (list == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,"Helper.AttachContentTypeToList:Parameter 'list' is NULL.");
                throw new ArgumentNullException("list");
            }
            if (contentType == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,"Helper.AttachContentTypeToList:Parameter 'contentType' is NULL.");
                throw new ArgumentNullException("contentType");
            }

            SPContentType addedCts = null;
            try
            {
                using (SPSite site = new SPSite(list.ParentWeb.Site.ID))
                {
                    using (SPWeb rootWeb = site.OpenWeb(list.ParentWeb.ID))
                    {

                        Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName, "list.ContentTypesEnabled = true;");
                        list = rootWeb.Lists[list.ID];
                        list.ContentTypesEnabled = true;
                        SPContentType foundedCts = list.ContentTypes[list.ContentTypes.BestMatch(contentType.Id)];
                        if (!(foundedCts.Parent.Id == contentType.Id))
                        {
                            Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName, ("ContentType not found, so add it"));
                            addedCts = list.ContentTypes.Add(contentType);
                            Logger.WriteLog(Logger.Category.Information, typeof(CommonUtilities).FullName, "Update List");
                            list.Update();
                        }
                        else
                        {
                            addedCts = foundedCts;
                        }

                        if (addedCts != null && pSetAsDefualtContentType)
                        {
                            SetContentTypeAsDefault(list.ParentWeb.Lists[list.ID], addedCts.Id);
                        }

                        return addedCts;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Set specified content type as default for pList
        /// </summary>
        /// <param name="pList">list</param>
        /// <param name="pCtsID">content type id</param>
        public static void SetContentTypeAsDefault(SPList pList, SPContentTypeId pCtsID)
        {
            //check preconditions
            if (pList == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName, "Helper.AttachContentTypeToList:Parameter 'pList' is NULL.");
                throw new ArgumentNullException("pList");
            }

            SPContentType[] newContentTypeOrderArray = new SPContentType[pList.RootFolder.ContentTypeOrder.Count];
            newContentTypeOrderArray[0] = pList.ContentTypes[pCtsID];

            if (newContentTypeOrderArray[0] == null)
            {
                string msg = String.Format("Helper.SetContentTypeAsDefault:wrong contentype guid:{0}.", pCtsID);
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName, msg);

                throw new ArgumentException(msg, "pCtsID");
            }

            int counter = 1;
            foreach (SPContentType cts in pList.RootFolder.ContentTypeOrder)
            {
                if (cts.Id == pCtsID)
                {
                    continue;
                }

                if (counter >= newContentTypeOrderArray.Length)
                {
                    break;
                }
                newContentTypeOrderArray[counter] = cts;
                counter++;
            }

            pList.RootFolder.UniqueContentTypeOrder = newContentTypeOrderArray;
            pList.RootFolder.Update();
        }

        /// <summary>
        /// Add specified field to content type (or update existing with specified props)
        /// </summary>
        /// <param name="pWeb"></param>
        /// <param name="pContentType"> </param>
        /// <param name="pField"></param>
        /// <param name="pRequired">should this field be required or not</param>
        /// <param name="pReadOnly"> </param>
        public static void AddFieldToContentType(SPWeb pWeb, SPContentType pContentType, SPField pField, bool pRequired, bool pReadOnly, string pDisplayName)
        {
            //preconditions
            if (pWeb == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,"Helper.AttachContentTypeToList:Parameter 'web' is NULL.");
                throw new ArgumentNullException("pWeb");
            }

            if (pField == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,"Helper.AttachContentTypeToList:Parameter 'pField' is NULL.");
                throw new ArgumentNullException("pField");
            }

            if (pContentType == null)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(CommonUtilities).FullName,"Helper.AttachContentTypeToList:Parameter 'pField' is NULL.");
                throw new ArgumentNullException("pField");
            }

            using (SPSite site = new SPSite(pWeb.Site.ID))
            {
                using (SPWeb rootWeb = site.OpenWeb(site.RootWeb.ID))
                {
                    rootWeb.AllowUnsafeUpdates = true;
                    SPFieldLink fieldLink;
                    if (!pContentType.Fields.Contains(pField.Id))
                    {

                        fieldLink = new SPFieldLink(pField);
                        
                        pContentType.FieldLinks.Add(fieldLink);
                        
                    }
                    else
                    {
                        fieldLink = pContentType.FieldLinks[pField.Id];
                    }

                    fieldLink.Required = pRequired;
                    fieldLink.DisplayName = string.IsNullOrEmpty(pDisplayName) ? pField.Title : pDisplayName;

                    if (pRequired)
                    {
                        fieldLink.ReadOnly = false;
                    }
                    else
                    {
                        fieldLink.ReadOnly = pReadOnly;

                    }

                    SPContentType checkContentType = rootWeb.AvailableContentTypes[pContentType.Id];
                    pContentType.Update(null != checkContentType);
                    rootWeb.AllowUnsafeUpdates = false;
                }
            }
        }

        /// <summary>
        /// add event receiver to spcified list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <param name="className"></param>
        /// <param name="synchronous"></param>
        public static void AddListEventReceiver(SPList list, SPEventReceiverType type, string assembly, string className, bool synchronous)
        {
            using (SPSite site = new SPSite(list.ParentWeb.Site.ID))
            {
                using (SPWeb rootWeb = site.OpenWeb(list.ParentWeb.ID))
                {
                    list = rootWeb.Lists[list.ID];
                    DeleteListEventReceiver(list, type);


                    list.EventReceivers.Add(type,
                                           assembly,
                                           className);

                    if (synchronous)
                    {
                        foreach (SPEventReceiverDefinition receiver in list.EventReceivers)
                        {
                            if (receiver.Type == type)
                            {
                                receiver.Synchronization = SPEventReceiverSynchronization.Synchronous;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// remove event receiver from specified list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        public static void DeleteListEventReceiver(SPList list, SPEventReceiverType type)
        {
            foreach (SPEventReceiverDefinition evt in list.EventReceivers)
            {
                if (evt.Type == type)
                {
                    evt.Delete();
                    break;
                }
            }

            list.Update();
        }

        /// <summary>
        /// break role ingeritance and assigne permission to specified item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="principal">SPUser or SPGroup</param>
        /// <param name="roleType"></param>
        public static void AssignPermissionsToItem(SPListItem item, SPPrincipal principal, SPRoleType roleType)
        {
            if (!item.HasUniqueRoleAssignments)
            {
                item.BreakRoleInheritance(false, true);
            }

            SPRoleAssignment roleAssignment = new SPRoleAssignment(principal);
            SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

            item.RoleAssignments.Add(roleAssignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="web"></param>
        /// <param name="formatedAddBatchCommands"></param>
        /// <returns></returns>
        public static string BatchAddListItems(SPWeb web, List<string> formatedAddBatchCommands)
        {
            StringBuilder methodBuilder = new StringBuilder();

            string batch = String.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            foreach (string item in formatedAddBatchCommands)
            {
                methodBuilder.Append(item);
            }

            // put the pieces together.
            //string method = string.Format(methodFormat, itemId, listId, value);
            batch = String.Format(batchFormat, methodBuilder);

            // process batch commands.
            string batchReturn = web.ProcessBatchData(batch);

            return batchReturn;
        }
    }
}
