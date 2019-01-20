namespace Change.Intranet.Features.ChangeListsForHQ
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("938634aa-6e5a-4bbf-8d1e-44b56a089b22")]
    public class ChangeListsForHQEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                // add folder strucure
                Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "add folder strucure");
                VisualMerchandiseLib(web);
                MarketingLib(web);
                ProductAssortmentLib(web);
                SalesTrainingLib(web);
                DailyOperationLib(web);
                ChangeAcademyLib(web);
                HRLib(web);
                ITLib(web);
                FinanceLib(web);
            }
        }

        private void VisualMerchandiseLib(SPWeb web)
        {
            // visual Merchandise
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add visual Merchandise");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/VisualMerchandise"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePhotoReports", "COSIntranet", web.Language);
            SPFolder photoReports = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleThingsOnHangers", "COSIntranet", web.Language);
            SPFolder thingsOnHangers = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleManuals", "COSIntranet", web.Language);
            SPFolder manuals = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMGuidelines", "COSIntranet", web.Language);
            SPFolder vmGuidelines = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMGoldenRules", "COSIntranet", web.Language);
            vmGuidelines.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMBasic", "COSIntranet", web.Language);
            vmGuidelines.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMSeasonal", "COSIntranet", web.Language);
            vmGuidelines.SubFolders.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForDivisionsEventReceiver", "End add visual Merchandise");
        }

        private void MarketingLib(SPWeb web)
        {
            // Marketing
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add Marketing");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/Marketing"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCurrentCampaign", "COSIntranet", web.Language);
            SPFolder campaign = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleClaimsMarketing", "COSIntranet", web.Language);
            campaign.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCurrentCampaign", "COSIntranet", web.Language);
            campaign.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleGenericInstore", "COSIntranet", web.Language);
            campaign.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrderSize", "COSIntranet", web.Language);
            campaign.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMarketingGuidelines", "COSIntranet", web.Language);
            SPFolder vmGuidelines = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleContentPlan", "COSIntranet", web.Language);
            SPFolder ctplan = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEvents", "COSIntranet", web.Language);
            SPFolder events = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleChristmas", "COSIntranet", web.Language);
            SPFolder xmass = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitions", "COSIntranet", web.Language);
            xmass.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePreperation", "COSIntranet", web.Language);
            xmass.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleHalloween", "COSIntranet", web.Language);
            SPFolder halloweenn = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitions", "COSIntranet", web.Language);
            halloweenn.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePreperation", "COSIntranet", web.Language);
            halloweenn.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEaster", "COSIntranet", web.Language);
            SPFolder easter = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePreperation", "COSIntranet", web.Language);
            easter.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitions", "COSIntranet", web.Language);
            easter.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOpenByNight", "COSIntranet", web.Language);
            SPFolder obnight = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitions", "COSIntranet", web.Language);
            obnight.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePreperation", "COSIntranet", web.Language);
            obnight.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBlackFriday", "COSIntranet", web.Language);
            SPFolder bfriday = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitions", "COSIntranet", web.Language);
            bfriday.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePreperation", "COSIntranet", web.Language);
            bfriday.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVouchersAndFlyvers", "COSIntranet", web.Language);
            SPFolder vandf = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVoucher", "COSIntranet", web.Language);
            vandf.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFlyer", "COSIntranet", web.Language);
            vandf.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEDM", "COSIntranet", web.Language);
            SPFolder edm = events.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleIdeasEDM", "COSIntranet", web.Language);
            edm.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrderTemplates", "COSIntranet", web.Language);
            events.SubFolders.Add(folderUrl);


            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleLYTemplate", "COSIntranet", web.Language);
            SPFolder yearwheeltmp = folderColl.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYearwheel", "COSIntranet", web.Language);
            //yearwheeltmp.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYearCalender", "COSIntranet", web.Language);
            //yearwheeltmp.SubFolders.Add(folderUrl);

            

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleContentPlan", "COSIntranet", web.Language);
            //ctplan.SubFolders.Add(folderUrl);

            
            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForDivisionsEventReceiver", "End add Marketing");
        }

        private void ProductAssortmentLib(SPWeb web)
        {
            // Product Assortment
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add Product Assortment");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/ProductAssortment"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCatalogOverview", "COSIntranet", web.Language);
            SPFolder catalog = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePDS", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleProgramBoards", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCollectionOverview", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCollectionQtyOrderedStyles", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSizeOverview", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBasicLookbook", "COSIntranet", web.Language);
            SPFolder basicLookbook = catalog.SubFolders.Add(folderUrl);            

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCPLookbooks", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFurnitureMannequinFittings", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStoreSupply", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMerchandiseFlow", "COSIntranet", web.Language);
            SPFolder mflow = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDosDontsReturn", "COSIntranet", web.Language);
            SPFolder eosReturn = mflow.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOutphasedItems", "COSIntranet", web.Language);
            eosReturn.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleReceiving", "COSIntranet", web.Language);
            SPFolder receiving = mflow.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleClaimsheetTmpGuidelines", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrderingDays", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrdersHowTo", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleContactsFAQ", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFreightPricelist", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSwapList", "COSIntranet", web.Language);
            receiving.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePrepackOrderConfirmation", "COSIntranet", web.Language);
            mflow.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSMS", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleProductInfo", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, "ChangeListsForDivisionsEventReceiver", "End add Product Assortment");
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
