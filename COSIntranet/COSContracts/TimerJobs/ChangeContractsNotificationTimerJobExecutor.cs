namespace Change.Contracts.TimerJobs
{
    using Change.Contracts.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Utilities;
    using System;
    using static Change.Contracts.Common.ListUtilities;

    /// <summary>
    /// An instance of this class is called from the timer job that handles the Change Contracts Notification/Actions.
    /// </summary>
    public class ChangeContractsNotificationTimerJobExecutor
    {
        private const string warningDateFieldName = "Warning_x0020_date"; //prod
        //private const string warningDateFieldName = "Warning_x0020_date1"; //test
        //private const string queryLateContracts =
        //                            @"<Where>
        //                             <And>
        //                              <Lt>
        //                                <FieldRef Name='{0}' />
        //                                <Value Type='DateTime'>
        //                                  <Today/>
        //                                </Value>
        //                              </Lt>
        //                              <Eq>
        //                                <FieldRef Name='ChangeContractContractStatus' />
        //                                <Value Type='Text'>Active</Value>
        //                              </Eq>
        //                            </And>
        //                           </Where>";

        private const string queryLateContracts =
                                   @"<Where>
                                     <And>
                                      <Eq>
                                        <FieldRef Name='{0}' />
                                        <Value Type='DateTime'>
                                          <Today/>
                                        </Value>
                                      </Eq>
                                      <Eq>
                                        <FieldRef Name='ChangeContractContractStatus' />
                                        <Value Type='Text'>Active</Value>
                                      </Eq>
                                    </And>
                                   </Where>";

        /// <summary>
        /// resx key for project created notification subject
        /// </summary>
        private const string ChangeContractOverdueTitle = "ChangeContractOverdueTitle";

        /// <summary>
        /// resx key for project created notification body
        /// </summary>
        private const string ChangeContractOverdueBody = "ChangeContractOverdueBody";

        /// <summary>
        /// Data Analysis mail
        /// </summary>
        private const string DataAnalysisMail = "dataanalysis@change.com";

        internal void Execute(ChangeContractsNotificationTimerJob notificationTimerJob)
        {
            SPWebApplication webApplication = notificationTimerJob.WebApplication;
            Guid contractInfratrustureFeatureId = Guid.Parse("{02077383-5ff0-4e15-8173-928214ff7c13}");
            Guid siteId = CommonUtilities.FindSiteCollIdByFeature(webApplication, contractInfratrustureFeatureId);
            if (!Guid.Empty.Equals(siteId))
            {
                try
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.Contracts));
                            SPQuery query = new SPQuery();

                            // late contracts
                            query.Query = string.Format(queryLateContracts, warningDateFieldName);
                            SPListItemCollection projectTasks = list.GetItems(query);
                            string subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSContracts,{0}", ChangeContractOverdueTitle), "COSContracts", web.Language);
                            string body = SPUtility.GetLocalizedString(string.Format("$Resources:COSContracts,{0}", ChangeContractOverdueBody), "COSContracts", web.Language);

                            SendNotificationForLateContracts(web, projectTasks, subject, body);
                            //SetContractStatus(web, Fields.StatusExpired);
                        }
                    }

                }
                catch (Exception exception)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, typeof(ChangeContractsNotificationTimerJobExecutor).FullName, string.Format("Error while sending notifications:{0}", exception.Message));
                }
            }
        }

        private static void SendNotificationForLateContracts(SPWeb web, SPListItemCollection projectTasks, string mailTitle, string mailBodyTemplate)
        {
            foreach (SPListItem taskItem in projectTasks)
            {
                string mailAddress = DataAnalysisMail;//Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                string conractName = taskItem.Title;
                string customWarnDate = Convert.ToString(taskItem[warningDateFieldName]);
                string[] customWarnDateValue = customWarnDate.Split(new char[] { ';', '#' }, StringSplitOptions.RemoveEmptyEntries);
                DateTime warnDate = Convert.ToDateTime(customWarnDateValue[1]);
                DateTime endDate = Convert.ToDateTime(taskItem[Fields.ChangeContractEndDate]);
                int diffMonth = ((endDate.Year - warnDate.Year) * 12) + endDate.Month - warnDate.Month;
                SPFieldLookupValue customer = new SPFieldLookupValue(Convert.ToString(taskItem[Fields.Customer]));
                SPFieldLookupValue vendor = new SPFieldLookupValue(Convert.ToString(taskItem[Fields.Vendor]));

                string itemUrl = string.Format("{0}/{1}?ID={2}", web.Url, taskItem.ParentList.Forms[PAGETYPE.PAGE_DISPLAYFORM].Url, taskItem.ID);

                Logger.WriteLog(Logger.Category.Information, typeof(ChangeContractsNotificationTimerJobExecutor).FullName, string.Format("contract:{0}, warndate date:{1}", conractName, warnDate.ToShortDateString()));
                if (!string.IsNullOrEmpty(mailAddress))
                {
                    string mailBody = string.Format(mailBodyTemplate, conractName, customer.LookupValue, vendor.LookupValue, diffMonth, itemUrl);
                    CommonUtilities.SendEmail(web, mailAddress, mailBody, mailTitle);
                }

            }
        }

        /// <summary>
        /// Set contract status for late contracts
        /// </summary>
        /// <param name="web">Contracts web</param>
        /// <param name="status">Contract status</param>
        private static void SetContractStatus(SPWeb web, string status)
        {
            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, Urls.Contracts));
            SPQuery query = new SPQuery();

            // late contracts
            query.Query = queryLateContracts;
            SPListItemCollection contracts = list.GetItems(query);

            using (DisableEventFiring scope = new DisableEventFiring())
            {
                foreach (SPListItem contractItem in contracts)
                {
                    contractItem[Fields.ChangeContractContractStatus] = status;
                    contractItem.SystemUpdate(false);
                }
            }
        }
    }
}
