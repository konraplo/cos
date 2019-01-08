namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;

    /// <summary>
    /// Event receivers for CHANGE task list
    /// </summary>
    public class TaskListEventReceiver : SPItemEventReceiver
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

        private void SetLocalization(SPListItem item)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("SetLocalization for id:{0}, ct:{1}",item.ID, item.ContentType.Name));
            if (item.ContentType.Parent.Id == ContentTypeIds.Project)
            {
                EventFiringEnabled = false;
                EventFiringEnabled = true;
            }
            else if(item.ContentType.Parent.Id == ContentTypeIds.ProjectTask)
            {
                EventFiringEnabled = false;
                EventFiringEnabled = true;
            }
        }
    }
}
