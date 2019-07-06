﻿namespace Change.Contracts.TimerJobs
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
        private const string queryLateContracts =
                                    @"<Where>
                                     <And>
                                      <Lt>
                                        <FieldRef Name='ChangeContractWarnDate' />
                                        <Value Type='DateTime'>
                                          <Today/>
                                        </Value>
                                      </Lt>
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

        internal void Execute(ChangeContractsNotificationTimerJob notificationTimerJob)
        {
            SPWebApplication webApplication = notificationTimerJob.WebApplication;
            string siteUrl = CommonUtilities.FindBusinessDevelopmentSiteId(webApplication);
            if (!string.IsNullOrEmpty(siteUrl))
            {
                try
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.Contracts));
                            SPQuery query = new SPQuery();

                            // late contracts
                            query.Query = queryLateContracts;
                            SPListItemCollection projectTasks = list.GetItems(query);
                            string subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSContracts,{0}", ChangeContractOverdueTitle), "COSContracts", web.Language);
                            string body = SPUtility.GetLocalizedString(string.Format("$Resources:COSContracts,{0}", ChangeContractOverdueBody), "COSContracts", web.Language);

                            SendNotificationForLateContracts(web, projectTasks, subject, body);
                            SetContractStatus(web, Fields.StatusExpired);
                        }
                    }

                }
                catch (Exception exception)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, typeof(ChangeContractsNotificationTimerJobExecutor).FullName, string.Format("Error while sending notifications:{0}", exception.Message));
                }
            }
        }

        private static void SendNotificationForLateContracts(SPWeb web, SPListItemCollection projectTasks, string mailTitle, string mailBody)
        {
            foreach (SPListItem taskItem in projectTasks)
            {
                string mailAddress = "konrad.plocharski@wp.pl";//Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                string conractName = taskItem.Title;
                DateTime warnDate = Convert.ToDateTime(taskItem[Fields.ChangeContractWarnDate]);
                Logger.WriteLog(Logger.Category.Information, typeof(ChangeContractsNotificationTimerJobExecutor).FullName, string.Format("contract:{0}, warndate date:{1}", conractName, warnDate.ToShortDateString()));
                if (!string.IsNullOrEmpty(mailAddress))
                {
                    //CommonUtilities.SendEmail(web, user.User.Email, string.Format(mailBody, taskItem.Title, dueDate.ToShortDateString()), mailTitle);
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
