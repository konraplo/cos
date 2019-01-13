namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;
    using System;

    /// <summary>
    /// Event receivers for store list
    /// </summary>
    public class StoreListEventReceiver : SPItemEventReceiver
    {
       
        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
            SetStoreId(properties.ListItem);
        }

        /// <summary>
        /// Ein Element wurde aktualisiert..
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemUpdated");
            SetStoreId(properties.ListItem);
        }

        private void SetStoreId(SPListItem storeItem)
        { 
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Set Store Id");
            if (string.IsNullOrEmpty(Convert.ToString(storeItem[Fields.StoreId])))
            {
                storeItem[Fields.StoreId] = string.Format("{0}-{1}", storeItem.ID, storeItem.Title);
                storeItem.Update();
            }
        }
    }
}
