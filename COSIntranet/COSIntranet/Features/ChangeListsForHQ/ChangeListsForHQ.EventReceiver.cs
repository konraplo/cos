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
                //ITLib(web);
                //FinanceLib(web);
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
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add visual Merchandise");
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
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add Marketing");
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
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add Product Assortment");
        }

        private void SalesTrainingLib(SPWeb web)
        {
            // Sales Training
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add Sales Training");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/SalesTraining"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitle4StepsSalesMethod", "COSIntranet", web.Language);
            SPFolder stepsSalesMethod = folderColl.Add(folderUrl);
          
            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitionsInternal", "COSIntranet", web.Language);
            SPFolder competitionsInternal = folderColl.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleInternally", "COSIntranet", web.Language);
            //competitionsInternal.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesGaming", "COSIntranet", web.Language);
            //competitionsInternal.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleObjectiveTools", "COSIntranet", web.Language);
            SPFolder objectiveTools = folderColl.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleObjectiveTools", "COSIntranet", web.Language);
            //objectiveTools.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTipsandTricks", "COSIntranet", web.Language);
            SPFolder tipsAndTricks = folderColl.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesTips", "COSIntranet", web.Language);
            //tipsAndTricks.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTipsReturnExchange", "COSIntranet", web.Language);
            //tipsAndTricks.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMaintainTarget", "COSIntranet", web.Language);
            //tipsAndTricks.SubFolders.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add Sales Training");
        }

        private void DailyOperationLib(SPWeb web)
        {
            // Daily Operation
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add Daily Operation");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/DailyOperation"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStoreExpences", "COSIntranet", web.Language);
            SPFolder storeExpences = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStoreAcqGuide", "COSIntranet", web.Language);
            storeExpences.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEXRulesGiftCard", "COSIntranet", web.Language);
            SPFolder exRulesGiftCard = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleExchangeRules", "COSIntranet", web.Language);
            SPFolder titleExchangeRules = exRulesGiftCard.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFl", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDK", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePL", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSE", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNO", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDE", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCAN", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleAustria", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrdinary", "COSIntranet", web.Language);
            //titleExchangeRules.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleChristmas", "COSIntranet", web.Language);
            //titleExchangeRules.SubFolders.Add(folderUrl);

            //folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSale", "COSIntranet", web.Language);
            //titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleRetailOperationManual", "COSIntranet", web.Language);
            SPFolder retailOperationManual = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNewsletter", "COSIntranet", web.Language);
            SPFolder newsletter = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYear", "COSIntranet", web.Language);
            newsletter.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTRUCustody", "COSIntranet", web.Language);
            SPFolder truc = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleHandling", "COSIntranet", web.Language);
            truc.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleProcYearwheel", "COSIntranet", web.Language);
            SPFolder pywheel = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDesign", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTechnical", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleProduction", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleWarehouse", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrderMgnt", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleIT", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleRetail", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBusinessDev", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFinance", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMarketing", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEcommerce", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePlanning", "COSIntranet", web.Language);
            pywheel.SubFolders.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add Sales Training");
        }

        private void ChangeAcademyLib(SPWeb web)
        {
            // Change Academy
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add Change Academy");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/ChangeAcademy"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOnline", "COSIntranet", web.Language);
            SPFolder online = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleLogin", "COSIntranet", web.Language);
            SPFolder login = online.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleGuideChangeAcademy", "COSIntranet", web.Language);
            online.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOnsightEducation", "COSIntranet", web.Language);
            SPFolder onsightEducation = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBraFitting", "COSIntranet", web.Language);
            onsightEducation.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNeighbourSize", "COSIntranet", web.Language);
            onsightEducation.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompendium", "COSIntranet", web.Language);
            onsightEducation.SubFolders.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End add Change Academy");
        }

        private void HRLib(SPWeb web)
        {
            // HR
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Start add HR");
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/HR"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleWorkenvironment", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePhonelistHQinfo", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrganisazionChart", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleGymProgram", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStafflHandbook", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCareepath", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOnboarding", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleJobDescriptions", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNewEmployee", "COSIntranet", web.Language);
            folderColl.Add(folderUrl); 


            list.OnQuickLaunch = true;
            list.Update();
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "End HR");
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
