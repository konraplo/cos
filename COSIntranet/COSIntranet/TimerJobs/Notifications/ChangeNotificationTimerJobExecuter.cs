namespace Change.Intranet.TimerJobs
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using System;

    /// <summary>
    /// An instance of this class is called from the timer job that handles the Change Notification.
    /// </summary>
    public class ChangeNotificationTimerJobExecuter
    {
        /// <summary>
        /// This Method is an extract of the timer job execute method.
        /// </summary>
        /// <param name="notificationTimerJob">Jobdefinition of the notification Timerjob.</param>
        internal void Execute(ChangeNotificationTimerJob notificationTimerJob)
        {
            SPWebApplication webApplication = notificationTimerJob.WebApplication;
            Guid siteId = CommonUtilities.FindBusinessDevelopmentSiteId(webApplication);
            if (siteId != Guid.Empty)
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                    }
                }
            }
        }

    }
}
