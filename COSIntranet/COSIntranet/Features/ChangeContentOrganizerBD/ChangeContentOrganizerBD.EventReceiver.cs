using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Change.Intranet.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Change.Intranet.Features.ChangeContentOrganizerBD
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("60454f6b-c357-471f-8fbd-1b949135c88a")]
    public class ChangeContentOrganizerBDEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
               Upgradeto11(web);
            }
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

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            Logger.WriteLog(Logger.Category.Medium, this.GetType().Name, string.Format("upgrading - web:{0}, action:{1}", web.Url, upgradeActionName));

            switch (upgradeActionName)
            {

                case "UpgradeToV1.1":
                    Upgradeto11(web);
                    break;

            }
        }

        private void Upgradeto11(SPWeb web)
        {
            Logger.WriteLog(Logger.Category.Medium, typeof(ChangeContentOrganizerBDEventReceiver).FullName, string.Format("Upgradeto11 web:{0}", web.Url));
            if (web != null)
            {
                // add folder structure for project
                string projectTemplatesUrl = SPUrlUtility.CombineUrl(web.ServerRelativeUrl.TrimEnd('/'), ListUtilities.Urls.ProjectTemplatesDocuments);
                SPList projectTemplatesList = web.GetList(projectTemplatesUrl);
                UpdateFolderStrucutreMarketingLib(projectTemplatesList);
                UpdateFolderStrucutreDrawingsLib(projectTemplatesList);
                UpdateFolderStrucutreGeneralInformationLib(projectTemplatesList);
                UpdateFolderStrucutreLogisticLib(projectTemplatesList);
                UpdateFolderStrucutrePicturesLib(projectTemplatesList);
                UpdateFolderStrucutreEvaluationLib(projectTemplatesList);
            }

            Logger.WriteLog(Logger.Category.Medium, typeof(ChangeContentOrganizerBDEventReceiver).FullName, string.Format("Upgradeto11 fnished web:{0}", web.Url));
        }

        private static void UpdateFolderStrucutreMarketingLib(SPList list)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "Start update Marketing");
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Marketing";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);

            string fromMarketingToPartnerFolderUrl = "From Marketing to partner";
            SPFolder fromMarketingToPartner = projectFolderObj.SubFolders.Add(fromMarketingToPartnerFolderUrl);
            folderUrl = "Center Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);
            folderUrl = "Own Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);
            folderUrl = "External Channels";
            fromMarketingToPartner.SubFolders.Add(folderUrl);

            string fromPartnerToMarketingFolderUrl = "From partner to Marketing";
            SPFolder fromPartnerToMarketing = projectFolderObj.SubFolders.Add(fromPartnerToMarketingFolderUrl);
            folderUrl = "Center information";
            fromPartnerToMarketing.SubFolders.Add(folderUrl);
            list.Update();

            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\MarketingTemplates");
            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketin_order.xlsx".TrimStart('\\'));
            string trargetFolderRelativeUrl = string.Format(@"Marketing/{0}/{1}", fromPartnerToMarketingFolderUrl, folderUrl);
            CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_Timeline.xlsx".TrimStart('\\'));
            CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Marketing_overview.xlsx".TrimStart('\\'));
            CommonUtilities.AddDocumentToLibrary(list, trargetFolderRelativeUrl, docPath);


            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreMarketingLib", "End update Marketing");
        }

        private static void UpdateFolderStrucutreDrawingsLib(SPList list)
        {
            // Drawings
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "Start update Drawings");

            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Drawings";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "Center before opening";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Recieved from center";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Final - PDF";
            SPFolder finalPDF = projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Final - CAD";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Not approved";
            finalPDF.SubFolders.Add(folderUrl);

            list.Update();

            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreDrawingsLib", "End update Drawings");
        }

        private static void UpdateFolderStrucutreGeneralInformationLib(SPList list)
        {
            // GeneralInformation
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "Start update GeneralInformation");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "GeneralInformation";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\GeneralInformationTemplates");
            list.Update();

            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Costruction_Scope_of_Work.docx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Frontpage.xlsx".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);

            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreGeneralInformationLib", "End update GeneralInformation");
        }

        private static void UpdateFolderStrucutreLogisticLib(SPList list)
        {
            // Logistic
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "Start update Logistic");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Logistic";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "Order";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Order confirmation";
            projectFolderObj.SubFolders.Add(folderUrl);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreLogisticLib", "End update Logistic");
        }

        private static void UpdateFolderStrucutrePicturesLib(SPList list)
        {
            // Pictures
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "Start update Pictures");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Pictures";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);
            folderUrl = "From Warehouse";
            projectFolderObj.SubFolders.Add(folderUrl);
            folderUrl = "Center, after opening";
            projectFolderObj.SubFolders.Add(folderUrl);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutrePicturesLib", "End update Pictures");
        }

        private static void UpdateFolderStrucutreEvaluationLib(SPList list)
        {
            // Evaluation
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "Start update Evaluation");


            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = "Evaluation";
            SPFolder projectFolderObj = folderColl.Add(folderUrl);

            list.Update();

            string rootDirectory = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\FEATURES\COSIntranet_ChangeBusinessDevelopment\EvaluationTemplates");

            string docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_contractor.doc".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);
            docPath = string.Format(@"{0}\{1}", rootDirectory, @"Quality_report_departments.xls".TrimStart('\\'));
            Change.Intranet.Common.CommonUtilities.AddDocumentToLibrary(list, folderUrl, docPath);

            list.Update();
            Logger.WriteLog(Logger.Category.Information, "UpdateFolderStrucutreEvaluationLib", "End update Evaluation");
        }

    }
}
