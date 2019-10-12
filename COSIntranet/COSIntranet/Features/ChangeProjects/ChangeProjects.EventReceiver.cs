

namespace Change.Intranet.Features.ChangeProjects
{
    using Change.Intranet.Common;
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.SharePoint;
    using System.Reflection;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("bb4a92ab-bc88-4890-b21f-dfface5b10dd")]
    public class ChangeProjectsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                Logger.WriteLog(Logger.Category.Information, "ChangeProjectsEventReceiver", "add CT and ER");
                //AddFieldsCtErToLists(web);

                
                //Upgradeto12(web);
                //Upgradeto13(web);
            }
        }

        /// <summary>
        /// Add fields, content types end event receivers to list
        /// </summary>
        /// <param name="web"></param>
        private void AddFieldsCtErToLists(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Find lists");

            string deptUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Departments);
            SPList deptList = web.GetList(deptUrl);
            
            string tasksUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTasks);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", tasksUrl));
            SPList tasksList = web.GetList(tasksUrl);

            string projectsUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.Projects);
            SPList projectsList = web.GetList(projectsUrl);

            string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplates);
            SPList projectTemplatesList = web.GetList(projectTemplatesUrl);

            // add lookups
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", "Add lookups");

            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", deptUrl));
            SPFieldLookup deptLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Department, "$Resources:COSIntranet,ChangeColDeparment", Fields.Title, deptList, false, false);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", projectsUrl));
            SPFieldLookup projectLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.Project, "$Resources:COSIntranet,ChangeColProject", Fields.Title, projectsList, false, false);
            Logger.WriteLog(Logger.Category.Information, "ChangeBusinessDevelopmentEventReceiver", string.Format("add Lookups to:{0}", projectTemplatesUrl));
            SPFieldLookup projecttemplateLookup = CommonUtilities.CreateLookupField(web, Fields.ChangeFieldsGroup, Fields.ProjectTemplate, "$Resources:COSIntranet,ChangeColProjectTemplate", Fields.Title, projectTemplatesList, false, false);

            // add ct to lists
            SPContentType deptContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Department];
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", deptContentType.Name, deptUrl));
            SPContentType deptListContentType = CommonUtilities.AttachContentTypeToList(deptList, deptContentType, true, false);
            CommonUtilities.AddFieldToContentType(web, deptListContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColParentdeparment");

            SPContentType projectContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.Project];
            projectContentType.FieldLinks.Delete(SPBuiltInFieldId.Predecessors);
            projectContentType.FieldLinks[SPBuiltInFieldId.Title].DisplayName = "$Resources:COSIntranet,ChangeProjectTitle";
            projectContentType.FieldLinks[SPBuiltInFieldId.TaskDueDate].DisplayName = "$Resources:COSIntranet,ChangeOpeningDate";
            projectContentType.FieldLinks[SPBuiltInFieldId.AssignedTo].DisplayName = "$Resources:COSIntranet,ChangeProjectCoordinator";
            projectContentType.FieldLinks[SPBuiltInFieldId.StartDate].ReadOnly = true;
            CommonUtilities.AddFieldToContentType(web, projectContentType, deptLookup, true, false, string.Empty);
            CommonUtilities.AddFieldToContentType(web, projectContentType, projecttemplateLookup, false, false, string.Empty);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectContentType.Name, projectsUrl));
            CommonUtilities.AttachContentTypeToList(projectsList, projectContentType, true, false);

            SPContentType projectTaskContentType = web.Site.RootWeb.ContentTypes[ContentTypeIds.ProjectTask];
            projectTaskContentType.FieldLinks[Fields.ChangeDeparmentmanager].ReadOnly = true;

            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, projectLookup, true, false, "$Resources:COSIntranet,ChangeColParentProject");
            CommonUtilities.AddFieldToContentType(web, projectTaskContentType, deptLookup, false, false, "$Resources:COSIntranet,ChangeColResponsibleDepartment");

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ct:{0} to:{1}", projectTaskContentType.Name, tasksUrl));
            CommonUtilities.AttachContentTypeToList(tasksList, projectTaskContentType, true, false);

            //add ER to Lists
            
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", tasksUrl));
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.ProjectMGMT.ProjecTaskListEventReceiver", false);
            CommonUtilities.AddListEventReceiver(tasksList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.ProjectMGMT.ProjecTaskListEventReceiver", false);

            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("add ER to List, {0}", projectsUrl));
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemAdded, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.ProjectMGMT.ProjectEventReceiver", false);
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.ProjectMGMT.ProjectEventReceiver", false);
            CommonUtilities.AddListEventReceiver(projectsList, SPEventReceiverType.ItemDeleted, Assembly.GetExecutingAssembly().FullName, "Change.Intranet.EventReceivers.ProjectMGMT.ProjectEventReceiver", false);

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
