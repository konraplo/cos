using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Change.Intranet.Features.ChangeListsForDivisions
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("a53b7b35-d1c1-434b-917a-a398fb2b9f64")]
    public class ChangeListsForDivisionsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                // add folder strucure
                VisualMerchandiseLib(web);
                MarketingLib(web);
                ProductAssortmentLib(web);
                SalesTrainingLib(web);
                DailyOperationLib(web);
                ChangeAcademyLib(web);
                HRLib(web);
            }
        }

        private static void VisualMerchandiseLib(SPWeb web)
        {
            // visual Merchandise
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/VisualMerchandise"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBookingOfCraftsmen", "COSIntranet", web.Language);
            SPFolder newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrderingHandcraftmen", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCentersOrderingHandcraftmen", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBookingOfLights", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDanskLyskildeOrdersheet", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSignsForPrint", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleWindowSigns", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);                      

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSmallCampaignsSigns", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePhotoReports", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleManualPhotoReports", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleUploadPicturesGuide", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);
            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMountingOfFoil", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMountingFoil", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMountingTextFoil", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleThingsOnHangers", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePonHAllProducts", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleManuals", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleInstoreInventoryManuals", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMGuidelines", "COSIntranet", web.Language);
            newFolder = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMGoldenRules", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMBasic", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleVMSeasonal", "COSIntranet", web.Language);
            newFolder.SubFolders.Add(folderUrl);
            list.OnQuickLaunch = true;
            list.Update();
        }

        private static void MarketingLib(SPWeb web)
        {
            // Marketing
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
                        
            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleLYTemplate", "COSIntranet", web.Language);
            SPFolder yearwheeltmp = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYearwheel", "COSIntranet", web.Language);
            yearwheeltmp.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYearCalender", "COSIntranet", web.Language);
            yearwheeltmp.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleContentPlan", "COSIntranet", web.Language);
            SPFolder ctplan = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleContentPlan", "COSIntranet", web.Language);
            ctplan.SubFolders.Add(folderUrl);

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

            list.OnQuickLaunch = true;
            list.Update();
        }

        private static void ProductAssortmentLib(SPWeb web)
        {
            // Product Assortment
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/ProductAssortment"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCatalogOverview", "COSIntranet", web.Language);
            SPFolder catalog = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBasicLookbook", "COSIntranet", web.Language);
            SPFolder basicLookbook = catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSizeOverview", "COSIntranet", web.Language);
            basicLookbook.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOutphasedItems", "COSIntranet", web.Language);
            basicLookbook.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCPLookbooks", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleFurnitureMannequinFittings", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStoreSupply", "COSIntranet", web.Language);
            catalog.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMerchandiseFlow", "COSIntranet", web.Language);
            SPFolder mflow = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleEOSReturn", "COSIntranet", web.Language);
            mflow.SubFolders.Add(folderUrl);

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

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePrepackOrderConfirmation", "COSIntranet", web.Language);
            mflow.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOtherStoresTransfer", "COSIntranet", web.Language);
            SPFolder otherStoresTransfer = mflow.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleGLSpricelist", "COSIntranet", web.Language);
            otherStoresTransfer.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleManual", "COSIntranet", web.Language);
            otherStoresTransfer.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSwapList", "COSIntranet", web.Language);
            otherStoresTransfer.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleProductInfo", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);
            
            list.OnQuickLaunch = true;
            list.Update();
        }

        private static void SalesTrainingLib(SPWeb web)
        {
            // Sales Training
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/SalesTraining"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitle4StepsSalesMethod", "COSIntranet", web.Language);
            SPFolder stepsSalesMethod = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesTrainingMethod", "COSIntranet", web.Language);
            stepsSalesMethod.SubFolders.Add(folderUrl);           

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCompetitionsInternal", "COSIntranet", web.Language);
            SPFolder competitionsInternal = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleInternally", "COSIntranet", web.Language);
            competitionsInternal.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesGaming", "COSIntranet", web.Language);
            competitionsInternal.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleObjectiveTools", "COSIntranet", web.Language);
            SPFolder objectiveTools = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleObjectiveTools", "COSIntranet", web.Language);
            objectiveTools.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTipsandTricks", "COSIntranet", web.Language);
            SPFolder tipsAndTricks = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesTips", "COSIntranet", web.Language);
            tipsAndTricks.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTipsReturnExchange", "COSIntranet", web.Language);
            tipsAndTricks.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMaintainTarget", "COSIntranet", web.Language);
            tipsAndTricks.SubFolders.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
        }

        private static void DailyOperationLib(SPWeb web)
        {
            // Daily Operation
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

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrdinary", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleChristmas", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSale", "COSIntranet", web.Language);
            titleExchangeRules.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBuddyStores", "COSIntranet", web.Language);
            SPFolder buddyStores = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNearestContact", "COSIntranet", web.Language);
            buddyStores.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMoneyBank", "COSIntranet", web.Language);
            SPFolder moneyBank = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMoneyHandling", "COSIntranet", web.Language);
            moneyBank.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleBagsOrdering", "COSIntranet", web.Language);
            moneyBank.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDiffReportTemplate", "COSIntranet", web.Language);
            moneyBank.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDailyRutines", "COSIntranet", web.Language);
            SPFolder dailyRutines = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleCleaningMaintenance", "COSIntranet", web.Language);
            dailyRutines.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSalesFollowUp", "COSIntranet", web.Language);
            SPFolder salesFollowUp = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleDWMFollowup", "COSIntranet", web.Language);
            salesFollowUp.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleMorningMeetings", "COSIntranet", web.Language);
            salesFollowUp.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStoreVisitReport", "COSIntranet", web.Language);
            SPFolder storeVisitReport = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTemplateGuide", "COSIntranet", web.Language);
            storeVisitReport.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleTRUCashRegister", "COSIntranet", web.Language);
            SPFolder truCashRegister = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleHandling", "COSIntranet", web.Language);
            truCashRegister.SubFolders.Add(folderUrl);
           
            list.OnQuickLaunch = true;
            list.Update();
        }

        private static void ChangeAcademyLib(SPWeb web)
        {
            // visual Merchandise
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/ChangeAcademy"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOnline", "COSIntranet", web.Language);
            SPFolder online = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleLogin", "COSIntranet", web.Language);
            SPFolder login = online.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleSharedLogin", "COSIntranet", web.Language);
            login.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleForgotPassword", "COSIntranet", web.Language);
            login.SubFolders.Add(folderUrl);

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
        }


        private static void HRLib(SPWeb web)
        {
            // HR
            SPList list = web.GetList(SPUrlUtility.CombineUrl(web.Url, "Lists/HR"));
            SPFolderCollection folderColl = list.RootFolder.SubFolders;

            string folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleAPV", "COSIntranet", web.Language);
            SPFolder apv = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleAPVChecklist", "COSIntranet", web.Language);
            apv.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitlePhonelist", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);
            
            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleOrganisazionChart", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStaff", "COSIntranet", web.Language);
            SPFolder staff = folderColl.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleYoungstrersWokinghours", "COSIntranet", web.Language);
            staff.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleNewEmployee", "COSIntranet", web.Language);
            staff.SubFolders.Add(folderUrl);

            folderUrl = SPUtility.GetLocalizedString("$Resources:ChangeFolderTitleStafflHandbook", "COSIntranet", web.Language);
            folderColl.Add(folderUrl);

            list.OnQuickLaunch = true;
            list.Update();
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
