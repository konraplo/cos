namespace Change.Contracts.TimerJobs
{
    using Change.Contracts.Common;
    using Microsoft.SharePoint.Administration;
    using System;

    /// <summary>
    /// This job definition represents the Timer job responsible for the Change contracts notifications
    /// </summary>
    public class ChangeContractsNotificationTimerJob : SPJobDefinition
    {
        /// <summary>
        /// Empty CTOR
        /// </summary>
        public ChangeContractsNotificationTimerJob() : base()
        {

        }

        /// <summary>
        /// Unused CTOR
        /// </summary>
        /// <param name="jobName">Name of the job</param>
        /// <param name="service">The Service</param>
        /// <param name="server">The server</param>
        /// <param name="targetType">SPJobLockType</param>
        public ChangeContractsNotificationTimerJob(string jobName, SPService service, SPServer server, SPJobLockType targetType) : base(jobName, service, server, targetType)
        {

        }

        /// <summary>
        /// Unused CTOR
        /// </summary>
        /// <param name="jobName">Name of the job</param>
        /// <param name="webApplication">WebApplication object</param>
        public ChangeContractsNotificationTimerJob(string jobName, SPWebApplication webApplication) : base(jobName, webApplication, null, SPJobLockType.Job)
        {
            this.Title = CommonUtilities.ChangeNotificationTimerJobName;
        }

        /// <summary>
        /// Execute-Method.
        /// </summary>
        /// <param name="targetInstanceId">ID of the job instance</param>
        public override void Execute(Guid targetInstanceId)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Entered Executemethod.");
            ChangeContractsNotificationTimerJobExecutor executer = new ChangeContractsNotificationTimerJobExecutor();
            executer.Execute(this);
        }
    }
}
