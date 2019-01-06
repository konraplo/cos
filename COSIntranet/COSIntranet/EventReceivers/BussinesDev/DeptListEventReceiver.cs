namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;

    /// <summary>
    /// Event receivers for department list
    /// </summary>
    public class DeptListEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
        }

        /// <summary>
        /// Ein Element wurde aktualisiert..
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemUpdated");
        }
    }
}
