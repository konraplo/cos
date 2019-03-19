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
        /// <summary>
        /// This Method is an extract of the timer job execute method.
        /// </summary>
        /// <param name="notificationTimerJob">Jobdefinition of the notification Timerjob.</param>
        internal void Execute(ChangeNotificationTimerJob notificationTimerJob)
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
                            SPList list = web.GetList(SPUtility.ConcatUrls(web.Url, ListUtilities.Urls.ProjectTasks));
                            SPQuery query = new SPQuery();
                            string queryStringLateTasks =
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
                            query.Query = queryStringLateTasks;
                            SPListItemCollection projectTasks = list.GetItems(query);
                            foreach (SPListItem taskItem in projectTasks)
                            {
                                string taskOwner = Convert.ToString(taskItem[SPBuiltInFieldId.AssignedTo]);
                                string taskName = taskItem.Title;
                                DateTime dueDate = Convert.ToDateTime(taskItem[SPBuiltInFieldId.TaskDueDate]);
                                Logger.WriteLog(Logger.Category.Information, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("task:{0}, owner:{1}, duedate:{2}", taskName, taskOwner, dueDate.ToShortDateString()));
                                if (!string.IsNullOrEmpty(taskOwner))
                                {
                                    SPFieldUserValue user = new SPFieldUserValue(web, taskOwner);
                                    if (string.IsNullOrEmpty(user.User.Email))
                                    {
                                        // send reminder
                                        Logger.WriteLog(Logger.Category.Information, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("send reminder to :{0}", user.User.Email));

                                    }
                                }
                                
                            }
                        }
                    }

                }
                catch (Exception exception)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, typeof(ChangeNotificationTimerJobExecuter).FullName, string.Format("Error while sending notifications:{0}", exception.Message));
                }
            }
        }

    }
}
