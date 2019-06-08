using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Change.Contracts.Common;
using Change.Contracts.TimerJobs;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Change.Contracts.Features.ChangeContractsTimerjobs
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("4f45e6d3-9c24-49ec-8dce-98675817a8ee")]
    public class ChangeContractsTimerjobsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    SPWebApplication parentWebApp = (SPWebApplication)properties.Feature.Parent;
                    this.RemoveTimmerJobs(parentWebApp);
                    this.SetupTimerJobs(parentWebApp);
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, this.GetType().Name, string.Format("Error wgile activating Feature:{0}", ex.Message));
                throw;
            }
        }

        private void RemoveTimmerJobs(SPWebApplication webApp)
        {
            foreach (SPJobDefinition spJobDefinition in webApp.JobDefinitions)
            {
                if (spJobDefinition.Name == CommonUtilities.ChangeNotificationTimerJobName)
                {
                    Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("delete:{0}", spJobDefinition.Name));
                    spJobDefinition.Delete();
                }
            }
        }

        /// <summary>
        /// This method initialize all timerjobs necessary for the solution
        /// </summary>
        /// <param name="webApp"></param>
        protected void SetupTimerJobs(SPWebApplication webApp)
        {
            // notification timer job
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("set up:{0}", CommonUtilities.ChangeNotificationTimerJobName));
            SPJobDefinition job = new ChangeContractsNotificationTimerJob(CommonUtilities.ChangeNotificationTimerJobName, webApp);

            SPDailySchedule schedule = new SPDailySchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 0;
            schedule.BeginHour = 23;
            schedule.EndHour = 23;
            schedule.BeginMinute = 0;
            schedule.EndMinute = 30;

            job.Schedule = schedule;
            job.Update();
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                this.RemoveTimmerJobs(webApp);
            }
            catch (Exception exception)
            {
                Logger.WriteLog(Logger.Category.Unexpected, this.GetType().Name, string.Format("Error while Deactivating Feature:{0}", exception.Message));
                throw;
            }
        }


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
