namespace Change.Intranet.TimerJobs
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Utilities;
    using System;

    /// <summary>
    /// An instance of this class is called from the timer job that handles the Change Notification.
    /// </summary>
    public class ChangeNotificationTimerJobExecuter
    {
        private const string queryStringLateTasks =
                                    @"<Where>
                                    <And>
                                      <Or>
                                        <IsNull>
                                          <FieldRef Name='PercentComplete' />
                                         </IsNull>
                                         <Neq>
                                           <FieldRef Name = 'PercentComplete' />
                                            <Value Type='Number'>1</Value>
                                        </Neq>
                                     </Or>
                                     <Lt>
                                        <FieldRef Name='DueDate' />
                                        <Value Type='DateTime'>
                                            <Today/>
                                        </Value>
                                     </Lt>
                                    </And></Where>";

        private const string queryTasksAtRisk =
                                    @"<Where>
                                    <And>
                                      <Or>
                                        <IsNull>
                                          <FieldRef Name='PercentComplete' />
                                         </IsNull>
                                         <Neq>
                                           <FieldRef Name = 'PercentComplete' />
                                            <Value Type='Number'>1</Value>
                                        </Neq>
                                     </Or>
                                     <Eq>
                                        <FieldRef Name='DueDate' />
                                        <Value Type='DateTime'>
                                            <Today OffsetDays='1' />
                                        </Value>
                                     </Eq>
                                    </And></Where>";

        /// <summary>
        /// resx key for project created notification subject
        /// </summary>
        private const string ChangeTaskOverdueFirstReminderTitle = "ChangeTaskOverdueFirstReminderTitle";

        /// <summary>
        /// resx key for project created notification body
        /// </summary>
        private const string ChangeTaskOverdueFirstReminderBody = "ChangeTaskOverdueFirstReminderBody";


        /// <summary>
        /// resx key for project created notification subject
        /// </summary>
        private const string ChangeTaskOverdueSecondReminderTitle = "ChangeTaskOverdueSecondReminderTitle";

        /// <summary>
        /// resx key for project created notification body
        /// </summary>
        private const string ChangeTaskOverdueSecondReminderBody = "ChangeTaskOverdueSecondReminderBody";

        /// <summary>
        /// This Method is an extract of the timer job execute method.
        /// </summary>
        /// <param name="notificationTimerJob">Jobdefinition of the notification Timerjob.</param>
        internal void Execute(ChangeNotificationTimerJob notificationTimerJob)
        {
            SPWebApplication webApplication = notificationTimerJob.WebApplication;
            string siteUrl = CommonUtilities.FindBusinessDevelopmentSiteId(webApplication);
            SendTasksNotifications(siteUrl);

            siteUrl = CommonUtilities.FindProjectsSiteId(webApplication);
            SendTasksNotifications(siteUrl);
        }

        private static void SendTasksNotifications(string siteUrl)
        {
            if (!string.IsNullOrEmpty(siteUrl))
            {
                try
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.ProjectTasks));
                            SPQuery query = new SPQuery();

                            // late tasks
                            query.Query = queryStringLateTasks;
                            SPListItemCollection projectTasks = list.GetItems(query);
                            string subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ChangeTaskOverdueSecondReminderTitle), "COSIntranet", web.Language);
                            string body = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ChangeTaskOverdueSecondReminderBody), "COSIntranet", web.Language);

                            SendNotificationForTasksOwners(web, projectTasks, subject, body, 2);

                            // tasks at risk
                            query = new SPQuery();
                            query.Query = queryTasksAtRisk;
                            projectTasks = list.GetItems(query);
                            subject = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ChangeTaskOverdueFirstReminderTitle), "COSIntranet", web.Language);
                            body = SPUtility.GetLocalizedString(string.Format("$Resources:COSIntranet,{0}", ChangeTaskOverdueFirstReminderBody), "COSIntranet", web.Language);

                            SendNotificationForTasksOwners(web, projectTasks, subject, body, 1);
                        }
                    }

                }
                catch (Exception exception)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("Error while sending notifications:{0}", exception.Message));
                }
            }
        }

        private static void SendNotificationForTasksOwners(SPWeb web, SPListItemCollection projectTasks, string mailTitle, string mailBodyTemplate, int reminderCount)
        {
            foreach (SPListItem taskItem in projectTasks)
            {
                string taskOwner = Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                string taskName = taskItem.Title;
                DateTime dueDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.TaskDueDate]);
                Logger.WriteLog(Logger.Category.Information, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("task:{0}, owner:{1}, due date:{2}", taskName, taskOwner, dueDate.ToShortDateString()));
                if (!string.IsNullOrEmpty(taskOwner))
                {
                    SPFieldUserValue user = new SPFieldUserValue(web, taskOwner);
                    if (!string.IsNullOrEmpty(user.User.Email))
                    {
                        // send reminder
                        Logger.WriteLog(Logger.Category.Information, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("send reminder to :{0}", user.User.Email));
                        if (reminderCount == 1) //first reminder
                        {
                            string mailBody = string.Format(mailBodyTemplate, taskItem.Title, dueDate.ToShortDateString());
                            CommonUtilities.SendEmail(web, user.User.Email, mailBody, mailTitle);
                        }
                        else //second reminder
                        {
                            string mailBody = string.Format(mailBodyTemplate, taskItem.Title);
                            CommonUtilities.SendEmail(web, user.User.Email, mailBody, mailTitle);
                        }
                    }
                }

            }
        }
    }
}
